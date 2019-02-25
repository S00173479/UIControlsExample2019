using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using UIControlsExample.Models;

namespace UIControlsExample.Controllers
{
    public class UINodesController : Controller
    {
        private UIContext db = new UIContext();

        // GET: UINodes
        public ActionResult Index()
        {
            return View(db.SampleNodes.ToList());
        }

        // GET: UINodes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UINode uINode = db.SampleNodes.Find(id);
            if (uINode == null)
            {
                return HttpNotFound();
            }
            return View(uINode);
        }

        // GET: UINodes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: UINodes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "NodeID,FromDate,ToDate,NumberOfHits")] UINode uINode)
        {
            if (ModelState.IsValid)
            {
                db.SampleNodes.Add(uINode);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(uINode);
        }

        // GET: UINodes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UINode uINode = db.SampleNodes.Find(id);
            if (uINode == null)
            {
                return HttpNotFound();
            }
            return View(uINode);
        }

        // POST: UINodes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "NodeID,FromDate,ToDate,NumberOfHits")] UINode uINode)
        {
            if (ModelState.IsValid)
            {
                db.Entry(uINode).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(uINode);
        }

        // GET: UINodes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UINode uINode = db.SampleNodes.Find(id);
            if (uINode == null)
            {
                return HttpNotFound();
            }
            return View(uINode);
        }

        // POST: UINodes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            UINode uINode = db.SampleNodes.Find(id);
            db.SampleNodes.Remove(uINode);
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
