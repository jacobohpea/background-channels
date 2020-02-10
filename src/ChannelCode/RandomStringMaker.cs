using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace WorkerService.ChannelCode
{
    public class RandomStringMaker
    {
        public async IAsyncEnumerable<string> GetRandomStrings(int numberOfStrings, [EnumeratorCancellation] CancellationToken token = default)
        {
            for (var i = 0; i < numberOfStrings; i++)
            {
                yield return Guid.NewGuid().ToString();

                await Task.Delay(TimeSpan.FromSeconds(1.5), token);
            }
        }
    }
}