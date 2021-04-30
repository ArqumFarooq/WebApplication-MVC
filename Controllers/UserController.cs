using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication_MVC.BL;
using WebApplication_MVC.Models;
using WebApplication_MVC.ViewModel;

namespace WebApplication_MVC.Controllers
{
    public class UserController : Controller
    {
        private readonly DatabaseEntities db = new DatabaseEntities();

        // GET: User
        public ActionResult UserDashBoard(string msg = "")
        {
            try
            {
                ViewBag.msg = msg;
                if (Session["UserID"] != null && (Session["Role"]).ToString() == "2")
                {
                    //int bookCount  = new BookBL().GetBookList(db).Count();
                    List<Book> userslist = new BookBL().GetBookList(db);
                    List<BookViewModel> data = new List<BookViewModel>();
                    foreach (var book in userslist)
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
                    var i = Convert.ToInt32(Session["UserID"]);
                    //List<BookViewModel> viewdata = data.Where(x => x.UserId == Convert.ToInt32(Session["UserID"])).ToList();
                    List<BookViewModel> viewdata = new BookBL().GetOnlyUserBookList(data, i);
                    ViewBag.TotalBooks = viewdata.Count;
                    return View(data);
                }
                return RedirectToAction("Login", "Auth", new { msg = "Session Expired, plz login again" });
            }
            catch
            {
                return RedirectToAction("Error", "Auth", new { msg = "wrong path " });
            }

        }

        public ActionResult UserProfile(int id, string msg = "")
        {
            try
            {
                ViewBag.msg = msg;
                if (Session["UserID"] != null && (Session["Role"]).ToString() == "2")
                {
                    User user = new BookBL().GetUserIdForProfileUpdate(db, id);
                    ViewBag.User = user;
                    return View();
                }
                return RedirectToAction("Login", "Auth", new { msg = "Session Expired, plz login again" });
            }
            catch
            {
                return RedirectToAction("Error", "Auth", new { msg = "wrong path " });
            }

        }

        [HttpPost]
        public ActionResult PostUserProfile(User _user)
        {
            try
            {
                if (Session["UserID"] == null)
                {
                    RedirectToAction("Login", "Auth", new { msg = "Session Expired, plz login again" });
                }
                if (Session["Role"].ToString() == "2")
                {
                    _user.Role = 2;
                    _user.IsActive = 1;
                    _user.CreatedAt = DateTime.Now;
                    bool checkUser = new UserBL().UpdateUser(_user, db);
                    if (checkUser == true)
                    {
                        Session["UserName"] = _user.FirstName + _user.LastName;
                        return RedirectToAction("UserDashBoard", "User", new { msg = "Profile updated successfully" });
                    }
                    else
                    {
                        return RedirectToAction("UserDashBoard", "User", new { msg = "Profile updated Unsuccessfully" });
                    }
                }
                return RedirectToAction("Login", "Auth", new { msg = "Session Expired, plz login again" });
            }
            catch
            {
                return RedirectToAction("Error", "Auth", new { msg = "wrong path " });
            }
            
        }

        public ActionResult UserViewBook(String msg = "")
            {
            try
            {
                ViewBag.msg = msg;
                if (Session["UserID"] == null)
                {
                    return RedirectToAction("Login", "Auth", new { msg = "Session Expired, plz login again" });
                }
                if (Session["Role"].ToString() == "2")
                {
                    List<Book> userslist = new BookBL().GetBookList(db);
                    List<BookViewModel> data = new List<BookViewModel>();
                    foreach (var book in userslist)
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
                    var i = Convert.ToInt32(Session["UserID"]);
                    List<BookViewModel> viewdata = new BookBL().GetOnlyUserBookList(data, i);
                    return View(viewdata);
                }
                return RedirectToAction("Login", "Auth", new { msg = "Session Expired, plz login again" });
            }
            catch
            {
                return RedirectToAction("Error", "Auth", new { msg = "wrong path " });
            }

        }
        
        [HttpPost]
        public ActionResult UseViewBook(int id)
        {
            try
            {
                if (Session["UserID"] == null)
                {
                    return RedirectToAction("Login", "Auth", new { msg = "Session Expired, plz login again" });
                }

                if (Session["Role"].ToString() == "2")
                {
                    Book booklist = new BookBL().GetUserIdForSearchBook(db, id);
                    return View(booklist);
                }
                return RedirectToAction("Login", "Auth", new { msg = "the session is over or maybe you access other user book" });
            }
            catch
            {
                return RedirectToAction("Error", "Auth", new { msg = "wrong path " });
            }

        }

        [HttpGet]
        public ActionResult UserAddBook(string msg = "")
        {
            try
            {
                ViewBag.msg = msg;
                if (Session["UserID"] == null)
                {
                    return RedirectToAction("Login", "Auth", new { msg = "Session Expired, plz login again" });
                }
                return View();
            }
            catch
            {
                return RedirectToAction("Error", "Auth", new { msg = "wrong path " });
            }

        }

        [HttpPost]
        public ActionResult PostUserAddBook(Book _book)
        {
            try
            {
                if (Session["UserID"] == null)
                {
                    return RedirectToAction("Login", "Auth", new { msg = "Session Expired, plz login again" });
                }
                int bookTitletName = _book.Title.Length;
                if (bookTitletName > 0)
                {
                    if (Session["Role"].ToString() == "2")
                    {
                        _book.UserId = Convert.ToInt32(Session["UserID"]);
                        _book.IsActive = 1;
                        _book.CreatedAt = DateTime.Now;
                        bool checkUser = new BookBL().AddBook(_book, db);
                        if (checkUser == true)
                        {
                            ViewData["res"] = "visibility:visible;";
                            return View("UserAddBook");
                        }
                        else
                        {
                            ViewData["nerr"] = "User Record is empty";
                        }
                    }
                }
                else
                {
                    ViewData["nerr"] = "User Record is empty";
                }
                return View("UserAddBook");
            }
            catch
            {
                return RedirectToAction("Error", "Auth", new { msg = "wrong path " });
            }
        }

