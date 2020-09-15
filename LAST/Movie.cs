using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAST
{
    public class Movie
    {
        public string ImageUrl { get; set; }
        public List <string> Images{get;set;}
        public string ImdbTitleID { get; set; }
        public string Name { get; set; }
        public string DatePublished { get; set; }
        public string Rate { get; set; }
        public string Desc { get; set; }
        public List<Cast> Casts { get; set; }
        public List<Genre> Genres { get; set; }
        public Movie()
        {
            this.Casts = new List<Cast>();
            this.Genres = new List<Genre>();
        }
        public override string ToString()
        {
            return this.Name;
        }
    }
}
