using Newtonsoft.Json;
using P2PChatProj.Exceptions;
using P2PChatProj.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P2PChatProj.Services
{
    public static class FileService
    {
        private static string historyFilePath;
        private static string imageSentDirectoryPath;
        private static string imageReceivedDirectoryPath;

        public static void DirectorySetupCheck()
        {
            string executingDirectoryPath = AppDomain.CurrentDomain.BaseDirectory;

            string imageDirectoryPath = Path.Combine(executingDirectoryPath, "images");

            if(!Directory.Exists(imageDirectoryPath))
            {
                Directory.CreateDirectory(imageDirectoryPath);
            }

            imageReceivedDirectoryPath = Path.Combine(imageDirectoryPath, "received");
            imageSentDirectoryPath = Path.Combine(imageDirectoryPath, "sent");

            if (!Directory.Exists(imageReceivedDirectoryPath))
            {
                Directory.CreateDirectory(imageReceivedDirectoryPath);
            }

            if (!Directory.Exists(imageSentDirectoryPath))
            {
                Directory.CreateDirectory(imageSentDirectoryPath);
            }

            string histDirPath = Path.Combine(executingDirectoryPath, "history");

            if (!Directory.Exists(histDirPath))
            {
                Directory.CreateDirectory(histDirPath);
            }

            historyFilePath = Path.Combine(histDirPath, "history.json");
        }

        public static async Task<string> SaveImage(Bitmap image, string name, bool sent = true)
        {
            string newImagePath;

            if (sent)
            {
                newImagePath = Path.Combine(imageSentDirectoryPath, name);
            }
            else
            {
                newImagePath = Path.Combine(imageReceivedDirectoryPath, name);
            }

            while (File.Exists(newImagePath))
            {
                newImagePath = newImagePath.Split('.')[0] + 
                               new Random().Next(0, 999).ToString() +
                               "." + newImagePath.Split('.')[1];
            }

            await Task.Run(() => image.Save(newImagePath, ImageFormat.Jpeg));
            return newImagePath;
        }

        public static async Task WriteHistoryAsync(List<ChatData> history)
        {
            string jsonData = JsonConvert.SerializeObject(history);

            await Task.Run(() =>
            {
                try
                {
                    File.WriteAllText(historyFilePath, jsonData);
                }
                catch (IOException)
                {
                    Console.WriteLine("ERROR: Failed to write history to file");
                }
            });
        }

        public static async Task<List<ChatData>> LoadHistoryAsync()
        {
            string jsonData = await Task.Run(() =>
            {
                try
                {
                    return File.ReadAllText(historyFilePath);
                }
                catch (IOException)
                {
                    Console.WriteLine("ERROR: Failed to read history from file");
                    return "";
                }
            });

            if (jsonData != "")
            {
                 return JsonConvert.DeserializeObject<List<ChatData>>(jsonData);
            }
            else
            {
                throw new HistoryNotFoundException("History file could not be found");
            }
        }
    }
}
