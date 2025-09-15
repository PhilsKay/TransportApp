using TransportApp.Application.Helper;
using TransportApp.Domain.DTO.Response;
using TransportApp.Domain.Model;

namespace TransportApp.Application.Interfaces;

public interface ITokenGenerator
{
    /// <summary>
    /// Generate jwt token
    /// </summary>
    /// <param name="user"></param>
    /// <param name="fullName"></param>
    /// <param name="roles"></param>
    /// <returns></returns>
    Task<Response<AuthResponse>> GenerateJwtToken(ApplicationUser user, IList<string> roles = null);

    ///// <summary>
    ///// This service is used in verifying the refresh token to generate new jwt
    ///// </summary>
    ///// <param name="RefreshToken"></param>
    ///// <returns></returns>
    //Task<Response<AuthResponse>> VerifyRefreshToken(string RefreshToken);
}
