using Akka.Actor;
using DustInTheWind.ConsoleTools;
using DustInTheWind.ConsoleTools.InputControls;
using DustInTheWind.ConsoleTools.Menues;
using DustInTheWind.ConsoleTools.Spinners;
using DustInTheWind.ConsoleTools.TabularData;
using JackIsBack.NetCoreLibrary.Interfaces;
using JackIsBack.NetCoreLibrary.Messages;
using JackIsBack.NetCoreLibrary.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Akka.Routing;
using JackIsBack.NetCoreLibrary;
using JackIsBack.NetCoreLibrary.Actors;
using Serilog.Debugging;
using ICommand = DustInTheWind.ConsoleTools.Menues.ICommand;

namespace JackIsBack.Console
{
    public class TwitterConsole : ReceiveActor
    {
        private static ActorSystem ActorSystem;
        private static YesNoAnswer _answer = YesNoAnswer.Yes;
        private static IActorRef _twitterConsoleRef;
        private static IActorRef _twitterEngineActorRef;
        
        private static int? _totalNumberOfTweets = 0;
        private static double? _percentOfTweetsWithEmojis = 0.0;
        private static double? _percentOfTweetsContainingURL = 0.0;
        private static double? _percentOfTweetsContainingPhotoURL = 0.0;
        private static double? _averageTweetsPerHour = 0.0;
        private static double? _averageTweetsPerMinute = 0.0;
        private static double? _averageTweetsPerSecond = 0.0;
        private static string _topEmojiUsed = string.Empty;
        private static List<string>? _topEmojisUsed = new List<string>();
        private static List<string>? _topDomainsUsed = new List<string>();
        private static List<string>? _topHashTagsUsed = new List<string>();

        private static int Count = 0;

        public TwitterConsole()
        {
            Receive<GetAllStatisticsMessageResponse>(HandleGetAllStatisticsMessageResponse);
        }

        private static void HandleGetAllStatisticsMessageResponse(GetAllStatisticsMessageResponse message)
        {
            try
            {
                if (message != null)
                {
                    _totalNumberOfTweets = message.TotalNumberOfTweets;
                    _percentOfTweetsContainingPhotoURL = message.PercentOfTweetsWithPhotoUrl;
                    _percentOfTweetsContainingURL = message.PercentOfTweetsWithUrl;
                    _percentOfTweetsWithEmojis = message.PercentOfTweetsContainingEmojis;
                    _averageTweetsPerHour = message.AverageTweetsPerHour;
                    _averageTweetsPerMinute = message.AverageTweetsPerMinute;
                    _averageTweetsPerSecond = message.AverageTweetsPerSecond;
                    _topDomainsUsed = message.TopDomains;
                    _topEmojisUsed = message.TopEmojis;
                    _topHashTagsUsed = message.TopHashTags;
                }
            }
            catch (Exception exception)
            {
                System.Console.WriteLine($"Exception message: {exception.Message}, StackTrace: {exception.StackTrace}");
            }
        }

        public static void Main(string[] args)
        {
            try
            {
                SetupConsoleUI();

                var result = SetupActorSystem();
                if (result == InitToggleCommandResponse.StartedUp)
                {
                   // ActorSystem.EventStream.Subscribe(_twitterConsoleRef, typeof(GetAllStatisticsMessageResponse));

                    DisplayMenu();
                }
                else
                {
                    System.Console.WriteLine($"Something else came back other than InitToggleCommandResponse.StartedUp");
                }
            }
            catch (Exception exception)
            {
                System.Console.WriteLine(exception);
            }

            System.Console.WriteLine("PROGRAM DONE!");
            System.Console.ReadLine();
        }

        private static InitToggleCommandResponse SetupActorSystem()
        {
            //todo:Move back ActorSystem from constructor init
            ActorSystem = ActorSystem.Create("TwitterStatisticsActorSystem");


            _twitterEngineActorRef = ActorSystem.ActorOf<TwitterEngine>("TwitterEngine");
            _twitterConsoleRef = ActorSystem.ActorOf<TwitterConsole>("TwitterConsole");
            //TweetStatisticsActor.IActorRefs.Add("TwitterEngine", _twitterEngineActorRef);
            //TweetStatisticsActor.IActorRefs.Add("TwitterConsole", _twitterConsoleRef);

            var result = _twitterEngineActorRef.Ask<InitToggleCommandResponse>(InitToggleCommandRequest.StartUp).Result;

            return result;
        }

        private static void DisplayMenu()
        {
            System.Console.Clear();

            TextMenu textMenu = new TextMenu
            {

                TitleText = "Twitter Statistics Application",
                ForegroundColor = ConsoleColor.Cyan,
                Margin = new Thickness { Bottom = 1, Left = 1, Right = 1, Top = 1 },
                EraseAfterClose = true,
            };

            textMenu.AddItems(new List<TextMenuItem>
                {
                    new TextMenuItem
                    {
                        Id = "1",
                        Text = "Open Twitter Statistics",
                        Command = new OpenTwitterStatisticsCommand(),
                        IsVisible = true
                    },
                    new TextMenuItem
                    {
                        Id = "2",
                        Text = "Exit",
                        Command = new ExitCommand(),
                        IsVisible = true
                    }
                }
            );

            do
            {
                textMenu.Display();
                //System.Console.Clear();
            } while (textMenu.SelectedItem.Id != "2");

        }

