using KarthikeyasakthiTransport.Library;
using KarthikeyasakthiTransport.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace KarthikeyasakthiTransport.Controllers
{
    public class LoginController : Controller
    {
        private DatabaseContext _context;

        public LoginController(DatabaseContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel loginViewModel)
        {
            try
            {
                if (!string.IsNullOrEmpty(loginViewModel.Username) && !string.IsNullOrEmpty(loginViewModel.Password))
                {
                    var Username = loginViewModel.Username;
                    var password = EncryptionLibrary.EncryptText(loginViewModel.Password);

                    var result = ValidateUser(Username, password);

                    if (result != null)
                    {
                        if (result.ID == 0 || result.ID < 0)
                        {
                            ViewBag.errormessage = "Entered Invalid Username and Password";
                        }
                        else
                        {
                            var RoleID = result.RoleID;
                            remove_Anonymous_Cookies(); //Remove Anonymous_Cookies

                            HttpContext.Session.SetString("UserID", Convert.ToString(result.ID));
                            HttpContext.Session.SetString("RoleID", Convert.ToString(result.RoleID));
                            HttpContext.Session.SetString("Username", Convert.ToString(result.Username));
                            if (RoleID == 1)
                            {
                                return RedirectToAction("Dashboard", "Home");
                            }
                            else if (RoleID == 2)
                            {
                                return RedirectToAction("Dashboard", "Customer");
                            }
                            else if (RoleID == 3)
                            {
                                return RedirectToAction("Dashboard", "SuperAdmin");
                            }
                        }
                    }
                    else
                    {
                        ViewBag.errormessage = "Entered Invalid Username and Password";
                        return View();
                    }
                }
                return View();
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        public ActionResult Logout()
        {
            try
            {
                CookieOptions option = new CookieOptions();

                if (Request.Cookies["EventChannel"] != null)
                {
                    option.Expires = DateTime.Now.AddDays(-1);
                    Response.Cookies.Append("EventChannel", "", option);
                }

                HttpContext.Session.Clear();
                return RedirectToAction("Login", "Login");
            }
            catch (Exception)
            {
                throw;
            }
        }

        [NonAction]
        public void remove_Anonymous_Cookies()
        {
            if (Request.Cookies["EventChannel"] != null)
            {
                CookieOptions option = new CookieOptions();
                option.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Append("EventChannel", "", option);
            }
        }

        public Registration ValidateUser(string userName, string passWord)
        {
            try
            {
                var validate = (from user in _context.Registration
                                where user.Username == userName && user.Password == passWord
                                select user).SingleOrDefault();

                return validate;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool UpdatePassword(Registration Registration)
        {
            _context.Registration.Attach(Registration);
            _context.Entry(Registration).Property(x => x.Password).IsModified = true;
            int result = _context.SaveChanges();

            if (result > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}