using System;
using Azure.Storage.Queues.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace TransformAndSend
{
    public class Payload
    {
        public int ControlNumber { get; set; }
        public string SalesOrder { get; set; }
        public List<Container> Containers { get; set; }
        public DeliveryAddress DeliveryAddress{ get; set; }
    }
    public class Container
    {
        public string LoadId { get; set; }

        private string _containerType;
        public string ContainerType 
        { 
            get => _containerType; 
            set => _containerType = value switch
            {
                "20RF" => "REF20",
                "40RF" => "REF40",
                "20HC" => "HC20",
                "40HC" => "HC40",
            }; 
        }
        public List<Item> Items { get; set; }
    }

    public class Item
    {
        public string ItemCode { get; set; }
        public int Quantity { get; set; }
        public decimal CartonWeight { get; set; }
    }
    public class DeliveryAddress
    {
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
    }

    public class CsvRow
    {
        public string CustomerReference { get; set; }
        public string LoadId { get; set; }
        public string ContainerType { get; set; }
        public string ItemCode { get; set; }
        public int ItemQuantity { get; set; }
        public decimal ItemWeight { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
    }
}
