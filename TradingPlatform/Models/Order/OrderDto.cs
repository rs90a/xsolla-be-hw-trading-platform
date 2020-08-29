using System;
using System.ComponentModel.DataAnnotations;

namespace TradingPlatform.Models.Order
{
    public class OrderDto
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public int UserId { get; set; }
        
        [Required]
        public int SellerId { get; set; }
        
        [Required]
        public int GameId { get; set; }
        
        [Required]
        public string Key { get; set; }
        
        [Required]
        public DateTime DateTime { get; set; }
        
        [Required]
        [DataType(DataType.EmailAddress)]
        public string RecipientEmail { get; set; }
        
        [Required]
        public double Amount { get; set; }
    }
}