using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyDiscs.Models
{
    public class Disc
    {
        [Key]
        public int DiscId { get; set; }

        [Required(ErrorMessage = "Please enter a disc name.")]
        [StringLength(50, MinimumLength = 2)]
        [Display(Name = "Disc name:")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Please enter speed.")]
        [Display(Name = "Speed:")]
        public int? Speed { get; set; }

        [Required(ErrorMessage = "Please enter glide.")]
        [Display(Name = "Glide:")]
        public int? Glide { get; set; }

        [Required(ErrorMessage = "Please enter turn.")]
        [Display(Name = "Turn:")]
        public int? Turn { get; set; }

        [Required (ErrorMessage = "Please enter fade.")]
        [Display(Name = "Fade:")]
        public int? Fade { get; set; }

        [Required(ErrorMessage = "Please enter a plastic type.")]
        [Display(Name = "Plastic type:")]
        public string? Plastic { get; set; }

        [Required(ErrorMessage = "Please enter if bagged or not.")]
        [Display(Name = "In bag:")]
        public bool Bagged { get; set; }

        [Display(Name = "Image:")]
        public string? ImageName { get; set; }

        [NotMapped]
        [Display(Name = "Image:")]
        public IFormFile? ImageFile { get; set; }

        [Required(ErrorMessage = "Please select a category.")]
        [ForeignKey("CategoryId")]
        [Display(Name = "Category:")]
        public int? CategoryId { get; set; }
        public Category? Category { get; set; }

        [Required(ErrorMessage = "Please select a brand.")]
        [ForeignKey("BrandId")]
        [Display(Name = "Brand:")]
        public int? BrandId { get; set; }
        public Brand? Brand { get; set; }


    }

    public class Category
    {
        [Key]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Please enter a category name.")]
        [Display(Name = "Category Name:")]
        [StringLength(40, MinimumLength = 3, ErrorMessage = "Category name must be between 3 and 40 characters.")]
        [RegularExpression(@"^[a-zA-Z\s]*$", ErrorMessage = "Category name must be alphabetical characters only.")]
        public string? CategoryName { get; set; }
    }

    public class Brand
    {
        [Key]
        public int BrandId { get; set; }

        [Required(ErrorMessage = "Please enter a brand name.")]
        [Display(Name = "Brand Name:")]
        [StringLength(40, MinimumLength = 3, ErrorMessage = "Brand name must be between 3 and 40 characters.")]
        public string? BrandName { get; set; }
    }
}
