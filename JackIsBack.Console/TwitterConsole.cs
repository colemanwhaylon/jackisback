using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Akka.Actor;
using DustInTheWind.ConsoleTools;
using DustInTheWind.ConsoleTools.Menues;
using DustInTheWind.ConsoleTools.Spinners;
using DustInTheWind.ConsoleTools.TabularData;
using JackIsBack.NetCoreLibrary.Interfaces;
using ICommand = DustInTheWind.ConsoleTools.Menues.ICommand;

namespace JackIsBack.Console
{
    public class TwitterConsole
    {
        private static ActorSystem ActorSystem;

        private static int _totalNumberOfTweets = 0;
        private static string _topEmojiUsed = ":)";
        private static double _percentOfTweetsWithEmojis = 0.0;
        private static double _percentOfTweetsContainingURL = 0.0;
        private static double _percentOfTweetsContainingPhotoURL = 0.0;

        public static async Task Main(string[] args)
        {
            try
            {
                SetupConsoleUI();

                var result = await SetupActorSystem();
                if (result == TweetGeneratorActorCommandResponse.StartedUp)
                {
                    DisplayMenu();
                }
                else
                {
                    System.Console.WriteLine($"Something else came back other than TweetGeneratorActorCommandResponse.StartedUp");
                }
            }
            catch (Exception exception)
            {
                System.Console.WriteLine(exception);
            }

            System.Console.WriteLine("PROGRAM DONE!");
            System.Console.ReadLine();
        }

        private static async Task<TweetGeneratorActorCommandResponse> SetupActorSystem()
        {
            ActorSystem = ActorSystem.Create("TwitterStatisticsActorSystem");

            var twitterEngineActorRef = ActorSystem.ActorOf<TwitterEngine>("TwitterEngine");
            twitterEngineActorRef.Ask<>



            return TweetGeneratorActorCommandResponse.StartedUp;
        }

        private static void DisplayMenu()
        {
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
                System.Console.Clear();
            } while (textMenu.SelectedItem.Id != "2");

        }

        private class OpenTwitterStatisticsCommand : ICommand
        {
            public void Execute()
            {
                DisplayEntireUI();
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
            DataGrid domainDatagrid = new DataGrid("Top Domains of URLs in Tweets");
            domainDatagrid.Columns.Add("Top Domains");
            var domainRows = new List<string>
            {
                "http://www.google.com",
                "http://www.google.com",
                "http://www.google.com",
                "http://www.google.com",
                "http://www.google.com"
            };
            foreach (var row in domainRows)
            {
                domainDatagrid.Rows.Add($"{row}");
            }

            domainDatagrid.DisplayBorderBetweenRows = true;
            domainDatagrid.DisplayColumnHeaders = true;
            domainDatagrid.BorderTemplate = BorderTemplate.DoubleLineBorderTemplate;
            domainDatagrid.Display();
            System.Console.WriteLine();
            System.Console.WriteLine();

            //TABLE #2
            DataGrid hashTagDatagrid = new DataGrid("Top HashTag in Tweets");
            hashTagDatagrid.Columns.Add("Top HashTags");
            var hashTagRows = new List<string>
            {
                ":)",
                ":>",
                ":<",
                "(<>)",
                ")("
            };
            foreach (var row in hashTagRows)
            {
                hashTagDatagrid.Rows.Add($"{row}");
            }

            hashTagDatagrid.DisplayBorderBetweenRows = true;
            hashTagDatagrid.DisplayColumnHeaders = true;
            hashTagDatagrid.BorderTemplate = BorderTemplate.DoubleLineBorderTemplate;
            hashTagDatagrid.Display();
            System.Console.WriteLine();
            System.Console.WriteLine();

            RefreshProgressBar();
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

        public void RefreshScreen()
        {



        }
    }
}