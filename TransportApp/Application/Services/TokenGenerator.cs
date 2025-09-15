using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TransportApp.Application.Helper;
using TransportApp.Application.Interfaces;
using TransportApp.Domain.DTO.Config;
using TransportApp.Domain.DTO.Response;
using TransportApp.Domain.Model;
using TransportApp.Persistence;

namespace TransportApp.Application.Services;

public sealed class TokenGenerator : ITokenGenerator
{
    private readonly DataContext _context;
    private readonly UserManager<ApplicationUser> userManager;
    private readonly JwtSettings _jwtSettings;

    public TokenGenerator(DataContext context, IOptions<JwtSettings> jwtsettings, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        this.userManager = userManager;
        _jwtSettings = jwtsettings.Value;
    }

    public async Task<Response<AuthResponse>> GenerateJwtToken(ApplicationUser user, IList<string> roles = null)
    {
        var expirationDate = DateTime.UtcNow.AddMonths(1);

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtSettings.Secret));

        string fullName = $"{user.FirstName} {user.LastName}";
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
       {
            new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Exp, expirationDate.ToString()),
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.MobilePhone, user.PhoneNumber ?? string.Empty),
            new Claim(ClaimTypes.Email, user.Email ?? string.Empty),
            new Claim(ClaimTypes.Name, fullName ?? string.Empty),
            //new Claim(ClaimTypes.Role, roles.FirstOrDefault() ?? ""),
            //new Claim(ClaimTypes.Sid, customer_id ?? string.Empty),
        }),
            SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature),
            Issuer = _jwtSettings.Site,
            Audience = _jwtSettings.Audience,
            Expires = expirationDate
        };


        if (roles != null)
        {
            roles.ToList().ForEach(role =>
            {
                tokenDescriptor.Subject.AddClaim(new Claim(ClaimTypes.Role, role));
            });
        }

        SecurityToken Token = tokenHandler.CreateToken(tokenDescriptor);

        string user_token = tokenHandler.WriteToken(Token);

        string RefreshToken = await GenerateRefreshToken(); //generate a refresh token

        await SaveRefreshToken(user.Id, Token.Id, RefreshToken); //save the refresh token

        var authResponse = new AuthResponse()
        {
            RefreshToken = RefreshToken,
            AccessToken = user_token,
            UserId = user.Id,
            FirstName = fullName?.Split(' ')[0],
            LastName = fullName?.Split(' ')[1],
            Username = user.UserName,
            PhoneNumber = user.PhoneNumber,
            Email = user.Email,
            FullName = fullName,
            Roles = roles?.ToArray() ?? [],
            ExpiryTime = expirationDate
        };

        return Response<AuthResponse>.SuccessResponse(authResponse);
    }

    private async Task<string> GenerateRefreshToken()
    {
        var RefreshTokens = await _context.Set<RefreshToken>().ToListAsync();
        bool isUsed = true;
        string refresh_token = string.Empty;
        do
        {
            refresh_token = Util.GenerateRandomString(10) + "-" + Guid.NewGuid();
            isUsed = RefreshTokens.Any(x => x.Token.Equals(refresh_token, StringComparison.OrdinalIgnoreCase));

        } while (isUsed);
        return refresh_token;
    }

    private async Task SaveRefreshToken(string userId, string jwtId, string refresh_token)
    {
        //save refresh token
        RefreshToken NewRefreshToken = new()
        {
            JwtId = jwtId,
            IsUsed = false,
            UserId = userId,
            AddedDate = DateTime.UtcNow,
            ExpiryDate = DateTime.UtcNow.AddYears(1),
            IsRevoked = false,
            Token = refresh_token,
            JwtExpiryDate = DateTime.UtcNow.Add(_jwtSettings.TokenLifeTime)
        };

        _context.Set<RefreshToken>().Add(NewRefreshToken);
        await _context.SaveChangesAsync();
    }
}
