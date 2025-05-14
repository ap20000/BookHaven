//using Microsoft.AspNetCore.Mvc;
//using Serilog;

//namespace BookHaven.Controllers
//{
//    [ApiController]
//    [Route("api/image")]
//    public class ImageUploadController : ControllerBase
//    {
//        private readonly string _imageDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");

//        public ImageUploadController()
//        {
//            if (!Directory.Exists(_imageDirectory))
//            {
//                Directory.CreateDirectory(_imageDirectory);
//                Log.Information($"Created directory: {_imageDirectory}");
//            }
//        }

//        [HttpPost("uploadImage")]
//        public async Task<IActionResult> UploadImage([FromForm] IFormFile imageFile)
//        {
//            if (imageFile == null || imageFile.Length == 0)
//            {
//                return BadRequest(new { message = "No image file provided" });
//            }

//            try
//            {
//                // Generate a unique file name for the uploaded image
//                var fileName = Path.GetFileNameWithoutExtension(imageFile.FileName) + "_" + Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
//                var filePath = Path.Combine(_imageDirectory, fileName);

//                // Save the image file to the server
//                using (var stream = new FileStream(filePath, FileMode.Create))
//                {
//                    await imageFile.CopyToAsync(stream);
//                }

//                // Generate the image URL dynamically using the current request scheme and host
//                var imageUrl = $"{Request.Scheme}://{Request.Host}/images/{fileName}";  // Use dynamic scheme and host

//                // Log the success
//                Log.Information($"Image uploaded successfully: {imageUrl}");

//                // Return the image URL in the response
//                return Ok(new { imageUrl = imageUrl });
//            }
//            catch (Exception ex)
//            {
//                // Log the error
//                Log.Error($"Error uploading image: {ex.Message}");

//                return BadRequest(new { message = ex.Message });
//            }
//        }
//    }
//}