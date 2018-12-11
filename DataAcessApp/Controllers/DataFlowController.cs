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
        static UnDataService service;
        static List<DataFlow> flows;
        static DataStructure struc;
        // GET: DataFlow
        public ActionResult DataFlowIndex()
        {
            flows = new List<DataFlow>();
            service = new UnDataService();
            flows = service.AllDataFlows;
            return View(flows);
        }

        // GET: DataFlow/Details/5
        public ActionResult Details(string id)
        {
            DataFlow flow = flows.Single(flo => flo.Id == id);
            DataStructure structure = flow.GetDataStructure();
            structure.GetCodeLists();
            struc = structure;
            return View(structure);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Submit(DataStructure str)
        {
            List<string> selectedCodes = new List<string>();
            foreach(Dimension dim in str.DimensionList)
            {
                selectedCodes.Add(dim.SelectedCode);
            }
            QueryResult result =service.GetDataFromDataFlowAndDimensions(str.Id, selectedCodes).Result;
            result.BuildDisplayName(struc.DimensionList, selectedCodes);
            return View("DataResult", result);
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