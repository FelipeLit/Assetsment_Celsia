using assetsment_Celsia.Models;
using X.PagedList;

namespace assetsment_Celsia.Interfaces
{
    public interface IClientRepository
    {
        Task<IPagedList<Client>> GetClients(int page, int size);
        Task AddClient(Client client);
        Task UpdateClientAsync(Client client);
        Task<Client> GetClientByIdAsync(int id);
    }
}