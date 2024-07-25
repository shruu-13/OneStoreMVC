using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace OneStoreMVC.Models
{
    public class ProductDto
    {
		[Required, MaxLength(100)]
		public string Name { get; set; } = "";

		[Required, MaxLength(100)]
		public string Brand { get; set; } = "";

		[Required, MaxLength(100)]

		public string Catagory { get; set; } = "";

		[Required]
		public int Price { get; set; }


		[Required]
		public string Description { get; set; } = "";
        
        public IFormFile? ImageFile { get; set; }
    }
}
