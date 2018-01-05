using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web_RailWay.Controllers
{
    public class tetsController : Controller
    {
        // GET: tets
        public ActionResult Index()
        {
            return View();
        }

        // GET: tets/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: tets/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: tets/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: tets/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: tets/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: tets/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: tets/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
