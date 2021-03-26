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

        private static string StartText = "Приветствую👋! Как было описано выше, я бот для создания демотиваторов. Я все ещё нахожусь в стадии активной разработки и во мне возможно множество недоработок, но, не смотря на это, создатель уверен, что тебе будет за что меня полюбить❤️" + "\n" +
            "Для создания собсвенного демотиватора тебе необходимо лишь прислать мне картинку с подписью. Если хочешь, чтобы в демотиваторе была одна строка, пиши одну, если две, пиши две."+ "\n\n" +
            "1️⃣ Нажми первую кнопку, чтобы узнать мои преимущества \n2️⃣ Нажми вторую кнопку, чтобы получить помощь.";

        public static string startText
        {
            get => StartText;
        }

        private static string help = "Список всех комманд:" + "\n\n" +
            "/help - Список всех комманд" + "\n" +
            "/info - Информация" + "\n\n" +
            "Заметка: Ты можешь создать демотиватор, ответив на сообщение, в котором содержится картинка! Это может быть полезно, если ты хочешь сделать демотиватор внутри другого демотиватора."
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

        private static string DefaultAnswer = "Отправь картинку с подписью, чтобы создать демотиватор! " + "\n" +
            "Нужна помощь? Нажми --> /help";

        public static string defaultAnswer
        {
            get
            {
                return DefaultAnswer;
            }
        }

        public static readonly string advantagesText = "Мои преимущества:" + "\n" +
            "1. Отсутствие вотермарки. Создавай свои демотиваторы без отвлекающих внимание надписей!" + "\n" +
            "2. Простота и удобство использования." + "\n" +
            "3. Так как проект ещё зелёный, ты можешь внести вклад в проект, написав [разработчику](https://t.me/@senya_danfor) с проблемой или предложением, и, если того пожелаешь, будешь упомянут в /info. Вместе мы можем сделать больше!" + "\n\n" +
            "Надеюсь этого хватит, чтобы удержать тебя❤️.";

    }
}
