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

        public static readonly string startText = "Приветствую👋! Как было описано выше, я бот для создания демотиваторов. Я все ещё нахожусь в стадии активной разработки и во мне возможно множество недоработок, но, не смотря на это, создатель уверен, что тебе будет за что меня полюбить❤️" + "\n" +
            "Для создания собсвенного демотиватора тебе необходимо лишь прислать мне картинку с подписью. Если хочешь, чтобы в демотиваторе была одна строка, пиши одну, если две, пиши две."+ "\n\n" +
            "1️⃣ Нажми первую кнопку, чтобы узнать мои преимущества \n2️⃣ Нажми вторую кнопку, чтобы получить помощь.";

        

        public static readonly string helpText = "Список всех комманд:" + "\n\n" +
            "/help - Список всех комманд" + "\n" +
            "/info - Информация" + "\n\n" +
            "Заметка: Ты можешь создать демотиватор, ответив на сообщение, в котором содержится картинка! Это может быть полезно, если ты хочешь сделать демотиватор внутри другого демотиватора."
            ;

        public static readonly string infoText = "Информация:" + "\n\n" +
            "Создатель - @senya_danfor" + "\n" +
            "Бот написан на языке C# с использованием:" + "\n" +
            "SixLabors.ImageSharp - Библиотека для работы с картинками" + "\n" +
            "Telegram Bot API - Официальная библиотека телеграм ботов" + "\n" +
            "\nВерсия 1.0.2"
            ;

        

        public static InlineKeyboardMarkup startInlineKeyboard = new InlineKeyboardMarkup(new []
        {

            InlineKeyboardButton.WithCallbackData("1️⃣", "advantages"),

            InlineKeyboardButton.WithCallbackData("2️⃣", "help")

        });

        public static readonly string defaultAnswer = "Отправь картинку с подписью, чтобы создать демотиватор! " + "\n" +
            "Нужна помощь? Нажми --> /help";

        public static readonly string advantagesText = "Мои преимущества:" + "\n" +
            "1. Отсутствие вотермарки. Создавай свои демотиваторы без отвлекающих внимание надписей!" + "\n" +
            "2. Простота и удобство использования." + "\n" +
            "3. Так как проект ещё зелёный, ты можешь внести свой вклад в проект, написав [разработчику](https://t.me/@senya_danfor) с проблемой или предложением, и, если того пожелаешь, будешь упомянут в /info. Вместе мы можем сделать больше!" + "\n\n" +
            "Итак-c... Начнём?";

    }
}
