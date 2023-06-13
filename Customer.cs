using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace helloworld_dotnetcore5
{
    public class Customer
    {
        public string EmailId { get; set; }
        public string CustomerPwd { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string PStation { get; set; }
        public string PSPhoneNumber { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string PinCode { get; set; }
        public string UserType { get; set; }
        public string status { get; set; }
    }
    public class JsonResponse
    {
        public bool Result { get; set; }
        public string STATUS { get; set; }
    }
    public enum UserType
    {
        Social_Organisation,
        Police,
        Event_Organiser
    }
    public static class EnumExtensions
    {
        public static string GetStringValue(this UserType value)
        {
            switch (value)
            {
                case UserType.Social_Organisation:
                    return "Social Organisation";
                case UserType.Police:
                    return "Police";
                case UserType.Event_Organiser:
                    return "Event Organiser";
                default:
                    throw new ArgumentOutOfRangeException(nameof(value), value, "Unknown enum value");
            }
        }
    }
}
