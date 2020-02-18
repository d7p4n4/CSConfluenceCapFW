using CSConfluenceClassesFW.DeletePage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSConfluenceCapFW
{
    public class DeletePageResult
    {
        public DeletePageFailedResponse FailedResponse { get; set; }
        public DeletePageSuccessResponse SuccessResponse { get; set; }
    }
}
