﻿namespace Orders.Core.Messages.Consumers
{
    using MassTransit;
    using Messaging.Core.Commands;
    using Microsoft.AspNetCore.SignalR;
    using Orders.Core.Hubs;
    using Orders.Core.Models;
    using Orders.Core.Models.Constants;
    using Orders.Core.Persistence.Repository;
    using System;
    using System.Threading.Tasks;

    public class UpdateOrderStatusConsumer : IConsumer<UpdateOrderStatus>
    {
        private readonly IOrderRepository orderRepository;
        private readonly IHubContext<OrderHub> hubContext;

        public UpdateOrderStatusConsumer(IOrderRepository orderRepository, IHubContext<OrderHub> hubContext)
        {
            this.orderRepository = orderRepository;
            this.hubContext = hubContext;
        }

        public async Task Consume(ConsumeContext<UpdateOrderStatus> context)
        {
            var message = context.Message;
            var orderId = message.OrderId;
            UpdateDataBase(orderId);

            await this.hubContext.Clients.All.SendAsync(SignalRHubConstants.UpdateOrdersCommand, "Order Dispatched", orderId);
        }

        private void UpdateDataBase(Guid orderId)
        {
            var order = this.orderRepository.GetOrderById(orderId);
            if (order == null)
            {
                throw new InvalidOperationException("Order does not exists");
            }

            order.Status = Status.Sent;
            this.orderRepository.UpdateOrder(order);
        }
    }
}