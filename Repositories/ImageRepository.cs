
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

namespace Novitas_Blog.Repositories
{
    public class ImageRepository : IImageRepository
    {
        private readonly IConfiguration _configuration;
        private readonly Account _account;
        public ImageRepository(IConfiguration configuration)
        {
            this._configuration = configuration;

            var CloudName = _configuration.GetSection("CloudName").Value;
            var ApiKey = _configuration.GetSection("ApiKey").Value;
            var ApiSecret = _configuration.GetSection("ApiSecret").Value;

            _account = new Account(CloudName, ApiKey, ApiSecret);
        }
        public async Task<string> UploadAsync(IFormFile file)
        {
            var client = new Cloudinary(_account);
            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(file.FileName, file.OpenReadStream()),
                DisplayName = file.FileName
            };
            var upLoadResult = await client.UploadAsync(uploadParams);
            return upLoadResult != null && upLoadResult.StatusCode == System.Net.HttpStatusCode.OK ? upLoadResult.SecureUrl.ToString() : null;
        }
    }
}
