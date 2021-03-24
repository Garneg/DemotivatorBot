using System;
using System.Net;
using System.IO;
using System.Drawing;
using System.Text;
using Telegram.Bot;
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

            Bot.StartReceiving();

            BotMethods.AddToQueue();

            Console.WriteLine("Write a line to stop bot from receiving:"); //Не работает!!

            Console.ReadLine();

            Bot.StopReceiving();

            
            
        }
    }
}
