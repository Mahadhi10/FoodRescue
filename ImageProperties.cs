using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace helloworld_dotnetcore5
{
    public class ImageProperties
    {
        public List<ImageLabels> ImgLabels { get; set; }
        public List<ImageColor> ImgColors { get; set; }
        public ImageProperties()
        {
            ImgLabels = new List<ImageLabels>();
            ImgColors = new List<ImageColor>();
        }
    }

    public class ImageLabels
    {
        public string Description { get; set; }
        public string Score { get; set; }
    }

    public class ImageColor
    {
        public string Red { get; set; }
        public string Blue { get; set; }
        public string Green { get; set; }
        public string HexaCode { get; set; }
        public string Score { get; set; }
    }
}
