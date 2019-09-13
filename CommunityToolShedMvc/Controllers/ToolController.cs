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
        
    }
}