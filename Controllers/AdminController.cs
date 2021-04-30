using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication_MVC.BL;
using WebApplication_MVC.Models;
using WebApplication_MVC.ViewModel;

namespace WebApplication_MVC.Controllers
{
    public class AdminController : Controller
    {
        private readonly DatabaseEntities db = new DatabaseEntities();

        // GET: Admin
        public ActionResult AdminDashboard(string msg = "")
        {
            try
            {
                ViewBag.msg = msg;
                if (Session["UserID"] != null && (Session["Role"]).ToString() == "1")
                {
                    List<Book> datebook = new BookBL().GetBookList(db);
                    ViewBag.TotalBooks = datebook.Count;
                    List<User> userdata = new UserBL().GetActiveUserListWhereNoAdmin(db);
                    ViewBag.TotalUsers = userdata.Count;
                    return View();
                }
                return RedirectToAction("Login", "Auth", new {msg =" login page"});
            }
            catch
            {
                return RedirectToAction("Error", "Auth", new { msg = "wrong path " });
            }

        }

        public ActionResult AdminProfile(int id, string msg = "")
        {
            try
            {
                ViewBag.msg = msg;
                if (Session["UserID"] != null && (Session["Role"]).ToString() == "1")
                {
                    User admin = new BookBL().GetUserIdForProfileUpdate(db, id);
                    ViewBag.User = admin;
                    return View();
                }
                return RedirectToAction("Login", "Auth", new { msg = "Session over" });
            }
            catch
            {
                return RedirectToAction("Error", "Auth", new { msg = "wrong path " });
            }

        }

        [HttpPost]
        public ActionResult PostAdminProfile(User _user)
        {
            try
            {
                if (Session["UserID"] == null)
                {
                    return RedirectToAction("Login", "Auth", new { msg = " login page " });
                }
                if (Session["Role"].ToString() == "1")
                {
                    if (_user != null)
                    {
                        _user.Role = 1;
                        _user.IsActive = 1;
                        _user.CreatedAt = DateTime.Now;
                        bool checkUser = new UserBL().UpdateUser(_user, db);
                        if (checkUser == true)
                        {
                            Session["UserName"] = _user.FirstName + _user.LastName;
                            return RedirectToAction("AdminDashboard", "Admin", new { msg = "Profile updated successfully" });
                        }
                        else
                        {
                            return RedirectToAction("UserDashBoard", "User", new { msg = "Profile updated Unsuccessfully" });
                        }
                    }
                }
                return RedirectToAction("Login", "Auth", new { msg = "login page " });
            }
            catch
            {
                return RedirectToAction("Error", "Auth", new { msg = "wrong path " });
            }

        }

        public ActionResult AdminViewUser(string msg = "")
        {
            try
            {
                ViewBag.msg = msg;
                if (Session["UserID"] == null)
                {
                    return RedirectToAction("Login", "Auth", new { msg = "Session Expired, plz login again" });
                }
                if (Session["Role"].ToString() == "1")
                {
                    List<User> userslist = new UserBL().GetActiveUserListWhereNoAdmin(db);
                    return View(userslist);
                }

                return RedirectToAction("Login", "Auth", new { msg = "Session Expired, plz login again" });
            }
            catch
            {
                return RedirectToAction("Error", "Auth", new { msg = "wrong path " });
            }
        }

        [HttpPost]
        public ActionResult AdminViewUse(int id, string msg="")
        {
            try
            {
                ViewBag.msg = msg;
                if (Session["UserID"] == null)
                {
                    return RedirectToAction("Login", "Auth", new { msg = "Session over" });
                }
                if (Session["Role"].ToString() == "1")
                {
                    User userlist = new UserBL().PostAdminSearchUser(db, id);
                    return View(userlist);
                }
                return RedirectToAction("Login", "Auth", new { msg = "Session over" });
            }
            catch
            {
                return RedirectToAction("Error", "Auth", new { msg = "wrong path " });
            }

        }

