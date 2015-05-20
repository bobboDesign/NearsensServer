using Nearsens.Web.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace Nearsens.Web.Controllers
{
    public class FileUploadController : ApiController
    {
        // api/FileUpload/

        [HttpPost]
        public async Task<List<string>> PostAsync()
        {
            if (Request.Content.IsMimeMultipartContent())
            {
                string uploadPath = HttpContext.Current.Server.MapPath("~/NewsImages");

                MyStreamProvider streamProvider = new MyStreamProvider(uploadPath);

                await Request.Content.ReadAsMultipartAsync(streamProvider);

                List<string> messages = new List<string>();
                foreach (var file in streamProvider.FileData)
                {
                    FileInfo fi = new FileInfo(file.LocalFileName);
                    string[] nome = file.LocalFileName.Split('\\');
                    string path = nome[nome.Length - 1];
                    // messages.Add("File uploaded as " + fi.FullName + " (" + fi.Length + " bytes)");
                    messages.Add(path);

                }

                return messages;
            }
            else
            {
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.BadRequest, "Invalid Request!");
                throw new HttpResponseException(response);
            }
        }
    }
}
