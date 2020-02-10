using System.Threading.Channels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WorkerService.ChannelCode;

namespace WorkerService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    var channel = Channel.CreateBounded<string>(new BoundedChannelOptions(2)
                    {
                        FullMode = BoundedChannelFullMode.Wait
                    });

                    services.AddTransient(provider => channel.Reader);
                    services.AddTransient(provider => channel.Writer);

                    services.AddHostedService<StringLoggerWorker>();
                    services.AddHostedService<ChannelWriterWorker>();
                });
        }
    }
}