namespace Notification.Customer.NotificationService.Core.Consumers
{
    using MassTransit;
    using Messaging.Core.Commands;
    using Messaging.Core.Constants;
    using Microsoft.Extensions.Logging;
    using Notification.Customer.EmailService.Models;
    using Notification.Customer.EmailService.Senders;
    using SixLabors.ImageSharp;
    using System.Threading.Tasks;

    public class SendOrderToUserEmailConsumer : IConsumer<SendOrderToUserEmail>
    {
        private readonly ILogger<SendOrderToUserEmailConsumer> logger;
        private readonly IEmailSender emailSender;

        public SendOrderToUserEmailConsumer(ILogger<SendOrderToUserEmailConsumer> logger, IEmailSender emailSender)
        {
            this.logger = logger;
            this.emailSender = emailSender;
        }

        public Task Consume(ConsumeContext<SendOrderToUserEmail> context)
        {
            try
            {
                this.logger.LogInformation($"{nameof(SendOrderToUserEmail)} event received");

                var orderProcessedEvent = context.Message;

                //Commented to work locally
                //this.StoreFaces(orderProcessedEvent);

                var mailAddess = new string[] { orderProcessedEvent.UserEmail };

                this.emailSender.SendEmailAsync(new Message(mailAddess, $"Your order {orderProcessedEvent.OrderId}", "From FacesAndFaces", orderProcessedEvent.Faces));

                context.Publish<UpdateOrderStatus>(new
                {
                    OrderId = orderProcessedEvent.OrderId,
                    DispatchDateTime = DateTime.UtcNow
                });

                return Task.CompletedTask;
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, $"Error in {nameof(SendOrderToUserEmailConsumer)}");
                throw;
            }
        }

        private void StoreFaces(SendOrderToUserEmail orderProcessedEvent)
        {
            if (orderProcessedEvent.Faces == null ||
                !orderProcessedEvent.Faces.Any())
            {
                this.logger.LogWarning("No faces detected");
                return;
            }

            var facesRootDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Faces");
            Directory.CreateDirectory($"{facesRootDirectory}/{orderProcessedEvent.OrderId}");

            for (var i = 0; i < orderProcessedEvent.Faces.Count; i++)
            {
                var face = orderProcessedEvent.Faces[i];
                var ms = new MemoryStream(face);
                var image = Image.Load(ms);
                var imageNamePath = $"{facesRootDirectory}/{orderProcessedEvent.OrderId}/face{i}.jpg";
                image.Save(imageNamePath);
                this.logger.LogInformation($"{imageNamePath} stored");
            }
        }
    }
}