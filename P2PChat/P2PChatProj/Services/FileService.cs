using Newtonsoft.Json;
using P2PChatProj.Exceptions;
using P2PChatProj.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P2PChatProj.Services
{
    public static class FileService
    {
        private const string PATH = "history.json";

        public static async Task WriteHistoryAsync(List<SavedChatData> history)
        {
            string jsonData = JsonConvert.SerializeObject(history);

            await Task.Run(() =>
            {
                try
                {
                    File.WriteAllText(PATH, jsonData);
                }
                catch (IOException)
                {
                    Console.WriteLine("ERROR: Failed to write history to file");
                }
            });
        }

        public static async Task<List<SavedChatData>> LoadHistoryAsync()
        {
            string jsonData = await Task.Run(() =>
            {
                try
                {
                    return File.ReadAllText(PATH);
                }
                catch (IOException)
                {
                    Console.WriteLine("ERROR: Failed to read history from file");
                    return "";
                }
            });

            if (jsonData != "")
            {
                 return JsonConvert.DeserializeObject<List<SavedChatData>>(jsonData);
            }
            else
            {
                throw new HistoryNotFoundException("History file could not be found");
            }
        }
    }
}
