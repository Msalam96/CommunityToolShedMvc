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
    public class CommunityController : Controller
    {
        // GET: Community
        public ActionResult Index(int id)
        {
            Community community = DatabaseHelper.RetrieveSingle<Community>(@"
                select c.Id, c.[Open], c.CommunityName, 
                p.FirstName + ' ' + LastName as OwnerName 
                from Community c
                join Person p
                    on    p.Id = c.OwnerId
                where c.Id = @Id
            ",
                new SqlParameter("@Id", id));

            community.Tools = DatabaseHelper.Retrieve<Tool>(@"
                select i.Id, i.ItemName, i.OwnerId, i.Usage, i.Warning, i.Age,
                p.FirstName as OwnerName
                from CommunityItems ci
                join Item i
                    on i.Id = ci.ItemId
                join Person p
                    on p.Id = i.OwnerId
                where ci.CommunityId = @Id
            ",
                new SqlParameter("Id", id));

            return View(community);
        }

        public ActionResult Create()
        {
            Community community = new Community();
            return View(community);
        }

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            Community community = new Community();
            community.CommunityName = collection.Get("CommunityName");
            CustomPrincipal currentUser = (CustomPrincipal)User;

            int? communityId = DatabaseHelper.Insert(@"
                    insert Community (
                        CommunityName, 
                        [Open], 
                        OwnerId 
                    ) values (
                        @CommunityName, 
                        @Open, 
                        @OwnerId 
                    )
                ",
                    new SqlParameter("CommunityName", community.CommunityName),
                    new SqlParameter("Open", community.Open),
                    new SqlParameter("OwnerId", currentUser.Person.Id));

            DatabaseHelper.Insert(@"
                insert PersonCommunity (
                    PersonId,
                    CommunityId
                ) values (
                    @PersonId, 
                    @CommunityId 
                )
            ",
                new SqlParameter("@PersonId", currentUser.Person.Id),
                new SqlParameter("@CommunityId", communityId));

            DatabaseHelper.Insert(@"
                Insert PersonRole (
                    PersonId,
                    CommunityId,  
                    RoleId
                ) values (
                    @PersonId,
                    @CommunityId,  
                    1
                )
            ", 
                new SqlParameter("@PersonId", currentUser.Person.Id),
                new SqlParameter("@CommunityId", communityId));

            DatabaseHelper.Insert(@"
                Insert PersonRole (
                    PersonId,
                    CommunityId,  
                    RoleId
                ) values (
                    @PersonId,
                    @CommunityId,  
                    2
                )
            ",
                new SqlParameter("@PersonId", currentUser.Person.Id),
                new SqlParameter("@CommunityId", communityId));

            DatabaseHelper.Insert(@"
                Insert PersonRole (
                    PersonId,
                    CommunityId,  
                    RoleId
                ) values (
                    @PersonId,
                    @CommunityId,  
                    3
                )
            ",
                new SqlParameter("@PersonId", currentUser.Person.Id),
                new SqlParameter("@CommunityId", communityId));

            DatabaseHelper.Insert(@"
                Insert PersonRole (
                    PersonId,
                    CommunityId,  
                    RoleId
                ) values (
                    @PersonId,
                    @CommunityId,  
                    4
                )
            ",
                new SqlParameter("@PersonId", currentUser.Person.Id),
                new SqlParameter("@CommunityId", communityId));

            return RedirectToAction("Index", "Home");
 
        }

        [HttpGet]
        public ActionResult Join()
        {
            List<Community> communities = new List<Community>();
            CustomPrincipal currentUser = (CustomPrincipal)User;

            communities = DatabaseHelper.Retrieve<Community>(@"
                select c.CommunityName, c.Id
                from Community c
                left join PersonCommunity pc 
                on pc.CommunityId = c.Id and pc.PersonId = @PersonId
                where pc.Id is null 
            ",
               new SqlParameter("@PersonId", currentUser.Person.Id));

            return View(communities);
        }

        public ActionResult joinCommunity(int id)
        {
            CustomPrincipal currentUser = (CustomPrincipal)User;

            DatabaseHelper.Insert(@"
                insert PersonCommunity (
                    PersonId,
                    CommunityId
                ) values (
                    @PersonId,
                    @CommunityId
                )
            ",
                new SqlParameter("@PersonId", currentUser.Person.Id),
                new SqlParameter("@CommunityId", id));

            DatabaseHelper.Insert(@"
                Insert PersonRole (
                    PersonId,
                    CommunityId,  
                    RoleId
                ) values (
                    @PersonId,
                    @CommunityId,  
                    1
                )
            ",
                new SqlParameter("@PersonId", currentUser.Person.Id),
                new SqlParameter("@CommunityId", id));

            return RedirectToAction("Index", "Home");
        }
    }
}