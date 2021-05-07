using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WebApplication_MVC.BL;
using WebApplication_MVC.Models;
using WebApplication_MVC.Helping_Classes;

namespace WebApplication_MVC.Controllers
{
    public class AuthController : Controller
    {
        private readonly DatabaseEntities db = new DatabaseEntities();
        GeneralPurpose gp = new GeneralPurpose();
        SessionDTO sdto = new SessionDTO();

        public bool ValidateEmail(string email)
        {
            int emailCount = new UserBL().GetActiveUserList(db).Where(x => x.Email.ToLower() == email.ToLower()).Count();
            if (emailCount > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public int ValidateLogin()
        {
            if (sdto.getEmail() != null)
            {
                if (sdto.getRole() == 1)
                {
                    return 1;
                }
                else if (sdto.getRole() == 2)
                {
                    return 2;
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                return 0;
            }
        }

        public ActionResult Profile(int id, string msg = "", string color = "black")
        {
            try
            {
                if (ValidateLogin() == 0)
                {
                    return RedirectToAction("Login", "Auth", new { msg = "Session Expired from User DashBoard, plz login again", color = "Red" });
                }
                User user = new UserBL().GetUserById(id, db);
                ViewBag.msg = msg;
                ViewBag.color = color;
                ViewBag.User = user;
                return View();
            }
            catch
            {
                return RedirectToAction("Error", "Auth", new { msg = "search for the page was not found", color = "Red" });
            }

        }

        [HttpPost]
        public ActionResult PostProfile(User _user)
        {
            try
            {
                if (ValidateLogin() == 0)
                {
                    return RedirectToAction("Login", "Auth", new { msg = "Session Expired from ADMIN DashBoard, plz login again", color = "Red" });
                }
                bool chkEmail = gp.ValidateEmail(_user.Email, _user.Id);
                if (chkEmail == false)
                {
                    return RedirectToAction("Profile", "Auth", new { id = _user.Id, msg = "Email exists", color = "Red" });
                }
                User updateAdmin = new UserBL().GetUserById(_user.Id, db);
                updateAdmin.FirstName = _user.FirstName.Trim();
                updateAdmin.LastName = _user.LastName.Trim();
                updateAdmin.Contact = _user.Contact.Trim();
                updateAdmin.Password = _user.Password.Trim();
                updateAdmin.Email = _user.Email.Trim();
                bool checkUser = new UserBL().UpdateUser(updateAdmin, db);
                if (checkUser == true)
                {
                    if (_user.Id == 1)
                    {
                        return RedirectToAction("AdminDashboard", "Admin", new { msg = "Profile updated successfully", color = "green" });
                    }
                    else
                    {
                        return RedirectToAction("UserDashBoard", "User", new { msg = "Profile updated successfully", color = "green" });
                    }
                }
                else
                {
                    if (_user.Id == 1)
                    {
                         return RedirectToAction("AdminDashboard", "Admin", new { msg = "Profile updated Unsuccessfully bcz yoy enter empty name or maybe same email which exist in DB", color = "Red" });
                    }
                    else
                    {
                        return RedirectToAction("UserDashBoard", "User", new { msg = "Profile updated Unsuccessfully bcz yoy enter empty name or maybe same email which exist in DB", color = "Red" });
                    }
                }
            }
            catch
            {
                return RedirectToAction("Error", "Auth", new { msg = "search for the page was not found", color = "Red" });
            }

        }

        // GET: Auth
        [HttpGet]
        public ActionResult Login(string msg = "", string color = "black")
        {
            try
            {
                ViewBag.color = color;
                ViewBag.msg = msg;
                return View();
            }
            catch
            {
                return RedirectToAction("Error", "Auth", new { msg = "search for the page was not found", color = "Red" });
            }

        }

        [HttpPost]
        public ActionResult PostLogin(User _User)
        {
            try
            {
                SessionDTO dto = new SessionDTO();
                User obj = new UserBL().GetActiveUserList(db).Where(x => x.Email.ToLower() == _User.Email.ToLower() && x.Password == _User.Password).FirstOrDefault();
                if (obj != null)
                {
                    dto.Id = obj.Id;
                    dto.Name = obj.FirstName + " " + obj.LastName;
                    dto.Email = obj.Email;
                    dto.Role = (int)obj.Role;
                    Session["Session"] = dto;

                    if (obj.Role == 1)
                    {
                        return RedirectToAction("AdminDashBoard", "Admin");
                    }
                    else
                    {
                        return RedirectToAction("UserDashBoard", "User");
                    }
                }
                else
                {
                    return RedirectToAction("Login", new { msg = "Match not found", color = "Red" });
                }
            }
            catch
            {
                return RedirectToAction("Error", new { msg = "search for the page was not found", color = "Red" });
            }

        }

        public ActionResult SignUp(string msg = "", string color = "black")
        {
            try
            {
                ViewBag.color = color;
                ViewBag.msg = msg;
                return View();
            }
            catch
            {
                return RedirectToAction("Error", "Auth", new { msg = "search for the page was not found", color = "Red" });
            }

        }

        [HttpPost]
        public ActionResult PostSignUp(User _User)
        {
            try
            {

                bool chkEmail = gp.ValidateEmail(_User.Email, _User.Id);
                if (chkEmail == false)
                {
                    return RedirectToAction("SignUp", "Auth", new { msg = "Emaial exists", color = "Red" });
                }
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
                    return RedirectToAction("Login", "Auth", new { msg = "Sign up successful", color = "green" });
                }
                else
                {
                    return RedirectToAction("SignUp", "Auth", new { msg = "error catch something wrong", color = "red" });
                }
            }
            catch
            {
                return RedirectToAction("Error", "Auth", new { msg = "search for the page was not found", color = "Red" });
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
                return RedirectToAction("Error", "Auth", new { msg = "search for the page was not found", color = "Red" });
            }

        }

        //[HandleError]
        public ActionResult Error(string msg = "", string color = "black")
        {
            ViewBag.color = color;
            ViewBag.msg = msg;
            return View();
        }

        public ActionResult ForgotPassword(string msg = "", string color = "black")
        {
            ViewBag.Message = msg;
            ViewBag.color = color;
            return View();
        }

        public bool SendResetPasswordEmail(string email)
        {
            try
            {
                MailMessage msg = new MailMessage();
                string text = "<link href='https://fonts.googleapis.com/css?family=Bree+Serif' rel='stylesheet'><style>  * {";
                text += "  font-family: 'Bree Serif', serif; }";
                text += " .list-group-item {       border: none;  }    .hor {      border-bottom: 5px solid black;   }";
                text += " .line {       margin-bottom: 20px; }";
                msg.From = new MailAddress("nodlaysa@gmail.com", "Admin");
                msg.To.Add(email);
                msg.Subject = "Project Name | Password Reset";
                msg.IsBodyHtml = true;
                string temp = "<html><head></head><body><nav class='navbar navbar-default'><div class='container-fluid'></div> </nav><center><div><h1 class='text-center'>Password Reset!</h1><p class='text-center'> Simply Click the button showing below to reset your password: </p><br><button style = 'background-color: rgb(0,174,239);'><a href='LINKFORFORGOTPASSWORD' style='text-decoration:none;font-size:15px;color:white;'>Reset Password</a></button></div></center>";
                temp += "<script src = 'https://ajax.googleapis.com/ajax/libs/jquery/3.2.1/jquery.min.js' ></ script ></ body ></ html >";
                string link = "https://localhost:44396/auth/ResetPassword?email=" + email + "&t=" + DateTime.Now.ToString();

                link = link.Replace("+", "%20");
                temp = temp.Replace("LINKFORFORGOTPASSWORD", link);
                msg.Body = temp;

                //following method is used on local server for testing
                using (SmtpClient smt = new SmtpClient())
                {
                    smt.Host = "smtp.gmail.com";
                    System.Net.NetworkCredential ntwd = new NetworkCredential();
                    ntwd.UserName = "nodlaysa@gmail.com"; //Your Email ID
                    ntwd.Password = "Nodlays@123"; // Your Password
                    smt.UseDefaultCredentials = false;
                    smt.Credentials = ntwd;
                    smt.Port = 587;
                    smt.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smt.EnableSsl = true;
                    smt.Send(msg);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public ActionResult PostForgotPassword(string Email = "")
        {
            bool check1 = ValidateEmail(Email);

            if (check1 == true)
            {
                bool check2 = SendResetPasswordEmail(Email);

                if (check2 == true)
                {
                    return RedirectToAction("ForgotPassword", "Auth", new { msg = "Please Check your Inbox", color = "green" });
                }
                else
                {
                    return RedirectToAction("ForgotPassword", "Auth", new { msg = "Mail Sending fail!!", color = "red" });
                }
            }
            else
            {
                return RedirectToAction("ForgotPassword", "Auth", new { msg = "Email Does Not Belong to Any Record!!", color = "red" });
            }

        }


        public ActionResult ResetPassword(string email = "", string msg = "", string color = "Black")
        {
            ViewBag.Email = email;
            ViewBag.Message = msg;
            ViewBag.color = color;
            return View();
        }

        [HttpPost]
        public ActionResult PostResetPassword(string Email = "", string NewPassword = "", string ConfirmPassword = "")
        {
            if (NewPassword != ConfirmPassword)
            {
                return RedirectToAction("ResetPassword", "Auth", new { email = Email, msg = "Password and Confirm Password did not match", color = "red" });
            }
            User getuser = new UserBL().GetActiveUserList(db).Where(x => x.Email == Email).FirstOrDefault();
            getuser.Password = NewPassword;

            //User user = new User()
            //{
            //    Id = getuser.Id,
            //    FirstName = getuser.FirstName,
            //    LastName = getuser.LastName,
            //    Contact = getuser.Contact,
            //    //Address = getuser.Address,
            //    Email = getuser.Email,
            //    Password = NewPassword,
            //    Role = getuser.Role,
            //    IsActive = getuser.IsActive,
            //    CreatedAt = getuser.CreatedAt,
            //};
            bool check2 = new UserBL().UpdateUser(getuser, db);
            if (check2 == true)
            {
                return RedirectToAction("Login", "Auth", new { msg = "Password Reset Successful, Try to Login", color = "green" });
            }
            else
            {
                return RedirectToAction("ResetPassword", "Auth", new { email = Email, msg = "Somethings Wrong!", color = "red" });
            }
        }
    }
}