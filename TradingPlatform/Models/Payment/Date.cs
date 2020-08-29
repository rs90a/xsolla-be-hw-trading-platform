using System.ComponentModel.DataAnnotations;

namespace TradingPlatform.Models.Payment
{
    /// <summary>
    /// Срок действия карты
    /// </summary>
    public class Date
    {
        [Required] [Range(1, 12)] public int Month { get; set; }

        [Required]
        [Range(2000, 9999)]
        public int Year { get; set; }
    }
}