        [HttpGet]
        public ActionResult AdminAddUser(string msg = "")
        {
            try
            {
                ViewBag.msg = msg;
                if (Session["UserID"] == null)
                {
                    return RedirectToAction("Login", "Auth", new { msg = "Session over" });
                }
                return View();
            }
            catch
            {
                return RedirectToAction("Error", "Auth", new { msg = "wrong path " });
            }
        }

        [HttpPost]
        public ActionResult PostAdminAddUser(User _user)
        {
            try
            {
                if (Session["UserID"] == null)
                {
                    return RedirectToAction("Login", "Auth", new { msg = "Session over" });
                }
                int userFirstName = _user.FirstName.Length;
                User check = new UserBL().AdminAddUser(db, _user);
                if (userFirstName > 0 && check == null)
                {
                    if (Session["Role"].ToString() == "1")
                    {
                        _user.Role = 2;
                        _user.IsActive = 1;
                        _user.CreatedAt = DateTime.Now;
                        bool checkUser = new UserBL().AddUser(_user, db);
                        if (checkUser == true)
                        {
                            ViewData["res"] = "visibility:visible;";
                            return View("AdminAddUser");
                        }
                        else
                        {
                            ViewData["nerr"] = "User Record is empty or the email you enter is exist in DataBase";
                            return View("AdminAddUser");
                        }
                    }
                }
                ViewData["nerr"] = "User Record is empty or the email you enter is exist in DataBase";
                return View("AdminAddUser");
            }
            catch
            {
                return RedirectToAction("Error", "Auth", new { msg = "wrong path " });
            }

        }

        [HttpGet]
        public ActionResult AdminEditUser(int id, string msg = "")
        {
            try
            {
                ViewBag.msg = msg;
                if (Session["UserID"] == null)
                {
                    return RedirectToAction("Login", "Auth", new { msg = "login page" });
                }
                if (Session["Role"].ToString() == "1")
                {
                    User empedit = new UserBL().PostAdminSearchUser(db, id);
                    return View(empedit);
                }
                return RedirectToAction("Login", "Auth", new { msg = "login page " });
            }
            catch
            {
                return RedirectToAction("Error", "Auth", new { msg = "wrong path " });
            }

        }

        [HttpPost]
        [ActionName("AdminEditUser")]
        public ActionResult PostAdminEditUser(User _user)
        {
            try
            {
                if (Session["UserID"] == null)
                {
                    return RedirectToAction("Login", "Auth" , new { msg = "login page " });
                }
                int a = _user.FirstName.Length;
                if (a > 0)
                {
                    if (Session["Role"].ToString() == "1")
                    {
                        _user.Role = 2;
                        _user.IsActive = 1;
                        _user.CreatedAt = DateTime.Now;
                        bool checkUser = new UserBL().UpdateUser(_user, db);
                        if (checkUser == true) { return RedirectToAction("AdminViewUser" , new { msg = "view user " }); }
                        ViewData["nerr1"] = "not null";
                        AdminEditUser(_user.Id);
                    }
                }
                ViewData["nerr1"] = "not null";
                AdminEditUser(_user.Id);
                return View();
            }
            catch
            {
                return RedirectToAction("Error", "Auth", new { msg = "wrong path " });
            }

        }

        //[HttpGet]
        //public ActionResult AdminSearchUser(string msg = "")
        //{
        //    try
        //    {
        //        ViewBag.msg = msg;
        //        if (Session["UserID"] == null)
        //        {
        //            return RedirectToAction("Login", "Auth");
        //        }

        //        if (Session["Role"].ToString() == "1")
        //        {
        //            //List<User> userlist = db.Users.Where(x => x.Role != 1).ToList();
        //            List<User> userlist = new UserBL().GetActiveUserListWhereNoAdmin(db);
        //            return View(userlist);
        //        }
        //        return RedirectToAction("Login", "Auth", new { msg = "Session over" });
        //    }
        //    catch
        //    {
        //        return RedirectToAction("Error", "Auth", new { msg = "wrong path " });
        //    }

        //}

