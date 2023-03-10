using IoTHubTrigger = Microsoft.Azure.WebJobs.EventHubTriggerAttribute;

using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using System.Text;
using System.Net.Http;
using Microsoft.Extensions.Logging;
using Azure.Messaging.EventHubs;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.VisualBasic;

namespace IoTEnrichment
{
    public class Function1
    {
        private static HttpClient client = new HttpClient();

        [FunctionName("Function1")]
        public async void Run([IoTHubTrigger("messages/events", Connection = "IotHub", ConsumerGroup = "function")] EventData message,
            [EventHub("enricheddata", Connection = "EventHub")] IAsyncCollector<string> outputEvents,
            ILogger log)
        {
            var sb = new StringBuilder();

            // Get IoT Hub System Properties
            foreach(var kv in message.SystemProperties)
            {
                sb.AppendLine($"\"{kv.Key}\": \"{kv.Value}\"");
            }

            var msg = $"C# IoT Hub trigger function processed a message: {Encoding.UTF8.GetString(message.EventBody)} with system properties {sb}";

            log.LogInformation(msg);
      
            await outputEvents.AddAsync(msg);
        }
    }
}