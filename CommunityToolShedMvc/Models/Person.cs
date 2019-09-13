using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CommunityToolShedMvc.Models
{
    public class Person
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string HashedPassword { get; set; }

        public List<CommunityRole> Roles { get; set; }
        public List<Community> Communities { get; set; }
        public List<Tool> BorrowedTools { get; set; }
    }
}