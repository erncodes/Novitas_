
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

            _account = new Account (
                     _configuration.GetSection("CloudinaryInfo")["CloudName"],
                     _configuration.GetSection("CloudinaryInfo")["ApiKey"],
                     _configuration.GetSection("CloudinaryInfo")["ApiSecret"]
                                    );
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

            if (upLoadResult != null && upLoadResult.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return upLoadResult.SecureUrl.ToString();
            }
            return null;
        }
    }
}
