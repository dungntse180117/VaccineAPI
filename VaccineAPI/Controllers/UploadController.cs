using Microsoft.AspNetCore.Mvc;
using VaccineAPI.DataAccess.Models;

namespace VaccineAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadController : Controller
    {
        [HttpPost]
        [Route("UploadFile")]
        public imageResponse UploadFile([FromForm] FileModel fileModel)
        {
            imageResponse imageResponse = new imageResponse();
            try
            {
                string path = Path.Combine("", fileModel.FileName);
                using (Stream stream = new FileStream(path, FileMode.Create))
                {
                    fileModel.file.CopyTo(stream);
                }
                imageResponse.StatusCode = 20;
                imageResponse.ErrorMessage = "image create succesfully";
            }
            catch (Exception ex)
            {
                imageResponse.StatusCode = 100;
                imageResponse.ErrorMessage = "Error";
            }
            return imageResponse;
        }
    }
}
