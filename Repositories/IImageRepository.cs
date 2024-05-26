namespace Novitas_Blog.Repositories
{
    public interface IImageRepository
    {
        public Task<string> UploadAsync(IFormFile formFile);
    }

}
