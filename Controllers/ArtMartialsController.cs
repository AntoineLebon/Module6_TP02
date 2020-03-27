using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Module6Tp1Dojo.Data;
using Module6Tp1Dojo_BO;

namespace Module6Tp1Dojo.Controllers
{
    public class ArtMartialsController : Controller
    {
        private Context db = new Context();

        public ActionResult Index()
        {
            return View(db.ArtMartials.ToList());
        }

        public ActionResult Details(int? unId)
        {
            if (unId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ArtMartial unArtMartial = db.ArtMartials.Find(unId);
            if (unArtMartial == null)
            {
                return HttpNotFound();
            }
            return View(unArtMartial);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Nom")] ArtMartial unArtMartial)
        {
            if (ModelState.IsValid)
            {
                db.ArtMartials.Add(unArtMartial);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(unArtMartial);
        }

        public ActionResult Edit(int? unId)
        {
            if (unId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ArtMartial UnArtMartial = db.ArtMartials.Find(unId);
            if (UnArtMartial == null)
            {
                return HttpNotFound();
            }
            return View(UnArtMartial);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Nom")] ArtMartial unArtMartial)
        {
            if (ModelState.IsValid)
            {
                db.Entry(unArtMartial).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(unArtMartial);
        }

        public ActionResult Delete(int? unId)
        {
            if (unId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ArtMartial unArtMartial = db.ArtMartials.Find(unId);
            if (unArtMartial == null)
            {
                return HttpNotFound();
            }
            return View(unArtMartial);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int unId)
        {
            ArtMartial unArtMartial = db.ArtMartials.Find(unId);
            db.ArtMartials.Remove(unArtMartial);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
