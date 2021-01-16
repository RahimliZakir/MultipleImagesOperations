using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MultipleImagesOperationsWebApplication.Models.Entity
{
    public class Images : BaseEntity
    {
        public string ImagePath { get; set; }

        public int ProductId { get; set; }

        public bool IsMain { get; set; }

        public virtual Product Product { get; set; }
    }
}
