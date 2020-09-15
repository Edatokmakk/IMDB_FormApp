using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAST
{
    public enum Role
    {
        Director = 1,
        Actor = 2,
        Writer = 3
    }
    public class Cast
    {
        public Cast()
        {
            this.Roles = new List<Role>();
        }
        public string ImdbID { get; set; }
        public string Name { get; set; }
        public List<Role> Roles { get; set; }
        public string ImageUrl { get; set; }
        public List<string> Images { get; set; }
        public string Bio { get; set; }
        public string BirthDate { get; set; }
        public string JobTitle { get; set; }
        public override string ToString()
        {
            return this.Name;
        }
    }
}