        private class OpenTwitterStatisticsCommand : ICommand
        {
            public void Execute()
            {
                try
                {
                    do
                    {
                        for (var i = 0; i < 6; i++)
                        {
                            DisplayEntireUI();
                            Task.Delay(500);
                        }
                        AskToContinueAndClearScreen();
                        if (_answer == YesNoAnswer.Yes) System.Console.Clear();
                    } while (_answer == YesNoAnswer.Yes);
                }
                catch (Exception exception)
                {
                    System.Console.WriteLine(exception.Message);
                }
            }

            public bool IsActive { get; } = true;
        }


        private class ExitCommand : ICommand
        {
            public void Execute()
            {
                Environment.Exit(0);
            }

            public bool IsActive { get; } = true;
        }

        private static void DisplayEntireUI()
        {
            //TABLE #1
            DataGrid dataGrid = new DataGrid("Twitter Statistics");

            dataGrid.Columns.Add("Total Number Of Tweets");
            dataGrid.Columns.Add("Top Emoji Used");
            dataGrid.Columns.Add("% of Tweets With Emojis");
            dataGrid.Columns.Add("% of Tweets That Contain a URL");
            dataGrid.Columns.Add("% of Tweets That Contain a Photo URL");

            _topEmojiUsed = ((_topEmojisUsed != null && _topEmojisUsed.Any()) ? _topEmojisUsed?.First() : ":)") ?? string.Empty;
            dataGrid.Rows.Add($"{_totalNumberOfTweets}",
                $"{_topEmojiUsed}",
                $"{_percentOfTweetsWithEmojis}",
                $"{_percentOfTweetsContainingURL}",
                $"{_percentOfTweetsContainingPhotoURL}");
            dataGrid.DisplayBorderBetweenRows = true;
            dataGrid.DisplayColumnHeaders = true;
            dataGrid.BorderTemplate = BorderTemplate.DoubleLineBorderTemplate;
            dataGrid.Display();
            System.Console.WriteLine();
            System.Console.WriteLine();

            //TABLE #2 
            //DataGrid domainDatagrid = new DataGrid("Top TopDomains of URLs in Tweets");
            //domainDatagrid.Columns.Add("Top TopDomains");
            //var domainRows = _topDomainsUsed;
            //foreach (var row in domainRows.Take(5))
            //{
            //    domainDatagrid.Rows.Add($"{row.Key}");
            //}

            //domainDatagrid.DisplayBorderBetweenRows = true;
            //domainDatagrid.DisplayColumnHeaders = true;
            //domainDatagrid.BorderTemplate = BorderTemplate.DoubleLineBorderTemplate;
            //domainDatagrid.Display();
            //System.Console.WriteLine();
            //System.Console.WriteLine();

            ////TABLE #3
            //DataGrid hashTagDatagrid = new DataGrid("Top HashTag in Tweets");
            //hashTagDatagrid.Columns.Add("Top HashTags");
            //var hashTagRows = _topHashTagsUsed;
            //foreach (var row in hashTagRows.Take(5))
            //{
            //    hashTagDatagrid.Rows.Add($"{row.Key}");
            //}

            //hashTagDatagrid.DisplayBorderBetweenRows = true;
            //hashTagDatagrid.DisplayColumnHeaders = true;
            //hashTagDatagrid.BorderTemplate = BorderTemplate.DoubleLineBorderTemplate;
            //hashTagDatagrid.Display();
            System.Console.WriteLine();
            System.Console.WriteLine();

            //RefreshProgressBar();
            SendSignalToRefreshAllDataFields();

            //System.Console.Clear();
        }

        private static void AskToContinueAndClearScreen()
        {
            YesNoQuestion yesNoQuestion = new YesNoQuestion("Do you want to continue?")
            {
                AcceptCancel = true,
                DefaultAnswer = YesNoAnswer.Yes,
            };

            _answer = yesNoQuestion.ReadAnswer();
        }

        private static void SetupConsoleUI()
        {
            System.Console.Title = "Twitter Statistics Console App";
            System.Console.Clear();
            System.Console.SetWindowPosition(0, 0);
            System.Console.SetWindowSize(System.Console.LargestWindowWidth - 15, System.Console.LargestWindowHeight - 5);
            var consoleWnd = System.Diagnostics.Process.GetCurrentProcess().MainWindowHandle;
            Imports.SetWindowPos(consoleWnd, 0, 0, 0, 0, 0, Imports.SWP_NOSIZE | Imports.SWP_NOZORDER);
        }

        private static void RefreshProgressBar()
        {
            ManualResetEventSlim finishEvent = new ManualResetEventSlim();
            finishEvent.Reset();

            ProgressBar progressBar = new ProgressBar();
            progressBar.LabelText = "Updating";

            Task.Run<Task>(async () =>
            {
                progressBar.Display();

                for (int i = 0; i < 100; i++)
                {
                    await Task.Delay(50);
                    progressBar.Value++;
                }

                finishEvent.Set();
            });

            finishEvent.Wait();
            progressBar.Display();

        }

        public static void SendSignalToRefreshAllDataFields()
        {
            var message = RefreshStatisticsRequest.Update;
            _twitterEngineActorRef.Tell(message);
        }
    }
}