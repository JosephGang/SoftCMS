﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SoftCMS.Models;
using SoftCMS.IDatabaseRepository;

namespace SoftCMS.Areas.Manager.Controllers
{
    [Authorize(Users = "wang@mail.com")]
    public class MainThemesController : Controller
    {
        private SoftContext db = new SoftContext();
        private IDatabaseRepository.IDatabaseRepository repository = null;

        public MainThemesController()
        {
            this.repository = new MainThemeRepository(db);
        }

        // GET: Manager/MainThemes
        public async Task<ActionResult> Index()
        {
            return View(await db.MainThemes.ToListAsync());
        }

        // GET: Manager/MainThemes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Manager/MainThemes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        async public Task<ActionResult> Create([Bind(Include = "ID,ContentText,PublichDate,CreateUser")] MainThemes mainThemes)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await repository.Insert(mainThemes);
                    return RedirectToAction("Index");
                }
            }
            catch (DataException)
            {
                ModelState.AddModelError("", "無法儲存，請聯絡網站管理員");
            }

            return View(mainThemes);
        }

        // GET: Manager/MainThemes/Edit/5
        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MainThemes mainThemes = await db.MainThemes.FindAsync(id);
            if (mainThemes == null)
            {
                return HttpNotFound();
            }
            return View(mainThemes);
        }

        // POST: Manager/MainThemes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        async public Task<ActionResult> Edit([Bind(Include = "ID,ContentText,PublichDate,CreateUser")] MainThemes mainThemes)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await repository.Update(mainThemes);
                    return RedirectToAction("Index");
                }
            }
            catch (DataException)
            {
                ModelState.AddModelError("", "無法儲存，請聯絡網站管理員");
            }
            return View(mainThemes);
        }

        // GET: Manager/MainThemes/Delete/5
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MainThemes mainThemes = await db.MainThemes.FindAsync(id);
            if (mainThemes == null)
            {
                return HttpNotFound();
            }
            return View(mainThemes);
        }

        // POST: Manager/MainThemes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        async public Task<ActionResult> DeleteConfirmed(Guid id)
        {
            try
            {
                await repository.Delete(id);
            }
            catch (DataException)
            {
                ModelState.AddModelError("", "無法儲存，請聯絡網站管理員");
            }
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
