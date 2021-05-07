using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication_MVC.Models;
using WebApplication_MVC.BL;

namespace WebApplication_MVC.Helping_Classes
{
    public class GeneralPurpose
    {
        DatabaseEntities de = new DatabaseEntities();

        public bool ValidateEmail(string email, int id = -1)
        {
            int emailCount = 0;

            if (id != -1)
            {
                emailCount = new UserBL().GetActiveUserList(de).Where(x => x.Email.ToLower() == email.ToLower() && x.Id != id && x.IsActive != 0).Count();
            }
            else
            {
                emailCount = new UserBL().GetActiveUserList(de).Where(x => x.Email.ToLower() == email.ToLower() && x.IsActive != 0).Count();
            }

            if (emailCount > 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

    }
}