using BudgetApplication.Base.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetApplication.Base.EventArgs
{
    public class MessageViewerEventArgs
    {
        public MessageViewerResult Result { get; set; }

        public MessageViewerEventArgs(MessageViewerResult result = MessageViewerResult.Ok)
        {
            this.Result = result;
        }
    }
}
