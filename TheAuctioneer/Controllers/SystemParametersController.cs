using BusinessLogicLayer.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TheAuctioneer.Attributes;
using ViewModelLayer.Models.SystemParameters;

namespace TheAuctioneer.Controllers
{
    [AuthorizeUser(RolesAllowed = "Admin")]
    public class SystemParametersController : Controller
    {

        private readonly SystemParametersBl _systemParametersBl = new SystemParametersBl();

        // GET: SystemParameters
        public ActionResult Index()
        {
            var models = _systemParametersBl.GetAll();
            return View(models);
        }

        // GET: SystemParameters/Edit/id
        public ActionResult Edit(Guid id)
        {
            var model = _systemParametersBl.GetSingle(id);
            return View(model);
        }

        // POST: SystemParameters/Edit/id
        [HttpPost]
        public ActionResult Edit(DisplaySystemParametersModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _systemParametersBl.UpdateSystemParameter(model);
                    return RedirectToAction("Index");
                }
                return View(model);
            }
            catch
            {
                return View(model);
            }
        }
    }
}