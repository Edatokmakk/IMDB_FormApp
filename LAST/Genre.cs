using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAST
{
    public class Genre
    {
        public int GenreId { get; set; }
        public string GenreName { get; set; }
        public override string ToString()
        {
            return this.GenreName;
        }
    }

}
