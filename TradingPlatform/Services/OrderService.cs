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

        public OrderService(TradingPlatformDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        /// <summary>
        /// Добавление сведений о заказе в историю заказов
        /// </summary>
        public async Task AddOrder(User user, PaymentInfoCache paymentInfoCache)
        {
            using (var transaction = await dbContext.Database.BeginTransactionAsync())
            {
                var orderGamesDto = await AddOrderGamesDto(paymentInfoCache);
                var orderSellersDto = await AddOrderSellersDto(paymentInfoCache);
                var orderDto = GetOrderDto(orderGamesDto, orderSellersDto, paymentInfoCache, user);

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

        private OrderDto GetOrderDto(OrderGamesDto orderGamesDto, OrderSellersDto orderSellersDto,
            PaymentInfoCache paymentInfoCache, User user) =>
            new OrderDto
            {
                Id = 0,
                UserId = user.Id,
                SellerId = orderSellersDto.Id,
                GameId = orderGamesDto.Id,
                Key = paymentInfoCache.KeyDto.Key,
                DateTime = DateTime.Now,
                RecipientEmail = user.Email,
                Amount = paymentInfoCache.Amount
            };
    }
}