using FeederAnalysis.Business;
using FeederAnalysis.Job;
using FeederAnalysis.Tokusai;
using System.Web.Mvc;

namespace FeederAnalysis.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            //var res = new TokusaiHelper().GetAll();
            var model = Repository.GetAllFeeder();
            return View(model);
        }

        public ActionResult SteelMesh()
        {
            var model = Repository.GetStellMesh();
            return View(model);
        }

        public ActionResult Sqeegee()
        {
            var model = Repository.GetSqeegee();
            return View(model);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Chương trình quản lý nhìn thấy dữ liệu SMT (Design: PI-IT)";
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Contact: IT(3143).";
            return View();
        }
        //#region Method
        //public IEnumerable<FeederEntity> CheckExpried(List<PVSService.MATERIAL_ORDER_ITEMSEntity> feeders, int dayUse)
        //{
        //    List<FeederEntity> result = new List<FeederEntity>();
        //    using (DataContext context = new DataContext())
        //    {
        //        foreach (var item in feeders)
        //        {
        //            var feeder = context.FeederInfoViews.Find(item.FEEDER_ID);
        //            DateTime dateCurr = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
        //            var a = DateTime.Now.AddDays(dayUse);
        //            int stt = 1;
        //            string about = "Over";
        //            if (feeder == null)
        //            {
        //                stt = 3;
        //                continue;
        //            }
        //            if (DateTime.Compare(DateTime.Now, Convert.ToDateTime(feeder.DatePlan)) >= 0) //NG
        //            {
        //                stt = 3;
        //            }
        //            else if (DateTime.Compare(DateTime.Now.AddDays(dayUse), Convert.ToDateTime(feeder.DatePlan)) >= 0) //WR
        //            {
        //                stt = 2;
        //                about = $"{feeder.DatePlan.Subtract(dateCurr).TotalDays} days";
        //            }

        //            if (stt != 1)
        //            {
        //                var material = _pvs_service.GetMaterialById(item.MATERIAL_ORDER_ID);
        //                //if (material != null)
        //                //{
        //                //    try
        //                //    {
        //                //        var machine = _pvs_service.MaterialLines().FirstOrDefault(r => r.LINE_ID.ToUpper() == material.LINE_ID);
        //                //        lines = machine.SETTING_NAME;

        //                //    }
        //                //    catch
        //                //    {
        //                //    }
        //                //}
        //                result.Add(new FeederEntity()
        //                {
        //                    feederNo = item.FEEDER_ID,
        //                    machineID = item.MACHINE_ID,
        //                    machineSlot = item.MACHINE_SLOT,
        //                    lineID = material != null ? material.LINE_ID : "N/A",
        //                    machines = item.MACHINE_SETTING_NAME,
        //                    feederStatus = stt,
        //                    exDate = feeder.DatePlan,
        //                    about = about
        //                });
        //            }
        //        }
        //    }
        //    return result.OrderByDescending(h => h.feederStatus).ThenBy(h => h.exDate);
        //}
        //#endregion
    }
}
