using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repoitory
{
    public interface IProcessRepository
    {
        Task AddHashToDatabase(Hashes hash);
        bool CheckHashIfExists(string sourceMD5HashCode);
        string GenerateMd5Hash(MD5 md5Hash, string input);
        Task<bool> GetHashFromDatabase(string name, string size, string amount, string zdj, string link);
        bool VerifyMd5Hash(MD5 md5Hash, string input, string hash);
        IQueryable<Websites> GetWebsites();
    }
}
