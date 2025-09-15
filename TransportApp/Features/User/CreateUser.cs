using FluentValidation;
using Microsoft.AspNetCore.Identity;
using TransportApp.Application.Helper;
using TransportApp.Application.Interfaces;
using TransportApp.Domain.DTO.Response;
using TransportApp.Domain.Enum;
using TransportApp.Domain.Model;
using Wolverine;

namespace TransportApp.Features.User;


public record CreateUserRequest
(
    string FirstName,
    string LastName,
    string Email,
    Roles Role,
    string Password,
    string ConfirmPassword
);

public class CreateUserValidator : AbstractValidator<CreateUserRequest>
{
    public CreateUserValidator()
    {
        RuleFor(x => x.FirstName).NotEmpty().WithMessage("First name is required.");
        RuleFor(x => x.FirstName).Must(x => !x.Contains(' ')).WithMessage("First name must not contain whitespace");
        RuleFor(x => x.LastName).Must(x => !x.Contains(' ')).WithMessage("Last name must not contain whitespace");
        RuleFor(x => x.Role).IsInEnum().WithMessage("Invalid role selected.");
        RuleFor(x => x.LastName).NotEmpty().WithMessage("Last name is required.");
        RuleFor(x => x.Email).EmailAddress().WithMessage("A valid email is required.");
        RuleFor(x => x.Password).MinimumLength(6).WithMessage("Password must be at least 6 characters long.");
        RuleFor(x => x.ConfirmPassword).Equal(x => x.Password).WithMessage("Passwords do not match.");
    }
}

public class CreateUserHandler
{
    public static async Task<Response<AuthResponse>> Handle(CreateUserRequest request, UserManager<ApplicationUser> userManager, ITokenGenerator tokenGenerator)
    {
        var user = new ApplicationUser
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            UserName = request.Email,
            Email = request.Email
        };
        var result = await userManager.CreateAsync(user, request.Password);
        if (!result.Succeeded)
            return Response<AuthResponse>.FailureResponse(string.Join(", ", result.Errors.Select(e => e.Description)));

        string role = request.Role.ToString();
        // if (await userManager.IsInRoleAsync(user, role))
        var roleResult = await userManager.AddToRoleAsync(user, role);
        if (!roleResult.Succeeded)
            return Response<AuthResponse>.FailureResponse(string.Join(", ", result.Errors.Select(e => e.Description)));

        return await tokenGenerator.GenerateJwtToken(user, [role]);
    }
}

public class CreateUserEndpoint : IEndpointDefinition
{
    public void MapEndpoints(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/users", async (IMessageBus bus, IValidator<CreateUserRequest> validator, CreateUserRequest request) =>
        {
            var validationResult = await ValidationHelper.ValidateAndResult(request, validator);    
            if (validationResult is not null)
                return validationResult;

            var response = await bus.InvokeAsync<Response<AuthResponse>>(request);

            return response.Success ? Results.Ok(response) : Results.BadRequest(response);
        })
      .WithName("CreateUser")
      .Produces<Response<AuthResponse>>(StatusCodes.Status200OK)
      .Produces<Response<AuthResponse>>(StatusCodes.Status400BadRequest)
      .WithTags("Users");
    }
}