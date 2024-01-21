using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CodingAssessment_UserStory2
{
    class Flight
    {
        public int Number { get; set; }
        public string Departure { get; set; }
        public string Arrival { get; set; }
        public int Day { get; set; }
    }
    class Order
    {
        public string OrderId { get; set; }
        public string Destination { get; set; }
    }
    class Program
    {
        static void Main(string[] args)
        {

            //User Story 2
            try
            {
                // Read the flight schedule from a JSON file
                string json = System.IO.File.ReadAllText("D:\\Assessment\\coding-assigment-orders.json");
                JObject jsonObject = JObject.Parse(json);

                // Extract flights from the JSON object
                List<Flight> flights = jsonObject.Properties()
                    .Where(p => p.Value["destination"] != null)
                    .Select(p => new Flight
                    {
                        Number = int.Parse(p.Name.Replace("order-", "")),
                        Departure = "YUL",  // Assuming YUL as the default departure
                        Arrival = p.Value["destination"].ToString(),
                        Day = (int.Parse(p.Name.Replace("order-", "")) % 2) + 1 // Assuming flights are scheduled every 2 days
                    })
                    .ToList();

                // Read orders from the JSON file
                string ordersJson = System.IO.File.ReadAllText("D:\\Assessment\\coding-assigment-orders.json");
                Dictionary<string, JObject> orderData = JsonConvert.DeserializeObject<Dictionary<string, JObject>>(ordersJson);

                // Display flight itineraries for orders
                Console.WriteLine("Flight Itineraries:");
                foreach (var orderEntry in orderData)
                {
                    string orderId = orderEntry.Key;
                    JObject orderDetails = orderEntry.Value;

                    string destination = orderDetails["destination"]?.ToString();
                    Order order = new Order { OrderId = orderId, Destination = destination };

                    Flight scheduledFlight = flights.FirstOrDefault(f => f.Arrival == order.Destination);
                    if (scheduledFlight != null)
                    {
                        Console.WriteLine($"order: {order.OrderId}, flightNumber: {scheduledFlight.Number}, departure: {scheduledFlight.Departure}, arrival: {scheduledFlight.Arrival}, day: {scheduledFlight.Day}");
                    }
                    else
                    {
                        Console.WriteLine($"order: {order.OrderId}, flightNumber: not scheduled.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }

        }
    }
}

