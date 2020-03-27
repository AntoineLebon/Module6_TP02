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
    public class ArmesController : Controller
    {
        private Context db = new Context();

        public ActionResult Index()
        {
            return View(db.Armes.ToList());
        }

        public ActionResult Details(int? unId)
        {
            if (unId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Arme uneArme = db.Armes.Find(unId);
            if (uneArme == null)
            {
                return HttpNotFound();
            }
            return View(uneArme);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Nom,Degats")] Arme uneArme)
        {
            if (ModelState.IsValid)
            {
                db.Armes.Add(uneArme);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(uneArme);
        }

        public ActionResult Edit(int? unId)
        {
            if (unId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Arme uneArme = db.Armes.Find(unId);
            if (uneArme == null)
            {
                return HttpNotFound();
            }
            return View(uneArme);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Nom,Degats")] Arme uneArme)
        {
            if (ModelState.IsValid)
            {
                db.Entry(uneArme).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(uneArme);
        }

        public ActionResult Delete(int? unId)
        {
            if (unId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Arme unArme = db.Armes.Find(unId);
            if (arme == null)
            {
                return HttpNotFound();
            }
            return View(unArme);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int unId)
        {
            if (ModelState.IsValid) 
            {
                if (db.Samourais.Any(s => s.Arme.Id == unId))
                {
                    ModelState.AddModelError("", "Impossible de supprimer cette arme car elle est utilisée par des Samourais");
                    return View(db.Armes.Find(unId));
                }

                Arme unArme = db.Armes.Find(unId);
                db.Armes.Remove(unArme);
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            return View(db.Armes.Find(unId));
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
