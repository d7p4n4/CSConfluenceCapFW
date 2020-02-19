using AddNewPage;
using CSConfluenceClassesFW.DeletePage;
using CSConfluenceClassesFW.GetIdByTitle;
using CSConfluenceClassesFW.IsPageExists;
using CSConfluenceClassesFW.UploadAttachment;
using Newtonsoft.Json;
using System;
using System.Configuration;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Xsl;

namespace CSConfluenceCapFW
{
    public class ConfluenceAPIMetodusok
    {
        public ConfluenceAPIMetodusok()
        {

        }

        public string TransformXMLToHTML(string inputXml, string xsltString)
        {
            XslCompiledTransform transform = new XslCompiledTransform();
            using (XmlReader reader = XmlReader.Create(new StringReader(xsltString)))
            {
                transform.Load(reader);
            }
            StringWriter results = new StringWriter();
            using (XmlReader reader = XmlReader.Create(new StringReader(inputXml)))
            {
                transform.Transform(reader, null, results);
            }
            return results.ToString();
        }

        public DeletePageResult DeletePage(string jelszo, string felhasznaloNev, string URL, string oldalAzonosito, string terAzonosito)
        {
            DeletePageResult deletePageResult = new DeletePageResult();
            
            using (var httpClient = new HttpClient())
            {
                using (var request = new HttpRequestMessage(new HttpMethod("DELETE"), URL + "/" + oldalAzonosito + "?status=current"))
                {
                    var base64authorization = Convert.ToBase64String(Encoding.ASCII.GetBytes(felhasznaloNev + ":" + jelszo));
                    request.Headers.TryAddWithoutValidation("Authorization", $"Basic {base64authorization}");

                    //var response = await httpClient.SendAsync(request).Result;
                    HttpResponseMessage message = httpClient.SendAsync(request).Result;
                    string description = string.Empty;
                    string result = message.Content.ReadAsStringAsync().Result;

                    if (message.IsSuccessStatusCode)
                    {
                        DeletePageSuccessResponse JSONObjSuccess = new DeletePageSuccessResponse();
                        JSONObjSuccess = JsonConvert.DeserializeObject<DeletePageSuccessResponse>(result);

                        deletePageResult.SuccessResponse = JSONObjSuccess;
                    }
                    else
                    {

                        DeletePageFailedResponse JSONObjFailed = new DeletePageFailedResponse();
                        JSONObjFailed = JsonConvert.DeserializeObject<DeletePageFailedResponse>(result);

                        deletePageResult.FailedResponse = JSONObjFailed;

                    }

                    return deletePageResult;
                }
            }
        }

        public IsPageExistsResult IsPageExists(string URL, string oldalAzonosito, string terAzonosito, string felhasznaloNev, string jelszo)
        {
            IsPageExistsResult isPageExistsResult = new IsPageExistsResult();
            
            try
            {
                int oldalAzonositoSzam = Convert.ToInt32(oldalAzonosito);
            }
            catch (Exception exception)
            {
                return isPageExistsResult;
            }

            bool eredmeny = false;
            using (var httpClient = new HttpClient())
            {
                using (var request = new HttpRequestMessage(new HttpMethod("GET"), URL + "/" + oldalAzonosito + "?status=any"))
                {
                    var base64authorization = Convert.ToBase64String(Encoding.ASCII.GetBytes(felhasznaloNev + ":" + jelszo));
                    request.Headers.TryAddWithoutValidation("Authorization", $"Basic {base64authorization}");

                    //var response = await httpClient.SendAsync(request).Result;
                    HttpResponseMessage message = httpClient.SendAsync(request).Result;
                    string description = string.Empty;
                    string result = message.Content.ReadAsStringAsync().Result;

                    if (message.IsSuccessStatusCode)
                    {
                        IsPageExistsSuccessResponse JSONObjSuccess = new IsPageExistsSuccessResponse();
                        JSONObjSuccess = JsonConvert.DeserializeObject<IsPageExistsSuccessResponse>(result);

                        isPageExistsResult.SuccessResponse = JSONObjSuccess;
                    }
                    else
                    {

                        IsPageExistsFailedResponse JSONObjFailed = new IsPageExistsFailedResponse();
                        JSONObjFailed = JsonConvert.DeserializeObject<IsPageExistsFailedResponse>(result);

                        isPageExistsResult.FailedResponse = JSONObjFailed;

                    }

                    return isPageExistsResult;
                }
            }

        }

        public AddNewPageResult AddConfluencePage(string cim, string terAzonosito, string szuloOsztalyAzonosito, string html, string URL, string felhasznaloNev, string jelszo)
        {
            AddNewPageResult addNewPageResult = new AddNewPageResult();

            html = html.Replace("\r", "").Replace("\n", "").Replace("\t", "").Replace("\"", "'");
            
            string DATA = "{\"type\":\"page\",\"ancestors\":[{\"type\":\"page\",\"id\":" + szuloOsztalyAzonosito +
                "}],\"title\":\"" + cim + "\",\"space\":{\"key\":\"" + terAzonosito + "\"},\"body\":{\"storage\":{\"value\":\""
                + html + "\",\"representation\":\"storage\"}}}";

            System.Net.Http.HttpClient client = new System.Net.Http.HttpClient();
            client.BaseAddress = new System.Uri(URL);
            byte[] cred = UTF8Encoding.UTF8.GetBytes(felhasznaloNev + ":" + jelszo);
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(cred));
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            System.Net.Http.HttpContent content = new StringContent(DATA, UTF8Encoding.UTF8, "application/json");

