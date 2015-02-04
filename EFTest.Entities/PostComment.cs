using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFTest.Entities
{
    public class PostComment
    {
        public int Id { get; set; }
        public int PostId { get; set; }
        public string Comment { get; set; }
      

        public virtual Post Post { get; set; }
    }
}
