using EMS.Common;
using EMS.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EMS.Web.Controllers
{
    [CustomAuthorize]
    public class FlatOwnersController : Controller
    {
        // GET: FlatOwners
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Item()
        {
            return View();
        }
        public ActionResult ListData()
        {
            FlatOwners bll = new FlatOwners();
            return new JsonNetResult(bll.LIST_DATA());

        }
        public ActionResult GetItem(int id)
        {
            FlatOwners bll = new FlatOwners();
            return new JsonNetResult(bll.GetItem(id));
        }
        public ActionResult Save(FlatOwnerModel model)
        {
            FlatOwners bll = new FlatOwners();
            return new JsonNetResult(bll.Save(model));
        }
    }
}