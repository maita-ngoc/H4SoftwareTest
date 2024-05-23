using H4SoftwareTest.Data.Models;

namespace H4SoftwareTest.Data.Manager
{
    public interface ICprManager
    {
        public Task<bool> isEmailExist(string email);
        public Task<bool> isCprExist(string cpr);
        Task<int> GetIdByEmail(string email);
        Task CreateCprAsync(string email, string cprNumber);
        Task<List<Cpr>> GetAllUser();
    }
}
