﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Linq;
using Telegram.Bot.Types;
using Telegram.Bot.Args;
using Telegram.Bot;
using Telegram.Bot.Types.InputFiles;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Requests;
using Telegram.Bot.Types.InlineQueryResults;
using Telegram.Bot.Types.Enums;
using System.Globalization;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.Fonts;
using SixLabors.ImageSharp.Drawing;
using SixLabors.ImageSharp.Drawing.Processing;


namespace DemotivatorBot
{
    class BotMethods
    {
        public static double renderTime;
        public static string[] captions;
        public static bool isReply;
        public static int State = 0;
        public static Queue<Message> messagesQueue = new Queue<Message>();
        public static int numOfCaptions = 2;
        public static FontCollection fontCollection = new FontCollection();
        public static FontFamily family = fontCollection.Install("C:/SavedPictures/Times.ttf");
        public static TelegramBotClient botClient;
        public static void MessageReceived(object sender, MessageEventArgs e)
        {

            Message message = e.Message;

            messagesQueue.Enqueue(message);

            renderTime = Convert.ToDouble((DateTime.Now.Second + "," + DateTime.Now.Millisecond).ToString());

            Console.WriteLine(renderTime);

        }
        public static async void MessageManipulations(Message message)
        {

            if (message.ReplyToMessage != null)
            {

                if (message.ReplyToMessage.Photo != null)
                {
                    isReply = true;
                }
                else
                {
                    isReply = false;
                }
            }
            

            if (message.Type == MessageType.Text && message.Text != null)
            {
                switch (message.Text.Split(' ').First())
                {
                    case "/start":
                        await botClient.SendTextMessageAsync(
                            chatId: message.Chat.Id,
                            text: Configuration.startText,
                            replyMarkup: Configuration.startInlineKeyboard
                            );
                        
                        break;
                    case "/help":
                        await botClient.SendTextMessageAsync
                            (
                            chatId: message.Chat.Id,
                            text: Configuration.helpText
                            );
                        break;

                    case "/info":
                        await botClient.SendTextMessageAsync
                            (
                            chatId: message.Chat.Id,
                            text: Configuration.infoText
                            );
                        break;
                    default:
                        if (message.ReplyToMessage == null)
                        {
                            await botClient.SendTextMessageAsync(
                                chatId: message.Chat.Id,
                                text: "Пришлите картинку с подписью, чтобы создать демотиватор!"
                                );
                        }
                        break;
                    
                }
                
                if (isReply == true)
                {
                    await botClient.SendChatActionAsync(
                    chatId: message.Chat.Id,
                    chatAction: ChatAction.UploadPhoto
                    );
                    PrepearePicture(message.ReplyToMessage.Photo, message.Text, botClient, message.Chat.Id);
                    Console.WriteLine(message.Text);
                }

            }

            if (message.Type == MessageType.Photo)
            {
                Console.WriteLine("\nPICTURE RECEIVED!!");
                await botClient.SendChatActionAsync(
                    chatId: message.Chat.Id,
                    chatAction: ChatAction.UploadPhoto
                    );
                PrepearePicture(message.Photo, message.Caption, botClient, message.Chat.Id);
            }
        }

