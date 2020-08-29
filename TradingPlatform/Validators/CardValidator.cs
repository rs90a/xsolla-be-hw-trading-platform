using System;
using System.ComponentModel.DataAnnotations;
using TradingPlatform.Models.Payment;

namespace TradingPlatform.Validators
{
    /// <summary>
    /// Валидатор платежной карты
    /// </summary>
    public class CardValidatorAttribute : ValidationAttribute
    {
        private const string ErrorExpirationMessage = "Срок действия карты истек";
        private const string ErrorNumberMessage = "Некорректный номер платежной карты";

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var card = (Card) value;

            if (!IsValidExpirationDate(card))
                return new ValidationResult(ErrorExpirationMessage);
            if (!IsValidCardNumber(card))
                return new ValidationResult(ErrorNumberMessage);

            return ValidationResult.Success;
        }

        /// <summary>
        /// Валидация срока действия карты
        /// </summary>
        /// <param name="card">Сведения о банковской карте</param>
        /// <returns>true - карта действующая, false - иначе</returns>
        private bool IsValidExpirationDate(Card card)
        {
            var dateExp = card.Date;

            return dateExp.Year > DateTime.Now.Year ||
                   dateExp.Year == DateTime.Now.Year && dateExp.Month >= DateTime.Now.Month;
        }

        /// <summary>
        /// Валидация номера карты (упрощенный алгоритм Луна)
        /// </summary>
        /// <param name="card">Сведения о банковской карте</param>
        /// <returns>true - номер карты валиден, false - иначе</returns>
        private bool IsValidCardNumber(Card card)
        {
            var sum = 0;
            var len = card.Number.Length;
            var controlNum = len % 2;

            for (var i = 0; i < len; i++)
            {
                var addition = card.Number[i] - '0';
                if (i % 2 == controlNum)
                {
                    addition *= 2;
                    addition -= addition > 9 ? 9 : 0;
                }

                sum += addition;
            }

            return sum % 10 == 0;
        }
    }
}