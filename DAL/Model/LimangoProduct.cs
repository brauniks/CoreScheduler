using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Model
{
    public class LimangoProduct
    {
        public string Name { get; set; }

        public decimal Cost { get; set; }

        public string Size { get; set; }

        public override string ToString()
        {
            return $"Product: { Name}  w rozmiarze: { Size }   za: { string.Format("{0:C}", Cost)}";
        }
    }
}
