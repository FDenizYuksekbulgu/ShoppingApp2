using Microsoft.EntityFrameworkCore;
using ShoppingApp2.Business.Operations.Order.Dtos;
using ShoppingApp2.Business.Operations.Product.Dtos;
using ShoppingApp2.Business.Types;
using ShoppingApp2.Data.Entities;
using ShoppingApp2.Data.Repositories;
using ShoppingApp2.Data.UnitOfWork;

namespace ShoppingApp2.Business.Operations.Order.Services
{
    public class OrderManager : IOrderService
    {
        private readonly IRepository<OrderEntity> _orderRepository;
        private readonly IRepository<OrderProductEntity> _orderProductRepository;
        private readonly IUnitOfWork _unitOfWork;

        public OrderManager(IRepository<OrderEntity> orderRepository,
                            IRepository<OrderProductEntity> orderProductRepository,
                            IUnitOfWork unitOfWork)
        {
            _orderRepository = orderRepository;
            _orderProductRepository = orderProductRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ServiceMessage> AddOrder(AddOrderDto order)
        {

            await _unitOfWork.BeginTransaction();

            var orderEntity = new OrderEntity
            {
                OrderDate = order.OrderDate,
                TotalAmount = order.TotalAmount,
                CustomerId = order.CustomerId
            };

            _orderRepository.Add(orderEntity);

            try
            {
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw new Exception("Sipariş kaydı sırasında bir sorun ile karşılaşıldı.");
            }

            foreach (var productId in order.ProductIds)
            {
                var orderProduct = new OrderProductEntity
                {
                    OrderId = orderEntity.Id,
                    ProductId = productId
                };

                _orderProductRepository.Add(orderProduct);
            }

            try
            {
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransaction();
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackTransaction();
                throw new Exception("Sipariş özellikleri eklenirken bir hata ile karşılaşıldı, süreç başa sarıldı.");
            }

            return new ServiceMessage { IsSucceed = true };
        }

        public async Task<List<OrderDto>> GetOrders()
        {
            var orders = await _orderRepository.GetAll()
                .Select(x => new OrderDto
                {
                    OrderDate = x.OrderDate,
                    TotalAmount = x.TotalAmount,
                    CustomerId = x.CustomerId,
                    OrderProducts = x.OrderProducts.Select(p => new ProductDto
                    {
                        Id = p.Product.Id,
                        ProductName = p.Product.ProductName,
                        Price = p.Product.Price,
                        StockQuantity = p.Product.StockQuantity,
                    })
                }).ToListAsync();

            return orders;
        }

        public async Task<OrderDto> GetOrder(int id)
        {
            var order = await _orderRepository.GetAll(x => x.Id == id)
                .Select(x => new OrderDto
                {
                    OrderDate = x.OrderDate,
                    TotalAmount = x.TotalAmount,
                    CustomerId = x.CustomerId,
                    OrderProducts = x.OrderProducts.Select(p => new ProductDto
                    {
                        Id = p.Product.Id,
                        ProductName = p.Product.ProductName,
                        Price = p.Product.Price,
                        StockQuantity = p.Product.StockQuantity,
                    })
                }).FirstOrDefaultAsync();

            if (order == null)
                throw new Exception("Sipariş bulunamadı.");

            return order;
        }

        public async Task<ServiceMessage> UpdateOrder(UpdateOrderDto order)
        {
            var orderEntity = _orderRepository.GetById(order.Id);

            if (orderEntity == null)
            {
                return new ServiceMessage
                {
                    IsSucceed = false,
                    Message = "Sipariş bulunamadı."
                };
            }

            await _unitOfWork.BeginTransaction();

            orderEntity.OrderDate = order.OrderDate;
            orderEntity.TotalAmount = order.TotalAmount;
            orderEntity.CustomerId = order.CustomerId;

            _orderRepository.Update(orderEntity);

            try
            {
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackTransaction();
                throw new Exception("Sipariş bilgilerini güncellenirken bir hata ile karşılaşıldı.");
            }

            var orderProducts = _orderProductRepository.GetAll(x => x.OrderId == orderEntity.Id).ToList();
            foreach (var orderProduct in orderProducts)
            {
                _orderProductRepository.Delete(orderProduct, false);
            }

            foreach (var productId in order.ProductIds)
            {
                var orderProduct = new OrderProductEntity
                {
                    OrderId = orderEntity.Id,
                    ProductId = productId
                };

                _orderProductRepository.Add(orderProduct);
            }

            try
            {
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransaction();
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackTransaction();
                throw new Exception("Sipariş bilgileri güncellenirken bir hata oluştu, süreç başa sarıldı.");
            }

            return new ServiceMessage { IsSucceed = true };
        }

        public async Task<ServiceMessage> DeleteOrder(int id)
        {
            var order = _orderRepository.GetById(id);
            if (order == null)
            {
                return new ServiceMessage
                {
                    IsSucceed = false,
                    Message = "Sipariş bulunamadı."
                };
            }

            _orderRepository.Delete(order);
            await _unitOfWork.SaveChangesAsync();

            return new ServiceMessage { IsSucceed = true };
        }
    }
}
