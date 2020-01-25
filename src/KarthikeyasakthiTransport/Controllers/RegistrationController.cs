using KarthikeyasakthiTransport.Library;
using KarthikeyasakthiTransport.Model;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace KarthikeyasakthiTransport.Controllers
{
    public class RegistrationController : Controller
    {
        private DatabaseContext _context;

        public RegistrationController(DatabaseContext context)
        {
            _context = context;
        }

        // GET: /<controller>/
        [HttpGet]
        public IActionResult Registration()
        {
            try
            {
                Registration Registration = new Registration();
                Registration.Country = null;
                Registration.City = null;
                Registration.State = null;
                return View(Registration);
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Registration(Registration Registration)
        {
            try
            {
                var isUsernameExists = CheckUserNameExists(Registration.Username);

                if (isUsernameExists)
                {
                    ModelState.AddModelError("", errorMessage: "Username Already Used try unique one!");
                }
                else
                {
                    Registration.CreatedOn = DateTime.Now;
                    Registration.RoleID = 1;
                    Registration.Password = EncryptionLibrary.EncryptText(Registration.Password);
                    Registration.ConfirmPassword = EncryptionLibrary.EncryptText(Registration.ConfirmPassword);
                    if (AddUser(Registration) > 0)
                    {
                        TempData["MessageRegistration"] = "Data Saved Successfully!";
                        return View(Registration);
                    }
                    else
                    {
                        return View(Registration);
                    }
                }

                return View(Registration);
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        public int AddUser(Registration entity)
        {
            _context.Registration.Add(entity);
            return _context.SaveChanges();
        }

        //public JsonResult CheckUserNameExists(string Username)
        //{
        //    try
        //    {
        //        var isUsernameExists = CheckUserNameExists(Username);
        //        if (isUsernameExists)
        //        {
        //            return Json(data: true);
        //        }
        //        else
        //        {
        //            return Json(data: false);
        //        }
        //    }
        //    catch (System.Exception)
        //    {
        //        throw;
        //    }
        //}

        public bool CheckUserNameExists(string Username)
        {
            var result = (from user in _context.Registration
                          where user.Username == Username
                          select user).Count();

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