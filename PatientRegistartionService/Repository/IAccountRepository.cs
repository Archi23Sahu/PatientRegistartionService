using Microsoft.AspNetCore.Identity;
using PatientRegistartionService.Models;

namespace PatientRegistartionService.Repository
{
    public interface IAccountRepository
    {
        Task<IdentityResult> SignUpAsync(SignUpModel signUpModel);
        Task<string> SigninAsync(SignInModel signInModel);



    }
}
