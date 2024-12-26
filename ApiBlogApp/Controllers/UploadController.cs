using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc;

namespace ApiBlogApp.Controllers;

[Route("api/upload")]
[ApiController]
public class UploadController : ControllerBase
{
    // POST: Upload file [host/api/upload]
    [HttpPost]
    [ApiExplorerSettings(IgnoreApi = true)] // It's not convenient to expose this endpoint 
    public IActionResult Upload()
    {
        try
        {
            var file = Request.Form.Files[0];
            var folderName = Path.Combine("PostsImages");
            var path = Path.Combine(Directory.GetCurrentDirectory(), folderName);
            
            if (file.Length > 0)
            {
                var fileName = Guid.NewGuid() + ContentDispositionHeaderValue
                    .Parse(file.ContentDisposition)
                    .FileName
                    .Trim('"');
                var fullPath = Path.Combine(path, fileName);
                var dbPath = Path.Combine(folderName + "/", fileName);

                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }

                return Ok(dbPath);
            }
            else return BadRequest();
        }
        catch (Exception e)
        {
            return StatusCode(500, $"Internal Server Error: {e.Message}");
        }
    }
}










