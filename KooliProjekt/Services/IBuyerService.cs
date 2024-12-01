using KooliProjekt.Data;

namespace KooliProjekt.Services
{
    public interface IBuyerService
    {
        Task<PagedResult<Buyer>> List(int page, int pageSize);
        Task<Buyer> Get(int id);
        Task Save(Buyer buyer);
        Task Delete(int id);
    }
}
