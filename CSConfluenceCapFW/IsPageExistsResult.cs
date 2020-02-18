using CSConfluenceClassesFW.IsPageExists;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSConfluenceCapFW
{
    public class IsPageExistsResult
    {
        public IsPageExistsFailedResponse FailedResponse { get; set; }
        public IsPageExistsSuccessResponse SuccessResponse { get; set; }
    }
}
