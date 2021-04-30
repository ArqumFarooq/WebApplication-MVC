using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication_MVC.ViewModel
{
    public class BookViewModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Title { get; set; }
        public string Auther { get; set; }
        public int? IsActive { get; set; }
        public Nullable<System.DateTime> CreatedAt { get; set; }
    }
}