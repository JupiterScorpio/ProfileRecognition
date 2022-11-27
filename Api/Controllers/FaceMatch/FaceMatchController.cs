namespace ImageRecongnitionApi.Controllers.FaceMatch
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/{v:apiVersion}/facematch")]
    public class FaceMatchController : ControllerBase
    {
        private IAllPhotos _allPhotosRepository;
        public FaceMatchController(IAllPhotos allPhotosRepository)
        {
            _allPhotosRepository = allPhotosRepository;
        }

        [HttpPost("getMatch")]
        public async Task<IActionResult> FetchMatchPhotos(Pagination pages)
        {
            return (await _allPhotosRepository.FetchMatchPhotos(pages)).Format(this);
        }

        [HttpPost("selectMatch")]
        public async Task<IActionResult> FetchPhotos(string Code)
        {
            return (await _allPhotosRepository.FetchSelectPhotos(Code)).Format(this);
        }

        [HttpPost("getUnMatch")]
        public async Task<IActionResult> FetchUnMatchPhotos(Pagination pages)
        {
            return (await _allPhotosRepository.FetchUnMatchPhotos(pages)).Format(this);
        }
    }
}
