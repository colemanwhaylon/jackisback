using System;
using System.Collections.Generic;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Akka.Actor;
using Akka.Actor.Internal;
using JackIsBack;
using JackIsBack.Console.Messages;
using Tweetinvi;
using Tweetinvi.Auth;
using Tweetinvi.Events;
using Tweetinvi.Streaming;

namespace JackIsBack.Console
{
    public class Driver : IDisposable
    {
        //private static ActorSystem TwitterStreamingActorSystem;
        private readonly HttpClient _client = new HttpClient();
        private TwitterInfo _twitterInfo = new TwitterInfo();
        private ISampleStream _sampleStream;
        public static async Task Main(string[] args)
        {
            //var jsonFilePath = @"C:\Users\Owner\source\repos\colemanwhaylon\jackisback\JackIsBack.Console\bin\Debug\netcoreapp3.1\tweetsample.json";
            var driver = new Driver();
            try
            {
                var twitterInfo = new TwitterInfo();
                var userClient = new TwitterClient(twitterInfo.Secrets.Key, twitterInfo.Secrets.SecretKey,
                    twitterInfo.Secrets.AccessToken, twitterInfo.Secrets.AccessTokenSecret);

                var user = await userClient.Users.GetAuthenticatedUserAsync();
                driver._sampleStream = userClient.Streams.CreateSampleStream();
                driver._sampleStream.TweetReceived += sampleStreamOnTweetReceived();

                await driver._sampleStream.StartAsync();
                System.Console.WriteLine(user);
            }
            catch (Exception exception)
            {
                System.Console.WriteLine(exception);
            }
            finally
            {
                driver._sampleStream.TweetReceived -= sampleStreamOnTweetReceived();
                driver._sampleStream.TweetReceived -= null;
            }

            //TwitterStreamingActorSystem = ActorSystemImpl.Create("TwitterStreamingActorSystem");
            //System.Console.WriteLine("Actor system created");

            //Props streamRetrieverActorProps = Props.Create<StreamRetrieverActor>();
            //IActorRef streamingActorRef = TwitterStreamingActorSystem.ActorOf(streamRetrieverActorProps, "StreamRetrieverActor");

            //ITweetRetriever tweetReciever = new FileTweetRetriever();
            //var tweets = await tweetReciever.GetFileAsync(jsonFilePath);

            //foreach (var root in tweets)
            //{
            //    streamingActorRef.Tell(new TwitterMessage(root));
            //}

            //var jsonFile = driver.ReadJsonFile(jsonFilePath);
            //driver.WriteOutputToScreen(jsonFile);
            //TwitterStreamingActorSystem.Terminate();


            System.Console.WriteLine("FINISHED!");
            System.Console.ReadLine();
        }

        private static EventHandler<TweetReceivedEventArgs> sampleStreamOnTweetReceived()
        {
            long count = 0;
            return (sender, eventArgs) =>
            {
                System.Console.WriteLine($"count: {count++}\t" +  eventArgs.Tweet);
            };
        }

        private async Task ProcessRepositories()
        {
            //_client.BaseAddress = new Uri(_twitterInfo.ApiLinks.Stream);
            //_client.DefaultRequestHeaders.Accept.Clear();

            //_client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",  _twitterInfo.Secrets.BearerToken);

            //var stringTask = await _client.GetStringAsync(_twitterInfo.ApiLinks.Stream);
            //var streamTask = _client.GetStreamAsync(_twitterInfo.ApiLinks.Stream);

            //await foreach (var x in stringTask)
            //{
            //}

            //var msg = GetStreamData();

            //await foreach (var s in msg)
            //{
            //    System.Console.WriteLine(s);
            //}

            //System.Console.WriteLine(msg);




            System.Console.ReadLine();
        }


        //public void WriteOutputToScreen(string jsonString)
        //{
        //    try
        //    {
        //        var modelJson = DeserializeTweet(jsonString);

        //        System.Console.WriteLine(modelJson);
        //    }
        //    catch (Exception ex)
        //    {
        //        System.Console.WriteLine(ex.Message);
        //    }
        //}
        public async IAsyncEnumerable<string> GetStreamData()
        {
            _client.BaseAddress = new Uri(_twitterInfo.ApiLinks.Stream);
            _client.DefaultRequestHeaders.Accept.Clear();

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _twitterInfo.Secrets.BearerToken);

            var count = 0;
            var r = new Random();
            while (true)
            {
                System.Console.WriteLine(count++);
                var streamString = await _client.GetStringAsync(_twitterInfo.ApiLinks.Stream).ConfigureAwait(false); 
                System.Console.WriteLine(streamString);
                yield break;
            }
        }


        public void Dispose()
        {
            
        }
    }


    public interface ITweetRetriever
    {
        Task<List<Root>> GetTweetsAsync(string resource);
        Task<string[]> GetFileAsync(string resource);
    }

    public class FileTweetRetriever : ITweetRetriever
    {
        public async Task<List<Root>> GetTweetsAsync(string resource)
        {
            var result = await Task.Factory.StartNew(() =>
                                    {
                                        var jsonFile = ReadJsonFile(resource);
                                        var tweet = DeserializeTweet(jsonFile);
                                        return tweet;
                                    });
            return result;
        }

        public async Task<string[]> GetFileAsync(string resource)
        {
            var result = await Task.Factory.StartNew(() =>
            {
                var jsonFile = ReadJsonFile(resource);
                var file = DeserializeFile(jsonFile);
                return file;
            });
            return result;
        }

        private static string ReadJsonFile(string jsonFilePath)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };

            var file = File.ReadAllText(jsonFilePath, Encoding.Unicode);
            var json = file.Split("}\r\n}{", StringSplitOptions.None);


            //MemoryMappedFile mmf = MemoryMappedFile.CreateFromFile(file);
            //MemoryMappedViewStream mms = mmf.CreateViewStream();
            //using (BinaryReader b = new BinaryReader(mms))
            //{
            //}

            //
            return file;
        }

        private static List<Root> DeserializeTweet(string jsonString)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };

            var jsonModel = JsonSerializer.Deserialize<List<Root>>(jsonString, options);

            return jsonModel;
        }

        private static string[] DeserializeFile(string jsonString)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };

            var jsonModel = JsonSerializer.Deserialize<string[]>(jsonString, options);

            return jsonModel;
        }
    }
}
