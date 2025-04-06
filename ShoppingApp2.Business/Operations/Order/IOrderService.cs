using ShoppingApp2.Business.Types;

namespace ShoppingApp2.Business.Operations.Order.Dtos
{
    public interface IOrderService
    {
        Task<ServiceMessage> AddOrder(AddOrderDto order);
        Task<OrderDto> GetOrder(int id);
        Task<List<OrderDto>> GetOrders();
        Task<ServiceMessage> DeleteOrder(int id);
        Task<ServiceMessage> UpdateOrder(UpdateOrderDto hotel);
    }
}