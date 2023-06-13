using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace helloworld_dotnetcore5.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly IWebHostEnvironment _hostingEnvironment;

        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IWebHostEnvironment hostingEnvironment)
        {
            _logger = logger;
            _hostingEnvironment = hostingEnvironment;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            _logger.LogInformation("information", "The get method is invoked");
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }
        [HttpGet]
        [Route("Create")]
        public ActionResult<IEnumerable<Customer>> Create()
        {
            _logger.LogInformation("information", "The create method is invoked");
            List<Customer> people = new List<Customer>
            {
                new Customer {EmailId = "Test1@email.com", CustomerPwd="test1password", FirstName = "John", LastName = "Doe",PhoneNumber="1234567890",PStation="Test1Station",
                            PSPhoneNumber="9876543210", Address1="Address Line 1",Address2="Address Line 2",PinCode="12345",UserType=UserType.Social_Organisation.GetStringValue()},
                new Customer {EmailId = "Test2@email.com", CustomerPwd="test2password", FirstName = "Jane", LastName = "Smith",PhoneNumber="1234567890",PStation="Test2Station",
                            PSPhoneNumber="9876543210", Address1="Address Line 1",Address2="Address Line 2",PinCode="12345",UserType=UserType.Police.GetStringValue()},
                new Customer {EmailId = "Test3@email.com", CustomerPwd="test3password", FirstName = "Jane", LastName = "Smith",PhoneNumber="1234567890",PStation="Test2Station",
                            PSPhoneNumber="9876543210", Address1="Address Line 1",Address2="Address Line 2",PinCode="12345",UserType=UserType.Police.GetStringValue()}
            };

            string json = JsonConvert.SerializeObject(people);

            string filePath = Path.Combine(_hostingEnvironment.ContentRootPath, "TestData.json");

            System.IO.File.WriteAllText(filePath, json);
            return Ok("Data saved successfully");
        }
        [HttpPost]
        [Route("Login")]
        public IActionResult SignIn([FromBody] Customer obj)
        {
            string filePath = Path.Combine(_hostingEnvironment.ContentRootPath, "TestData.json"); // Specify the path to your JSON file

            if (System.IO.File.Exists(filePath))
            {
                string jsonData = System.IO.File.ReadAllText(filePath);
                var userData = JsonConvert.DeserializeObject<List<Customer>>(jsonData);
                if (userData != null)
                {
                    foreach (var item in userData)
                    {
                        // Perform your validation logic here
                        if (item.PhoneNumber == obj.PhoneNumber && item.CustomerPwd == obj.CustomerPwd)
                        {
                            return Ok("Username is valid.");
                        }
                    }
                }
            }

            return BadRequest("User is invalid.");
        }
        [HttpGet]
        [Route("FoodRescueData")]
        public ActionResult<IEnumerable<Customer>> FoodRescueData()
        {
            _logger.LogInformation("information", "The GetJsonData method is invoked");

            string filePath = Path.Combine(_hostingEnvironment.ContentRootPath, "TestData.json");

            if (System.IO.File.Exists(filePath))
            {
                string jsonData = System.IO.File.ReadAllText(filePath);
                return Ok(jsonData);
            }
            else
            {
                return BadRequest("No Records");
            }

           
        }
    }
}
