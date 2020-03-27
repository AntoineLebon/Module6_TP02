using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Module6Tp1Dojo.Data;
using Module6Tp1Dojo.Models;
using Module6Tp1Dojo_BO;

namespace Module6Tp1Dojo.Controllers
{
    public class SamouraisController : Controller
    {
        private Context db = new Context();

        public ActionResult Index()
        {
            return View(db.Samourais.ToList());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SamouraiViewModel unSamourai = new SamouraiViewModel();
            unSamourai.Samourai = db.Samourais.Find(id);

            if (unSamourai.Samourai == null)
            {
                return HttpNotFound();
            }

            unSamourai.Potentiel = calculerPotentiel(unSamourai);

            return View(unSamourai);
        }

        private double calculerPotentiel(SamouraiViewModel unSamourai)
        {
            double force = unSamourai.Samourai.Force;

            double armeDegat = 0;
            if(unSamourai.Samourai.Arme != null)
            {
                armeDegat = unSamourai.Samourai.Arme.Degats;
            }

            double nbrArtsMartieux = 0;
            if (unSamourai.Samourai.ArtMartiaux != null)
            {
                nbrArtsMartieux = unSamourai.Samourai.ArtMartiaux.Count();
            }

            return (force + armeDegat) * (nbrArtsMartieux + 1);
        }

        public ActionResult Create()
        {
            SamouraiViewModel vm = new SamouraiViewModel();

            vm.Armes = db.Armes.Where(a => !db.Samourais.Any(s => s.Arme.Id == a.Id)).Select(a => new SelectListItem { Text = a.Nom, Value = a.Id.ToString() }).ToList();
            vm.ArtsMartiaux = db.ArtMartials.Select(am => new SelectListItem { Text = am.Nom, Value = am.Id.ToString() }).ToList();

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(SamouraiViewModel unSamouraiVM)
        {
            try
            {
                Samourai unSamourai = unSamouraiVM.Samourai;

                if(!db.Samourais.Any(s => s.Arme.Id == unSamouraiVM.IdArme))
                {
                    unSamourai.Arme = db.Armes.Find(unSamouraiVM.IdArme);
                }

                foreach(var idArtMartial in unSamouraiVM.IdsArtMartiaux)
                {
                    unSamourai.ArtMartiaux.Add(db.ArtMartials.Find(idArtMartial));
                }

                db.Samourais.Add(unSamourai);
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Edit(int? id)
        {
            SamouraiViewModel vm = new SamouraiViewModel();

            vm.Samourai = db.Samourais.Find(id);
            vm.Armes = db.Armes.Where(a => !db.Samourais.Any(s => s.Arme.Id == a.Id)).Select(a => new SelectListItem { Text = a.Nom, Value = a.Id.ToString() }).ToList();
            vm.ArtsMartiaux = db.ArtMartials.Select(am => new SelectListItem { Text = am.Nom, Value = am.Id.ToString() }).ToList();

            if (vm.Samourai.Arme != null)
            {
                vm.IdArme = vm.Samourai.Arme.Id;
                vm.Armes.Add(new SelectListItem { Text = vm.Samourai.Arme.Nom, Value = vm.Samourai.Arme.Id.ToString() });
            }

            if(vm.Samourai.ArtMartiaux != null && vm.Samourai.ArtMartiaux.Count() > 0)
            {
                vm.IdsArtMartiaux = vm.Samourai.ArtMartiaux.Select(am => am.Id).ToList();
            }

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(SamouraiViewModel unSamouraiVM)
        {
            try
            {
                Samourai unSamourai = db.Samourais.Find(unSamouraiVM.Samourai.Id);

                unSamourai.Nom = unSamouraiVM.Samourai.Nom;
                unSamourai.Force = unSamouraiVM.Samourai.Force;

                if(unSamouraiVM.IdArme == null)
                {
                    unSamourai.Arme = null;
                }
                else 
                {
                    if (!db.Samourais.Any(s => s.Arme.Id == unSamouraiVM.IdArme))
                    {
                        unSamourai.Arme = db.Armes.Find(unSamouraiVM.IdArme);
                    }
                }

                if(unSamouraiVM.IdsArtMartiaux.Count() > 0)
                {
                    unSamourai.ArtMartiaux.RemoveAll(am => am.Id > 0);
                    foreach (var idArtMartial in unSamouraiVM.IdsArtMartiaux)
                    {
                        unSamourai.ArtMartiaux.Add(db.ArtMartials.Find(idArtMartial));
                    }
                }

                db.Entry(unSamourai).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Delete(int? unId)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SamouraiViewModel unSamourai = new SamouraiViewModel();
            unSamourai.Samourai = db.Samourais.Find(unId);
            if (unSamourai == null)
            {
                return HttpNotFound();
            }
            samourai.Potentiel = calculerPotentiel(samourai);
            return View(samourai);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int unId)
        {
            Samourai unSamourai = db.Samourais.Find(unId);
            db.Samourais.Remove(unSamourai);
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
