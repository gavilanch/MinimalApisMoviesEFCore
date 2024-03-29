using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MinimalAPIsMovies.DTOs;
using MinimalAPIsMovies.Filters;
using MinimalAPIsMovies.Services;
using MinimalAPIsMovies.Utilities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace MinimalAPIsMovies.Endpoints
{
    public static class UsersEndpoints
    {
        public static RouteGroupBuilder MapUsers(this RouteGroupBuilder group)
        {
            group.MapPost("/register", Register)
                .AddEndpointFilter<ValidationFilter<UserCredentialsDTO>>();
            group.MapPost("/login", Login)
                .AddEndpointFilter<ValidationFilter<UserCredentialsDTO>>();

            group.MapPost("/makeadmin", MakeAdmin)
                .AddEndpointFilter<ValidationFilter<EditClaimDTO>>()
                .RequireAuthorization("isadmin");

            group.MapPost("/removeadmin", RemoveAdmin)
              .AddEndpointFilter<ValidationFilter<EditClaimDTO>>()
              .RequireAuthorization("isadmin");

            group.MapGet("/renewtoken", Renew).RequireAuthorization();
            return group;
        }

        static async Task<Results<Ok<AuthenticationResponseDTO>, 
            BadRequest<IEnumerable<IdentityError>>>> Register(UserCredentialsDTO userCredentialsDTO,
            [FromServices] UserManager<IdentityUser> userManager, IConfiguration configuration)
        {
            var user = new IdentityUser
            {
                UserName = userCredentialsDTO.Email,
                Email = userCredentialsDTO.Email
            };

            var result = await userManager.CreateAsync(user, userCredentialsDTO.Password);

            if (result.Succeeded)
            {
                var authenticationResponse = 
                    await BuildToken(userCredentialsDTO, configuration, userManager);
                return TypedResults.Ok(authenticationResponse);
            }
            else
            {
                return TypedResults.BadRequest(result.Errors);
            }
        }

        static async Task<Results<Ok<AuthenticationResponseDTO>, BadRequest<string>>> Login(
            UserCredentialsDTO userCredentialsDTO, 
            [FromServices] SignInManager<IdentityUser> signInManager,
            [FromServices] UserManager<IdentityUser> userManager,
            IConfiguration configuration
            )
        {
            var user = await userManager.FindByEmailAsync(userCredentialsDTO.Email);

            if (user is null)
            {
                return TypedResults.BadRequest("There was a problem with the email or the password");
            }

            var results = await signInManager.CheckPasswordSignInAsync(user,
                userCredentialsDTO.Password, lockoutOnFailure: false);
             
            if (results.Succeeded)
            {
                var authenticationResponse =
                   await BuildToken(userCredentialsDTO, configuration, userManager);
                return TypedResults.Ok(authenticationResponse);
            }
            else
            {
                return TypedResults.BadRequest("There was a problem with the email or the password");
            }
        }

        static async Task<Results<NoContent, NotFound>> MakeAdmin(EditClaimDTO editClaimDTO,
            [FromServices] UserManager<IdentityUser> userManager)
        {
            var user = await userManager.FindByEmailAsync(editClaimDTO.Email);

            if (user is null)
            {
                return TypedResults.NotFound();
            }

            await userManager.AddClaimAsync(user, new Claim("isadmin", "true"));
            return TypedResults.NoContent();
        }

        static async Task<Results<NoContent, NotFound>> RemoveAdmin(EditClaimDTO editClaimDTO,
            [FromServices] UserManager<IdentityUser> userManager)
        {
            var user = await userManager.FindByEmailAsync(editClaimDTO.Email);

            if (user is null)
            {
                return TypedResults.NotFound();
            }

            await userManager.RemoveClaimAsync(user, new Claim("isadmin", "true"));
            return TypedResults.NoContent();
        }

        private static async Task<Results<NotFound, Ok<AuthenticationResponseDTO>>> Renew(
            IUsersService usersService, IConfiguration configuration,
            [FromServices] UserManager<IdentityUser> userManager)
        {
            var user = await usersService.GetUser();

            if (user is null)
            {
                return TypedResults.NotFound();
            }

            var usersCredential = new UserCredentialsDTO { Email = user.Email! };
            var response = await BuildToken(usersCredential, configuration, userManager);
            return TypedResults.Ok(response);
        }

        private async static Task<AuthenticationResponseDTO> 
            BuildToken(UserCredentialsDTO userCredentialsDTO,
            IConfiguration configuration, UserManager<IdentityUser> userManager)
        {
            var claims = new List<Claim>
            {
                new Claim("email", userCredentialsDTO.Email),
                new Claim("Whatever I want", "this is a value")
            };  

            var user = await userManager.FindByNameAsync(userCredentialsDTO.Email);
            var claimsFromDB = await userManager.GetClaimsAsync(user!);

            claims.AddRange(claimsFromDB);

            var key = KeysHandler.GetKey(configuration).First();
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var expiration = DateTime.UtcNow.AddYears(1);

            var securityToken = new JwtSecurityToken(issuer: null, audience: null,
                claims: claims, expires: expiration, signingCredentials: credentials);

            var token = new JwtSecurityTokenHandler().WriteToken(securityToken);

            return new AuthenticationResponseDTO
            {
                Token = token,
                Expiration = expiration
            };
        }
    }
}
