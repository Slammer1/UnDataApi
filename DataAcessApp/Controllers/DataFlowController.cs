using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnDataApi.Services;
using UnDataApi.DataModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DataAcessApp.Controllers
{
    public class DataFlowController : Controller
    {
        // GET: DataFlow
        public ActionResult DataFlowIndex()
        {
            List<DataFlow> flow = new List<DataFlow>();
            UnDataService service = new UnDataService();
            flow = service.AllDataFlows;
            return View(flow);
        }

        // GET: DataFlow/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: DataFlow/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: DataFlow/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction(nameof(DataFlowIndex));
            }
            catch
            {
                return View();
            }
        }

        // GET: DataFlow/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: DataFlow/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(DataFlowIndex));
            }
            catch
            {
                return View();
            }
        }

        // GET: DataFlow/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: DataFlow/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(DataFlowIndex));
            }
            catch
            {
                return View();
            }
        }
    }
}