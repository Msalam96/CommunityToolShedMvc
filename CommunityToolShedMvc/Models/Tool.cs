using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CommunityToolShedMvc.Models
{
    public class Tool
    {
        public int Id { get; set; }
        public string ItemName { get; set; }
        public int OwnerId { get; set; }
        public string Usage { get; set; }
        public string Warning { get; set; }
        public string Age { get; set; }
        public string OwnerName { get; set; }
    }
}