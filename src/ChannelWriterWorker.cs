using System;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace WorkerService.ChannelCode
{
    public class ChannelWriterWorker : BackgroundService
    {
        private readonly ChannelWriter<string> _channelWriter;
        private readonly ILogger<ChannelWriterWorker> _logger;

        public ChannelWriterWorker(ILogger<ChannelWriterWorker> logger, ChannelWriter<string> channelWriter)
        {
            _logger = logger;
            _channelWriter = channelWriter;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

                await foreach (var item in new RandomStringMaker().GetRandomStrings(5, stoppingToken))
                {
                    await _channelWriter.WriteAsync(item, stoppingToken);
                }

                await Task.Delay(TimeSpan.FromMinutes(.5), stoppingToken);
            }

            var complete = _channelWriter.TryComplete();
        }
    }
}