using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PropertyGuide.DataAccess;
using PropertyGuide.Models;

namespace PropertyGuide.Controllers
{
    [Authorize(Roles = Helpers.Constants.UserType.Seller)]
    public class MyPropertiesController : Controller
    {
        private IPropertyManager _propertyManager;

        public MyPropertiesController()
        {
            
        }

        public MyPropertiesController(IPropertyManager propertyManager)
        {
            _propertyManager = propertyManager;
        }

        // GET: Properties
        public async Task<ActionResult> Index()
        {
            return View(await _propertyManager.GetList(User.Identity.Name));
        }

        // GET: Properties/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var property = await _propertyManager.Get(id);
            if (property == null)
            {
                return HttpNotFound();
            }
            return View(property);
        }

        // GET: Properties/Create
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Title,Description")] PropertyViewModel model)
        {
            if (ModelState.IsValid)
            {
                await _propertyManager.Add(model, User.Identity.Name);

                return RedirectToAction("Index");
            }

            return View(model);
        }

        // GET: Properties/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var property = await _propertyManager.Get(id);
            if (property == null)
            {
                return HttpNotFound();
            }

            var model = new PropertyViewModel
            {
                Id = property.Id,
                Title = property.Title,
                Description = property.Description,
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Title,Description")] PropertyViewModel model)
        {
            if (ModelState.IsValid)
            {
                await _propertyManager.Update(model);

                return RedirectToAction("Index");
            }
            return View(model);
        }

        // GET: Properties/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var property = await _propertyManager.Get(id);
            if (property == null)
            {
                return HttpNotFound();
            }
            return View(property);
        }

        // POST: Properties/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            await _propertyManager.Delete(id);

            return RedirectToAction("Index");
        }

        public async Task<ActionResult> Offers()
        {
            return View(await _propertyManager.GetListWithOffers(User.Identity.Name));
        }

        // GET: Properties/Accept/5
        public async Task<ActionResult> Accept(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var property = await _propertyManager.GetPropertyOfferViewModel(id);
            if (property == null)
            {
                return HttpNotFound();
            }
            return View(property);
        }

        [HttpPost, ActionName("Accept")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AcceptConfirmed(int id)
        {
            await _propertyManager.Accept(id);

            return RedirectToAction("Offers");
        }

        // GET: Properties/Accept/5
        public async Task<ActionResult> Reject(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var property = await _propertyManager.GetPropertyOfferViewModel(id);
            if (property == null)
            {
                return HttpNotFound();
            }
            return View(property);
        }

        [HttpPost, ActionName("Reject")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RejectConfirmed(int id)
        {
            await _propertyManager.Reject(id);

            return RedirectToAction("Offers");
        }

        public async Task<ActionResult> OffersAccepted()
        {
            return View(await _propertyManager.GetListWithOffersAccepted(User.Identity.Name));
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
