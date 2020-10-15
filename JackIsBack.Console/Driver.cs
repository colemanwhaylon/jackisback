using System;
using System.IO;
using System.Text.Json;

namespace JackIsBack.Console
{
    public class Driver
    {
        static void Main(string[] args)
        {
            var driver = new Driver();
            var jsonFilePath = @"C:\Users\Owner\source\repos\colemanwhaylon\jackisback\JackIsBack.Console\bin\Debug\netcoreapp3.1\tweetsample.json";
            
            var jsonFile = driver.ReadJsonFile(jsonFilePath);
            driver.WriteOutputToScreen(jsonFile);

            System.Console.WriteLine("FINISHED!");
            System.Console.ReadLine();
        }

        private string ReadJsonFile(string jsonFilePath)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };

            return File.ReadAllText(jsonFilePath);
            
            

        }

        private void WriteOutputToScreen(string jsonString)
        {
            try
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    WriteIndented = true
                };

                var jsonModel = JsonSerializer.Deserialize<Root>(jsonString, options);
                var modelJson = JsonSerializer.Serialize(jsonModel, options);

                System.Console.WriteLine(modelJson);
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
            }
        }

    }
}
