using System.ComponentModel.DataAnnotations;

namespace TradingPlatform.Models.Order
{
    public class OrderSellersDto
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public string UserName { get; set; }
        
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        
        [Required]
        public string TextId { get; set; }
    }
}