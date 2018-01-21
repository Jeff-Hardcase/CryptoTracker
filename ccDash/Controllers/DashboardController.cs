using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ccDash.Models.Repositories;

namespace ccDash.Controllers
{
    public class DashboardController : Controller
    {
        // GET: Dashboard
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Summary()
        {
            return View();
        }

        public ActionResult SummaryPartial()
        {
            var _repo = new TransactRepository();
            var _model = _repo.GetCurrentSummary();

            return PartialView(_model);
        }
    }
}