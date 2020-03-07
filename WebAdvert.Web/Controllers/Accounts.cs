using Amazon.AspNetCore.Identity.Cognito;
using Amazon.Extensions.CognitoAuthentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebAdvert.Web.Models.Accounts;

namespace WebAdvert.Web.Controllers
{
    public class Accounts : Controller
    {
        private readonly SignInManager<CognitoUser> signInManager;
        private readonly UserManager<CognitoUser> userManager;
        private readonly CognitoUserPool pool;

        public Accounts(
            SignInManager<CognitoUser> signInManager, 
            UserManager<CognitoUser> userManager, 
            CognitoUserPool pool)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.pool = pool;
        }

        [HttpGet]
        public async Task<IActionResult> Signup()
        {
            var model = new SingupModel();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Signup(SingupModel model)
        {
            if (ModelState.IsValid)
            {
                var user = pool.GetUser(model.Email);
                if (user.Status != null)
                {
                    ModelState.AddModelError("User Exists", "Email is already used");
                    return View(model);
                }

                user.Attributes.Add(CognitoAttribute.Name.AttributeName, model.Email);
                var result = await userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    return RedirectToAction("Confirm", new { model.Email});
                }
            }

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Confirm(string email)
        {
            var model = new ConfirmModel() { Email = email };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Confirm(ConfirmModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByEmailAsync(model.Email);

                if (user == null)
                {
                    ModelState.AddModelError("NotFound", "User not found");
                    return View(model);
                }

                var result = await ((CognitoUserManager<CognitoUser>) userManager).ConfirmSignUpAsync(user, model.Code, true);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                } 
                else
                {
                    foreach (var item in result.Errors)
                    {
                        ModelState.AddModelError(item.Code, item.Description);
                    }

                    return View(model);
                }
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Login()
        {
            var model = new LoginModel();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("LoginError", "Invalid credentials");

                    return View(model);
                }
            }

            return View("Login", model);
        }
    }
}
