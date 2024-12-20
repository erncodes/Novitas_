﻿using Microsoft.AspNetCore.Mvc;
using Novitas_Blog.Repositories;
using System.Net;

namespace Novitas_Blog.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        private readonly IImageRepository _imageRepository;
        public ImageController(IImageRepository imageRepository)
        {
            this._imageRepository = imageRepository;
        }
        [HttpPost]
        public async Task<IActionResult> UploadAsync(IFormFile file)
        {
            var ImageUrl = await _imageRepository.UploadAsync(file);
            return ImageUrl == null
                ? Problem("Something went wrong", null, (int)HttpStatusCode.InternalServerError)
                : new JsonResult(new { link = ImageUrl });
        }
    }
}
