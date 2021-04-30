using System.Collections.Generic;
using System.Linq;
using WebApplication_MVC.Models;
using WebApplication_MVC.ViewModel;

namespace WebApplication_MVC.DAL
{
    public class BookDAL
    {
        public List<Book> GetBookList(DatabaseEntities de)
        {
            return de.Books.Where(x=> x.IsActive == 1).ToList();
        }

        public Book GetUserIdForSearchBook(DatabaseEntities de, int id)
        {
            return de.Books.Single(book => book.Id == id);
        }

        public Book GetBookId(DatabaseEntities de, int id)
        {
            return de.Books.Find(id);
        }

        public List<Book> GetBookListByUserId(DatabaseEntities de, int id)
        {
            return de.Books.Where(x => x.UserId == id).ToList();
        }

        public User GetUserIdForProfileUpdate(DatabaseEntities de, int id)
        {
            return de.Users.FirstOrDefault(x => x.Id == id);
        }

        public List<BookViewModel> GetOnlyUserBookList(List<BookViewModel> de, int id)
        {
            return de.Where(x => x.UserId == id).ToList();
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
        public bool DeleteBook(Book _book, DatabaseEntities de)
        {
            try
            {
                //db.Books.Remove(_book);
                _book.IsActive = 0;
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