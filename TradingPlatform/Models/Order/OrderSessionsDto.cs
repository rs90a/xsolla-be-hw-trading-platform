using System.ComponentModel.DataAnnotations;

namespace TradingPlatform.Models.Order
{
    public class OrderSessionsDto
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public string SessionId { get; set; }
        
        [Required]
        public string CardNumber { get; set; }
    }
}