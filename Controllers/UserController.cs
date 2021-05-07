using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication_MVC.BL;
using WebApplication_MVC.Models;
using WebApplication_MVC.Helping_Classes;


namespace WebApplication_MVC.Controllers
{
    public class UserController : Controller
    {
        SessionDTO sdto = new SessionDTO();
        GeneralPurpose gp = new GeneralPurpose();
        private readonly DatabaseEntities db = new DatabaseEntities();

        public bool ValidateLogin()
        {
            if (sdto.getEmail() != null)
            {
                if (sdto.getRole() == 2)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        // GET: User
        public ActionResult UserDashBoard(string msg = "", string color = "black")
        {
            try
            {
                if (ValidateLogin() == false)
                {
                    return RedirectToAction("Login", "Auth", new { msg = "Session Expired from User DashBoard, plz login again", color = "Red" });
                }

                int bookCount = new BookBL().GetBookListByUserId( sdto.getId(), db).Count();
                
                ViewBag.BookCount = bookCount;
                ViewBag.msg = msg;
                ViewBag.color = color;
                return View();
            }
            catch
            {
                return RedirectToAction("Error", "Auth", new { msg = "search for the page was not found", color = "Red" });
            }

        }

        public ActionResult UserProfile(int id, string msg = "", string color = "black")
        {
            try
            {
                if (ValidateLogin() == false)
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
        public ActionResult PostUserProfile(User _user)
        {
            try
            {
                if (ValidateLogin() == false)
                {
                    return RedirectToAction("Login", "Auth", new { msg = "Session Expired from User DashBoard, plz login again", color = "Red" });
                }
                bool chkEmail = gp.ValidateEmail(_user.Email, _user.Id);
                if (chkEmail == false)
                {
                    return RedirectToAction("UserProfile", "User", new { id = _user.Id, msg = "Emaial exists", color = "Red" });
                }
                User updateUser = new UserBL().GetUserById(_user.Id, db);
                updateUser.FirstName = _user.FirstName.Trim();
                updateUser.LastName = _user.LastName.Trim();
                updateUser.Contact = _user.Contact.Trim();
                updateUser.Password = _user.Password.Trim();
                updateUser.Email = _user.Email.Trim();
                bool checkUser = new UserBL().UpdateUser(updateUser, db);
                if (checkUser == true)
                {
                    return RedirectToAction("UserDashBoard", "User", new { msg = "Profile updated successfully", color = "green" });
                }
                else
                {
                    return RedirectToAction("UserDashBoard", "User", new { msg = "Somethings' Wrong", color = "Red" });
                }
            }
            catch
            {
                return RedirectToAction("Error", "Auth", new { msg = "search for the page was not found", color = "Red" });
            }

        }

        public ActionResult UserViewBook(String msg = "", string color = "black", string title = "", string author = "")
        {
            try
            {
                if (ValidateLogin() == false)
                {
                    return RedirectToAction("Login", "Auth", new { msg = "Session Expired from User DashBoard, plz login again", color = "Red" });
                }

                List<Book> booklist = new BookBL().GetBookListByUserId( sdto.getId(), db).ToList(); 
                if(title != "")
                {
                    booklist = booklist.Where(x => x.Title.ToLower().Contains(title.ToLower())).ToList();
                }
                if(author != "")
                {
                    booklist = booklist.Where(x => x.Auther.ToLower().Contains(author.ToLower())).ToList();
                }

                ViewBag.BookList = booklist;
                ViewBag.msg = msg;
                ViewBag.color = color;
                ViewBag.title = title;
                ViewBag.author = author;
                return View();
            }
            catch
            {
                return RedirectToAction("Error", "Auth", new { msg = "search for the page was not found", color = "Red" });
            }
        }

        public ActionResult UserAddBook(string msg = "", string color = "black")
        {
            try
            {
                if (ValidateLogin() == false)
                {
                    return RedirectToAction("Login", "Auth", new { msg = "Session Expired from User DashBoard, plz login again", color = "Red" });
                }

                ViewBag.msg = msg;
                ViewBag.color = color;
                return View();
            }
            catch
            {
                return RedirectToAction("Error", "Auth", new { msg = "search for the page was not found", color = "Red" });
            }

        }

        [HttpPost]
        public ActionResult PostUserAddBook(Book _book)
        {
            try
            {
                if (ValidateLogin() == false)
                {
                    return RedirectToAction("Login", "Auth", new { msg = "Session Expired from User DashBoard, plz login again", color = "Red" });
                }
                
                Book obj = new Book()
                {
                    Title = _book.Title,
                    Auther = _book.Auther,
                    UserId = Convert.ToInt32(sdto.getId()),
                    IsActive = 1,
                    CreatedAt = DateTime.Now
                };
                

                bool checkUser = new BookBL().AddBook(obj, db);
                if (checkUser == true)
                {
                    return RedirectToAction("UserAddBook", "User", new { msg = "Book inserted Successfully", color = "green" });

                }
                else
                {
                    return RedirectToAction("UserAddBook", "User", new { msg = "Somethings' Wrong", color = "Red" });
                }
            }
            catch
            {
                return RedirectToAction("Error", "Auth", new { msg = "search for the page was not found", color = "Red" });
            }
        }

        public ActionResult UserEditBook(int id, string msg = "", string color = "black")
        {
            try
            {
                if (ValidateLogin() == false)
                {
                    return RedirectToAction("Login", "Auth", new { msg = "Session Expired from User DashBoard, plz login again", color = "Red" });
                }

                Book bookedit = new BookBL().GetBookById( id, db);
                ViewBag.color = color;
                ViewBag.msg = msg;
                ViewBag.Book = bookedit;
                return View();
                
            }
            catch
            {
                return RedirectToAction("Error", "Auth", new { msg = "search for the page was not found", color = "Red" });
            }

        }

        [HttpPost]
        public ActionResult PostUserEditBook(Book _book)
        {
            try
            {
                if (ValidateLogin() == false)
                {
                    return RedirectToAction("Login", "Auth", new { msg = "Session Expired from User DashBoard, plz login again", color = "Red" });
                }
                Book b = new BookBL().GetBookById(_book.Id, db);
                b.Auther = _book.Auther;
                b.Title = _book.Title;


                bool checkUser = new BookBL().UpdateBook(b, db);
                if (checkUser == true)
                {
                    return RedirectToAction("UserViewBook", "User", new { msg = "Book has been Edited Successful", color = "green" });
                }
                else
                {
                    return RedirectToAction("UserViewBook", "User", new { msg = "Somethings' Wrong", color = "Red" });
                }
            }
            catch
            {
                return RedirectToAction("Error", "Auth", new { msg = "search for the page was not found", color = "Red" });
            }

        }

        public ActionResult UserDeleteBook(int id)
        {
            try
            {
                if (ValidateLogin() == false)
                {
                    return RedirectToAction("Login", "Auth", new { msg = "Session Expired from User DashBoard, plz login again", color = "Red" });
                }
                Book b = new BookBL().GetBookById(id,db);
                b.IsActive = 0;


                bool checkUser = new BookBL().UpdateBook(b, db);
                if (checkUser == true)
                {
                    return RedirectToAction("UserViewBook", "User", new { msg = "Book has been deleted Successful", color = "green" });
                }
                else
                {
                    return RedirectToAction("UserViewBook", "User", new { msg = "Somethings' Wrong", color = "Red" });
                }
            }
            catch
            {
                return RedirectToAction("Error", "Auth", new { msg = "search for the page was not found ", color = "Red" });
            }

        }

    }
}