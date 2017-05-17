using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace UploadTexts
{
    public class TextMessage
    {
        public TextMessage(long MessageID = 0, long ThreadId = 0, string Address = null, long Timestamp = 0, string Body = null)
        {

            messageId = MessageID;
            threadId = threadId;
            address = Address;
            timestamp = Timestamp;
            body = Body;

        }
        public long messageId { get; set; }
        public long threadId { get; set; }
        public string address { get; set; }
        public long timestamp { get; set; }
        public string body { get; set; }
    }

}