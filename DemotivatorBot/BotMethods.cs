using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.IO;
using System.Net;
using System.Linq;
using System.Collections.Generic;
using Telegram.Bot.Types;
using Telegram.Bot.Args;
using Telegram.Bot;
using Telegram.Bot.Types.InputFiles;
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
        public static bool isReply;
        public static int State = 0;
        public static Queue<Message> messagesQueue = new Queue<Message>();
        public static int numOfCaptions = 2;
        public static string caption;
        public static string topCaption;
        public static string bottomCaption;
        public static string jsonResponse = "";
        public static int ch;
        public static HttpWebRequest requestForFilePath;
        public static HttpWebResponse responseForFilePath;
        public static Stream responseStreamForFilePath;
        public static string filePath;
        public static FontCollection fontCollection = new FontCollection();
        public static FontFamily family = fontCollection.Install("C:/SavedPictures/Times.ttf");
        public static async void MessageReceived(object sender, MessageEventArgs e)
        {

            Message message = e.Message;

            messagesQueue.Enqueue(message);

            

        }
        public static async void MessageManipulations(TelegramBotClient botClient, Message message)
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
                    case "/text":
                        
                        break;
                    
                }
                
                if (isReply == true)
                {
                    await botClient.SendChatActionAsync(
                    chatId: message.Chat.Id,
                    chatAction: ChatAction.UploadPhoto
                    );
                    PrepearePicture(message.ReplyToMessage.Photo, message.Text, botClient, message.Chat.Id);
                }

            }

            if (message.Type == MessageType.Photo)
            {
                await botClient.SendChatActionAsync(
                    chatId: message.Chat.Id,
                    chatAction: ChatAction.UploadPhoto
                    );
                PrepearePicture(message.Photo, message.Caption, botClient, message.Chat.Id);
            }
        }

        public static async void PrepearePicture(PhotoSize[] photoSize, string messageCaption, TelegramBotClient botClient, ChatId messageChatId)
        {
            


            FileStream photoStream = new FileStream("newphotos.png", FileMode.Create);

            Telegram.Bot.Types.File fl = await botClient.GetFileAsync(photoSize[photoSize.Length - 1].FileId);

            await botClient.DownloadFileAsync(fl.FilePath, photoStream);

            Thread.Sleep(75);

            photoStream.Close();

            Image<Rgba32> img = Image.Load<Rgba32>("newphotos.png");

            //img.Mutate(x => x.Resize(img.Width * Configuration.ResizerValue, img.Height * Configuration.ResizerValue));

            Console.WriteLine("Size before Mutate: Width - {0}, Height - {1}", img.Width, img.Height);

            if (img.Width < 400 || img.Height < 400)
            {
                int w = 512 - img.Width;
                int h = 512 - img.Height;
                int bigger = h;
                if (w > h) bigger = w;
                img.Mutate(x => x.Resize(img.Width + bigger, img.Height + bigger));
            }
            else
            {
                img.Mutate(x => x.Resize((int)(img.Width * 1.5), (int)(img.Height * 1.5)));
            }
            Console.WriteLine("Size after Mutate: Width - {0}, Height - {1}", img.Width, img.Height);


            //img.Mutate(x => x.Resize(512, 512));

            img.SaveAsPng("newphotos.png");

            caption = messageCaption;

            if (caption != null)
            {
                if (caption.IndexOf("\n") != -1)
                {
                    topCaption = caption.Substring(0, caption.IndexOf("\n"));

                    bottomCaption = caption.Substring(caption.IndexOf("\n") + 1);
                }
                else
                {
                    Console.WriteLine("Number of captions equals 1");
                    topCaption = caption;
                    bottomCaption = "";
                    numOfCaptions = 1;
                }

            }
            else
            {
                topCaption = "Не пишите длинный текст";

                bottomCaption = "Он не вмещается";
            }


            CreateDemotivator(topCaption, bottomCaption, numOfCaptions);

            Console.WriteLine("Demotivator created!");

            filePath = "";
            jsonResponse = "";



            FileStream fileStream = new FileStream("DemotivatorBotResult.png", FileMode.Open);

            InputOnlineFile ResultFile = new InputOnlineFile(fileStream, "result.png");


            Console.WriteLine("File is ready for uploading");

            await botClient.SendPhotoAsync(
                chatId: messageChatId,
                ResultFile
                );
            Console.WriteLine("Message sended(async)");
            fileStream.Close();

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

                if (Width < 100 || Height < 100)
                {
                    Width = 200;
                    Height = 200;
                }

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
                    RenderText(topCaption, originalImage.Width, minimalPoint);

                    SetImageToAnother(Image.Load<Rgba32>("RenderedImageSharpText.png"), ref resultImage, averagePoint, originalImage.Height + 10 + minimalPoint + (averagePoint / 2));
                }
                if (bottomCaption != null && topCaption != null)
                {
                    RenderText(topCaption, originalImage.Width, minimalPoint);

                    SetImageToAnother(Image.Load<Rgba32>("RenderedImageSharpText.png"), ref resultImage, averagePoint, originalImage.Height + 10 + minimalPoint + (averagePoint / 2));

                    RenderText(bottomCaption, originalImage.Width, (int)(minimalPoint * 0.75));

                    SetImageToAnother(Image.Load<Rgba32>("RenderedImageSharpText.png"), ref resultImage, averagePoint, originalImage.Height + 10 + minimalPoint + (int)(averagePoint * 1.5));

                }

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

        public static void AddToQueue(TelegramBotClient botClient)
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
                        MessageManipulations(botClient, message);
                        
                        
                    }
                }
            }

        }

    }
}
