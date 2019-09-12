using CommunityToolShedMvc.Data;
using CommunityToolShedMvc.Models;
using CommunityToolShedMvc.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace CommunityToolShedMvc.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        [AllowAnonymous]
        public ActionResult Register()
        {
            var viewModel = new RegisterViewModel();
            return View(viewModel);
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Register(RegisterViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                string hashedPassword = BCrypt.Net.BCrypt.HashPassword(viewModel.Password, 12);

                DatabaseHelper.Insert(@"
                    insert Person (
                        FirstName, 
                        LastName, 
                        Email, 
                        HashedPassword
                    ) values (
                        @FirstName, 
                        @LastName, 
                        @Email, 
                        @HashedPassword
                    )
                ",
                    new SqlParameter("FirstName", viewModel.FirstName),
                    new SqlParameter("LastName", viewModel.LastName),
                    new SqlParameter("@Email", viewModel.Email),
                    new SqlParameter("@HashedPassword", hashedPassword));

                FormsAuthentication.SetAuthCookie(viewModel.Email, false);
                return RedirectToAction("Index", "Home");
            }
            return View(viewModel);
        }
        // GET: Account
        [AllowAnonymous]
        public ActionResult Login()
        {
            var ViewModel = new LoginViewModel();
            return View(ViewModel);
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(LoginViewModel viewModel)
        {

            if(ModelState.IsValidField("Email") && ModelState.IsValidField("Password"))
            {
                Person person = DatabaseHelper.RetrieveSingle<Person>(@"
                    select HashedPassword
                    from Person
                    where Email = @Email
                ",
                    new SqlParameter("@Email", viewModel.Email));

                if (person == null || !BCrypt.Net.BCrypt.Verify(viewModel.Password, person.HashedPassword))
                {
                    ModelState.AddModelError("", "Login Failed.");   
                }
            }

            if (ModelState.IsValid)
            {
                FormsAuthentication.SetAuthCookie(viewModel.Email, false);
                return RedirectToAction("Index", "Home");
            }
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }
    }
}