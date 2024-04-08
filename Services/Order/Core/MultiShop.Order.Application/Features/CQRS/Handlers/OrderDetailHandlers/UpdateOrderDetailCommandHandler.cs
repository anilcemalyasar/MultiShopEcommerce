using MultiShop.Order.Application.Features.CQRS.Commands.OrderDetailCommands;
using MultiShop.Order.Application.Interfaces;
using MultiShop.Order.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiShop.Order.Application.Features.CQRS.Handlers.OrderDetailHandlers
{
    public class UpdateOrderDetailCommandHandler
    {
        private readonly IRepository<OrderDetail> _repository;

        public UpdateOrderDetailCommandHandler(IRepository<OrderDetail> repository)
        {
            _repository = repository;
        }

        public async Task Handle(UpdateOrderDetailCommand command)
        {
            var orderDetail = await _repository.GetByIdAsync(command.OrderDetailId);
            orderDetail.ProductName = command.ProductName;
            orderDetail.ProductPrice = command.ProductPrice;
            orderDetail.ProductTotalPrice = command.ProductTotalPrice;
            orderDetail.ProductId = command.ProductId;
            orderDetail.ProductAmount = command.ProductAmount;
            orderDetail.OrderingId = command.OrderingId;
            await _repository.UpdateAsync(orderDetail);
        }

    }
}
