using Banker.Extensions;
using Banker.Models;
using BankerLibrary.Repository.IRepository;
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
        private readonly IUserRepository _user;
        private readonly ILoginHistoryRepository _login;

        public AccountController(ILogger<AccountController> logger, IConfiguration config, IUserRepository user, ILoginHistoryRepository login)
        {
            _logger = logger;
            _config = config;
            _user = user;
            _login = login;
        }

        [HttpGet]
        [Route("/Registration")]
        public IActionResult Register()
        {
            _logger.LogInformation("The Register page has been accessed");
            return View();
        }

        [HttpPost]
        public IActionResult Register(RegisterModel rvm)
        {
            _logger.LogInformation("The Register Post methhod has been called");
            try
            {
                 //Query for user existence
                bool userExists = _user.UserAlreadyExists(rvm);

                if (userExists == true)
                {
                    ViewBag.Error = "Name and Email Already Exists";
                    return View();
                }
               
                //If user doesn't exists it inserts data into database
                int result = _user.Register(rvm);
                if (result > 0)
                {
                    _logger.LogInformation("User data Inserted");
                    return RedirectToRoute("default"); //Redirects to Home accounts index view

                }
            }
            catch (NullReferenceException e)
            {
                _logger.LogError($"Exception - '{e}'");
                ViewBag.Error = "Registration Failed, Please Try again!";
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
        public async Task<IActionResult>  Login(LoginModel lvm)
        {
            try
            {
                   
                    UserModel userDetails = _user.GetUserByEmail(lvm);
                    _logger.LogInformation($"Userdetails '{userDetails}'");

                    if (userDetails != null && userDetails.Email != null) //all data should be null checked
                    {
                        var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Email,userDetails.Email),
                            new Claim("Name", userDetails.Name),
                            new Claim(ClaimTypes.NameIdentifier, userDetails.OId.ToString()),
                            new Claim(ClaimTypes.Role,userDetails.Role)

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
                        
                        int result = _login.LoginHistory(userDetails);
                        
                        if (result > 0)
                        {
                            _logger.LogInformation("Login History Inserted");
                            _logger.LogInformation("User Logged in");
                            return RedirectToRoute("dashboard");
                        }
                        else
                        {
                            ViewBag.Error = "Wrong Email & Password, please try again";
                            return View();
                        }
                    }
                    else
                    {
                        ViewBag.Error = "Wrong Email & Password, please try again";
                        return View();
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

            HistoryModel hvm = _login.GetHistory(id);
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
