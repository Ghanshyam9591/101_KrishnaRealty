using EMS.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EMS.Web.Controllers
{
    [CustomAuthorize]
    public class RentalQueryModuleController : Controller
    {
        // GET: RentalQueryModule
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult GetGridDetail(int owner_id, int renter_id, int location_id, int building_id)
        {
            RentalQueryModule bll = new RentalQueryModule();
            return new JsonNetResult(bll.GetGridDetail(owner_id, renter_id, location_id, building_id));
        }
    }
}