        [HttpGet]
        public ActionResult UserEditBook(int id, string msg = "")
        {
            try
            {
                ViewBag.msg = msg;
                if (Session["UserID"] == null)
                {
                    return RedirectToAction("Login", "Auth", new { msg = "Session Expired, plz login again" });
                }
                if (Session["Role"].ToString() == "2")
                {
                    Book bookedit = new BookBL().GetUserIdForSearchBook(db, id);
                    return View(bookedit);
                }
                return RedirectToAction("Login", "Auth", new { msg = "Session Expired, plz login again" });
            }
            catch
            {
                return RedirectToAction("Error", "Auth", new { msg = "wrong path " });
            }

        }

        [HttpPost]
        public ActionResult PostUserEditBook(Book _book)
        {
            try
            {
                if (Session["UserID"] == null)
                {
                    return RedirectToAction("Login", "Auth", new { msg = "Session Expired, plz login again" });
                }
                int titleLength = _book.Title.Length;
                int authorLength = _book.Auther.Length;
                if (titleLength >= 0 && authorLength >= 0)
                {
                    if (Session["Role"].ToString() == "2")
                    {
                        _book.UserId = Convert.ToInt32(Session["UserID"]);
                        _book.IsActive = 1;
                        _book.CreatedAt = DateTime.Now;

                        bool checkUser = new BookBL().UpdateBook(_book, db);
                        if (checkUser == true)
                        {
                            return RedirectToAction("UserViewBook", "User", new { msg = "Edit Successful" });
                        }
                        else
                        {
                            ViewData["nerr1"] = "not null";
                            UserEditBook(_book.Id);
                        }
                    }
                }
                else
                {
                    ViewData["nerr1"] = "not null";
                    UserEditBook(_book.Id);
                }
                return View("UserEditBook");
            }
            catch
            {
                return RedirectToAction("Error", "Auth", new { msg = "wrong path " });
            }
            
        }

        //[HttpGet]
        //public ActionResult UserSearchBook(string msg = "")
        //{
        //    try
        //    {
        //        ViewBag.msg = msg;
        //        if (Session["UserID"] == null)
        //        {
        //            return RedirectToAction("Login", "Auth", new { msg = "Session Expired, plz login again" });
        //        }
        //        if (Session["Role"].ToString() == "2")
        //        {
        //            List<Book> userslist = new BookBL().GetBookList(db);
        //            List<BookViewModel> data = new List<BookViewModel>();
        //            foreach (var book in userslist)
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
        //            //List<BookViewModel> viewdata = data.Where(x => x.UserId == Convert.ToInt32(Session["UserID"])).ToList();
        //            List<BookViewModel> viewdata = new BookBL().GetOnlyUserBookList(data, i);
        //            return View(viewdata);
        //        }
        //        return RedirectToAction("Login", "Auth", new { msg = "Session Expired, plz login again" });
        //    }
        //    catch
        //    {
        //        return RedirectToAction("Error", "Auth", new { msg = "wrong path " });
        //    }

        //}

        //[HttpPost]
        //public ActionResult PostUserSearchBook(int id)
        //{
        //    try
        //    {
        //        if (Session["UserID"] == null)
        //        {
        //            return RedirectToAction("Login", "Auth", new { msg = "Session Expired, plz login again" });
        //        }

        //        if (Session["Role"].ToString() == "2")
        //        {
        //            Book booklist = new BookBL().GetUserIdForSearchBook(db, id);
        //            return View(booklist);
        //        }
        //        return RedirectToAction("Login", "Auth", new { msg = "the session is over or maybe you access other user book" });
        //    }
        //    catch
        //    {
        //        return RedirectToAction("Error", "Auth", new { msg = "wrong path " });
        //    }
           
        //}

        public ActionResult UserDeleteBook(int id)
        {
            try
            {
                if (Session["UserID"] == null)
                {
                    return RedirectToAction("Login", "Auth", new { msg = "Session Expired, plz login again" });
                }
                if (Session["Role"].ToString() == "2")
                {
                    Book book = new BookBL().GetBookId(db, id);
                    bool checkUser = new BookBL().DeleteBook(book, db);
                    if (checkUser == true)
                    {
                        return RedirectToAction("UserViewBook", new{msg = "delete Book"});
                    }
                    else
                    {
                        return RedirectToAction("Login", "Auth", new { msg = "Wrong Operation" });
                    }
                }
                return RedirectToAction("Login", "Auth", new { msg = "Session Expired, plz login again" });
            }
            catch
            {
                return RedirectToAction("Error", "Auth", new { msg = "wrong path " });
            }

        }

        public ActionResult UserDeleteAllBook()
        {
            try
            {
                if (Session["UserID"] == null)
                {
                    return RedirectToAction("Login", "Auth", new { msg = "Session Expired, plz login again" });
                }

                if (Session["Role"].ToString() == "2")
                {
                    db.Books.RemoveRange(new BookBL().GetBookList(db));
                    db.SaveChanges();
                    return View("UserDashBoard");
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