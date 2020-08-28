using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TradingPlatform.Database;
using TradingPlatform.Interfaces;
using TradingPlatform.Models.Game;
using TradingPlatform.Models.Keystore;

namespace TradingPlatform.Services
{
    /// <summary>
    /// Cервис для работы c игровыми ключами
    /// </summary>
    public class KeystoreService : IKeystoreService
    {
        private readonly TradingPlatformDbContext dbContext;
        private readonly IAccountService accountService;

        public KeystoreService(TradingPlatformDbContext dbContext, IAccountService accountService)
        {
            this.dbContext = dbContext;
            this.accountService = accountService;
        }

        /// <summary>
        /// Добавление набора игровых ключей для игры
        /// </summary>
        public async Task AddKeystore(AddKeystoreRequest addKeystoreRequest)
        {
            var isOwner = await UserIsOwnerOfGame(addKeystoreRequest.GameId);
            if (!isOwner)
                throw new ArgumentException("Невозможно добавить игровые ключи");

            var keys = addKeystoreRequest.Keys.Select(key => new KeyDto
            {
                Id = 0,
                GameId = addKeystoreRequest.GameId,
                Key = key,
                Reserved = false
            });
            
            await dbContext.Keys.AddRangeAsync(keys);
            await dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Удаление игрового ключа по id
        /// </summary>
        public async Task DeleteKey(int keyId)
        {
            var gameKey = dbContext.Keys.FirstOrDefault(key => key.Id == keyId);
            
            if (gameKey == null)
                throw new ArgumentException("Удаляемый игровой ключ не существует");
            
            var isOwner = await UserIsOwnerOfGame(gameKey.GameId);
            if (!isOwner)
                throw new ArgumentException("Невозможно удалить игровой ключ");

            dbContext.Keys.Remove(gameKey);
            await dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Резервирование игрового ключа
        /// </summary>
        public async Task<KeyDto> ReserveKey(PaymentGameRequest paymentGameRequest)
        {
            var keyDto = await GetKey(paymentGameRequest.Id);

            if (keyDto == null)
                throw new ArgumentException(@$"Игровые ключи для игры ""{paymentGameRequest.Name}"" закончились");

            keyDto.Reserved = true;
            dbContext.Keys.Update(keyDto);
            await dbContext.SaveChangesAsync();

            return keyDto;
        }

        /// <summary>
        /// Поиск игрового ключа доступного для покупки
        /// </summary>
        private async Task<KeyDto> GetKey(int gameId) =>
            await dbContext.Keys.FirstOrDefaultAsync(keyDto => keyDto.GameId == gameId && !keyDto.Reserved);
        
        /// <summary>
        /// Проверка, что игра принадлежит текущему пользователю
        /// </summary>
        private async Task<bool> UserIsOwnerOfGame(int gameId)
        {
            var userId = accountService.GetCurrentUserId();
            
            return await dbContext.Games.AnyAsync(game =>
                game.SellerId == userId && gameId == game.Id);
        }
    }
}