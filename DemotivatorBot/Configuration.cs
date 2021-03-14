using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot.Args;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.Passport;
using Telegram.Bot.Types.ReplyMarkups;

namespace DemotivatorBot
{
    class Configuration
    {
        private static string Token = "1612272941:AAE3rmxz9BVRSnU0K0UGBlfAiKkxN3s1WrI";

        public static string BotToken
        {
            get => Token;
        }

        private static string StartText = "Приветствую👋! Как было описано выше, я являюсь ботом для создания демотиваторов. Я все ещё нахожусь в стадии активной разработки и во мне возможно множество недоработок, но, не смотря на это, создатель уверен, что тебе будет за что меня полюбить❤️" + "\n\n" +
            "Нажми первую кнопку, чтобы узнать мои преимущества 1️⃣ \nНажми вторую кнопку, чтобы получить помощь 2️⃣.";

        public static string startText
        {
            get => StartText;
        }

        private static string help = "Список всех комманд:" + "\n\n" +
            "/help - Список всех комманд" + "\n" +
            "/info - Информация" + "\n\n" +
            "Заметка: Вы можете создать демотиватор, ответив на сообщение, в котором содержится картинка! Это может быть полезно когда вы хотите сделать демотиватор внутри другого демотиватора."
            ;

        public static string helpText
        {
            get => help;
        }

        private static string info = "Информация:" + "\n\n" +
            "Создатель - @senya_danfor" + "\n" +
            "Бот написан на языке C# с использованием следующих библиотек:" + "\n" +
            "SixLabors.ImageSharp - Библиотека для работы с картинками" + "\n" +
            "Telegram - Официальная библиотека для создания ботов в телеграмме"
            ;

        public static string infoText
        {
            get => info;
        }

        public static InlineKeyboardMarkup startInlineKeyboard = new InlineKeyboardMarkup(new []
        {

            InlineKeyboardButton.WithCallbackData("1️⃣", "advantages"),

            InlineKeyboardButton.WithCallbackData("2️⃣", "help")

        });

        public static readonly string advantagesText = "Мои преимущества:";

    }
}
