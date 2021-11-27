using Banker.Extensions;
using Banker.Helpers;
using Banker.Models.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Protocols;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Banker.Controllers
{
    public class AccountController : Controller
    {
        private readonly ILogger<AccountController> _logger;
        private readonly IConfiguration _config;
        private readonly ICommonHelper _helper;

        public AccountController(ILogger<AccountController> logger, IConfiguration config, ICommonHelper helper)
        {
            _logger = logger;
            _config = config;
            _helper = helper;
        }

        [HttpGet]
        [Route("/Registration")]
        public IActionResult Register()
        {
            _logger.LogInformation("The Register page has been accessed");
            return View();
        }

        [HttpPost]
        public IActionResult Register(RegisterViewModel rvm)
        {
            _logger.LogInformation("The Register Post methhod has been called");
            try
            {
                string UserExistsQuery = $"Select * from [User] where Name='{rvm.Name}'" + $"OR Email = '{rvm.Email}'"; //Query for user existence
                bool userExists = _helper.UserAlreadyExists(UserExistsQuery);

                if (userExists == true)
                {
                    ViewBag.Error = "Name and Email Already Exists";
                    return View();
                }
                //if user exists then returns to Account controller and redirects to register view

                string Query = "Insert into [User] (Name,Address,Gender,Phone,Email,Password,Balance)" +
                    $"values ('{rvm.Name}','{rvm.Address}','{rvm.Gender}','{rvm.Phone}','{rvm.Email}','{rvm.Password}','{100}')";
                //If user doesn't exists it inserts data into database
                int result = _helper.DMLTransaction(Query);
                if (result > 0)
                {
                    ViewBag.Success = "Registration Successful!";
                    return RedirectToRoute("default"); //Redirects to Home accounts index view

                }
            }
            catch (NullReferenceException e)
            {
                _logger.LogError($"Exception - '{e}'");
                ViewBag.Error = "Registration Failed, Please Try again!";
                ViewBag.email = HttpContext.Session.GetString("Email");
            }
            return View();
        }

        [HttpGet]
        [Route("/Login")]
        public IActionResult Login()
        {
            _logger.LogInformation("The Login page has been accessed");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult>  Login(LoginViewModel lvm)
        {
            try
            {
                if (string.IsNullOrEmpty(lvm.Email) && string.IsNullOrEmpty(lvm.Password))
                {
                    ViewBag.Error = "Email & Password are empty";
                    _logger.LogInformation("Email & Password input is empty");
                    return View();
                }
                else
                {
                    string query = $"select * from [User] where Email='{lvm.Email}' and Password='{lvm.Password}'";
                    _logger.LogInformation("Login query innitialized and GetUserByEmail class called in common helper class");
                    UserViewModel userDetails = _helper.GetUserByEmail(query);
                    _logger.LogInformation($"Userdetails '{userDetails}'");

                    if (userDetails.Email != null) //all data should be null checked
                    {
                        var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Email,userDetails.Email),
                    new Claim("Name", userDetails.Name),
                    new Claim(ClaimTypes.NameIdentifier, userDetails.OId.ToString())

                };
                        _logger.LogInformation("User Email, Name and Id set on claim");
                        var identity = new ClaimsIdentity(
                            claims, CookieAuthenticationDefaults.AuthenticationScheme);
                        var authProperties = new AuthenticationProperties
                        {
                            AllowRefresh = true,
                            // Refreshing the authentication session should be allowed.

                            ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10)
                            // The time at which the authentication ticket expires. A 
                            // value set here overrides the ExpireTimeSpan option of 
                            // CookieAuthenticationOptions set with AddCookie.
                        };

                        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity), authProperties);
                        var date = DateTime.Now;
                        string Query = "Insert into [LoginHistory] (UserId,DateTime)" + $"values ('{userDetails.OId}','{date}')";
                        int result = _helper.DMLTransaction(Query);
                        
                        if (result > 0)
                        {
                            _logger.LogInformation("Login History Inserted");
                            _logger.LogInformation("User Logged in");
                            return RedirectToRoute("dashboard");
                        }
                    }
                }
            }
            catch(NullReferenceException e)
            {
                _logger.LogError("Exception",e);
                ViewBag.Error = "There is an error while login, please contact admin";
            }  
                return View();
        }
        
        [Route("Home/Dashboard")]
        [Authorize]
        public IActionResult Welcome()
        {
            (int id,_) = HttpContext.GetUserInfo();

            _logger.LogInformation("The Login Dashboard page has been accessed");

            HistoryViewModel hvm = _helper.GetHistory(id);
            return View(hvm);
        }

       
        [Route("Logout")]
        public async Task<IActionResult>  Logout()
        {
            _logger.LogInformation("User logout called");
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            _logger.LogInformation("User logged out");
            return RedirectToRoute("default");
        }
    }
}
