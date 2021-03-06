﻿using System;
using System.Threading.Tasks;
using NServiceBus;

class Program
{
    static void Main()
    {
        AsyncMain().GetAwaiter().GetResult();
    }

    static async Task AsyncMain()
    {
        var configuration = ConfigBuilder.Build("Outbox");
        var endpoint = await Endpoint.Start(configuration);
        Console.WriteLine("Press 'Enter' to start a saga");
        Console.WriteLine("Press any other key to exit");
        try
        {
            while (true)
            {
                var key = Console.ReadKey();
                Console.WriteLine();

                if (key.Key != ConsoleKey.Enter)
                {
                    return;
                }
                await endpoint.SendLocal(new StartSagaMessage
                {
                    MySagaId = Guid.NewGuid()
                });
            }
        }
        finally
        {
            await endpoint.Stop();
        }
    }
}