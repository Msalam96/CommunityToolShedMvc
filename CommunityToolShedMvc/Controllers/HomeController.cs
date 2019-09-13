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
            Person person = new Person();
            CustomPrincipal currentUser = (CustomPrincipal)User;
            //List<Community> communities = new List<Community>();

            person.Communities = DatabaseHelper.Retrieve<Community>(@"
                    select c.Id, c.[Open], c.CommunityName, c.OwnerId
                    from PersonCommunity pC
                        join Community c
                        on c.Id = pC.CommunityId
                        join Person 
                        on pC.PersonId = Person.Id
                        where Person.Email = @Email
                ", 
                    new SqlParameter("@Email", User.Identity.Name));

            person.BorrowedTools = DatabaseHelper.Retrieve<Tool>(@"
                select i.ItemName
                from Borrow b
                    join CommunityItems ci
	                on ci.Id = b.CommunityItemId
	                join Item i
	                on ci.ItemId = i.Id
	                where b.BorrowerId = @Id
                ", 
                    new SqlParameter("@Id", currentUser.Person.Id));
            
            return View(person);

        }

    }
}
