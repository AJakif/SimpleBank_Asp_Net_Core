using Banker.Helpers;
using Banker.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Protocols;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Banker.Controllers
{
    public class AccountController : Controller
    {
        private readonly IConfiguration _config;
        readonly CommonHelper _helper; //Object of common helper

        public AccountController(IConfiguration config) //Constructor
        {
            _config = config;
            _helper = new CommonHelper(_config);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Register(RegisterViewModel rvm)
        {
            string UserExistsQuery = $"Select * from [User] where Name='{rvm.Name}'" + $"OR Email = '{rvm.Email}'"; //Query for user existence
            bool userExists = _helper.UserAlreadyExists(UserExistsQuery);

            if(userExists == true)
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
                /*EntryIntoSession(rvm.Email);*///Inserts Query into database and stores user name in session
                ViewBag.Success = "Registration Successful!";
                return RedirectToAction("Index", "Home"); //Redirects to Home accounts index view
             
            }
            ViewBag.Error = "Registration Failed, Please Try again!";
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel lvm)
        {
            if (string.IsNullOrEmpty(lvm.Email) && string.IsNullOrEmpty(lvm.Password))
            {
                ViewBag.Error = "Email & Password are empty";
                return View();
            }
            else
            {
                bool Isfind = SignInMethod(lvm.Email, lvm.Password);
                if (Isfind == true)
                {
                    var idStr = HttpContext.Session.GetString("OId");
                    int id = CommonHelper.ConvertToInt(idStr); //try-catch
                    var date = DateTime.Now;
                    string Query = "Insert into [LoginHistory] (UserId,DateTime)" + $"values ('{id}','{date}')";
                    int result = _helper.DMLTransaction(Query);
                    if (result > 0)
                    {
                        ViewBag.Success = "Login Successful";
                        return RedirectToAction("Welcome");
                    }
                    
                }
                ViewBag.Error = "There is an error while login, please contact admin";
                return View();
            }
        }
        public IActionResult Welcome()
        {
            ViewBag.email = HttpContext.Session.GetString("Email");
            if (!string.IsNullOrEmpty(ViewBag.Email))
            {
                var idStr = HttpContext.Session.GetString("OId");
                int id = CommonHelper.ConvertToInt(idStr); //try-catch
                HistoryViewModel hvm = _helper.GetHistory(id);
                ViewBag.Name = HttpContext.Session.GetString("Name");
                return View(hvm);
            }
            else
            {
                return RedirectToAction("Login");
            }
                
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            ViewBag.Success = "Logout successfull";
            return RedirectToAction("Index","Home");
        }

        private bool SignInMethod(string Email, string Password)
        {
            bool flag = false;
            string query = $"select * from [User] where Email='{Email}' and Password='{Password}'";
            UserViewModel userDetails = _helper.GetUserByEmail(query);

            if (userDetails.Email != null) //all data should be null checked
            {
                HttpContext.Session.SetString("OId", userDetails.OId.ToString());
                HttpContext.Session.SetString("Name", userDetails.Name);
                HttpContext.Session.SetString("Address", userDetails.Address);
                HttpContext.Session.SetString("Gender", userDetails.Gender);
                HttpContext.Session.SetString("Phone", userDetails.Phone);
                HttpContext.Session.SetString("Email", userDetails.Email);

                flag = true;
                
            }
            else
            {
                ViewBag.ErrorMsg = "Email & Password are wrong";
            }
            return flag;
        }


        private void EntryIntoSession(string Email)
        {
            HttpContext.Session.SetString("Email", Email);
        }

        public IActionResult Index()
        {
            return View();
        }

        

    }
}
