using H4SoftwareTest.Code;
using H4SoftwareTest.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace H4SoftwareTest.Data.Manager
{
    public class CprManager : ICprManager
    {
        private HashingHandler _hashingHanler;
        public TodoDbContext Context { get; set; }
        public CprManager(TodoDbContext context, HashingHandler hashingHandler)
        {
            Context = context;
            _hashingHanler = hashingHandler;
        }
        public async Task<bool> isEmailExist(string email)
        {
            return await Context.Cprs.AnyAsync(u => u.Email == email);

        }

        public async Task<bool> isCprExist(string cprNumber)
        {
            string hashedCprNumber = _hashingHanler.HMACHashing(cprNumber);
            return await Context.Cprs.AnyAsync(u => u.CprNumber == hashedCprNumber);

        }
        public async Task CreateCprAsync(string email, string cprNumber)
        {
            string hashedCprNumber = _hashingHanler.HMACHashing(cprNumber);
            var cpr = new Cpr
            {
                Email = email,
                CprNumber = hashedCprNumber
            };
            Context.Cprs.Add(cpr);
            await Context.SaveChangesAsync();
        }

        public async Task<int> GetIdByEmail(string email)
        {
            return Context.Cprs.FirstOrDefault(u => u.Email == email).Id;
        }
        public async Task<List<Cpr>> GetAllUser()
        {
            return await Context.Cprs.ToListAsync();
        }
    }
}
