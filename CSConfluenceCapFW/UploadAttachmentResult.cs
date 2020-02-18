using CSConfluenceClassesFW.UploadAttachment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSConfluenceCapFW
{
    public class UploadAttachmentResult
    {
        public UploadAttachmentFailedResponse FailedResponse { get; set; }
        public UploadAttachmentSuccessResponse SuccessResponse { get; set; }
    }
}
