using assetsment_Celsia.Data;
using assetsment_Celsia.Dtos;
using assetsment_Celsia.Helper;
using assetsment_Celsia.Interfaces;
using assetsment_Celsia.Models;
using Microsoft.EntityFrameworkCore;

namespace assetsment_Celsia.Services
{
    public class LoginRepository : ILoginRepository
    {
        private readonly CelsiaContext _context;

        public LoginRepository(CelsiaContext context)
        {
            _context = context;
        }

        public async Task<Administrator> AuthenticateUser(string email, string password)
        {
            var user = await _context.Administrators.FirstOrDefaultAsync(a => a.Email == email);
            if (user != null)
            {
                // Verifica la contraseña usando la clase EncrypPass
                var passCompare = new EncrypPass();
                var auth = passCompare.VerifyPassword(user.Password, password);
                if (auth)
                {
                    return user; // Devuelve el usuario si la autenticación es exitosa
                }
            }
            return null;
        }

        public async Task<Administrator> CreateAdmin(DtoAdmin admin)
        {
            try
            {
                // Verifica si ya existe un usuario con el mismo correo electrónico
                var existingAdmin = await _context.Administrators.FirstOrDefaultAsync(a => a.Email == admin.Email);
                if (existingAdmin != null)
                {
                    return null;
                }

                // Hashea la contraseña usando la clase EncrypPass
                var pass = new EncrypPass();
                var hashPassword = pass.HashPassword(admin.Password);
                admin.Password = hashPassword;

                // Crea un usuario
                var newAdmin = new Administrator
                {
                    Email = admin.Email,
                    Password = admin.Password,
                    RoleId = 1,
                };

                // Añade el usuario a la base de datos
                _context.Administrators.Add(newAdmin);
                await _context.SaveChangesAsync();
                return newAdmin;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while creating the user. Please try again later.", ex);
            }
        }

    }
}