using Schnauz.GrainInterfaces;
using Microsoft.Extensions.Logging;

namespace Schnauz.Grains;

public class HelloWorld(ILogger<HelloWorld> logger) : Grain, IHelloWorld
{
    private readonly ILogger _logger = logger;

    ValueTask<string> IHelloWorld.SayHello(string greeting)
    {
        _logger.LogInformation("""
                               SayHello message received: greeting = "{Greeting}"
                               """,
            greeting);
        
        return ValueTask.FromResult($"""

                                     Client said: "{greeting}", so HelloGrain says: Hello!
                                     """);
    }
}