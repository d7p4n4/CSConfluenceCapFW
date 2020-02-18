using CSConfluenceClassesFW.GetIdByTitle;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSConfluenceCapFW
{
    public class GetIdByTitleResult
    {
        public GetIdByTitleFailedResponse FailedResponse { get; set; }
        public GetIdByTitleSuccessResponse SuccessResponse { get; set; }
    }
}
