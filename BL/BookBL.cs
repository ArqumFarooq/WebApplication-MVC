using System;
using System.Collections.Generic;
using WebApplication_MVC.DAL;
using WebApplication_MVC.Models;

namespace WebApplication_MVC.BL
{
    public class BookBL
    {
        public List<Book> GetActiveBookList(DatabaseEntities de)
        {
            return new BookDAL().GetActiveBookList(de);
        }

        public List<Book> GetBookListByUserId(int id, DatabaseEntities de)
        {
            return new BookDAL().GetBookListByUserId(id, de);
        }

        public Book GetBookById(int id, DatabaseEntities de)
        {
            return new BookDAL().GetBookById(id, de);
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
            if (_book.Title == "" || _book.Auther == "" || _book.Title == null || _book.Auther == null)
            {
                return false;
            }
            else
            {
                return new BookDAL().UpdateBook(_book, de);
            }
        }

    }
}