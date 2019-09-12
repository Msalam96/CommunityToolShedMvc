using CommunityToolShedMvc.Data;
using CommunityToolShedMvc.Models;
using CommunityToolShedMvc.Security;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Principal;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;

namespace CommunityToolShedMvc
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }

        protected void Application_PostAuthenticateRequest()
        {
            IPrincipal user = HttpContext.Current.User;

            if (user.Identity.IsAuthenticated && user.Identity.AuthenticationType == "Forms")
            {
                FormsIdentity formsIdentity = (FormsIdentity)user.Identity;
                FormsAuthenticationTicket ticket = formsIdentity.Ticket;

                CustomIdentity customIdentity = new CustomIdentity(ticket);

                string currentUserEmail = ticket.Name;
                Person person = DatabaseHelper.RetrieveSingle<Person>(@"
                    select Id, FirstName, LastName, Email
                    from Person 
                    where Email = @Email
                ",
                    new SqlParameter("@Email", currentUserEmail));

                person.Roles = DatabaseHelper.Retrieve<CommunityRole>(@"
                    select r.RoleName, c.Id
                    from Person P
                    join PersonRole pr on pr.PersonId = p.id
                    join Role r on pr.RoleId = r.id
                    join Community c on c.Id = pr.CommunityId
                    where pr.PersonId = @Id
                ",
                    new SqlParameter("Id", person.Id));

                CustomPrincipal customPrinicipal = new CustomPrincipal(customIdentity, person);

                HttpContext.Current.User = customPrinicipal;
                Thread.CurrentPrincipal = customPrinicipal;
            }
        }

    }
}
