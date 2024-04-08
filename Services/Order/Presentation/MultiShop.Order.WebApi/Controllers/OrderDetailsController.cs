using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MultiShop.Order.Application.Features.CQRS.Commands.OrderDetailCommands;
using MultiShop.Order.Application.Features.CQRS.Handlers.OrderDetailHandlers;
using MultiShop.Order.Application.Features.CQRS.Queries.OrderDetailQueries;

namespace MultiShop.Order.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderDetailsController : ControllerBase
    {
        private readonly GetOrderDetailByIdQueryHandler _getOrderDetailByIdQueryHandler;
        private readonly GetOrderDetailQueryHandler _getOrderDetailQueryHandler;
        private readonly CreateOrderDetailCommandHandler _createOrderDetailCommandHandler;
        private readonly RemoveOrderDetailCommandHandler _removeOrderDetailCommandHandler;
        private readonly UpdateOrderDetailCommandHandler _updateOrderDetailCommandHandler;

        public OrderDetailsController(RemoveOrderDetailCommandHandler removeOrderDetailCommandHandler, 
                CreateOrderDetailCommandHandler createOrderDetailCommandHandler, 
                GetOrderDetailQueryHandler getOrderDetailQueryHandler, 
                GetOrderDetailByIdQueryHandler getOrderDetailByIdQueryHandler, 
                UpdateOrderDetailCommandHandler updateOrderDetailCommandHandler)
        {
            _removeOrderDetailCommandHandler = removeOrderDetailCommandHandler;
            _createOrderDetailCommandHandler = createOrderDetailCommandHandler;
            _getOrderDetailQueryHandler = getOrderDetailQueryHandler;
            _getOrderDetailByIdQueryHandler = getOrderDetailByIdQueryHandler;
            _updateOrderDetailCommandHandler = updateOrderDetailCommandHandler;
        }

        [HttpGet]
        public async Task<IActionResult> OrderDetailList()
        {
            var orderDetails = await _getOrderDetailQueryHandler.Handle();
            return Ok(orderDetails);
        }

        [HttpGet("id")]
        public async Task<IActionResult> GetOrderDetailById(int id)
        {
            var orderDetail = await _getOrderDetailByIdQueryHandler.Handle(new GetOrderDetailByIdQuery(id));
            return Ok(orderDetail);
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrderDetail(CreateOrderDetailCommand command)
        {
            await _createOrderDetailCommandHandler.Handle(command);
            return Ok("Ürün detay bilgileri başarıyla eklendi!");
        }

        [HttpPut]
        public async Task<IActionResult> UpdateOrderDetail(UpdateOrderDetailCommand command)
        {
            await _updateOrderDetailCommandHandler.Handle(command);
            return Ok("Ürün detay bilgileri başarıyla güncellendi!");
        }

        [HttpDelete("int")]
        public async Task<IActionResult> RemoveOrderDetail(RemoveOrderDetailCommand command)
        {
            await _removeOrderDetailCommandHandler.Handle(command);
            return Ok("Ürün detay bilgileri başarıyla silindi!");
        }



    }
}
