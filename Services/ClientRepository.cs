using assetsment_Celsia.Data;
using assetsment_Celsia.Interfaces;
using assetsment_Celsia.Models;
using Microsoft.EntityFrameworkCore;
using X.PagedList;
using X.PagedList.Extensions;

namespace assetsment_Celsia.Services
{

    public class ClientRepository : IClientRepository
    {
        private readonly CelsiaContext _context;
        public ClientRepository(CelsiaContext context)
        {
            _context = context;
        }
        public async Task AddClient(Client client)
        {
            var existingUser = await _context.Clients.FirstOrDefaultAsync(e => e.Email == client.Email);
            if (existingUser != null)
            {
                throw new InvalidOperationException("Email already exists.");
            }

            // Valores predeterminados
            client.Status = "Active";
            client.RoleId = 2;

            _context.Clients.Add(client);
            await _context.SaveChangesAsync();
        }

        public async Task<IPagedList<Client>> GetClients(int page, int size)
        {
            var pageNumber = page > 0 ? page : 1;
            var clientList = await _context.Clients.ToListAsync();
            var clientePageList = clientList.ToPagedList((int)pageNumber, size);

            return clientePageList;
        }

        public async Task UpdateClientAsync(Client client)
        {
            var existingClient = await _context.Clients.FindAsync(client.Id);
            if (existingClient == null)
            {
                throw new InvalidOperationException("Client not found.");
            }


            existingClient.Name = client.Name;
            existingClient.Address = client.Address;
            existingClient.Phone = client.Phone;
            existingClient.Email = client.Email;
            existingClient.Status = "Active";
            existingClient.RoleId = 2;

            // Guarda los cambios
            await _context.SaveChangesAsync();
        }
        public async Task<Client> GetClientByIdAsync(int id)
        {
            return await _context.Clients.FindAsync(id);
        }

    }
}