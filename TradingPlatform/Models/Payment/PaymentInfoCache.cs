using System.ComponentModel.DataAnnotations;
using TradingPlatform.Models.Keystore;

namespace TradingPlatform.Models.Payment
{
    public class PaymentInfoCache: PaymentInfo
    {
        [Required]
        public KeyDto KeyDto { get; set; }

        public PaymentInfoCache(PaymentInfo paymentInfo, KeyDto keyDtoDto): 
            base(paymentInfo.Amount, paymentInfo.Email, paymentInfo.Game)
        {
            KeyDto = keyDtoDto;
        }
    }
}