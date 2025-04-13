using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;
using System.IO;

namespace TransformAndSend
{
    public class CsvGenerator()
    {
        public string GenerateCsv(Payload payload){
            var records = new List<CsvRow>();

            foreach (var container in payload.Containers)
            {
                foreach (var item in container.Items)
                {
                    records.Add(new CsvRow
                    {
                        CustomerReference = payload.SalesOrder,
                        LoadId = container.LoadId,
                        ContainerType = container.ContainerType,
                        ItemCode = item.ItemCode,
                        ItemQuantity = item.Quantity,
                        ItemWeight = item.CartonWeight,
                        Street = payload.DeliveryAddress.Street,
                        City = payload.DeliveryAddress.City,
                        State = payload.DeliveryAddress.State,
                        PostalCode = payload.DeliveryAddress.PostalCode,
                        Country = payload.DeliveryAddress.Country
                    });
                }
            }

            using var writer = new StringWriter();
            using var csv = new CsvWriter(writer, new CsvConfiguration(CultureInfo.InvariantCulture));
            csv.Context.RegisterClassMap<PayloadCsvRowMap>();
            csv.WriteRecords(records);

            return writer.ToString();
        }

    }

    public sealed class PayloadCsvRowMap : ClassMap<CsvRow>
    {
        public PayloadCsvRowMap()
        {
            Map(m => m.CustomerReference).Name("CustomerReference");
            Map(m => m.LoadId).Name("LoadId");
            Map(m => m.ContainerType).Name("ContainerType");
            Map(m => m.ItemCode).Name("ItemCode");
            Map(m => m.ItemQuantity).Name("ItemQuantity");
            Map(m => m.ItemWeight).Name("ItemWeight");
            Map(m => m.Street).Name("Street");
            Map(m => m.City).Name("City");
            Map(m => m.State).Name("State");
            Map(m => m.PostalCode).Name("PostalCode");
            Map(m => m.Country).Name("Country");
        }
    }

}
