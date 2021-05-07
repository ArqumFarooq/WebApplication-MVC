using System.Collections.Generic;
using System.Linq;
using WebApplication_MVC.Models;

namespace WebApplication_MVC.DAL
{
    public class BookDAL
    {
        public List<Book> GetActiveBookList(DatabaseEntities de)
        {
            return de.Books.Where(x=> x.IsActive == 1).ToList();
        }

        public Book GetBookById(int id, DatabaseEntities de)
        {
            return de.Books.Where(x=>x.Id == id).FirstOrDefault(x=>x.IsActive == 1);
        }

        public List<Book> GetBookListByUserId(int id, DatabaseEntities de)
        {
            return de.Books.Where(x => x.UserId == id && x.IsActive == 1).ToList();
        }

        public bool AddBook(Book _book, DatabaseEntities de)
        {
            try
            {
                de.Books.Add(_book);
                de.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool UpdateBook(Book _book, DatabaseEntities de)
        {
            try
            {
                de.Entry(_book).State = System.Data.Entity.EntityState.Modified;
                de.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
        
    }
}