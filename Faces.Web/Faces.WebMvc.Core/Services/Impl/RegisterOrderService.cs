namespace Faces.WebMvc.Core.Services.Impl
{
    using Faces.WebMvc.Core.Models.Order.Requests;
    using MassTransit;
    using Messaging.Core.Commands;

    public class RegisterOrderService : IRegisterOrderService
    {
        private readonly IPublishEndpoint publishEndpoint;

        public RegisterOrderService(IPublishEndpoint publishEndpoint)
        {
            this.publishEndpoint = publishEndpoint;
        }

        public async Task<Guid> Register(Stream imageStream, RegisterOrderRequest registerOrderRequest)
        {
            ArgumentNullException.ThrowIfNull(imageStream);
            ArgumentNullException.ThrowIfNull(registerOrderRequest);

            var imageMemoryStream = new MemoryStream();
            await imageStream.CopyToAsync(imageMemoryStream);

            var orderId = Guid.NewGuid();

            await this.publishEndpoint.Publish<RegisterOrder>(new
            {
                OrderId = orderId,
                PictureUrl = registerOrderRequest.PictureUrl,
                UserEmail = registerOrderRequest.UserEmail,
                ImageData = imageMemoryStream.ToArray()
            });

            return orderId;
        }
    }
}