            HttpResponseMessage message = client.PostAsync(URL, content).Result;
            string description = string.Empty;
            string result = message.Content.ReadAsStringAsync().Result;

            if (message.IsSuccessStatusCode)
            {
                AddNewPageSuccessResponse JSONObjSuccess = new AddNewPageSuccessResponse();
                JSONObjSuccess = JsonConvert.DeserializeObject<AddNewPageSuccessResponse>(result);

                addNewPageResult.SuccessResponse = JSONObjSuccess;
            }
            else
            {

                AddNewPageFailedResponse JSONObjFailed = new AddNewPageFailedResponse();
                JSONObjFailed = JsonConvert.DeserializeObject<AddNewPageFailedResponse>(result);

                addNewPageResult.FailedResponse = JSONObjFailed;

            }

            return addNewPageResult;

        }

        public async Task<UploadAttachmentResult> KepFeltoltes(string felhasznaloNev, string jelszo, string URL, string oldalAzonosito, byte[] kepFajlBajtjai, string fajlNev)
        {

            UploadAttachmentResult uploadAttachmentResult = new UploadAttachmentResult();

            ByteArrayContent kepByteTomb = new ByteArrayContent(kepFajlBajtjai);
            
            using (var httpClient = new HttpClient())
            {
                using (var request = new HttpRequestMessage(new HttpMethod("POST"), URL + "/" + oldalAzonosito + "/child/attachment"))
                {
                    request.Headers.TryAddWithoutValidation("X-Atlassian-Token", "nocheck");

                    var base64authorization = Convert.ToBase64String(Encoding.ASCII.GetBytes(felhasznaloNev + ":" + jelszo));
                    request.Headers.TryAddWithoutValidation("Authorization", $"Basic {base64authorization}");

                    var multipartContent = new MultipartFormDataContent();
                    multipartContent.Add(kepByteTomb, "file", fajlNev);
                    multipartContent.Add(new StringContent("This is my File"), "comment");
                    request.Content = multipartContent;

                    var response = await httpClient.SendAsync(request);
                    string result = response.Content.ReadAsStringAsync().Result;


                    if (response.IsSuccessStatusCode)
                    {
                        UploadAttachmentSuccessResponse JSONObjSuccess = new UploadAttachmentSuccessResponse();
                        JSONObjSuccess = JsonConvert.DeserializeObject<UploadAttachmentSuccessResponse>(result);

                        uploadAttachmentResult.SuccessResponse = JSONObjSuccess;
                    }
                    else
                    {

                        UploadAttachmentFailedResponse JSONObjFailed = new UploadAttachmentFailedResponse();
                        JSONObjFailed = JsonConvert.DeserializeObject<UploadAttachmentFailedResponse>(result);

                        uploadAttachmentResult.FailedResponse = JSONObjFailed;

                    }

                    return uploadAttachmentResult;
                }
            }
        }

        public async Task<UploadAttachmentResult> UploadAttachment(string felhasznaloNev, string jelszo, string URL, string oldalNeve, string kepFajlBase64, string fajlNev)
        {

            return await KepFeltoltes(
                felhasznaloNev
                , jelszo
                , URL
                , oldalNeve
                , Convert.FromBase64String(kepFajlBase64)
                , fajlNev
                );
        }

        public GetIdByTitleResult GetIdByTitle(string felhasznaloNev, string jelszo, string terAzonosito, string URL, string oldalNeve)
        {
            GetIdByTitleResult getIdByTitleResult = new GetIdByTitleResult();

            using (var httpClient = new HttpClient())
            {
                using (var request = new HttpRequestMessage(new HttpMethod("GET"), URL + "?title=" + oldalNeve + "&spaceKey=" + terAzonosito + "&expand=history"))
                {
                    var base64authorization = Convert.ToBase64String(Encoding.ASCII.GetBytes(felhasznaloNev + ":" + jelszo));
                    request.Headers.TryAddWithoutValidation("Authorization", $"Basic {base64authorization}");

                    HttpResponseMessage message = httpClient.SendAsync(request).Result;
                    string description = string.Empty;
                    string result = message.Content.ReadAsStringAsync().Result;

                    if (message.IsSuccessStatusCode)
                    {
                        GetIdByTitleSuccessResponse JSONObjSuccess = new GetIdByTitleSuccessResponse();
                        JSONObjSuccess = JsonConvert.DeserializeObject<GetIdByTitleSuccessResponse>(result);

                        getIdByTitleResult.SuccessResponse = JSONObjSuccess;
                    }
                    else
                    {

                        GetIdByTitleFailedResponse JSONObjFailed = new GetIdByTitleFailedResponse();
                        JSONObjFailed = JsonConvert.DeserializeObject<GetIdByTitleFailedResponse>(result);

                        getIdByTitleResult.FailedResponse = JSONObjFailed;

                    }

                    return getIdByTitleResult;
                }
            }

        }

    }
}
