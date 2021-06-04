using CrudAPIWithRepositoryPattern.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace CrudAPIWithRepositoryPattern.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
   
    public class UserController : ControllerBase
    {
        [HttpPost]
        public IActionResult Save([FromBody] User user)
        {
            
            return Ok(user);
        }

        [Route("UserExists/{emailAddress}"),HttpPost]
        public IActionResult UserExists(string emailAddress)
        {
            return Ok();
        }
        

        [Route("/upload")]
        [HttpPost, DisableRequestSizeLimit]
        public IActionResult Upload()
        {
            try
            {
                var file = Request.Form.Files[0];
                var folderName = Path.Combine("Resources", "Images");
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);

                if (file.Length > 0)
                {
                    var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    var fileType = new FileInfo(file.FileName);
                    // fileType.Extension
                    fileName = Guid.NewGuid().ToString() + fileType.Extension;
                    var fullPath = Path.Combine(pathToSave, fileName);
                    var dbPath = Path.Combine(folderName, fileName);

                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }

                    return Ok(new { message = dbPath });
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }
    }
}
