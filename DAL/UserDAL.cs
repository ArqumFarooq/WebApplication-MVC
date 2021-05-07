using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication_MVC.Models;

namespace WebApplication_MVC.DAL
{
    public class UserDAL
    {
        public List<User> GetActiveUserList(DatabaseEntities de)
        {
            return de.Users.Where(x => x.IsActive == 1).ToList();
        }

        public User GetUserById(int _Id, DatabaseEntities de)
        {
            return de.Users.Where(x => x.Id == _Id).FirstOrDefault(x => x.IsActive == 1);
        }


        public bool AddUser(User _user, DatabaseEntities de)
        {
            try
            {
                de.Users.Add(_user);
                de.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool UpdateUser(User _user, DatabaseEntities de)
        {
            try
            {
                de.Entry(_user).State = System.Data.Entity.EntityState.Modified;
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