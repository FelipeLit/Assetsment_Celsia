using assetsment_Celsia.Dtos;
using assetsment_Celsia.Models;

namespace assetsment_Celsia.Interfaces
{
    public interface ILoginRepository
    {
        Task<Administrator> CreateAdmin(DtoAdmin admin);
        Task<Administrator> AuthenticateUser(string email, string password);
    }
}