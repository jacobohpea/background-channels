using System;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace WorkerService
{
    public class StringLoggerWorker : BackgroundService
    {
        private readonly ChannelReader<string> _channelToRead;
        private readonly ILogger<StringLoggerWorker> _logger;

        public StringLoggerWorker(ILogger<StringLoggerWorker> logger, ChannelReader<string> channelToRead)
        {
            _logger = logger;
            _channelToRead = channelToRead;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

                var item = await _channelToRead.ReadAsync(stoppingToken);

                _logger.LogInformation($"Hello, I am a Worker! I was asked to tell you '{item}'. Thanks!");
            }
        }
    }
}