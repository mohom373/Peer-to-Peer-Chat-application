using P2PChatProj.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace P2PChatProj.Models
{
    public static class HistoryHandler
    {
        public static List<SavedChatMessage> ObservableToHistoryList(ObservableCollection<ChatMessage> observableList)
        {
            List<SavedChatMessage> historyList = new List<SavedChatMessage>();
            
            foreach (ChatMessage chatMessage in observableList)
            {
                if (chatMessage.GetType() == typeof(ImageChatMessage))
                {
                    string imageDataString = ImageService.BitmapImageToString(((ImageChatMessage)chatMessage).ImageSource);
                    SavedChatMessage savedChatMessage = new SavedChatMessage(chatMessage.Name, chatMessage.Date,
                                                                             chatMessage.Visibility, "", true,
                                                                             imageDataString);
                    historyList.Add(savedChatMessage);
                }
                else
                {
                    SavedChatMessage savedChatMessage = new SavedChatMessage(chatMessage.Name, chatMessage.Date,
                                                                             chatMessage.Visibility,
                                                                             ((TextChatMessage)chatMessage).TextMessage);
                    historyList.Add(savedChatMessage);
                }
            }

            return historyList;
        }

        public static ObservableCollection<ChatMessage> HistoryListToObservable(List<SavedChatMessage> historyList)
        {
            ObservableCollection<ChatMessage> observableList = new ObservableCollection<ChatMessage>();

            foreach (SavedChatMessage savedChatMessage in historyList)
            {
                if (savedChatMessage.Image)
                {
                    BitmapImage imageSource = ImageService.StringToBitmapImage(savedChatMessage.ImageData);
                    ImageChatMessage imageChatMessage = new ImageChatMessage(savedChatMessage.Name,
                                                                             savedChatMessage.Date,
                                                                             imageSource,
                                                                             savedChatMessage.Visibility);
                    observableList.Add(imageChatMessage);
                }
                else
                {
                    TextChatMessage textChatMessage = new TextChatMessage(savedChatMessage.Name,
                                                                          savedChatMessage.Date,
                                                                          savedChatMessage.TextMessage,
                                                                          savedChatMessage.Visibility);
                    observableList.Add(textChatMessage);
                }
            }
            return observableList;
        }
    }
}
