using System;
using System.Text.Json;
using Azure.Storage.Queues.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace TransformAndSend
{
    public class TransformAndSend
    {
        private readonly ILogger<TransformAndSend> _logger;

        public TransformAndSend(ILogger<TransformAndSend> logger)
        {
            _logger = logger;
        }

        [Function(nameof(TransformAndSend))]
        public void Run([QueueTrigger("dispatch-processing-queue", Connection = "AzureWebJobsStorage")] string queueItem)
        {
            var jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            var payload = JsonSerializer.Deserialize<Payload>(queueItem, jsonOptions); // Container type already mapped
            _logger.LogInformation("Payload mapping complete");
            // Transform to csv
            var csvGen = new CsvGenerator();
            var csv = csvGen.GenerateCsv(payload);
            _logger.LogInformation("Csv Generated");
            // Store in Blob - maybe
            // post to folder
        }
    }
}
