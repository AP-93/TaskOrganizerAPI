using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using TaskOrganizerAPI.Model;

namespace TaskOrganizerAPI.Data
{
    public class AuthRepository : IAuthRepository
    {
        private readonly DataContext _dataContext;
        public AuthRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<User> Login(string username, string password)
        {
            var user = await _dataContext.Users.FirstOrDefaultAsync(x => x.Username == username);
            if (user == null)
                return null;
            if (!VerifyPasswordHash(password, user.PaswordSalt, user.PaswordHash))
                return null;

            return user;
        }

        private bool VerifyPasswordHash(string password, byte[] paswordSalt, byte[] paswordHash)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(paswordSalt))
            {
                //hash users pasword using key(paswordsalt) stored in database
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));

                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != paswordHash[i])
                        return false;
                }
            }
            return true;
        }

        public async Task<User> Register(User user, string password)
        {
            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(password, out passwordHash, out passwordSalt);//out pass reference not value

            user.PaswordHash = passwordHash;
            user.PaswordSalt = passwordSalt;

            await _dataContext.Users.AddAsync(user);
            await _dataContext.SaveChangesAsync();

            return user;
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())//hmacsha512 generates hash and key
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        public async Task<bool> UserExists(string username)
        {
            if (await _dataContext.Users.AnyAsync(x => x.Username == username))
                return true;

            return false;
        }
    }
}
