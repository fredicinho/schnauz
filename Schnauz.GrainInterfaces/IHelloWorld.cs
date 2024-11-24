namespace Schnauz.GrainInterfaces;

public interface IHelloWorld : IGrainWithIntegerKey
{
    ValueTask<string> SayHello(string greeting);
}