        public static async void PrepearePicture(PhotoSize[] photoSize, string messageCaption, TelegramBotClient botClient, ChatId messageChatId)
        {

            new Thread(new ThreadStart(async() =>
            {
                if (messageCaption != null)
                {
                    captions = await GetCaptions(messageCaption);
                }
                else
                {
                    captions = await GetCaptions("Не пишите длинный текст\nОн не вмещается");

                }
            })).Start();

            FileStream photoStream = new FileStream("newphotos.png", FileMode.Create);

            Telegram.Bot.Types.File fl = await botClient.GetFileAsync(photoSize[photoSize.Length - 1].FileId);

            await botClient.DownloadFileAsync(fl.FilePath, photoStream);

            Thread.Sleep(75);

            Console.WriteLine("\nPICTURE DOWNLOADED!!\n");

            photoStream.Close();

            Image<Rgba32> img = Image.Load<Rgba32>("newphotos.png");

            Console.WriteLine("Size before Mutate: Width - {0}, Height - {1}", img.Width, img.Height);

            if (img.Width < 500 || img.Height < 500)
            {
                int w = img.Width;
                int h = img.Height;
                int relationSize = w / h;
                if (w < h)
                {
                    img.Mutate(z => z.Resize(500, 500 * relationSize));
                }
                else
                {
                    img.Mutate(j => j.Resize(500 * relationSize, 500));
                }
               
            }
            Console.WriteLine("Size after Mutate: Width - {0}, Height - {1}", img.Width, img.Height);

            img.SaveAsPng("newphotos.png");

            CreateDemotivator(captions[0], captions[1], numOfCaptions);

            Console.WriteLine("\nDemotivator created!\n");

            FileStream fileStream = new FileStream("DemotivatorBotResult.png", FileMode.Open);

            InputOnlineFile ResultFile = new InputOnlineFile(fileStream, "result.png");


            Console.WriteLine("File is ready for uploading");

            await botClient.SendPhotoAsync(
                chatId: messageChatId,
                ResultFile
                );
            renderTime = Convert.ToDouble((DateTime.Now.Second + "," + DateTime.Now.Millisecond).ToString()) - renderTime;
            Console.WriteLine("Message sended(async) Time taken: {0:F}", renderTime);
            fileStream.Close();

            numOfCaptions = 2;

            State = 0;
            
        }

