using System;
using System.Net;
using System.IO;
using System.Drawing;
using System.Text;
using Telegram.Bot;
using System.Threading;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Requests;
using Telegram.Bot.Types.InputFiles;
using Telegram.Bot.Types.InlineQueryResults;
using Telegram.Bot.Types.ReplyMarkups;


namespace DemotivatorBot
{
    class Program
    {
        private static TelegramBotClient Bot;
        static void Main(string[] args)
        {

            Bot = new TelegramBotClient(Configuration.BotToken);

            BotMethods.botClient = Bot;

            Bot.OnMessage += BotMethods.MessageReceived;
            Bot.OnCallbackQuery += BotMethods.CallbackQueryReceived;

            Console.WriteLine("//Message handlers setup done");

            Bot.StartReceiving();

            Console.WriteLine("//Bot started");

            Thread M = new Thread(new ThreadStart(() => { BotMethods.AddToQueue(); }));

            M.Start();


            for (; ; )
            {
                Console.WriteLine("Write \"/stop\" to stop bot:"); //Не работает!!


                Stop:
                string answer = Console.ReadLine();

                if (answer.ToLower() == "/stop")
                {
                    BotMethods.State = 1;
                    Console.WriteLine("Bot has stopped!");
                }
                else
                {
                    goto Stop;
                }

                Console.WriteLine("To run a bot write \"/run\":");

                Run:
                answer = Console.ReadLine();

                if (answer.ToLower() == "/run")
                {
                    BotMethods.State = 0 ;
                }
                else
                {
                    goto Run;
                }

            }

            Bot.StopReceiving();

            
            
        }
    }
}
