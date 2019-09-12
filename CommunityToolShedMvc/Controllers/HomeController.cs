using CommunityToolShedMvc.Data;
using CommunityToolShedMvc.Models;
using CommunityToolShedMvc.Security;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CommunityToolShedMvc.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            CustomPrincipal currentUser = (CustomPrincipal)User;
            List<Community> communities = new List<Community>();

            communities = DatabaseHelper.Retrieve<Community>(@"
                    select c.Id, c.[Open], c.CommunityName, c.OwnerId
                    from PersonCommunity pC
                        join Community c
                        on c.Id = pC.CommunityId
                        join Person 
                        on pC.PersonId = Person.Id
                        where Person.Email = @Email
                ", 
                    new SqlParameter("@Email", User.Identity.Name));

            return View(communities);
        }

    }
}
