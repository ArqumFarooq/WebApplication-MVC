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
                return RedirectToAction("Login", "Auth", new { msg = "Session Expired from ADMIN DashBoard, plz login again" });
            }
            catch
            {
                return RedirectToAction("Error", "Auth", new { msg = "search for the page was not found" });
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
                return RedirectToAction("Login", "Auth", new { msg = "Session Expired from ADMIN DashBoard, plz login again" });
            }
            catch
            {
                return RedirectToAction("Error", "Auth", new { msg = "search for the page was not found" });
            }

        }

        [HttpPost]
        public ActionResult PostAdminProfile(User _user)
        {
            try
            {
                if (Session["UserID"] == null)
                {
                    return RedirectToAction("Login", "Auth", new { msg = " Session Expired from ADMIN DashBoard, plz login again" });
                }
                if (Session["Role"].ToString() == "1")
                {
                    if (_user != null)
                    {
                        int emailCount = new UserBL().GetActiveUserList(db).Where(x=> x.Email.ToLower() == _user.Email.ToLower() && x.Id != _user.Id).Count();
                        if (emailCount == 0)
                        {
                            User updateAdmin = new UserBL().GetUserById(_user.Id, db);
                            updateAdmin.FirstName = _user.FirstName.Trim();
                            updateAdmin.LastName = _user.LastName.Trim();
                            updateAdmin.Contact = _user.Contact.Trim();
                            updateAdmin.Password = _user.Password.Trim();
                            updateAdmin.Email = _user.Email.Trim();
                            bool checkUser = new UserBL().UpdateUser(updateAdmin, db);
                            if (checkUser == true)
                            {
                                Session["UserName"] = _user.FirstName + _user.LastName;
                                return RedirectToAction("AdminDashboard", "Admin", new { msg = "Profile updated successfully" });
                            }
                            else
                            {
                                return RedirectToAction("AdminDashboard", "Admin", new { msg = "Profile updated Unsuccessfully bcz yoy enter empty name or maybe same email which exist in DB" });
                            }
                        }
                        else
                        {
                            return RedirectToAction("AdminDashboard", "Admin", new { msg = "Profile updated Unsuccessfully either you enter the existent email" });
                        }
                    }
                }
                return RedirectToAction("Login", "Auth", new { msg = "Session Expired from ADMIN DashBoard, plz login again " });
            }
            catch
            {
                return RedirectToAction("Error", "Auth", new { msg = "search for the page was not found " });
            }

        }

        public ActionResult AdminViewUser(string msg = "", int id = -1)
        {
            try
            {
                ViewBag.msg = msg;
                if (Session["UserID"] == null)
                {
                    return RedirectToAction("Login", "Auth", new { msg = "Session Expired from ADMIN DashBoard, plz login again" });
                }
                if (Session["Role"].ToString() == "1")
                {
                    if (id == -1)
                    {
                        List<User> userslist = new UserBL().GetActiveUserListWhereNoAdmin(db);
                        ViewBag.allUser = userslist;
                        return View(userslist);
                    }
                    else
                    {
                        IEnumerable<User> userslist = new UserBL().GetActiveUserListWhereNoAdmin(db).Where(x => x.Id == id);
                        List<User> allUserslist = new UserBL().GetActiveUserListWhereNoAdmin(db); ;
                        ViewBag.allUser = allUserslist;
                        return View(userslist);
                    }
                }

                return RedirectToAction("Login", "Auth", new { msg = "Session Expired from ADMIN DashBoard, plz login again" });
            }
            catch
            {
                return RedirectToAction("Error", "Auth", new { msg = "search for the page was not found" });
            }
        }

        [HttpPost]
        public ActionResult AdminViewUse(int id, string msg = "")
        {
            try
            {
                ViewBag.msg = msg;
                if (Session["UserID"] == null)
                {
                    return RedirectToAction("Login", "Auth", new { msg = "Session Expired from ADMIN DashBoard, plz login again" });
                }
                if (Session["Role"].ToString() == "1")
                {
                    User userlist = new UserBL().PostAdminSearchUser(db, id);
                    return View(userlist);
                }
                return RedirectToAction("Login", "Auth", new { msg = "Session Expired from ADMIN DashBoard, plz login again" });
            }
            catch
            {
                return RedirectToAction("Error", "Auth", new { msg = "search for the page was not found" });
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
                    return RedirectToAction("Login", "Auth", new { msg = "Session Expired from ADMIN DashBoard, plz login again" });
                }
                return View();
            }
            catch
            {
                return RedirectToAction("Error", "Auth", new { msg = "search for the page was not found" });
            }
        }

        [HttpPost]
        public ActionResult PostAdminAddUser(User _user)
        {
            try
            {
                if (Session["UserID"] == null)
                {
                    return RedirectToAction("Login", "Auth", new { msg = "Session Expired from ADMIN DashBoard, plz login again" });
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
                return RedirectToAction("Error", "Auth", new { msg = "search for the page was not found" });
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
                    return RedirectToAction("Login", "Auth", new { msg = "Session Expired from ADMIN DashBoard, plz login again" });
                }
                if (Session["Role"].ToString() == "1")
                {
                    User empedit = new UserBL().PostAdminSearchUser(db, id);
                    return View(empedit);
                }
                return RedirectToAction("Login", "Auth", new { msg = "Session Expired from ADMIN DashBoard, plz login again " });
            }
            catch
            {
                return RedirectToAction("Error", "Auth", new { msg = "search for the page was not found " });
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
                    return RedirectToAction("Login", "Auth", new { msg = "Session Expired from ADMIN DashBoard, plz login again" });
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
                        if (checkUser == true) { return RedirectToAction("AdminViewUser", new { msg = "User has been Edited" }); }
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
                return RedirectToAction("Error", "Auth", new { msg = "search for the page was not found " });
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
                    RedirectToAction("Login", "Auth", new { msg = "Session Expired from ADMIN DashBoard, plz login again" });
                }
                if (Session["Role"].ToString() == "1")
                {
                    User user = new UserBL().GetUserId(db, id);
                    //List<Book> usersbBookslist = db.Books.Where(x => x.IsActive == 1 && x.UserId == id).ToList();
                    List<Book> usersbBookslist =  new BookBL().GetBookList(db).Where(x=> x.UserId == id).ToList();
                    ViewBag.BookList = usersbBookslist;
                    List<BookViewModel> data = new List<BookViewModel>();
                    foreach (Book book in usersbBookslist)
                    {

                        book.UserId = book.UserId;
                        book.CreatedAt = book.CreatedAt;
                        book.Auther = book.Auther;
                        book.Id = book.Id;
                        book.IsActive = 0;
                        book.Title = book.Title;
                        new BookBL().DeleteBook(book, db);
                    }
                    bool checkUser = new UserBL().DeleteUser(user, db);
                    if (checkUser == true)
                    {
                        return RedirectToAction("AdminViewUser", new { msg = " User has been deleted" });
                    }
                    else
                    {
                        return RedirectToAction("Login", "Auth", new { msg = "Wrong Operation OR Session Expired from ADMIN DashBoard, plz login again" });
                    }
                }
                return RedirectToAction("Login", "Auth", new { msg = "Session Expired from ADMIN DashBoard, plz login again" });
            }
            catch
            {
                return RedirectToAction("Error", "Auth", new { msg = "search for the page was not found" });
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
        public ActionResult AdminViewUserBook(int id = -1)
        {
            try
            {
                if (Session["UserID"] == null)
                {
                    return RedirectToAction("Login", "Auth", new { msg = "Session Expired from ADMIN DashBoard, plz login again" });
                }

                if ((Session["Role"]).ToString() == "1")
                {
                    if (id == -1)
                    {
                        //List<Book> usersbBookslist = db.Books.Where(x => x.IsActive == 1).ToList();
                        List<Book> usersbBookslist =  new BookBL().GetBookList(db);
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
                    else
                    {
                        //List<Book> usersbBookslist = db.Books.Where(x => x.IsActive == 1 && x.UserId == id).ToList();
                        List<Book> usersbBookslist = new BookBL().GetBookList(db).Where(x => x.UserId == id).ToList();
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
                        //List<BookViewModel> viewdata = data.Where(x => x.UserId == Convert.ToInt32(Session["UserID"]) && x.IsActive == 1 && x.Id == id).ToList();
                        List<BookViewModel> viewdata = data.Where(x => x.IsActive == 1).ToList();
                        return View(viewdata);
                    }
                }
                return RedirectToAction("Login", "Auth", new { msg = "Session Expired from ADMIN DashBoard, plz login again" });
            }
            catch
            {
                return RedirectToAction("Error", "Auth", new { msg = "search for the page was not found " });
            }

        }
    }
}