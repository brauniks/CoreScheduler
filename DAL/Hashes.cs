using System;
using System.ComponentModel.DataAnnotations;

namespace DAL
{
    public class Hashes
    { 
        [Key]
        public int Id { get; set; }

        public string MD5HashCode{ get; set; }

        public DateTime AddedDate{ get; set; }

        public string ProductName{ get; set; }

        public string ProductSize{ get; set; }

        public string ProductCost{ get; set; }
    }
}
