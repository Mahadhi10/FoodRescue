using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace helloworld_dotnetcore5
{
    public class SearchResponce
    {
        public string Status { get; set; }
        public string StatusDetails { get; set; }
        public string ImageType { get; set; }
        public ImageProperties ImageAttributes { get; set; }
        public List<ItemDetails> ItemList { get; set; }

        public SearchResponce()
        {
            Status = "Error";
            StatusDetails = string.Empty;
            ImageType = string.Empty;
            ImageAttributes = new ImageProperties();
            ItemList = new List<ItemDetails>();
        }
    }
}
