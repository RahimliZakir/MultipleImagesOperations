using MultipleImagesOperationsWebApplication.Models.FormModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MultipleImagesOperationsWebApplication.Models.ViewModel
{
    public class ProductViewModel
    {
        public int Id { get; set; }

        [NotMapped]
        public ImageItemFormModel[] Files { get; set; }
    }
}
