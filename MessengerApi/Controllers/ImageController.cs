using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.NodeServices;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MessengerApi.Controllers
{
    public class ImageController : Controller
    {
        [HttpPost]
        [Route("api/upload-image/")]
        public async Task<IActionResult> UploadImage(
            [FromServices]IHostingEnvironment hostingEnviroment,
            [FromServices]INodeServices nodeServices,
            [FromBody]Stream file) 
        {
            var result = await nodeServices.InvokeAsync<Stream>(
                "resizeImage.js", 
                file,
                300);

            return Ok(result);
        }
    }
}
