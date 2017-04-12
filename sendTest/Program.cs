
namespace sendTest
{
    using System;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.Azure.EventHubs;

    class Program
    {
        private static EventHubClient eventHubClient;
        //Initialising the connection settings
        private const string EhConnectionString = "Endpoint=sb://eventhubdemodiv.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=FveFVfhwjuVuhOYO9qMYx8OejbiuSzIUt95ZGCZkl94=";
        private const string EhEntityPath = "eventhubdemodiv";

        static void Main(string[] args)
        {
            MainAsync(args).GetAwaiter().GetResult();
        }

        private static async Task MainAsync(string[] args)
        {
            // Creates an EventHubsConnectionStringBuilder object from the connection string, and sets the EntityPath.
            // Typically, the connection string should have the entity path in it, but for the sake of this simple scenario
            // we are using the connection string from the namespace.
            var connectionStringBuilder = new EventHubsConnectionStringBuilder(EhConnectionString)
            {
                EntityPath = EhEntityPath
            };

            eventHubClient = EventHubClient.CreateFromConnectionString(connectionStringBuilder.ToString());

            //Send 100 meesages
            await SendMessagesToEventHub(100);

            //Closing connection
            await eventHubClient.CloseAsync();

            Console.WriteLine("Press ENTER to exit.");
            Console.ReadLine();
        }

        // Creates an Event Hub client and sends 100 messages to the event hub.
        private static async Task SendMessagesToEventHub(int numMessagesToSend)
        {
            //Looping through sent messages from 1 to 100 with an increment of +1
            for (var i = 0; i < numMessagesToSend; i++)
            {
                //Try Catch statement in order to be able to catch any exceptions
                try
                {
                    var message = $"Message {i}"; //Message is incremented by 1
                    Console.WriteLine($"Sending message: {message}");
                    await eventHubClient.SendAsync(new EventData(Encoding.UTF8.GetBytes(message)));
                }
                catch (Exception exception)
                {
                    Console.WriteLine($"{DateTime.Now} > Exception: {exception.Message}");
                }

                await Task.Delay(10);
            }

            Console.WriteLine($"{numMessagesToSend} messages sent.");
        }
    }
}
