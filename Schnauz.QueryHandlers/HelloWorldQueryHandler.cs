using Schnauz.GrainInterfaces;
using Schnauz.QueryHandlers.Core;
using Schnauz.Shared.Dtos;
using Schnauz.Shared.Queries;

namespace Schnauz.QueryHandlers;
public class HelloWorldQueryHandler(IClusterClient clusterClient) : QueryHandlerBase<HelloWorldQuery, HelloWorldDto>
{
    protected override async Task<HelloWorldDto> Execute(HelloWorldQuery query)
    {
        var helloWorldGrain = clusterClient.GetGrain<IHelloWorld>(0);
        var greeting = await helloWorldGrain.SayHello("Hello, World!");
        return new HelloWorldDto { Response = greeting };
    }
}