    public static async void CreateDemotivator(string topCaption, string bottomCaption = null, int captions = 2)
        {

            Image<Rgba32> originalImage = Image.Load<Rgba32>("newphotos.png");

            int averagePoint = (int)(originalImage.Width / 8.8);
            int minimalPoint = (int)(averagePoint / 1.4);

            int Width;
            int Height;
            int Thickness = averagePoint / 30;

            if (captions == 2)
            {
                Width = originalImage.Width + (averagePoint * 2);
                Height = originalImage.Height + minimalPoint + (averagePoint * 3);

                

                Image<Rgba32> resultImage = new Image<Rgba32>(Width, Height);

                int x = 0;
                int y = 0;

                for (int i = 0; i < resultImage.Width * resultImage.Height; i++)
                {
                    resultImage[x, y] = new Rgba32(0, 0, 0);

                    x++;

                    if (x >= resultImage.Width)
                    {
                        x = 0;
                        y++;
                    }
                }


                SetImageToAnother(originalImage, ref resultImage, averagePoint, minimalPoint);

                DrawSolidColorRectangle(ref resultImage, averagePoint - Thickness * 2, minimalPoint - Thickness * 2, Thickness, originalImage.Height + Thickness * 4, new Rgba32(255, 255, 255));

                DrawSolidColorRectangle(ref resultImage, averagePoint - Thickness * 2, minimalPoint - Thickness * 2, originalImage.Width + Thickness * 4, Thickness, new Rgba32(255, 255, 255));

                DrawSolidColorRectangle(ref resultImage, Width - averagePoint + Thickness, minimalPoint - Thickness * 2, Thickness, originalImage.Height + Thickness * 4, new Rgba32(255, 255, 255));

                DrawSolidColorRectangle(ref resultImage, averagePoint - Thickness * 2, minimalPoint + originalImage.Height + Thickness, originalImage.Width + Thickness * 4, Thickness, new Rgba32(255, 255, 255));



                if (bottomCaption == null)
                {
                    RenderText(topCaption, originalImage.Width + averagePoint, minimalPoint);

                    SetImageToAnother(Image.Load<Rgba32>("RenderedImageSharpText.png"), ref resultImage, averagePoint / 2, originalImage.Height + 10 + minimalPoint + (averagePoint / 2));
                }
                if (bottomCaption != null && topCaption != null)
                {
                    RenderText(topCaption, originalImage.Width + averagePoint, minimalPoint);

                    SetImageToAnother(Image.Load<Rgba32>("RenderedImageSharpText.png"), ref resultImage, averagePoint / 2, originalImage.Height + 10 + minimalPoint + (averagePoint / 2));

                    RenderText(bottomCaption, originalImage.Width + averagePoint, (int)(minimalPoint * 0.75));

                    SetImageToAnother(Image.Load<Rgba32>("RenderedImageSharpText.png"), ref resultImage, averagePoint / 2, originalImage.Height + 10 + minimalPoint + (int)(averagePoint * 1.5));

                }

                if (resultImage.Width > 2500 || resultImage.Height > 2500)
                {
                    int TooBigWidth = resultImage.Width;
                    int TooBigHeight = resultImage.Height;
                    int RelationSize = Width / Height;

                    if (TooBigWidth > TooBigHeight)
                    {
                        resultImage.Mutate(x => x.Resize(2500, 2500 * RelationSize));
                    }
                    else
                    {
                        resultImage.Mutate(x => x.Resize(2000 * RelationSize, 2500));
                    }
                }
                Console.WriteLine(resultImage.Width + "x" + resultImage.Height);

                await resultImage.SaveAsPngAsync("DemotivatorBotResult.png");

            }
            else
            {
                Width = originalImage.Width + (averagePoint * 2);
                Height = originalImage.Height + minimalPoint + (averagePoint * 2);

                Image<Rgba32> resultImage = new Image<Rgba32>(Width, Height);

                int x = 0;
                int y = 0;

                for (int i = 0; i < resultImage.Width*resultImage.Height; i++)
                {
                    resultImage[x, y] = new Rgba32(0, 0, 0);

                    x++;

                    if (x >= resultImage.Width)
                    {
                        x = 0;
                        y++;
                    }
                }


                SetImageToAnother(originalImage, ref resultImage, averagePoint, minimalPoint);

                DrawSolidColorRectangle(ref resultImage, averagePoint - Thickness * 2, minimalPoint - Thickness * 2, Thickness, originalImage.Height + Thickness * 4, new Rgba32(255, 255, 255));

                DrawSolidColorRectangle(ref resultImage, averagePoint - Thickness * 2, minimalPoint - Thickness * 2, originalImage.Width + Thickness * 4, Thickness, new Rgba32(255, 255, 255));

                DrawSolidColorRectangle(ref resultImage, Width - averagePoint + Thickness, minimalPoint - Thickness * 2, Thickness, originalImage.Height + Thickness * 4, new Rgba32(255, 255, 255));

                DrawSolidColorRectangle(ref resultImage, averagePoint - Thickness * 2, minimalPoint + originalImage.Height + Thickness, originalImage.Width + Thickness * 4, Thickness, new Rgba32(255, 255, 255));

                RenderText(topCaption, originalImage.Width, minimalPoint);

                SetImageToAnother(Image.Load<Rgba32>("RenderedImageSharpText.png"), ref resultImage, averagePoint, originalImage.Height + 10 + minimalPoint + (averagePoint / 2));
            
                if (resultImage.Width > 2500 || resultImage.Height > 2500)
                {
                    int TooBigWidth = resultImage.Width;
                    int TooBigHeight = resultImage.Height;
                    int RelationSize = Width / Height;

                    if (TooBigWidth > TooBigHeight)
                    {
                        resultImage.Mutate(x => x.Resize(2500, 2500 * RelationSize));
                    }
                    else
                    {
                        resultImage.Mutate(x => x.Resize(2500 * RelationSize, 2500));
                    }
                }
                Console.WriteLine(resultImage.Width + "x" + resultImage.Height);

                await resultImage.SaveAsPngAsync("DemotivatorBotResult.png");



            }
            
        }

