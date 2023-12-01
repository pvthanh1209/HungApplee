using System.ComponentModel.DataAnnotations;

namespace HungApple.Models
{
    public class Discount
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        [System.ComponentModel.DisplayName("Tên chương trình giảm giá")]
        public string? Name { get; set; }

        [Required]
        [System.ComponentModel.DisplayName("Giá giảm")]
        public decimal Amount { get; set; }

        [MaxLength(20)]
        [System.ComponentModel.DisplayName("Mã giảm giá")]
        public string? Code { get; set; }

        [Required]
        [System.ComponentModel.DisplayName("Ngày bẳt đầu")]
        public DateTime StartDate { get; set; }

        [Required]
        [System.ComponentModel.DisplayName("Ngày kết thúc")]
        public DateTime EndDate { get; set; }
    }
}
