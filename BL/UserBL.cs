using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication_MVC.DAL;
using WebApplication_MVC.Models;

namespace WebApplication_MVC.BL
{
    public class UserBL
    {
        public List<User> GetActiveUserList(DatabaseEntities de)
        {
            return new UserDAL().GetActiveUserList(de);
        }

        public User GetUserById(int _Id, DatabaseEntities de)
        {
            return new UserDAL().GetUserById(_Id, de);
        }

        public User GetUserId(DatabaseEntities de, int id)
        {
            return new UserDAL().GetUserId(de,id);
        }

        public List<User> GetActiveUserListWhereNoAdmin(DatabaseEntities de)
        {
            return new UserDAL().GetActiveUserListWhereNoAdmin(de);
        }

        public IQueryable<User> GetActiveUserWhereNoAdmin(DatabaseEntities de)
        {
            return new UserDAL().GetActiveUserWhereNoAdmin(de);
        }

        public List<Book> GetBookListByUserId(DatabaseEntities de, int id)
        {
            return new BookDAL().GetBookListByUserId(de, id);
        }

        public User AdminAddUser(DatabaseEntities de, User _user)
        {
            return new UserDAL().AdminAddUser(de, _user);
        }

        public User PostAdminSearchUser(DatabaseEntities de, int id)
        {
            return new UserDAL().PostAdminSearchUser(de, id);
        }

        public User CheckEmailUserExist(User _user, DatabaseEntities de)
        {
            return new UserDAL().CheckEmailUserExist(_user, de);
        }

        public User ComparerUserEmailPassword(User _user, DatabaseEntities de)
        {
            return new UserDAL().ComparerUserEmailPassword(_user, de);
        }

        public bool AddUser(User _user, DatabaseEntities de)
        {
            if (_user.FirstName == "" ||_user.LastName == "" ||_user.Contact == "" || _user.Email == "" || _user.Password == "" || _user.FirstName == null || _user.LastName == null ||_user.Contact == null || _user.Email == null || _user.Password == null)
            {
                return false;
            }
            else
            {
                return new UserDAL().AddUser(_user, de);
            }
        }

        public bool UpdateUser(User _user, DatabaseEntities de)
        {
            if (_user.FirstName == "" ||_user.LastName == "" ||_user.Contact == "" || _user.Email == "" || _user.Password == "" || _user.FirstName == null || _user.LastName == null ||_user.Contact == null || _user.Email == null || _user.Password == null)
            {
                return false;
            }
            else
            {
                return new UserDAL().UpdateUser(_user, de);
            }
        }

        public bool DeleteUser(User _user, DatabaseEntities de)
        {
            return new UserDAL().DeleteUser(_user, de);
        }
    }
}