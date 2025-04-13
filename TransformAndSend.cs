using System;
using System.Text.Json;
using System.Reflection;
using Azure.Storage.Queues.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;

namespace TransformAndSend
{
    public class TransformAndSend
    {
        private readonly ILogger<TransformAndSend> _logger;
        private readonly IConfiguration _config;

        public TransformAndSend(ILogger<TransformAndSend> logger, IConfiguration config)
        {
            _logger = logger;
            _config = _config;
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
            
            // Post to SFTP
            var fileName = $"Order-{payload.SalesOrder}-{DateTime.UtcNow:yyyyMMddHHmmss}.csv";
            var sftp = new SftpUploader(_config);
            sftp.UploadCsv(fileName, csv);

            _logger.LogInformation($"📤 CSV uploaded to SFTP: {fileName}");
        }
    }
}
