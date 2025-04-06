using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShoppingApp2.Business.Operations.Order.Dtos;
using ShoppingApp2.Business.Operations.Order.Services;
using ShoppingApp2.WebApi.Filters;
using ShoppingApp2.WebApi.Models;

namespace ShoppingApp2.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;
        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrder(int id)
        {
            var order = await _orderService.GetOrder(id);

            if (order is null)
                return NotFound();
            else
                return Ok(order);
        }

        [HttpGet]
        public async Task<IActionResult> GetOrders()
        {
            var orders = await _orderService.GetOrders();

            return Ok(orders);
        }

        [HttpPost]
        [TimeRestrictedActionFilter("09:00", "17:00")]
        public async Task<IActionResult> AddOrder(AddOrderRequest request)
        {
            var addOrderDto = new AddOrderDto
            {
                OrderDate = request.OrderDate,
                TotalAmount = request.TotalAmount,
                CustomerId = request.CustomerId 
            };

            var result = await _orderService.AddOrder(addOrderDto);

            if (!result.IsSucceed)
                return BadRequest(result.Message);
            else
                return Ok();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var result = await _orderService.DeleteOrder(id);

            if (!result.IsSucceed)
                return NotFound(result.Message);
            else
                return Ok();
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        //[TimeControlFilter]
        public async Task<IActionResult> UpdateOrder(int id, UpdateOrderRequest request)
        {
            var updateOrderDto = new UpdateOrderDto
            {
                Id = id,
                 CustomerId= request.CustomerId,
                 OrderDate = request.OrderDate,
                 TotalAmount = request.TotalAmount,
                ProductIds = request.ProductIds
            };

            var result = await _orderService.UpdateOrder(updateOrderDto);

            if (!result.IsSucceed)
                return NotFound(result.Message);
            else
                return await GetOrder(id);
        }
    }
}
