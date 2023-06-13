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
    public class FoodRescueController : ControllerBase
    {
        private readonly IWebHostEnvironment _hostingEnvironment;

        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<FoodRescueController> _logger;

        public FoodRescueController(ILogger<FoodRescueController> logger, IWebHostEnvironment hostingEnvironment)
        {
            _logger = logger;
            _hostingEnvironment = hostingEnvironment;
        }

        [HttpPost]
        [Route("Create")]
        public ActionResult<IEnumerable<Customer>> Create(Customer obj)
        {
            _logger.LogInformation("information", "The Create method is invoked");
            var jsonObject = new JsonResponse { Result = true, STATUS = "SUCCESS" };

            string filePath = Path.Combine(_hostingEnvironment.ContentRootPath, "UserData.json"); // Specify the path to your JSON file

            if (System.IO.File.Exists(filePath))
            {
                string jsonData = System.IO.File.ReadAllText(filePath);
                var people = JsonConvert.DeserializeObject<List<Customer>>(jsonData);
                if(people == null)
                {
                    people = new List<Customer>();
                }

                if (obj != null)
                {
                    var newData = new Customer
                    {
                        EmailId = obj.EmailId,
                        CustomerPwd = obj.CustomerPwd,
                        FirstName = obj.FirstName,
                        LastName = obj.LastName,
                        PhoneNumber = obj.PhoneNumber,
                        PStation = obj.PStation,
                        PSPhoneNumber = obj.PSPhoneNumber,
                        Address1 = obj.Address1,
                        Address2 = obj.Address2,
                        PinCode = obj.PinCode,
                        UserType = obj.UserType
                    };

                    people.Add(newData);
                }

                string updatedjson = JsonConvert.SerializeObject(people,Formatting.Indented);
                System.IO.File.WriteAllText(filePath, updatedjson);
                jsonObject.Result = true;
                jsonObject.STATUS = "SUCCESS";
                return Ok(jsonObject);
            }
            else
            {
                jsonObject.Result = false;
                jsonObject.STATUS = "ERROR";
                return Ok(jsonObject);
            }
            
        }
        [HttpPost]
        [Route("Login")]
        public IActionResult Login([FromBody] Customer obj)
        {
            _logger.LogInformation("information", "The Login method is invoked");

            string filePath = Path.Combine(_hostingEnvironment.ContentRootPath, "UserData.json"); // Specify the path to your JSON file

            Customer res = new Customer();
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
                            res.FirstName = item.FirstName;
                            res.LastName = item.LastName;
                            res.PhoneNumber = item.PhoneNumber;
                            res.UserType = item.UserType;
                            res.PinCode = item.PinCode;
                            res.PStation = item.PStation;
                            res.PSPhoneNumber = item.PSPhoneNumber;
                            res.status = "SUCCESS";
                            return Ok(res);
                        }
                    }
                }
            }
            res.status = "VALIDATIONERROR";
            return Ok(res);
        }
        [HttpGet]
        [Route("GetCustomerData")]
        public ActionResult<IEnumerable<Customer>> GetCustomerData()
        {
            _logger.LogInformation("information", "The GetCustomerData method is invoked");

            string filePath = Path.Combine(_hostingEnvironment.ContentRootPath, "UserData.json");

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
