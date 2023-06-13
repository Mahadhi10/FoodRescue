using System;

namespace helloworld_dotnetcore5
{
    public class Users
    {
        public int UserID { get; set; }
        public string? Mobile { get; set; }
        public bool IsServiceProvider { get; set; }
        public string? Longitude { get; set; }
        public string? Lantitude { get; set; }
        public DateTime RegisterDated { get; set; }
    }
}
