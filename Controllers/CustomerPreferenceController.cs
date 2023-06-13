using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RestSharp;
using Npgsql;
using System.Text.Json;
using Newtonsoft.Json;
using helloworld_dotnetcore5.Service;

namespace helloworld_dotnetcore5.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CustomerPreferenceController : ControllerBase
    {
        private ICustomerPreferenceService _preference;

        private readonly ILogger<CustomerPreferenceController> _logger;

        public CustomerPreferenceController(ILogger<CustomerPreferenceController> logger,ICustomerPreferenceService CustomerPreferenceService)
        {
            _logger = logger;
            _preference = CustomerPreferenceService;
        }

        [HttpPost("customerPreference")]
        public List<CustomerPreference> GetCustomerPreference([FromBody] SearchRequest request)
        {
            var client = new RestClient("https://tiffanyimageprocessor.container-crush-02-4044f3a4e314f4bcb433696c70d13be9-0000.eu-de.containers.appdomain.cloud/ImageProcess/ProcessByURL");
            // client.Authenticator = new HttpBasicAuthenticator(username, password);
            var req = new RestRequest();
            req.AddJsonBody(new { ImageURL = "https://media.tiffany.com/is/image/Tiffany/EcomItemM/tiffany-true-engagement-ring-with-a-tiffany-true-diamond-in-platinum-63594873_996049_ED_M.jpg?&op_usm=1.75,1.0,6.0&$cropN=0.1,0.1,0.8,0.8&defaultImage=NoImageAvailableInternal&" });
            req.AddHeader("Content-Type", "application/json");

            var response = client.Post(req);
            var content = response.Content; // Raw content as string

            SearchResponce deptObj = JsonConvert.DeserializeObject<SearchResponce>(content);



            var connectionString = "Host=172.30.136.71;Port=5432;Username=tiffanycodebigrade;Password=tiffanycodebigrade;Database=tiffanycodebigrade-db";
            using (var con = new NpgsqlConnection(connectionString))
            {
                con.Open();
                //NpgsqlCommand command = new NpgsqlCommand("UPDATE customer SET email_id ='sonaskar@in.ibm.com' WHERE customer_id =20211119103301;", con);
                NpgsqlCommand command = new NpgsqlCommand("Insert Into customer_pref (customer_pref_key,customer_id,item_type,material_type,reco_type,item_color,created_on ) values (@customer_pref_key, @customer_id,@item_type,@material_type,@reco_type,@item_color,@created_on); ", con);
                command.Parameters.AddWithValue("customer_pref_key", NpgsqlTypes.NpgsqlDbType.Bigint, 10002);
                command.Parameters.AddWithValue("customer_id", 2021);
                command.Parameters.AddWithValue("item_type", "Necklace");
                command.Parameters.AddWithValue("material_type", "Gold");
                command.Parameters.AddWithValue("reco_type", "Gold");
                command.Parameters.AddWithValue("item_color", "Red");
                command.Parameters.AddWithValue("created_on", DateTime.Now);
                command.ExecuteNonQuery();
                command.Dispose();
                con.Close();

                List<CustomerPreference> lstCusPref = new List<CustomerPreference>();

                CustomerPreference custPref = new CustomerPreference();
                custPref.EmailAddress = "manastewary@yahoo.co.in";
                custPref.VisitedShoppingCartApp = "Facebook";
                custPref.ProductType = "T Shirt";
                custPref.Color = "Red";
                custPref.Size = "XL";
                custPref.Price = 1500.80;

                lstCusPref.Add(custPref);

                custPref = new CustomerPreference();
                custPref.EmailAddress = "tapastewary@yahoo.co.in";
                custPref.VisitedShoppingCartApp = "Twiter";
                custPref.ProductType = "Round Neck T Shirt";
                custPref.Color = "Blue";
                custPref.Size = "XL";
                custPref.Price = 1700.80;

                lstCusPref.Add(custPref);

                return lstCusPref;

                //using (var rdr = cmd.ExecuteReader())
                //{

                //    while (rdr.Read())
                //    {

                //    }
                //}

            }

            
        }

        [HttpPost("AddCustomerPreference")]
        public IActionResult SetCustomerPreference([FromBody] SearchRequest req)
        {
            try
            {
                if (req == null)
                    return base.BadRequest("Request is malformed.");
           
          
                return base.Ok(_preference.AddCustomerPreference(req));
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpPost("DeleteCustomerPreference")]
        public IActionResult DeleteCustomerPreference()
        {
            try
            {
               
                return base.Ok(_preference.DeleteCustomerPreference());
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
