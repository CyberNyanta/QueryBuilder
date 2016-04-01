using QueryBuilder.DAL.Models;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;

namespace QueryBuilderMVC.Models
{
    public class ProjectViewModel
    {
        public IEnumerable<Project> Projects { get; set; }

        [Required(ErrorMessage = "Please enter project name")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Enter description")]
        public string Description { get; set; }
        public ConnectionViewModel ConnectionDb { get; set; }
    }
}