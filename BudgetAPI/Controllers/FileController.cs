using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;

namespace BudgetAPI.Controllers
{
    [Microsoft.AspNetCore.Mvc.Route("file")]
    [Authorize]
    public class FileController : ControllerBase
    {
        [HttpGet]
        [ResponseCache(VaryByQueryKeys =  new[] { "fileName" }, Duration = 1200)]
        public IActionResult GetFile([FromQuery] string fileName) {
            var rootPath = Directory.GetCurrentDirectory();
            var filePath = $"{rootPath}/PrivateFiles/{fileName}";
            var fileExists = System.IO.File.Exists(filePath);
            if(!fileExists)
            {
                return NotFound();
            }
            var contentProvider = new FileExtensionContentTypeProvider();
            contentProvider.TryGetContentType(filePath, out string contentType);

            var fileContent = System.IO.File.ReadAllBytes(filePath);
            return File(fileContent, contentType, fileName + "FromServer");

        }

        [HttpPost]
        public IActionResult Upload([FromForm] IFormFile file)
        {
            if(file != null && file.Length > 0) 
            {
                var rootPath = Directory.GetCurrentDirectory();
                var fileName = file.FileName;
                var fullPath = $"{rootPath}/PrivateFiles/{fileName}";
                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }
                return Ok();
            }
            return BadRequest();
        }
    }
}
