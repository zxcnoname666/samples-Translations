using Tests;
using Tests.gRPC;


Console.WriteLine("Введите номер теста:");
Console.WriteLine("1 - CheckSerialize");
Console.WriteLine("2 - UseDeepl");
Console.WriteLine("3 - GrpcClientTest");

switch (Console.ReadLine())
{
    case "1":
        CheckSerialize.Run();
        break;
    
    case "2":
        await UseDeepl.Run();
        break;
    
    case "3":
        GrpcClientTest.Run();
        break;
}