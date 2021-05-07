using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using WebApplication_MVC.BL;
using WebApplication_MVC.Models;
using WebApplication_MVC.Helping_Classes;

namespace WebApplication_MVC.Controllers
{
    public class AdminController : Controller
    {
        SessionDTO sdto = new SessionDTO();
        GeneralPurpose gp = new GeneralPurpose();
        private readonly DatabaseEntities db = new DatabaseEntities();

        public bool ValidateLogin()
        {
            if(sdto.getEmail() != null)
            {
                if(sdto.getRole() == 1)
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

        // GET: Admin
        public ActionResult AdminDashboard(string msg = "", string color = "black")
        {
            try
            {
                if(ValidateLogin() == false)
                {
                    return RedirectToAction("Login", "Auth", new { msg = "Session Expired from ADMIN DashBoard, plz login again", color = "Red" });
                }

                int userCount = new UserBL().GetActiveUserList(db).Where(x=> x.Role != 1).Count();
                int bookCount = new BookBL().GetActiveBookList(db).Count();
                
                ViewBag.TotalUsers = userCount;
                ViewBag.TotalBooks = bookCount;
                ViewBag.msg = msg;
                ViewBag.color = color;
                return View();
            }
            catch
            {
                return RedirectToAction("Error", "Auth", new { msg = "search for the page was not found", color = "Red" });
            }

        }

        public ActionResult AdminProfile(int id, string msg = "", string color = "black")
        {
            try
            {
                if (ValidateLogin() == false)
                {
                    return RedirectToAction("Login", "Auth", new { msg = "Session Expired from ADMIN DashBoard, plz login again", color = "Red" });
                }
                
                User admin = new UserBL().GetUserById(id, db);
                ViewBag.User = admin;
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
        public ActionResult PostAdminProfile(User _user)
        {
            try
            {
                if (ValidateLogin() == false)
                {
                    return RedirectToAction("Login", "Auth", new { msg = "Session Expired from ADMIN DashBoard, plz login again", color = "Red" });
                }
                bool chkEmail = gp.ValidateEmail(_user.Email, _user.Id);
                if (chkEmail == false)
                {
                    return RedirectToAction("AdminProfile", "Admin", new { id = _user.Id, msg = "Emaial exists", color = "Red" });
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
                    return RedirectToAction("AdminDashboard", "Admin", new { msg = "Profile updated successfully", color = "green" });
                }
                else
                {
                    return RedirectToAction("AdminDashboard", "Admin", new { msg = "Profile updated Unsuccessfully bcz yoy enter empty name or maybe same email which exist in DB", color = "Red" });
                }
                
            }
            catch
            {
                return RedirectToAction("Error", "Auth", new { msg = "search for the page was not found", color = "Red" });
            }

        }

        public ActionResult AdminViewUser(string msg = "", string color = "black", string Name = "", string Email="", string Contact="") {
            try
            {
                if (ValidateLogin() == false)
                {
                    return RedirectToAction("Login", "Auth", new { msg = "Session Expired from ADMIN DashBoard, plz login again", color = "Red" });
                }

                List<User> userslist = new UserBL().GetActiveUserList(db).Where( x=> x.Role != 1).ToList();
                int bookCount = new BookBL().GetActiveBookList(db).Count();
                if (Name != "")
                {
                    userslist = userslist.Where(x => x.FirstName.ToLower().Contains(Name.ToLower()) || x.LastName.ToLower().Contains(Name.ToLower())).ToList();
                }
                if (Email != "")
                {
                    userslist = userslist.Where(x => x.Email.ToLower().Contains(Email.ToLower())).ToList();
                }
                if (Contact != "")
                {
                    userslist = userslist.Where(x => x.Contact.ToLower().Contains(Contact.ToLower())).ToList();
                }
           
                ViewBag.msg = msg;
                ViewBag.color = color;
                ViewBag.userlist = userslist;
                ViewBag.Name = Name;
                ViewBag.Email = Email;
                ViewBag.Contact = Contact;

                return View();
            }
            catch
            {
                return RedirectToAction("Error", "Auth", new { msg = "search for the page was not found", color = "Red" });
            }
        }

        public ActionResult AdminAddUser(string msg = "", string color = "black")
        {
            try
            {
                if (ValidateLogin() == false)
                {
                    return RedirectToAction("Login", "Auth", new { msg = "Session Expired from ADMIN DashBoard, plz login again", color = "Red" });
                }
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
        public ActionResult PostAdminAddUser(User _user)
        {
            try
            {
                if (ValidateLogin() == false)
                {
                    return RedirectToAction("Login", "Auth", new { msg = "Session Expired from ADMIN DashBoard, plz login again", color = "Red" });
                }

                bool chkEmail = gp.ValidateEmail(_user.Email);
                if (chkEmail == false)
                {
                    return RedirectToAction("AdminAddUser", "Admin", new { msg = "Emaial exists", color = "Red" });
                }

                User addAdmin = new User();
                addAdmin.FirstName = _user.FirstName.Trim();
                addAdmin.LastName = _user.LastName.Trim();
                addAdmin.Contact = _user.Contact.Trim();
                addAdmin.Password = _user.Password.Trim();
                addAdmin.Email = _user.Email.Trim();
                addAdmin.Role = 2;
                addAdmin.IsActive = 1;
                addAdmin.CreatedAt = DateTime.Now;

                bool checkUser = new UserBL().AddUser(addAdmin, db);
                if (checkUser == true)
                {
                    return RedirectToAction("AdminAddUser", "Admin", new { msg = "The user has been added successfully", color = "green" });
                }
                else
                {
                    return RedirectToAction("AdminAddUser", "Admin", new { msg = "Somethings' Wrong", color = "Red" });
                }
            }
            catch
            {
                return RedirectToAction("Error", "Auth", new { msg = "search for the page was not found", color = "Red" });
            }

        }

        public ActionResult AdminEditUser(int id, string msg = "", string color = "black")
        {
            try
            {
                if (ValidateLogin() == false)
                {
                    return RedirectToAction("Login", "Auth", new { msg = "Session Expired from ADMIN DashBoard, plz login again", color = "Red" });
                }

                User empedit = new UserBL().GetUserById(id, db);

                ViewBag.user = empedit;
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
        public ActionResult PostAdminEditUser(User _user)
        {
            try
            {
                if (ValidateLogin() == false)
                {
                    return RedirectToAction("Login", "Auth", new { msg = "Session Expired from ADMIN DashBoard, plz login again", color = "Red" });
                }

                bool chkEmail = gp.ValidateEmail(_user.Email, _user.Id);
                if (chkEmail == false)
                {
                    return RedirectToAction("AdminEditUser", "Admin", new { msg = "Emaial exists", color = "Red" });
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
                    return RedirectToAction("AdminViewUser", new { msg = "User has been Edited", color = "green" });
                }
                else
                {
                    return RedirectToAction("AdminEditUser", new { msg = "User has not been Edited cuz either you enter any empty/Null Value or Email you enter exist in DB", color = "Red" });
                }
            }
            catch
            {
                return RedirectToAction("Error", "Auth", new { msg = "search for the page was not found ", color = "Red" });
            }

        }

        public ActionResult AdminDeleteUser(int id)
        {
            try
            {
                if (ValidateLogin() == false)
                {
                    return RedirectToAction("Login", "Auth", new { msg = "Session Expired from ADMIN DashBoard, plz login again", color = "Red" });
                }

                User user = new UserBL().GetUserById(id, db);
                user.IsActive = 0;
                
                List<Book> Bookslist = new BookBL().GetActiveBookList(db).Where(x => x.UserId == id).ToList();
                
                foreach (Book book in Bookslist)
                {
                    book.IsActive = 0;
                    
                    bool chkbook = new BookBL().UpdateBook(book, db);
                }

                bool checkUser = new UserBL().UpdateUser(user, db);
                if (checkUser == true)
                {
                    return RedirectToAction("AdminViewUser", new { msg = " User has been deleted", color = "green" });
                }
                else
                {
                    return RedirectToAction("AdminViewUser", "Admin", new { msg = "Somethings' Wrong", color = "Red" });
                }
                
            }
            catch
            {
                return RedirectToAction("Error", "Auth", new { msg = "search for the page was not found", color = "Red" });
            }

        }

        public ActionResult AdminViewUserBook(string Name = "", string Title="", string Author="" , int id = -1)
        {
            try
            {
                if (ValidateLogin() == false)
                {
                    return RedirectToAction("Login", "Auth", new { msg = "Session Expired from ADMIN DashBoard, plz login again", color = "Red" });
                }
                if(id == -1){
                    List<Book> bookList = new BookBL().GetActiveBookList(db).ToList();
                    if (Name != "")
                    {
                        bookList = bookList.Where(x => x.User.FirstName.ToLower().Contains(Name.ToLower()) || x.User.LastName.ToLower().Contains(Name.ToLower())).ToList();
                    }
                    if (Title != "")
                    {
                        bookList = bookList.Where(x => x.Title.ToLower().Contains(Title.ToLower())).ToList();
                    }
                    if (Author != "")
                    {
                        bookList = bookList.Where(x => x.Auther.ToLower().Contains(Author.ToLower())).ToList();
                    }
                    ViewBag.Book = bookList;
                    ViewBag.Name = Name;
                    ViewBag.Title = Title;
                    ViewBag.Author = Author;
                    return View();
                }
                else{
                    List<Book> bookList = new BookBL().GetActiveBookList(db).Where(x=> x.UserId == id).ToList();
                    if (Name != "")
                    {
                        bookList = bookList.Where(x => x.User.FirstName.ToLower().Contains(Name.ToLower()) || x.User.LastName.ToLower().Contains(Name.ToLower())).ToList();
                    }
                    if (Title != "")
                    {
                        bookList = bookList.Where(x => x.Title.ToLower().Contains(Title.ToLower())).ToList();
                    }
                    if (Author != "")
                    {
                        bookList = bookList.Where(x => x.Auther.ToLower().Contains(Author.ToLower())).ToList();
                    }
                    ViewBag.Book = bookList;
                    ViewBag.Name = Name;
                    ViewBag.Title = Title;
                    ViewBag.Author = Author;
                    return View();
                }
            }
            catch
            {
                return RedirectToAction("Error", "Auth", new { msg = "search for the page was not found ", color = "Red" });
            }

        }
    }
}