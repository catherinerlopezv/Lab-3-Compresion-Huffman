using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Lab3.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class Api : ControllerBase
    {
        public static IWebHostEnvironment _environment;

        public Api(IWebHostEnvironment environment)
        {
            _environment = environment;

        }
        public class FileUploadAPI
        {
            public IFormFile files { get; set; }

        }

        [HttpPost("compress")]
        public IActionResult post([FromForm]FileUploadAPI objFile)
        {

            try
            {
                if (objFile.files.Length > 0)
                {
                    if (!Directory.Exists(_environment.WebRootPath + "\\Upload\\"))
                    {
                        Directory.CreateDirectory(_environment.WebRootPath + "\\Upload\\");
                    }
                    using (FileStream fileStream = System.IO.File.Create(_environment.WebRootPath + "\\Upload\\" + objFile.files.FileName))
                    {
                        objFile.files.CopyTo(fileStream);
                        fileStream.Flush();
                        fileStream.Close();
                        string s = @_environment.WebRootPath;
                        Operaciones imp = new Operaciones(fileStream.Name, s);
                        imp.Comprimir();

                        return Ok();

                    }


                }
                else
                {
                    return StatusCode(500);
                }

            }
            catch (Exception e)
            {
                return StatusCode(500,new { name="Internal Server Error",message=e.Message});
            }
        
    
        }



    


        [HttpPost("decompress")]
        public IActionResult decompress([FromForm]FileUploadAPI objFile)
        {

            try
            {
                if (objFile.files.Length > 0)
                {
                    if (!Directory.Exists(_environment.WebRootPath + "\\Upload\\"))
                    {
                        Directory.CreateDirectory(_environment.WebRootPath + "\\Upload\\");
                    }
                    using (FileStream fileStream = System.IO.File.Create(_environment.WebRootPath + "\\Upload\\" + objFile.files.FileName))
                    {
                        objFile.files.CopyTo(fileStream);
                        fileStream.Flush();
                        fileStream.Close();
                        string s = @_environment.WebRootPath;
                        Operaciones imp = new Operaciones(fileStream.Name, s);
                        imp.Descomporimir();

                        return Ok();

                    }


                }
                else
                {
                    return StatusCode(500);
                }

            }
            catch (Exception e)
            {
                return StatusCode(500, new { name = "Internal Server Error", message = e.Message });
            }



        }


    }
}
