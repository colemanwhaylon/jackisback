using Akka.Actor;
using DustInTheWind.ConsoleTools;
using DustInTheWind.ConsoleTools.TabularData;
using JackIsBack.NetCoreLibrary.Actors;
using JackIsBack.NetCoreLibrary.Interfaces;
using JackIsBack.NetCoreLibrary.Messages;
using JackIsBack.NetCoreLibrary.Utility;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Akka.Remote;

namespace JackIsBack.Console
{
    /// <summary>
    /// This client application will call ITweetGenerator.Run()
    /// to start a Tweet Stream within JackIsBack.NetCoreLibrary to
    /// kickoff results streaming to the console.  The Akka.Net's config
    /// settings drive elastic scaling and processing of all Tweets.
    /// </summary>
    public class Program : ReceiveActor
    {
        private static ActorSystem ActorSystem;
        private static IActorRef _totalNumberOfTweetsActorRef;
        private static IActorRef _tweetGeneratorActorRef;

        public Program()
        {
            Receive<TweetGeneratorActorCommandResponse>(HandleTweetGeneratorActorCommandResponse);
        }

        private async void HandleTweetGeneratorActorCommandResponse(TweetGeneratorActorCommandResponse obj)
        {
            var getAllStatisticsMessage = new GetAllStatisticsMessage(-1);
            var statisticsMessage = ActorSystem
                .ActorSelection(SharedStrings.TotalNumberOfTweetsActorPath)
                .Ask<GetAllStatisticsMessage>(getAllStatisticsMessage).PipeTo(_tweetGeneratorActorRef);

            System.Console.WriteLine(
                $"Program.GetAllStatisticsMessage(): Yields = {statisticsMessage}");

            await ActorSystem.WhenTerminated.ConfigureAwait(false);

        }

        private void HandleGetAllStatisticsMessage(GetAllStatisticsMessage message)
        {
            System.Console.WriteLine($"Total Tweet Count = {message.TotalNumberOfTweets}");
        }

        //public static async Task Main(string[] args)
        //{
        //    var command = TweetGeneratorActorCommand.None;
        //    try
        //    {
        //        System.Console.Title = "Twitter Statistics App";
        //        System.Console.WriteLine("Started Main()!");

        //        ActorSystem = ActorSystem.Create("TwitterStatisticsActorSystem");

        //        command = TweetGeneratorActorCommand.StartUp;
        //        _tweetGeneratorActorRef = ActorSystem.ActorOf<TweetGeneratorActor>("TweetGeneratorActor");
        //        var result = await _tweetGeneratorActorRef.Ask<TweetGeneratorActorCommandResponse>(command);


        //        System.Console.ReadLine();
        //    }
        //    catch (Exception exception)
        //    {
        //        System.Console.WriteLine($"Message: {exception.Message}\n, StackTrace: {exception.StackTrace}\n, InnerException: {exception?.InnerException?.Message}");
        //    }
        //    finally
        //    {
        //        command = TweetGeneratorActorCommand.Shutdown;
        //        ActorSystem.ActorOf<TweetGeneratorActor>().Tell(command);

        //        await ActorSystem.Terminate();

        //        System.Console.WriteLine("Stopped ActorSystem!");
        //    }

        //    System.Console.WriteLine("Program Finished!");
        //    System.Console.ReadLine();
        //}

        private static async Task<GetAllStatisticsMessage> RefreshStatisticsAsync()
        {
            GetAllStatisticsMessage retVal = null;
            var message = new GetAllStatisticsMessage(0);
            var myStats = ActorSystem.ActorSelection(SharedStrings.TweetStatisticsActorPath)
                .Ask<GetAllStatisticsMessage>(message);
            Task.WaitAll(myStats);

            return myStats.Result;
        }

