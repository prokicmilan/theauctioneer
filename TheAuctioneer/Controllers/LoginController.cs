using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BusinessLogicLayer.Repositories;
using ViewModelLayer.Models.User;

namespace TheAuctioneer.Controllers
{
    public class LoginController : Controller
    {

        private readonly UserBl _userBl = new UserBl();

        // GET: Login
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginUserModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (_userBl.CheckCredentials(model.Username, model.Password))
                    {
                        
                    }
                }
            }
        }
    }
}