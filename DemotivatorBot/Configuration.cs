using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot.Args;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.Passport;

namespace DemotivatorBot
{
    class Configuration
    {
        private static string Token = "1612272941:AAE3rmxz9BVRSnU0K0UGBlfAiKkxN3s1WrI";

        public static string BotToken
        {
            get => Token;
        }

        public static int ResizerValue = 2;

        private static string help = "Список всех комманд:" + "\n\n" +
            "/help - Список всех комманд" + "\n" +
            "/info - Информация" + "\n\n" +
            "Совет: Для того, чтобы картинка не сжималась, отправьте её файлом!"
            ;

        public static string helpText
        {
            get => help;
        }

        private static string info = "Информация:" + "\n\n" +
            "Создатель - @senya_danfor" + "\n" +
            "Бот написан на языке C# с использованием следующих библиотек:" + "\n" +
            "SixLabors.ImageSharp - Библиотека для работы с картинками" + "\n" +
            "Telegram - Официальная библиотека для ботов"
            ;

        public static string infoText
        {
            get => info;
        }


    }
}
