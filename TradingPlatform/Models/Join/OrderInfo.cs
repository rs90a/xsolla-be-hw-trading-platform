using System;
using System.ComponentModel.DataAnnotations;
using TradingPlatform.Models.Order;

namespace TradingPlatform.Models.Join
{
    public class OrderInfo
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public string UserId { get; set; }
        
        [Required]
        public string Key { get; set; }
        
        [Required]
        public string DateTime { get; set; }
        
        [Required]
        [DataType(DataType.EmailAddress)]
        public string RecipientEmail { get; set; }
        
        [Required]
        public double Amount { get; set; }
        
        [Required]
        public OrderGamesDto Game { get; set; }
    }
}