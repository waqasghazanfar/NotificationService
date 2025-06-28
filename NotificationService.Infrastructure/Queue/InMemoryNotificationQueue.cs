namespace NotificationService.Infrastructure.Queue
{
    using System.Threading.Channels;
    using NotificationService.Application.Contracts.Infrastructure;
    using NotificationService.Domain.Entities;

    public class InMemoryNotificationQueue : IInMemoryNotificationQueue
    {
        private readonly Channel<NotificationLog> _queue;

        public InMemoryNotificationQueue()
        {
            var options = new BoundedChannelOptions(100)
            {
                FullMode = BoundedChannelFullMode.Wait
            };
            _queue = Channel.CreateBounded<NotificationLog>(options);
        }

        public async ValueTask EnqueueAsync(NotificationLog workItem)
        {
            if (workItem == null) throw new ArgumentNullException(nameof(workItem));
            await _queue.Writer.WriteAsync(workItem);
        }

        public async ValueTask<NotificationLog> DequeueAsync(CancellationToken cancellationToken)
        {
            return await _queue.Reader.ReadAsync(cancellationToken);
        }
    }
}