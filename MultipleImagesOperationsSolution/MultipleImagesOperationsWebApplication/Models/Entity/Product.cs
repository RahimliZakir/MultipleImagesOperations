using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MultipleImagesOperationsWebApplication.Models.Entity
{
    public class Product : BaseEntity
    {
        public string Name { get; set; }

        public string ImagePath { get; set; }

        public string ShortDescription { get; set; }

        public int CategoryId { get; set; }

        public virtual Category Category { get; set; }

        public virtual ICollection<Images> Images { get; set; }
    }
}