        /*
         *  private static void RefreshScreen()
        {
            CancellationTokenSource cts = new CancellationTokenSource();
            var token = cts.Token;

            Task t = Task.Factory.StartNew(
                () =>
                {
                    MainLoop(token);
                },
                token,
                TaskCreationOptions.LongRunning,
                TaskScheduler.Default
            );

            System.Console.ReadLine();
            cts.Cancel();

            try
            {
                t.Wait();
            }
            catch (AggregateException ae)
            {
                // catch inner exception 
            }
            catch (Exception crap)
            {
                // catch something else
            }
        }

        static async void MainLoop(CancellationToken token)
        {
            while (true)
            {
                // Poll on this property if you have to do 
                // other cleanup before throwing. 
                if (token.IsCancellationRequested)
                {
                    // Clean up here, then...
                    //  "cleanup".Dump();
                    token.ThrowIfCancellationRequested();
                }
                // do something here.
                try
                {
                    var statistics = await RefreshStatisticsAsync();
                    System.Console.Write(statistics);
                }
                catch { }

                Thread.Sleep(5000);
            }
        }

        private static async Task<GetAllStatisticsMessage> RefreshStatisticsAsync()
        {
            GetAllStatisticsMessage retVal = null;
            var message = new GetAllStatisticsMessage(0);
            var myStats = ActorSystem.ActorSelection(SharedStrings.TweetStatisticsActorPath)
                .Ask<GetAllStatisticsMessage>(message);
            Task.WaitAll(myStats);

            return myStats.Result;
        }
         *
         *
         *
         *
         *
         *
         *
         */
    }


    public class TwitterConsole
    {
        private static int _totalNumberOfTweets = 0;
        private static string _topEmojiUsed = ":)";
        private static double _percentOfTweetsWithEmojis = 0.0;
        private static double _percentOfTweetsContainingURL = 0.0;
        private static double _percentOfTweetsContainingPhotoURL = 0.0;

        public static void Main(string[] args)
        {
            System.Console.Title = "Twitter Statistics Console App";
            System.Console.Clear();
            System.Console.SetWindowPosition(0,0);
            System.Console.SetWindowSize(System.Console.LargestWindowWidth-15, System.Console.LargestWindowHeight-10);


            //TABLE #1
            DataGrid dataGrid = new DataGrid("Twitter Statistics");

            dataGrid.Columns.Add("Total Number Of Tweets");
            dataGrid.Columns.Add("Top Emoji Used");
            dataGrid.Columns.Add("% of Tweets With Emojis");
            dataGrid.Columns.Add("% of Tweets That Contain a URL");
            dataGrid.Columns.Add("% of Tweets That Contain a Photo URL");
            dataGrid.Rows.Add($"{_totalNumberOfTweets}",
                $"{_topEmojiUsed}",
                $"{_percentOfTweetsWithEmojis}",
                $"{_percentOfTweetsContainingURL}",
                $"{_percentOfTweetsContainingPhotoURL}");
            dataGrid.DisplayBorderBetweenRows = true;
            dataGrid.DisplayColumnHeaders = true;
            dataGrid.BorderTemplate = BorderTemplate.DoubleLineBorderTemplate;
            dataGrid.Display();
            System.Console.WriteLine(); System.Console.WriteLine();

            //TABLE #2 
            DataGrid domainDatagrid = new DataGrid("Top Domains of URLs in Tweets");
            domainDatagrid.Columns.Add("Top Domains");
            var domainRows = new List<string> { 
                "http://www.google.com",
                "http://www.google.com",
                "http://www.google.com",
                "http://www.google.com",
                "http://www.google.com" };
            foreach (var row in domainRows)
            {
                domainDatagrid.Rows.Add($"{row}");
            }
            domainDatagrid.DisplayBorderBetweenRows = true;
            domainDatagrid.DisplayColumnHeaders = true;
            domainDatagrid.BorderTemplate = BorderTemplate.DoubleLineBorderTemplate;
            domainDatagrid.Display();
            System.Console.WriteLine(); System.Console.WriteLine();

            //TABLE #2
            DataGrid hashTagDatagrid = new DataGrid("Top HashTag in Tweets");
            hashTagDatagrid.Columns.Add("Top HashTags");
            var hashTagRows = new List<string> {
                ":)",
                ":>",
                ":<",
                "(<>)",
                ")(" };
            foreach (var row in hashTagRows)
            {
                hashTagDatagrid.Rows.Add($"{row}");
            }
            hashTagDatagrid.DisplayBorderBetweenRows = true;
            hashTagDatagrid.DisplayColumnHeaders = true;
            hashTagDatagrid.BorderTemplate = BorderTemplate.DoubleLineBorderTemplate;
            hashTagDatagrid.Display();
            System.Console.WriteLine(); System.Console.WriteLine();

            System.Console.ReadLine();
        }

        public void RefreshScreen()
        {



        }
    }
}