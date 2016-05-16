using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using PagedList;
using PropertyGuide.DataAccess;
using PropertyGuide.Models;

namespace PropertyGuide.Controllers
{
    public class HomeController : Controller
    {
        private IPropertyManager _propertyManager;

        public HomeController(IPropertyManager propertyManager)
        {
            _propertyManager = propertyManager;
        }

        public ActionResult Index()
        {
            if (!Request.IsAuthenticated) return View();

            if (User.IsInRole(Helpers.Constants.UserType.Seller))
            {
                return RedirectToAction("Index", "MyProperties");
            }

            if (User.IsInRole(Helpers.Constants.UserType.Buyer))
            {
                return RedirectToAction("Search");
            }

            return View();
        }

        [Authorize(Roles = Helpers.Constants.UserType.Buyer)]
        public ActionResult Search(string keywords, int? page)
        {
            if (!string.IsNullOrEmpty(keywords))
            {
                page = 1;
            }

            ViewBag.Keywords = keywords;

            var pageNumber = page ?? 1;
            const int pageSize = Helpers.Constants.PageSize;

            var model = _propertyManager.GetPropertyOfferViewModelList(User.Identity.Name, keywords, pageNumber, pageSize);

            return View(model);
        }

        [Authorize(Roles = Helpers.Constants.UserType.Buyer)]
        public async Task<ActionResult> MakeOffer(int id)
        {
            var model = await _propertyManager.GetPropertyOfferViewModel(id, User.Identity.Name);

            return View(model);
        }

        [Authorize(Roles = Helpers.Constants.UserType.Buyer)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> MakeOffer(PropertyOfferViewModel model)
        {
            if (ModelState.IsValid)
            {
                var status = await _propertyManager.AddPropertyOffer(model, User.Identity.Name);

                if (status > 0)
                {
                    model.OfferSubmitted = true;
                }
            }

            return View(model);
        }

        [Authorize(Roles = Helpers.Constants.UserType.Buyer)]
        public async Task<ActionResult> MyOffers()
        {
            return View(await _propertyManager.GetMyPropertyOffers(User.Identity.Name));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_propertyManager != null)
                {
                    _propertyManager.Dispose();
                    _propertyManager = null;
                }
            }

            base.Dispose(disposing);
        }
    }
}