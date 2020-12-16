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

namespace TechJobsPersistent.Controllers
{
    public class HomeController : Controller
    {
        private JobDbContext context;

        public HomeController(JobDbContext dbContext)
        {
            context = dbContext;
        }

        public IActionResult Index()
        {
            List<Job> jobs = context.Jobs.Include(j => j.Employer).ToList();

            return View(jobs);
        }

        [HttpGet("/Add")]
        public IActionResult AddJob(AddJobViewModel viewModel)
        {
            ViewBag.Employers = context.Employers.ToList();
            ViewBag.Skills = context.Skills.ToList();
            AddJobViewModel Job = new AddJobViewModel(viewModel.Name, viewModel.EmployerId, context.Skills.ToList());
            return View(Job);
        }

        public IActionResult ProcessAddJobForm(AddJobViewModel viewModel, string[] selectedSkills)
        {
            if (ModelState.IsValid)
            {

                string name = viewModel.Name;
                int employerId = viewModel.EmployerId;
                int index;

                 foreach (string skillName in selectedSkills)
                {
                    Job newJob = new Job();
                    index = Array.IndexOf(context.Skills.ToArray(), skillName);

                    JobSkill newJobSkill = new JobSkill(newJob);
                    newJobSkill.SkillId = index;
                    context.JobSkills.Add(newJobSkill);

                    List<Job> emps = context.Jobs
                    .Where(emp => emp.Name == name)
                    .Where(emp => emp.EmployerId == employerId)
                    .ToList();

                    if (emps.Count == 0)
                    {
                        newJob.Name = name;
                        newJob.EmployerId = employerId;

                        context.Jobs.Add(newJob);

                    }
                }
                

                context.SaveChanges();

            }

            return Redirect("/Add");
        }

        public IActionResult Detail(int id)
        {
            Job theJob = context.Jobs
                .Include(j => j.Employer)
                .Single(j => j.Id == id);

            List<JobSkill> jobSkills = context.JobSkills
                .Where(js => js.JobId == id)
                .Include(js => js.Skill)
                .ToList();

            JobDetailViewModel viewModel = new JobDetailViewModel(theJob, jobSkills);
            return View(viewModel);
        }
    }
}
