using System;
using System.IO;
using System.Text.Json;
using Akka.Actor;
using Akka.Actor.Internal;
using JackIsBack.Console.Messages;

namespace JackIsBack.Console
{
    public class Driver
    {
        private static ActorSystem TwitterStreamingActorSystem;

        static void Main(string[] args)
        {
            var driver = new Driver();
            var jsonFilePath = @"C:\Users\Owner\source\repos\colemanwhaylon\jackisback\JackIsBack.Console\bin\Debug\netcoreapp3.1\tweetsample.json";

            TwitterStreamingActorSystem = ActorSystemImpl.Create("TwitterStreamingActorSystem");
            System.Console.WriteLine("Actor system created");

            Props streamRetrieverActorProps = Props.Create<StreamRetrieverActor>();
            IActorRef streamingActorRef = TwitterStreamingActorSystem.ActorOf(streamRetrieverActorProps, "StreamRetrieverActor");

            var jsonFile = driver.ReadJsonFile(jsonFilePath);
            var tweet = driver.DeserializeTweet(jsonFile);

            streamingActorRef.Tell(new TwitterMessage(tweet));
            

            //var jsonFile = driver.ReadJsonFile(jsonFilePath);
            //driver.WriteOutputToScreen(jsonFile);
            TwitterStreamingActorSystem.Terminate();


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

            var file = File.ReadAllText(jsonFilePath);
            return file;
        }

        private void WriteOutputToScreen(string jsonString)
        {
            try
            {
                var modelJson = DeserializeTweet(jsonString);

                System.Console.WriteLine(modelJson);
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
            }
        }

        private  Root DeserializeTweet(string jsonString)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };

            var jsonModel = JsonSerializer.Deserialize<Root>(jsonString, options);

            return jsonModel;
        }
    }
}
