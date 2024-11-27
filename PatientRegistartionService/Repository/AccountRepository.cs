using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using PatientRegistartionService.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PatientRegistartionService.Repository
{
    public class AccountRepository : IAccountRepository
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AccountRepository> _logger;



        public AccountRepository(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IConfiguration configuration, ILogger<AccountRepository> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<IdentityResult> SignUpAsync(SignUpModel signUpModel)
        {
            try
            {
                var user = new IdentityUser()
                {
                    Email = signUpModel.Email,
                    UserName = signUpModel.Email
                };

                var result = await _userManager.CreateAsync(user, signUpModel.Password);
                if (!result.Succeeded)
                {   
                    // log error if user not created
                    foreach (var error in result.Errors)
                    {
                       _logger.LogError($"Error while creating user: {error.Description}");
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while registering a user.");
                return IdentityResult.Failed(new IdentityError
                {
                    Code = "SignUp Error",
                    Description = ex.Message
                });
            }
            
        }

        public async Task<string> SigninAsync(SignInModel signInModel)
        {
            try
            {
                var result = await _signInManager.PasswordSignInAsync(signInModel.Email, signInModel.Password, false, false);

                if (!result.Succeeded)
                {
                    _logger.LogWarning($"Sign-in failed for user {signInModel.Email}.");
                    return null;
                }

                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, signInModel.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };
                var authSigninKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["JWT:SecretKey"]));

                var token = new JwtSecurityToken(
                    issuer: _configuration["JWT:ValidIssuer"],
                    audience: _configuration["JWT:ValidAudience"],
                    expires: DateTime.Now.AddDays(1),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authSigninKey, SecurityAlgorithms.HmacSha256Signature)
                    );

                return new JwtSecurityTokenHandler().WriteToken(token);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred during sign-in.");
                return ("An error occurred during sign-in");
            }
        }



    }
}
