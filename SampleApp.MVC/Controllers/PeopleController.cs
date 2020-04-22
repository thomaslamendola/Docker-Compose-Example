using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FizzWare.NBuilder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SampleApp.MVC.Models;
using SampleApp.MVC.Repositories;

namespace SampleApp.MVC.Controllers
{
    public class PeopleController : Controller
    {
        private readonly IMongoRepository<Person> _peopleRepository;
        private readonly ILogger<PeopleController> _logger;

        public PeopleController(IMongoRepository<Person> peopleRepository, ILogger<PeopleController> logger)
        {
            _peopleRepository = peopleRepository;
            _logger = logger;
        }

        public IActionResult Index()
        {
            var people = _peopleRepository.FilterBy(
                filter => filter.FirstName != "test",
                projection => projection.FirstName
            );

            _logger.LogInformation("Returning: " + JsonConvert.SerializeObject(people));

            return View(people);
        }
    }
}