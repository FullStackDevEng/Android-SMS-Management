using Android.App;
using Android.Widget;
using Android.OS;
using System;
using System.Net;
using System.IO;

namespace UploadTexts
{
    [Activity(Label = "UploadTexts", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
		private static string SMS_DIRECTORY = "content://sms";
		private static string INBOX = "inbox";
		private static string UNREAD_CONDITION = "read=0 and ";
		private static string SMS_BASE_QUERY = "date>0 and body is not null and body != ''";
		private static string DATE_FILTER = "date ASC";

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

			var SMS = getSMS(true); // true for all unread messages only and false (default) for all SMS

            if (SMS != null)
            {
				// do something here with the SMS 
             	
            }
        }
        private void WebCall(string url)
        {
            try
            {               
                string jsonResult = GET(url);
                string result = jsonResult;
            }
			catch (Exception e) { DisplayLongToast(e.ToString()); }
        }
        private void DisplayLongToast(string message)
        {
            Toast.MakeText(this, message, ToastLength.Long).Show();
        }
        private static string GET(string url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            WebResponse response = request.GetResponse();
            using (Stream responseStream = response.GetResponseStream())
            {
                StreamReader reader = new StreamReader(responseStream, System.Text.Encoding.UTF8);
                return reader.ReadToEnd();
            }

        }
   
		public System.Collections.Generic.List<TextMessage> getSMS(bool unreadOnly = false)
        {
			Android.Net.Uri SMS_CONTENT_URI = Android.Net.Uri.Parse(SMS_DIRECTORY);
			Android.Net.Uri SMS_INBOX_CONTENT_URI = Android.Net.Uri.WithAppendedPath(SMS_CONTENT_URI, INBOX);

            string unread = string.Empty;
			if (unreadOnly) { unread = UNREAD_CONDITION; }
            string[] projection = new string[] { "_id", "thread_id", "address", "date", "body" };
			string selection = unread + SMS_BASE_QUERY;
            string[] selectionArgs = null;

			var cursor = ContentResolver.Query(SMS_INBOX_CONTENT_URI, projection, selection, selectionArgs, DATE_FILTER);

            if (cursor == null || cursor.Count <= 0) { return null; }
            System.Collections.Generic.List<TextMessage> textMessages = new System.Collections.Generic.List<TextMessage>();
            //bool NoErrors = true;


            while (cursor.MoveToNext())
            {
                long messageId = cursor.GetLong(0);
                long threadId = cursor.GetLong(1);
                string address = cursor.GetString(2);
                long timestamp = cursor.GetLong(3);
                string body = cursor.GetString(4);

                if (string.IsNullOrEmpty(address) == false && string.IsNullOrEmpty(body)== false && timestamp > 0 && body.Contains(AFK))
                {
                    TextMessage currmessage = new TextMessage(messageId, threadId, address.Replace("+",""), timestamp, body);
                    textMessages.Add(currmessage);
                }
            }


            cursor.Close();

            if (textMessages.Count > 0)
            {
                return textMessages;
            }
            else
            {
                return null;
            }

        }


    }
}

