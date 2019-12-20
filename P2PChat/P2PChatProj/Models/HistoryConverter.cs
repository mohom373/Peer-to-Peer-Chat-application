using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P2PChatProj.Models
{
    public static class HistoryConverter
    {
        public static ObservableCollection<ChatMessage> HistoryToChat(List<SavedChatMessage> historyMessages)
        {
            ObservableCollection<ChatMessage> chatMessages = new ObservableCollection<ChatMessage>();
            
            foreach (SavedChatMessage historyMessage in historyMessages)
            {
                if (historyMessage.Image)
                {
                    ImageChatMessage imageMessage = new ImageChatMessage(historyMessage.Name, historyMessage.Date,
                                                                         historyMessage.Data, historyMessage.Visibility);
                    chatMessages.Add(imageMessage);
                }
                else
                {
                    TextChatMessage textMessage = new TextChatMessage(historyMessage.Name, historyMessage.Date,
                                                                         historyMessage.Data, historyMessage.Visibility);
                    chatMessages.Add(textMessage);
                }
            }

            return chatMessages;
        }

        public static List<SavedChatMessage> ChatToHistory(ObservableCollection<ChatMessage> chatMessages)
        {
            List<SavedChatMessage> historyMessages = new List<SavedChatMessage>();

            foreach (ChatMessage chatMessage in chatMessages)
            {
                if (chatMessage.GetType() == typeof(ImageChatMessage))
                {
                    SavedChatMessage savedMessage = new SavedChatMessage(chatMessage.Name, chatMessage.Date,
                                                                         chatMessage.Visibility,
                                                                         ((ImageChatMessage)chatMessage).ImagePath,
                                                                         true);
                    historyMessages.Add(savedMessage);
                }
                else if (chatMessage.GetType() == typeof(TextChatMessage))
                {
                    SavedChatMessage savedMessage = new SavedChatMessage(chatMessage.Name, chatMessage.Date,
                                                                         chatMessage.Visibility,
                                                                         ((TextChatMessage)chatMessage).TextMessage);
                    historyMessages.Add(savedMessage);
                }
            }

            return historyMessages;
        }
    }
}
