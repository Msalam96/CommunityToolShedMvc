using CommunityToolShedMvc.Data;
using CommunityToolShedMvc.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CommunityToolShedMvc.Controllers
{
    public class CommunityController : Controller
    {
        // GET: Community

        public ActionResult Index(int id)
        {
            Community community = DatabaseHelper.RetrieveSingle<Community>(@"
                Select c.[Open], c.CommunityName, 
                p.FirstName + ' ' + LastName as OwnerName 
                from Community c
                join Person p
                    on    p.Id = c.OwnerId
                where c.Id = @Id
            ",
                new SqlParameter("@Id", id));

            return View();
        }
    }
}