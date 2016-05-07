using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvalaraRestAPIHelper.Entities
{
    public class RestAPIResponse
    {
        public Enums.Status Status { get; set; }
        public string ResponseData { get; set; }
    }
}
