using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CommunityToolShedMvc.Models
{
    public class Community
    {
        public int Id { get; set; }

        [Display (Name = "Community Name")]
        public string CommunityName { get; set; }

        public bool Open { get; set; }
        public int OwnerId { get; internal set; }
        public string OwnerName { get; internal set; }
        public List<Tool> Tools { get; set; }
    }
}