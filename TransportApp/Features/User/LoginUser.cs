using FluentValidation;
using Microsoft.AspNetCore.Identity;
using TransportApp.Application.Helper;
using TransportApp.Application.Interfaces;
using TransportApp.Domain.DTO.Response;
using TransportApp.Domain.Enum;
using TransportApp.Domain.Model;
using Wolverine;

namespace TransportApp.Features.User;


public record LoginUserRequest
(
    string Email,
    Roles Role,
    string Password
);

public class LoginUserValidator : AbstractValidator<LoginUserRequest>
{
    public LoginUserValidator()
    {
        RuleFor(x => x.Email).EmailAddress().WithMessage("A valid email is required.");
        RuleFor(x => x.Role).IsInEnum().WithMessage("Invalid role selected.");
        RuleFor(x => x.Password).MinimumLength(6).WithMessage("Password must be at least 6 characters long.");
    }
}

public class LoginUserHandler
{
    public static async Task<Response<AuthResponse>> Handle(LoginUserRequest request, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ITokenGenerator tokenGenerator)
    {
        var user = await userManager.FindByNameAsync(request.Email);

        if (user is null)
            return Response<AuthResponse>.FailureResponse("Email is incorrect");

        string role = request.Role.ToString();

        if (!await userManager.IsInRoleAsync(user, role))
            return Response<AuthResponse>.FailureResponse("Not a valid role, access denied.");

        var result = await signInManager.CheckPasswordSignInAsync(user, request.Password, false);
        if (!result.Succeeded)
            return Response<AuthResponse>.FailureResponse("Password is incorrect");

        return await tokenGenerator.GenerateJwtToken(user, [role]);
    }
}

public class LoginUserEndpoint : IEndpointDefinition
{
    public void MapEndpoints(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/users/login", async (IMessageBus bus, IValidator<LoginUserRequest> validator, LoginUserRequest request) =>
        {
            var validationResult = await ValidationHelper.ValidateAndResult(request, validator);
            if (validationResult is not null)
                return validationResult;

            var response = await bus.InvokeAsync<Response<AuthResponse>>(request);

            return response.Success ? Results.Ok(response) : Results.BadRequest(response);
        })
      .WithName("LoginUser")
      .Produces<Response<AuthResponse>>(StatusCodes.Status200OK)
      .Produces<Response<AuthResponse>>(StatusCodes.Status400BadRequest)
      .WithTags("Users");
    }
}