using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repoitory
{
    public class ProcessRepository : IProcessRepository
    {
        private readonly PostgreDbContext context;

        public ProcessRepository(PostgreDbContext context)
        {
            this.context = context;
        }


        public IQueryable<Websites> GetWebsites()
        {
            return  this.context.Websites.AsQueryable(); 
        }

        /// <summary>
        /// The CheckHashIfExists
        /// </summary>
        /// <param name="sourceMD5HashCode">The <see cref="string"/></param>
        /// <returns>The <see cref="bool"/></returns>
        public bool CheckHashIfExists(string sourceMD5HashCode)
        {
            return this.context.Hashes.Any(hash => hash.MD5HashCode == sourceMD5HashCode);            
        }

        /// <summary>
        /// The GetHashFromDatabase
        /// </summary>
        /// <param name="name">The <see cref="string"/></param>
        /// <param name="size">The <see cref="string"/></param>
        /// <param name="amount">The <see cref="string"/></param>
        /// <param name="zdj">The <see cref="string"/></param>
        /// <param name="link">The <see cref="string"/></param>
        /// <returns>The <see cref="bool"/></returns>
        public async Task<bool> GetHashFromDatabase(string name, string size, string amount, string zdj, string link)
        {
            var source = new string(string.Concat(name, "|", size, "|", amount).Replace(" ", "").Where(c => !char.IsControl(c)).ToArray());

            using (MD5 md5hash = MD5.Create())
            {
                string hash = this.GenerateMd5Hash(md5hash, source);
                if (!CheckHashIfExists(hash))
                {

                    await this.AddHashToDatabase(new Hashes() { MD5HashCode = hash, ProductName = name, ProductSize = size, ProductCost = amount, AddedDate = DateTime.Now });

                    return false;
                }
                else
                {
                    //Console.WriteLine("Product exist in database, ignored");
                    return true;
                }

            }
        }

        /// <summary>
        /// The AddHashToDatabase
        /// </summary>
        /// <param name="hash">The <see cref="Hashes"/></param>
        public async Task AddHashToDatabase(Hashes hash)
        {
            this.context.Hashes.Add(hash);
            await this.context.SaveChangesAsync();
        }

        /// <summary>
        /// The VerifyMd5Hash
        /// </summary>
        /// <param name="md5Hash">The <see cref="MD5"/></param>
        /// <param name="input">The <see cref="string"/></param>
        /// <param name="hash">The <see cref="string"/></param>
        /// <returns>The <see cref="bool"/></returns>
        public bool VerifyMd5Hash(MD5 md5Hash, string input, string hash)
        {
            string hashOfInput = GenerateMd5Hash(md5Hash, input);

            StringComparer comparer = StringComparer.OrdinalIgnoreCase;

            if (0 == comparer.Compare(hashOfInput, hash))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// The GenerateMd5Hash
        /// </summary>
        /// <param name="md5Hash">The <see cref="MD5"/></param>
        /// <param name="input">The <see cref="string"/></param>
        /// <returns>The <see cref="string"/></returns>
        public string GenerateMd5Hash(MD5 md5Hash, string input)
        {
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            StringBuilder sBuilder = new StringBuilder();

            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            return sBuilder.ToString();
        } 
    }
}
