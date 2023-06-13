using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace helloworld_dotnetcore5
{
    public class ProductContextDto
    {

        [Required]
        public string Portal { get; set; }
    }
}
