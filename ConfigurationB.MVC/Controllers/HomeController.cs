using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ConfigurationB.MVC.Models;
using ConfigurationB.Management.Repositories;
using ConfigurationB.Management.Entities;

namespace ConfigurationB.MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly IAsyncRepository<ConfigurationItem> _configurationItemRepository;
        public HomeController(IAsyncRepository<ConfigurationItem> configurationItemRepository)
        {
            _configurationItemRepository = configurationItemRepository;
        }

        public async Task<IActionResult> Index()
        {
            List<ConfigurationItem> configurationItems = await _configurationItemRepository.ListAllAsync();
            return View(configurationItems);
        }

        [HttpPost]
        public async Task<IActionResult> Index(string name)
        {
            List<ConfigurationItem> configurationItems = await _configurationItemRepository.ListAsync(x => x.Name.Contains(name));
            return View(configurationItems);
        }

        // GET: ConfigurationItems/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var configurationItem = await _configurationItemRepository.GetByIdAsync(id.GetValueOrDefault());
            if (configurationItem == null)
            {
                return NotFound();
            }

            return View(configurationItem);
        }

        // GET: ConfigurationItems/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ConfigurationItems/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Type,Value,IsActive,ApplicationName")] ConfigurationItem configurationItem)
        {
            if (ModelState.IsValid)
            {
                await _configurationItemRepository.AddAsync(configurationItem);
                return RedirectToAction(nameof(Index));
            }
            return View(configurationItem);
        }

        // GET: ConfigurationItems/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var configurationItem = await _configurationItemRepository.GetByIdAsync(id.GetValueOrDefault());
            if (configurationItem == null)
            {
                return NotFound();
            }
            return View(configurationItem);
        }

        // POST: ConfigurationItems/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Type,Value,IsActive,ApplicationName")] ConfigurationItem configurationItem)
        {
            if (id != configurationItem.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _configurationItemRepository.UpdateAsync(configurationItem);
                }
                catch (Exception ex)
                {
                    return NotFound();
                }
                return RedirectToAction(nameof(Index));
            }
            return View(configurationItem);
        }

        // GET: ConfigurationItems/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var configurationItem = await _configurationItemRepository.GetByIdAsync(id.GetValueOrDefault());
            if (configurationItem == null)
            {
                return NotFound();
            }

            return View(configurationItem);
        }

        // POST: ConfigurationItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var configurationItem = await _configurationItemRepository.GetByIdAsync(id);
            await _configurationItemRepository.DeleteAsync(configurationItem);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
