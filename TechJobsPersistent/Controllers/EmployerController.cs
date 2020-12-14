using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TechJobsPersistent.Models;
using TechJobsPersistent.ViewModels;
using TechJobsPersistent.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TechJobsPersistent.Controllers
{
    public class EmployerController : Controller
    {
        private JobDbContext context;


        public EmployerController(JobDbContext dbContext)
        {
            context = dbContext;
        }

        // GET: /<controller>/
        [HttpGet]
        public IActionResult Index()
        {
            List<Employer> employers = context.Employers.ToList();
            return View(employers);
        }

        public IActionResult Add()
        {
            AddEmployerViewModel Employer = new AddEmployerViewModel();
            return View(Employer);
        }

        [HttpPost]
        public IActionResult Add(Employer employer)
        {
            if (ModelState.IsValid)
            {
                context.Employers.Add(employer);
                context.SaveChanges();
                return Redirect("/employer/");
            }

            return View("Add", employer);
        }

        [HttpPost]
        [Route("/employer/add")]
        public IActionResult ProcessAddEmployerForm(AddEmployerViewModel viewModel)
        {
            if (ModelState.IsValid)
            {

                string name = viewModel.Name;
                string location = viewModel.Location;

                List<Employer> emps = context.Employers
                    .Where(emp => emp.Name == name)
                    .Where(emp => emp.Location == location)
                    .ToList();

                if (emps.Count == 0)
                {
                    Employer employer = new Employer
                    {
                        Name = name,
                        Location = location
                    };
                    context.Employers.Add(employer);
                    context.SaveChanges();
                }

                return Redirect("/Home/AddJob");
            }

            return View(viewModel);
        }

        public IActionResult About(int id)
        {
            List<Employer> employers = context.Employers
                   .Where(emp => emp.Id == id)
                   .Include(emp => emp.Name)
                   .Include(emp => emp.Location)
                   .ToList();

            return View(employers);
        }
    }
}
