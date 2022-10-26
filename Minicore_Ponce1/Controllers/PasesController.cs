using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Minicore_Ponce1.Models;

namespace Minicore_Ponce1.Controllers
{
    public class PasesController : Controller
    {
        private miniCoreDbContext db = new miniCoreDbContext();

        // GET: Pases
        public ActionResult Index()
        {
            var pases = db.Pases.Include(p => p.Usuario);
            return View(pases.ToList());
        }

        public ActionResult Calcular(DateTime fechainicio)
        {
            DateTime hoy = DateTime.Now;
            var pases = db.Pases.Include(p => p.Usuario).ToList();
            List<Pase> activos = new List<Pase>();
            foreach (Pase pass in pases)
            {
                TimeSpan res = hoy - fechainicio;
                double pasesUsados = res.Days;
                if (pass.TtipoPase.Equals("Mensual"))
                {
                    double pasesRestantes = 96 - pasesUsados;
                    pass.FinTentativo = hoy.AddDays(pasesRestantes);
                    pass.pasesRestantes = Convert.ToInt32(pasesRestantes);
                    pass.saldoRestante = pasesRestantes * 0.26;

                }
                else if (pass.TtipoPase.Equals("Semestral"))
                {
                    double pasesRestantes = 576 - pasesUsados;
                    pass.FinTentativo = hoy.AddDays(pasesRestantes);
                    pass.pasesRestantes = Convert.ToInt32(pasesRestantes);
                    pass.saldoRestante = pasesRestantes * 0.087;
                }
                else
                {
                    double pasesRestantes = 1052 - pasesUsados;
                    pass.FinTentativo = hoy.AddDays(pasesRestantes);
                    pass.pasesRestantes = Convert.ToInt32(pasesRestantes);
                    pass.saldoRestante = pasesRestantes * 0.076;
                }

                if (pass.pasesRestantes > 0)
                {
                    activos.Add(pass);
                }
                db.Entry(pass).State = EntityState.Modified;
                db.SaveChanges();
            }

            return View("Index", activos);
        }

        // GET: Pases/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pase pase = db.Pases.Find(id);
            if (pase == null)
            {
                return HttpNotFound();
            }
            return View(pase);
        }

        // GET: Pases/Create
        public ActionResult Create()
        {
            ViewBag.UsuarioID = new SelectList(db.Usuarios, "UsuarioID", "Nombre");
            return View();
        }

        // POST: Pases/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PaseID,UsuarioID,FechaCompra,TtipoPase")] Pase pase)
        {
            if (ModelState.IsValid)
            {
                db.Pases.Add(pase);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.UsuarioID = new SelectList(db.Usuarios, "UsuarioID", "Nombre", pase.UsuarioID);
            return View(pase);
        }

        // GET: Pases/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pase pase = db.Pases.Find(id);
            if (pase == null)
            {
                return HttpNotFound();
            }
            ViewBag.UsuarioID = new SelectList(db.Usuarios, "UsuarioID", "Nombre", pase.UsuarioID);
            return View(pase);
        }

        // POST: Pases/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PaseID,UsuarioID,FechaCompra,TtipoPase")] Pase pase)
        {
            if (ModelState.IsValid)
            {
                db.Entry(pase).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UsuarioID = new SelectList(db.Usuarios, "UsuarioID", "Nombre", pase.UsuarioID);
            return View(pase);
        }

        // GET: Pases/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pase pase = db.Pases.Find(id);
            if (pase == null)
            {
                return HttpNotFound();
            }
            return View(pase);
        }

        // POST: Pases/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Pase pase = db.Pases.Find(id);
            db.Pases.Remove(pase);
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
