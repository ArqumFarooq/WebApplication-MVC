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

    }
}