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
    public class TwitterConsole
    {
        private static ActorSystem ActorSystem;
        private static YesNoAnswer _answer = YesNoAnswer.Yes;
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

        public static void Main(string[] args)
        {
            try
            {
                SetupConsoleUI();

                var result = SetupActorSystem();
                if (result == InitToggleCommandResponse.StartedUp)
                {
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
            ActorSystem = ActorSystem.Create("TwitterStatisticsActorSystem");

            _twitterEngineActorRef = ActorSystem.ActorOf<TwitterEngine>("TwitterEngine");
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
            RefreshAllDataFields();
            UpdateStatisticsAndRebind();

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

        public static void RefreshAllDataFields()
        {
            var message = RefreshStatisticsRequest.Update;
            _twitterEngineActorRef.Tell(message);
        }

        public static void UpdateStatisticsAndRebind()
        {
            var message = new GetAllStatisticsMessageResponse();
            var response = _twitterEngineActorRef.Ask<GetAllStatisticsMessageResponse>(message).Result;
            if (response != null)
            {
                _totalNumberOfTweets = response.TotalNumberOfTweets;
                _percentOfTweetsContainingPhotoURL = response.PercentOfTweetsWithPhotoUrl;
                _percentOfTweetsContainingURL = response.PercentOfTweetsWithUrl;
                _percentOfTweetsWithEmojis = response.PercentOfTweetsContainingEmojis;
                _averageTweetsPerHour = response.AverageTweetsPerHour;
                _averageTweetsPerMinute = response.AverageTweetsPerMinute;
                _averageTweetsPerSecond = response.AverageTweetsPerSecond;
                _topDomainsUsed = response.TopDomains;
                _topEmojisUsed = response.TopEmojis;
                _topHashTagsUsed = response.TopHashTags;
            }

        }
    }
}