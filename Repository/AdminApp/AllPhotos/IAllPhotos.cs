
namespace Repository
{
    public interface IAllPhotos
    {
        Task<Response> FetchAllPhotos(Pagination pages);
        Task<Response> DeleteProfilePhotos(string Code);
        Task<Response> FetchMatchPhotos(Pagination pages);
        Task<Response> FetchUnMatchPhotos(Pagination pages);
        Task<Response> FetchSelectPhotos(string id);
    }
}
