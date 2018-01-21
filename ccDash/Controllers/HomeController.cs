using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ccDash.Models;
using ccDash.Models.Repositories;
using ccDash.Models.Domain;
using ccDash.Models.View;
using ccDash.Extensions;

namespace ccDash.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }

        public JsonResult TestExport()
        {
            var _repo = new ExportRepository();
            var _result = _repo.GetAssetBalances();

            return Json(_result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult TestGetPrice(string asset, string source)
        {
            var _repo = new TransactRepository();
            var _type = new AssetType();
            var _src = new AssetPriceSource();

            
            var _asset = Enum.TryParse<AssetType>(asset, out _type) ? _type : AssetType.Bitcoin;
            var _source = Enum.TryParse<AssetPriceSource>(source, out _src) ? _src : AssetPriceSource.GDAX;

            var result = _repo.GetPrice(_asset, _source);

            return new CustomJsonResult { Data = result };
        }
    
        public JsonResult TestGetBTCPriceGDAX()
        {
            var _repo = new TransactRepository();
            var result = _repo.GetBTCPriceGDAX();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult TestGetList()
        {
            var _repo = new TransactRepository();
            var result = _repo.GetAssetList();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult TestGetDeposits()
        {
            var _repo = new TransactRepository();
            var result = _repo.GetDepositsUSD();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult TestSummary()
        {
            var _repo = new TransactRepository();
            var result = _repo.GetCurrentSummary();

            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}