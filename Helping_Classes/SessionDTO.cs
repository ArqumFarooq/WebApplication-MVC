using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication_MVC.Helping_Classes
{
    public class  SessionDTO
    {
        public int Id { set; get; }
        public string Name { set; get; }
        public string Email { set; get; }
        public int Role { set; get; }


        public int getId()
        {
            SessionDTO sdto = (SessionDTO)HttpContext.Current.Session["Session"];
            if (sdto == null)
                return -1;

            return sdto.Id;
        }

        public string getName()
        {
            SessionDTO sdto = (SessionDTO)HttpContext.Current.Session["Session"];
            if (sdto == null)
                return null;

            return sdto.Name;
        }

        public string getEmail()
        {
            SessionDTO sdto = (SessionDTO)HttpContext.Current.Session["Session"];
            if (sdto == null)
                return null;

            return sdto.Email;
        }

        public int getRole()
        {
            SessionDTO sdto = (SessionDTO)HttpContext.Current.Session["Session"];
            if (sdto == null)
                return -1;

            return sdto.Role;
        }

    }
}