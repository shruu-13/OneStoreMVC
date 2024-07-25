using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace OneStoreMVC.Models
{
    public class Product
    {
        public int Id { get; set; }

		[MaxLength(100)]
		public string Name { get; set; } = "";

		[MaxLength(100)]
		public string Brand { get; set; } = "";

		[MaxLength(100)]
		public string Catagory { get; set; } = "";
       

        public int Price { get; set; }


		[MaxLength(100)]
		public string Description { get; set; } = "";
        
        public string ImageFileName { get; set; } = "";

        public DateTime CreateAt { get; set; }



    }
}
