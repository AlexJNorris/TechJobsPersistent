using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using TechJobsPersistent.Models;

namespace TechJobsPersistent.ViewModels
{
    public class AddJobViewModel
    {
        [Required(ErrorMessage = "Job Name is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Employer is required")]
        public int EmployerId { get; set; }

        public List<SelectListItem> Employers { get; set; }

        public string[] SelectedSkills;

        public List<SelectListItem> Skills { get; set; }
        public Employer Employer { get; internal set; }

        public AddJobViewModel()
        {
        }
        public AddJobViewModel(string name, int employerId, string[] selectedSkills)
        {
            Name = name;
            EmployerId = employerId;
            SelectedSkills = selectedSkills;

            }
        }
}
