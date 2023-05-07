using Microsoft.Build.Framework;
using System.ComponentModel.DataAnnotations;
using RequiredAttribute = System.ComponentModel.DataAnnotations.RequiredAttribute;

namespace Task.Models
{
    public class Slide
    {
        public int Id { get; set; }
        [MaxLength(100)]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; } 
        public string Image { get; set; }   

        public int Offer {get; set; } 
    }
}
