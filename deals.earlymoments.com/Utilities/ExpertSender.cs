using System;
using System.IO;
using System.Net;

namespace deals.earlymoments.com.Utilities
{
    public class ExpertSender
    {
        public string PostXmlData(string apiUrl, string requestXmlData)
        {
            try
            {
                var request = (HttpWebRequest)WebRequest.Create(apiUrl);
                byte[] bytes;
                bytes = System.Text.Encoding.ASCII.GetBytes(requestXmlData);
                request.ContentType = "text/xml; encoding='utf-8'";
                request.ContentLength = bytes.Length;
                request.Method = "POST";
                var requestStream = request.GetRequestStream();
                requestStream.Write(bytes, 0, bytes.Length);
                requestStream.Close();
                var response = (HttpWebResponse)request.GetResponse();
                if (response.StatusCode != HttpStatusCode.OK) return null;
                var responseStream = response.GetResponseStream();
                var responseStr = new StreamReader(responseStream).ReadToEnd();
                return responseStr;
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }

        public string AddSubscriber(string email, string firstname, string lastname, string vendor, string IPAddress, string orderId)
        {
            string _data = @"<ApiRequest xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xs=""http://www.w3.org/2001/XMLSchema"">
   <ApiKey>9PXf7JVmiDzNYesRf4eA</ApiKey>
   <ReturnData>true</ReturnData>
   <VerboseErrors>true</VerboseErrors>
   <MultiData>
     <Subscriber>
        <Mode>AddAndUpdate</Mode>
        <Force>true</Force>
        <ListId>4</ListId><Email>" + email + @"</Email>
        <Firstname>" + firstname + @"</Firstname>
        <Lastname>" + lastname + @"</Lastname>
        <TrackingCode>" + orderId + @"</TrackingCode>
        <Vendor>" + vendor + @"</Vendor>
        <Ip>" + (IPAddress.Length < 7 ? "10.60.40.100" : IPAddress) + @"</Ip>
     </Subscriber>
   </MultiData>
</ApiRequest>";


            var response = PostXmlData("https://api4.esv2.com/Api/Subscribers", _data);
            return response;
        }

        public string PrepareTransactionalEmail(string email, string emailBody, string orderId, string project)
        {
            string _data = @"<ApiRequest xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xs=""http://www.w3.org/2001/XMLSchema"">
     <ApiKey>9PXf7JVmiDzNYesRf4eA</ApiKey>
  <Data>
    <Receiver>
      <Email>" + email + @"</Email>
    </Receiver>
<Snippets>
      <Snippet>
        <Name>subjectLine</Name>
        <Value><![CDATA[" + project.Replace("®", "").Replace("™", "") + @"]]></Value>
      </Snippet>
      <Snippet>
        <Name>fromName</Name>
        <Value><![CDATA[" + project.Replace("®", "").Replace("™", "") + @"]]></Value>
      </Snippet>
      <Snippet>
        <Name>orderConfirmationTexts</Name>
        <Value><![CDATA[" + emailBody + @"]]></Value>
      </Snippet>
    </Snippets>
  </Data>
</ApiRequest>";


            var response = PostXmlData("https://api4.esv2.com/Api/Transactionals/16", _data);
            return response;
        }
    }
}