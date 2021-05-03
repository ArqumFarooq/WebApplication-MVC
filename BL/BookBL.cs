using System;
using System.Collections.Generic;
using WebApplication_MVC.DAL;
using WebApplication_MVC.Models;
using WebApplication_MVC.ViewModel;

namespace WebApplication_MVC.BL
{
    public class BookBL
    {
        public List<Book> GetBookList(DatabaseEntities de)
        {
            return new BookDAL().GetBookList(de);
        }

        public User GetUserIdForProfileUpdate(DatabaseEntities de, int id)
        {
            return new BookDAL().GetUserIdForProfileUpdate(de, id);
        }

        public Book GetBookId(DatabaseEntities de, int id)
        {
            return new BookDAL().GetBookId(de, id);
        }

        public Book GetUserIdForSearchBook(DatabaseEntities de, int id)
        {
            return new BookDAL().GetUserIdForSearchBook(de, id);
        }

        public List<BookViewModel> GetOnlyUserBookList(List<BookViewModel> de, int id)
        {
            return new BookDAL().GetOnlyUserBookList(de, id);
        }

        public bool AddBook(Book _book, DatabaseEntities de)
        {
            if (_book.Title == "" || _book.Auther == "" || _book.Title == null || _book.Auther == null)
            {
                return false;
            }
            else
            {
                return new BookDAL().AddBook(_book, de);
            }
        }

        public bool UpdateBook(Book _book, DatabaseEntities de)
        {
            return new BookDAL().UpdateBook(_book, de);
        }

        public bool DeleteBook(Book _book, DatabaseEntities de)
        {
            return new BookDAL().DeleteBook(_book, de);
        }
    }
}