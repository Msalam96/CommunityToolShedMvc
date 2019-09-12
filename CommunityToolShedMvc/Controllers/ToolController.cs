using CommunityToolShedMvc.Models;
using System;
using System.Collections.Generic;
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
            return View();
        }
        
    }
}