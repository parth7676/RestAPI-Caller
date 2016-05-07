using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvalaraRestAPIHelper.Entities
{
    public class Response
    {
        public Enums.Status Status { get; set; }

        public AvalaraSuccessResponse SuccessResponse { get; set; }
        public AvalaraErrorResponse ErrorResponse { get; set; }
    }
}
