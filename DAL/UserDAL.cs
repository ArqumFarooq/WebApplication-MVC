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

        public List<User> GetActiveUserListWhereNoAdmin(DatabaseEntities de)
        {
            return de.Users.Where(x => x.Role != 1 && x.IsActive == 1).ToList();
        }
        public IQueryable<User> GetActiveUserWhereNoAdmin(DatabaseEntities de)
        {
            return de.Users.Where(x => x.Role != 1);
        }

        public User AdminAddUser(DatabaseEntities de, User _user)
        {
            return de.Users.FirstOrDefault(x => string.Equals(x.Email.ToLower(), _user.Email.ToLower()));
        }

        public User PostAdminSearchUser(DatabaseEntities de, int id)
        {
            return de.Users.Single(emp => emp.Id == id);
        }

        public User GetUserId(DatabaseEntities de, int id)
        {
            return de.Users.Find(id);
        }


        public User GetUserById(int _Id, DatabaseEntities de)
        {
            return de.Users.Where(x => x.Id == _Id).FirstOrDefault(x => x.IsActive == 1);
        }

        public User CheckEmailUserExist(User _user, DatabaseEntities de)
        {
            return de.Users.FirstOrDefault(x => String.Equals(x.Email.ToLower(), _user.Email.ToLower()));
        }

        public User ComparerUserEmailPassword(User _user, DatabaseEntities de)
        {
            return de.Users.Where(x => String.Equals(x.Email.ToLower(), _user.Email.ToLower()) && String.Equals(x.Password, _user.Password)).FirstOrDefault();
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
        public bool DeleteUser(User _user, DatabaseEntities de)
        {
            try
            {
                _user.IsActive = 0;
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