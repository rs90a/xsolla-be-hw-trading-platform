using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TradingPlatform.Database;
using TradingPlatform.Interfaces;
using TradingPlatform.Models;
using TradingPlatform.Models.Order;
using TradingPlatform.Models.Payment;

namespace TradingPlatform.Services
{
    /// <summary>
    /// Сервис для работы с историей заказов
    /// </summary>
    public class OrderService : IOrderService
    {
        private readonly TradingPlatformDbContext dbContext;
        private readonly IHashService hashService;
        
        public OrderService(TradingPlatformDbContext dbContext, IHashService hashService)
        {
            this.dbContext = dbContext;
            this.hashService = hashService;
        }

        /// <summary>
        /// Добавление сведений о заказе в историю заказов
        /// </summary>
        public async Task AddOrder(User user, PaymentInfoCache paymentInfoCache, SessionInfo sessionInfo)
        {
            using (var transaction = await dbContext.Database.BeginTransactionAsync())
            {
                var orderGamesDto = await AddOrderGamesDto(paymentInfoCache);
                var orderSellersDto = await AddOrderSellersDto(paymentInfoCache);
                var orderSessionsDto = await AddOrderSessionsDto(sessionInfo);
                var orderDto = GetOrderDto(orderGamesDto, orderSellersDto, orderSessionsDto, paymentInfoCache, user);

                await dbContext.Orders.AddAsync(orderDto);
                await dbContext.SaveChangesAsync();

                await transaction.CommitAsync();
            }
        }

        private async Task<OrderGamesDto> AddOrderGamesDto(PaymentInfoCache paymentInfoCache)
        {
            var orderGamesDto = GetOrderGamesDto(paymentInfoCache);

            var orderGamesFromDb = await dbContext.OrderGames.FirstOrDefaultAsync(orderGames =>
                orderGames.Description == orderGamesDto.Description
                && orderGames.Name == orderGamesDto.Name);

            if (orderGamesFromDb != null)
                return orderGamesFromDb;

            await dbContext.OrderGames.AddAsync(orderGamesDto);
            await dbContext.SaveChangesAsync();

            return orderGamesDto;
        }

        private async Task<OrderSellersDto> AddOrderSellersDto(PaymentInfoCache paymentInfoCache)
        {
            var orderSellersDto = await GetOrderSellersDto(paymentInfoCache);

            var orderSellersFromDb = await dbContext.OrderSellers.FirstOrDefaultAsync(orderSellers =>
                orderSellers.Email == orderSellersDto.Email
                && orderSellers.IdentityId == orderSellersDto.IdentityId
                && orderSellers.UserName == orderSellersDto.UserName);

            if (orderSellersFromDb != null)
                return orderSellersFromDb;

            await dbContext.OrderSellers.AddAsync(orderSellersDto);
            await dbContext.SaveChangesAsync();

            return orderSellersDto;
        }

        private async Task<OrderSessionsDto> AddOrderSessionsDto(SessionInfo sessionInfo)
        {
            var orderSessionsDto = GetOrderSessionsDto(sessionInfo);

            await dbContext.OrderSessions.AddAsync(orderSessionsDto);
            await dbContext.SaveChangesAsync();

            return orderSessionsDto;
        }
        
        private OrderGamesDto GetOrderGamesDto(PaymentInfoCache paymentInfoCache) =>
            new OrderGamesDto
            {
                Id = 0,
                Name = paymentInfoCache.Game.Name,
                Description = paymentInfoCache.Game.Description
            };

        private async Task<OrderSellersDto> GetOrderSellersDto(PaymentInfoCache paymentInfoCache)
        {
            var seller = await dbContext.Users.FindAsync(paymentInfoCache.Game.SellerId);

            if (seller == null)
                throw new ArgumentException("Продавец не найден");

            return new OrderSellersDto
            {
                Id = 0,
                UserName = seller.UserName,
                Email = seller.Email,
                IdentityId = seller.Id,
            };
        }

        private OrderSessionsDto GetOrderSessionsDto(SessionInfo sessionInfo) =>
            new OrderSessionsDto
            {
                Id = 0,
                CardNumber = hashService.CreateHash(sessionInfo.CardNumber),
                SessionId = sessionInfo.SessionId
            };
        
        private OrderDto GetOrderDto(OrderGamesDto orderGamesDto, OrderSellersDto orderSellersDto,
            OrderSessionsDto orderSessionsDto, PaymentInfoCache paymentInfoCache, User user) =>
            new OrderDto
            {
                Id = 0,
                UserId = user.Id,
                SellerId = orderSellersDto.Id,
                GameId = orderGamesDto.Id,
                SessionId = orderSessionsDto.Id,
                Key = paymentInfoCache.KeyDto.Key,
                DateTime = DateTime.Now,
                RecipientEmail = user.Email,
                Amount = paymentInfoCache.Amount
            };
    }
}