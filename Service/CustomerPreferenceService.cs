using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Npgsql;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace helloworld_dotnetcore5.Service
{
    public partial class CustomerPreferenceService : ICustomerPreferenceService
    {
        private readonly string _pgDBConnectionString = string.Empty;
        private readonly string _imageProcessorAPI = string.Empty;

        public CustomerPreferenceService(IOptions<ConfigurationSettings> optionsAccessor)
        {
            _pgDBConnectionString = optionsAccessor.Value.PGConnectionString;
            _imageProcessorAPI = optionsAccessor.Value.ImageProcessApiEndpoint;
        }

        public responseDto AddCustomerPreference(SearchRequest request)
        {
            try
            {
                var client = new RestClient(_imageProcessorAPI);
                var req = new RestRequest();
                req.AddJsonBody(new { ImageURL = request.ImageURL });
                req.AddHeader("Content-Type", "application/json");

                var response = client.Post(req);
                var content = response.Content; // Raw content as string

                SearchResponce deptObj = JsonConvert.DeserializeObject<SearchResponce>(content);

                //Insert Data
                var connectionString = _pgDBConnectionString;
                long custID = 0;
                using (var con = new NpgsqlConnection(connectionString))
                {
                    con.Open();

                    //Get Item Type
                    List<ItemType> itemTypeList = new List<ItemType>();

                    string sql = "Select item_type from item_type";
                    using (var cmd = new NpgsqlCommand(sql, con))
                    {
                        using (var rdr = cmd.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                ItemType itmType = new ItemType();
                                itmType.Item_Type = rdr.GetString(0);
                                itemTypeList.Add(itmType);
                            }
                            rdr.Close();
                        }
                    }

                    var selectedItemType = deptObj.ImageAttributes.ImgLabels.OrderByDescending(x => x.Score).Where(x => itemTypeList.Any(y => y.Item_Type == x.Description))?.FirstOrDefault();
                    var strItemType = "Necklace";
                    if (selectedItemType != null)
                    {
                        strItemType = ((ImageLabels)selectedItemType).Description;
                    }


                    //Get Material Type

                    List<MaterialType> materialTypeList = new List<MaterialType>();

                    sql = "Select material_type from material_type";
                    using (var cmd = new NpgsqlCommand(sql, con))
                    {
                        using (var rdr = cmd.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                MaterialType mateType = new MaterialType();
                                mateType.Material_Type = rdr.GetString(0);
                                materialTypeList.Add(mateType);
                            }
                            rdr.Close();
                        }
                    }

                    var selectedMaterialType = deptObj.ImageAttributes.ImgLabels.OrderByDescending(x => x.Score).Where(x => materialTypeList.Any(y => y.Material_Type == x.Description))?.FirstOrDefault();
                    var strMateType = "Gold";
                    if (selectedMaterialType != null)
                    {
                        strMateType = ((ImageLabels)selectedMaterialType).Description;
                    }

                    //Get Color

                    string imgColor = deptObj.ImageAttributes.ImgColors.OrderByDescending(x => x.Score).FirstOrDefault<ImageColor>().HexaCode;

                    sql = "Select customer_id from customer where email_id='" + request.EmailAddress + "'";
                    using (var cmd = new NpgsqlCommand(sql, con))
                    {
                        using (var rdr = cmd.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                custID = (long)rdr[0];
                            }
                            rdr.Close();
                        }
                    }

                    //Check before insert
                    int count = 0;
                    sql = "Select count(1) from customer_pref where customer_id=" + custID + " and item_type='" + strItemType + "' and material_type='" + strMateType + "' and item_color='" + imgColor + "'";
                    using (var cmd = new NpgsqlCommand(sql, con))
                    {
                        using (var rdr = cmd.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                count = rdr.GetInt32(0);
                            }
                            rdr.Close();
                        }
                    }

                    if (count == 0)
                    {
                        NpgsqlCommand command = new NpgsqlCommand("Insert Into customer_pref (customer_pref_key,customer_id,item_type,material_type,reco_type,item_color,created_on ) values (@customer_pref_key, @customer_id,@item_type,@material_type,@reco_type,@item_color,@created_on); ", con);
                        command.Parameters.AddWithValue("customer_pref_key", NpgsqlTypes.NpgsqlDbType.Bigint, DateTime.Now.Ticks);
                        command.Parameters.AddWithValue("customer_id", custID);
                        command.Parameters.AddWithValue("item_type", strItemType);
                        command.Parameters.AddWithValue("material_type", strMateType);
                        command.Parameters.AddWithValue("reco_type", "Recomendation");
                        command.Parameters.AddWithValue("item_color", imgColor);
                        command.Parameters.AddWithValue("created_on", DateTime.Now);
                        command.ExecuteNonQuery();
                        command.Dispose();
                        con.Close();
                    }

                }

                responseDto res = new responseDto();
                res.Status = "Success";
                return res;
            }
            catch (Exception ex)
            {
                responseDto res = new responseDto();
                res.Status = "Fail";
                res.Error = ex.Message;
                return res;
            }
        }

        public responseDto DeleteCustomerPreference()
        {
            try
            {

                //Delete Data
                var connectionString = _pgDBConnectionString;
                long custID = 0;
                using (var con = new NpgsqlConnection(connectionString))
                {
                    con.Open();

                    NpgsqlCommand command = new NpgsqlCommand("delete from customer_pref where customer_id=20211218184101;", con);
                    command.ExecuteNonQuery();
                    command.Dispose();
                    con.Close();
                    responseDto res = new responseDto();
                    res.Status = "Success";
                    return res;
                }
            }
            catch (Exception ex)
            {
                responseDto res = new responseDto();
                res.Status = "Fail";
                res.Error = ex.Message;
                return res;
            }
        }

    }

    public interface ICustomerPreferenceService
    {
        responseDto AddCustomerPreference(SearchRequest request);
        responseDto DeleteCustomerPreference();
    }
}
