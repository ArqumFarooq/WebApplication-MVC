using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WebApplication_MVC.BL;
using WebApplication_MVC.Models;

namespace WebApplication_MVC.Controllers
{
    public class AuthController : Controller
    {
        private readonly DatabaseEntities db = new DatabaseEntities();

        // GET: Auth
        [HttpGet]
        public ActionResult Login(string msg = "")
        {
            try
            {
                ViewBag.msg = msg;
                return View();
            }
            catch
            {
                return RedirectToAction("Error", "Auth", new { msg = "search for the page was not found" });
            }

        }

        [HttpPost]
        public ActionResult PostLogin(User _User)
        {
            try
            {
                //throw new Exception();
                User obj = new UserBL().ComparerUserEmailPassword(_User, db);
                if (obj != null)
                {
                    Session["UserID"] = obj.Id.ToString();
                    Session["UserName"] = obj.FirstName + "  " + obj.LastName;
                    Session["UserEmail"] = obj.Email;
                    Session["Role"] = obj.Role.ToString();
                    var role = Session["Role"];
                    if (Equals(role, "1"))
                    {
                        return RedirectToAction("AdminDashBoard", "Admin");
                    }
                    return RedirectToAction("UserDashBoard", "User", new { msg = "" });
                }
                return RedirectToAction("Login", new { msg = "Match not found" });
            }
            catch
            {
                return RedirectToAction("Error", new { msg = "search for the page was not found" });
            }

        }

        [HttpGet]
        public ActionResult SignUp(string msg = "")
        {
            try
            {
                ViewBag.msg = msg;
                return View();
            }
            catch
            {
                return RedirectToAction("Error", "Auth", new { msg = "search for the page was not found" });
            }

        }

        [HttpPost]
        public ActionResult PostSignUp(User _User)
        {
            try
            {
                User check = new UserBL().CheckEmailUserExist(_User, db);
                if (check == null)
                {
                    _User = new User()
                    {
                        FirstName = _User.FirstName.Trim(),
                        LastName = _User.LastName.Trim(),
                        Email = _User.Email.Trim(),
                        Password = _User.Password,
                        Contact = _User.Contact.Trim(),
                        Role = 2,
                        IsActive = 1,
                        CreatedAt = DateTime.Now
                    };
                    bool checkUser = new UserBL().AddUser(_User, db);
                    if (checkUser == true)
                    {
                        return RedirectToAction("Login", "Auth", new { msg = "Sign up successful" });
                    }
                    else
                    {
                        return RedirectToAction("SignUp", "Auth", new { msg = "error catch something wrong-" });
                    }
                }
                return RedirectToAction("SignUp", "Auth", new { msg = "email already exist" });
            }
            catch
            {
                return RedirectToAction("Error", "Auth", new { msg = "search for the page was not found" });
            }

        }

        public ActionResult Logout()
        {
            try
            {
                FormsAuthentication.SignOut();
                Session.Abandon(); // it will clear the session at the end of request
                return RedirectToAction("Login", "Auth");
            }
            catch
            {
                return RedirectToAction("Error", "Auth", new { msg = "search for the page was not found" });
            }

        }

        //[HandleError]
        public ActionResult Error(string msg = "")
        {
            ViewBag.msg = msg;
            return View();
        }
    }
}