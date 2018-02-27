using FileUploadTest.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;

namespace FileUploadTest.Controllers
{

    public class CustomMultipartFormDataStreamProvider : MultipartFormDataStreamProvider
    {
        public CustomMultipartFormDataStreamProvider(string path) : base(path) { }

        public override string GetLocalFileName(HttpContentHeaders headers)
        {
            return headers.ContentDisposition.FileName.Replace("\"", string.Empty);
        }
    }

    [RoutePrefix("api/test")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class FileUploadController : ApiController
    {
        #region [ Attribute ]
        private static readonly string ServerUploadFolder = "C:\\Temp"; //Path.GetTempPath(); 
        #endregion

        #region [ PROPERTIES ]
        private ArrayList mimeType;
        public ArrayList MimeTypes
        {
            get
            {
                mimeType = new ArrayList();
                mimeType.Add("image/jpeg");
                mimeType.Add("image/jpg");
                mimeType.Add("image/png");
                mimeType.Add("application/pdf");
                return mimeType;
            }
        }
        #endregion

        [Route("files")]
        [HttpPost]
        [ValidateMimeMultipartContentFilter]
        public async Task<FileResult> UploadSingleFile()
        {
            CustomMultipartFormDataStreamProvider streamProvider = new CustomMultipartFormDataStreamProvider(ServerUploadFolder);

            await Request.Content.ReadAsMultipartAsync(streamProvider);

            foreach (MultipartFileData file in streamProvider.FileData)
            {
                if (MimeTypes.Contains(file.Headers.ContentType.MediaType))
                {
                    //string filename = file.Headers.ContentDisposition.FileName;
                    //string a = file.Headers.ContentDisposition.CreationDate.ToString();
                    //string b = file.Headers.ContentDisposition.Name;

                    return new FileResult
                    {
                        FileNames = streamProvider.FileData.Select(entry => entry.LocalFileName.Replace("\"", string.Empty)),
                        Names = streamProvider.FileData.Select(entry => entry.Headers.ContentDisposition.FileName.Replace("\"", string.Empty)),
                        ContentTypes = streamProvider.FileData.Select(entry => entry.Headers.ContentType.MediaType),
                        ClassID = streamProvider.FormData["classID"],
                        Token = streamProvider.FormData["token"],
                        CreatedTimestamp = DateTime.UtcNow,
                        UpdatedTimestamp = DateTime.UtcNow
                    };
                }
            }

            return null;
        }
    }
}

