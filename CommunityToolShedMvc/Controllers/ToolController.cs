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
    public class ToolController : Controller
    {
        // GET: Tool
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Create(int communityid)
        {
            Tool tool = new Tool();
            return View(tool);
        }

        [HttpPost]
        public ActionResult Create(FormCollection collection, int communityid)
        {
            Tool tool = new Tool();
            tool.ItemName = collection.Get("ItemName");
            tool.Usage = collection.Get("Usage");
            tool.Warning = collection.Get("Warning");
            tool.Age = collection.Get("Age");
            CustomPrincipal currentUser = (CustomPrincipal)User;

            int? itemId = DatabaseHelper.Insert(@"
                    insert Item (
                        ItemName,  
                        OwnerId,
                        Usage,
                        Warning,
                        Age 
                    ) values (
                        @ItemName,  
                        @OwnerId,
                        @Usage,
                        @Warning,
                        @Age  
                    )
                ",
                    new SqlParameter("@ItemName", tool.ItemName),
                    new SqlParameter("@OwnerId", currentUser.Person.Id),
                    new SqlParameter("@Usage", tool.Usage),
                    new SqlParameter("@Warning", tool.Warning),
                    new SqlParameter("@Age", tool.Age));

            DatabaseHelper.Insert(@"
                insert CommunityItems (
                        ItemId,  
                        CommunityId
                    ) values (
                        @ItemId,
                        @CommunityId
                    )
                ",
                    new SqlParameter("@ItemId", itemId),
                    new SqlParameter("@CommunityId", communityid));
  
            return RedirectToAction("Index", "Community", new { id = communityid });
        }
        
        public ActionResult Borrow(int communityid, int id)
        {
            int communityitemId = DatabaseHelper.ExecuteScalar<int>(@"
                select ci.id
                from CommunityItems ci
                where ci.ItemId = @ItemId and ci.CommunityId = @CommunityId
            ",
                new SqlParameter("@ItemId", id),
                new SqlParameter("@CommunityId", communityid));

            DatabaseHelper.Insert(@"
                    insert Borrow (
                        CommunityItemId,  
                        BorrowerId,
                        DateRequested
                    ) values (
                        @CommunityItemId,  
                        @BorrowerId,
                        @DateRequested  
                    )
                ",
                    new SqlParameter("CommunityItemId", communityitemId),
                    new SqlParameter("BorrowerId", ((CustomPrincipal)User).Person.Id),
                    new SqlParameter("DateRequested", DateTime.Now));

            //return RedirectToAction("Index", "Community", new { id = communityid });
            return RedirectToRoute("Default", new { controller = "Community", action = "Index", id = communityid});
        }

        public ActionResult Remove(int communityid, int id)
        {
            DatabaseHelper.Update(@"
            delete from CommunityItems
            where ItemId = @ItemId and CommunityId = @CommunityId
            ",
                new SqlParameter("@ItemId", id),
                new SqlParameter("@CommunityId", communityid));

            return RedirectToRoute("Default", new { controller = "Community", action = "Index", id = communityid });
        }
    }
}