        public static void SetImageToAnother(Image<Rgba32> sourceImage, ref Image<Rgba32> destinationImage, int x0, int y0)
        {
            int x = x0;
            int y = y0;

            int x1 = 0;
            int y1 = 0;

            for (int i = 0; i < sourceImage.Width*sourceImage.Height; i++)
            {
                if (sourceImage[x1, y1].A < 255)
                {
                    destinationImage[x, y] = new Rgba32(sourceImage[x1, y1].A, sourceImage[x1, y1].A, sourceImage[x1, y1].A);
                }
                else
                {
                    destinationImage[x, y] = sourceImage[x1, y1];
                }

                x++;
                x1++;

                if (x1 >= sourceImage.Width)
                {
                    x = x0;
                    y++;
                    x1 = 0;
                    y1++;
                }
            }
        }

        public static void DrawSolidColorRectangle(ref Image<Rgba32> destinationImage, int x0, int y0, int rectWidth, int rectHeight, Rgba32 color)
        {
            int x = x0;
            int y = y0;

            for (int i = 0; i < rectWidth*rectHeight; i++)
            {
                destinationImage[x, y] = color;

                x++;

                if (x >= rectWidth+x0)
                {
                    x = x0;
                    y++;
                }
                if (y >= destinationImage.Height) break ;
            }
        }

        public static async void RenderText(string text, int maxWidth, int maxHeight)
        {

            //family = fontCollection.Install("Symbola.ttf");

            Font font = family.CreateFont(maxHeight);

            Image<Rgba32> img = new Image<Rgba32>(maxWidth, maxHeight);

            TextGraphicsOptions options = new TextGraphicsOptions()
            {
                TextOptions = new TextOptions()
                {
                    HorizontalAlignment = HorizontalAlignment.Left,
                    
                }

            };

            FontRectangle fontRectangle = TextMeasurer.Measure(text, new RendererOptions(font));

            img.Mutate(x => x.DrawText(options, text, font, new Color(new Rgba32(255, 255, 255)), new PointF((img.Width/2) - (fontRectangle.Width/2), (img.Height/2) - (fontRectangle.Height/2))));

            await img.SaveAsPngAsync("RenderedImageSharpText.png");
            
        }

        public static void AddToQueue()
        {
            
            Message message = new Message();
            for (; ; )
            {
                if (State == 0)
                {
                    bool hasMessage = messagesQueue.TryDequeue(out message);
                    if (hasMessage == true)
                    {
                        State = 1;
                        MessageManipulations(message);
                        
                        
                    }
                }
            }

        }
         
        public static async void CallbackQueryReceived(object sender, CallbackQueryEventArgs callbackQueryEvent)
        {
            await botClient.AnswerCallbackQueryAsync(callbackQueryEvent.CallbackQuery.Id);

            string content = callbackQueryEvent.CallbackQuery.Data;

            switch (content)
            {
                case "advantages":
                    await botClient.SendTextMessageAsync(
                        chatId: callbackQueryEvent.CallbackQuery.Message.Chat.Id,
                        text: Configuration.advantagesText,
                        parseMode: ParseMode.Markdown
                        );
                    break;
                case "help":
                    await botClient.SendTextMessageAsync(
                        chatId: callbackQueryEvent.CallbackQuery.Message.Chat.Id,
                        text: Configuration.helpText, 
                        parseMode: ParseMode.MarkdownV2
                        );
                    break;
            }
        }

        public static async Task<string[]> GetCaptions(string caption)
        {
            string[] cap;
            if (caption.IndexOf("\n") != -1)
            {
                cap = new string[]
                {
                caption.Substring(0, caption.IndexOf("\n")),

                caption.Substring(caption.IndexOf("\n") + 1)
                };
            }
            else
            {
                Console.WriteLine("Number of captions equals 1");
                cap = new string[] { caption, "" };
                numOfCaptions = 1;
            }

            return cap;
        }
    }
}