        //[HttpPost]
        //public ActionResult PostAdminSearchUser(int id)
        //{
        //    try
        //    {
        //        if (Session["UserID"] == null)
        //        {
        //            return RedirectToAction("Login", "Auth", new { msg = "Session over" });
        //        }
        //        if (Session["Role"].ToString() == "1")
        //        {
        //            User userlist = new UserBL().PostAdminSearchUser(db, id);
        //            return View(userlist);
        //        }
        //        return RedirectToAction("Login", "Auth", new { msg = "Session over" });
        //    }
        //    catch
        //    {
        //        return RedirectToAction("Error", "Auth", new { msg = "wrong path " });
        //    }

        //}

        public ActionResult AdminDeleteUser(int id)
        {
            try
            {
                if (Session["UserID"] == null)
                {
                    RedirectToAction("Login", "Auth", new { msg = "Session over" });
                }
                if (Session["Role"].ToString() == "1")
                {
                    User user = new UserBL().GetUserId(db, id);
                    bool checkUser = new UserBL().DeleteUser(user, db);
                    if (checkUser == true)
                    {
                        return RedirectToAction("AdminViewUser", new { msg = "delete User" });
                    }
                    else
                    {
                        return RedirectToAction("Login", "Auth", new { msg = "Wrong Operation" });
                    }
                }
                return RedirectToAction("Login", "Auth", new { msg = "Session over" });
            }
            catch
            {
                return RedirectToAction("Error", "Auth", new { msg = "wrong path " });
            }

        }

        //public ActionResult AdminViewUserBook()
        //{
        //    try
        //    {
        //        if (Session["UserID"] == null)
        //        {
        //            return RedirectToAction("Login", "Auth", new { msg = "Session Expired, plz login again" });
        //        }

        //        if ((Session["Role"]).ToString() == "1")
        //        {
        //            List<Book> usersbBookslist = new BookBL().GetBookList(db);
        //            ViewBag.BookList = usersbBookslist;
        //            List<BookViewModel> data = new List<BookViewModel>();
        //            foreach (var book in usersbBookslist)
        //            {
        //                data.Add(new BookViewModel()
        //                {
        //                    UserId = book.UserId ?? 0,
        //                    CreatedAt = book.CreatedAt,
        //                    Auther = book.Auther,
        //                    Id = book.Id,
        //                    IsActive = book.IsActive,
        //                    Title = book.Title
        //                });
        //            }
        //            var i = Convert.ToInt32(Session["UserID"]);
        //            List<BookViewModel> viewdata = data.Where(x => x.UserId == Convert.ToInt32(Session["UserID"])).ToList();
        //            //List<Book>viewdata = new BookBL().GetOnlyUserBookList(db, i);
        //            return View(viewdata);
        //        }
        //        return RedirectToAction("Login", "Auth", new { msg = "Session Expired, plz login again" });
        //    }
        //    catch
        //    {
        //        return RedirectToAction("Error", "Auth", new { msg = "wrong path " });
        //    }

        //}
        public ActionResult AdminViewUserBook()
        {
            try
            {
                if (Session["UserID"] == null)
                {
                    return RedirectToAction("Login", "Auth", new { msg = "Session Expired, plz login again" });
                }

                if ((Session["Role"]).ToString() == "1")
                {
                    List<Book> usersbBookslist = db.Books.Where(x=> x.IsActive ==1).ToList();
                    ViewBag.BookList = usersbBookslist;
                    var data = new List<BookViewModel>();
                    foreach (var book in usersbBookslist)
                    {
                        data.Add(new BookViewModel()
                        {
                            UserId = book.UserId ?? 0,
                            CreatedAt = book.CreatedAt,
                            Auther = book.Auther,
                            Id = book.Id,
                            IsActive = book.IsActive,
                            Title = book.Title
                        });
                    }
                    List<BookViewModel> viewdata = data.Where(x => x.UserId == Convert.ToInt32(Session["UserID"]) && x.IsActive == 1).ToList();
                    return View(viewdata);
                }
                return RedirectToAction("Login", "Auth", new { msg = "Session Expired, plz login again" });
            }
            catch 
            {
                 return RedirectToAction("Error", "Auth", new { msg = "wrong path " });
            }

        }
    }
}