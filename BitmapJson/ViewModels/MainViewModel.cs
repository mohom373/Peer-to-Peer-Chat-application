using BitmapJson.Models;
using BitmapJson.ViewModels.Commands;
using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media.Imaging;

namespace BitmapJson.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        public List<string> Messages { get; set; }

        public ObservableCollection<ChatData> RemoteData { get; set; }

        public ObservableCollection<ChatData> UserData { get; set; }

        public DelegateCommand AddRemoteTextCommand { get; set; }

        public DelegateCommand AddRemoteImageCommand { get; set; }

        public DelegateCommand AddUserTextCommand { get; set; }

        public DelegateCommand AddUserImageCommand { get; set; }

        public MainViewModel()
        {
            RemoteData = new ObservableCollection<ChatData>();
            UserData = new ObservableCollection<ChatData>();
            Messages = new List<string>();
            FillMessages();
            AddUserTextCommand = new DelegateCommand(AddUserText);
            AddRemoteTextCommand = new DelegateCommand(AddRemoteText);
            AddRemoteImageCommand = new DelegateCommand(AddRemoteImage);
            AddUserImageCommand = new DelegateCommand(AddUserImage);
        }

        private void FillMessages()
        {
            Messages.Add("Hej!");
            Messages.Add("Tjenare hörru!");
            Messages.Add("Här har vi ett jädrans långt meddelande som aldrig tar slut och förhoppningsvis wrappar textrutan");
            Messages.Add("Big hmmmmmmmmm!!!!");
            Messages.Add("Dra åt helvete va fin testapp man har fått till. Nu blire att dra hem å fira med en tårta");
        }

        private void AddUserText()
        {
            Console.WriteLine("Adding a user text message");

            string randomMessage = Messages.ElementAt(new Random().Next(0, 5));

            // SENDING SIMULATION
            NetworkData networkData = new NetworkData("Victor", randomMessage, false);
            string jsonNetworkData = JsonConvert.SerializeObject(networkData);

            Console.WriteLine("JSON Network Data:");
            Console.WriteLine(jsonNetworkData);
            
            NetworkData receivedNetworkData = JsonConvert.DeserializeObject<NetworkData>(jsonNetworkData);

            if (!receivedNetworkData.ImageMessage)
            {
                TextData visibleTextData = new TextData(receivedNetworkData.Name, receivedNetworkData.Data);
                TextData hiddenTextData = new TextData(receivedNetworkData.Name, receivedNetworkData.Data, Visibility.Hidden);

                UserData.Add(visibleTextData);
                RemoteData.Add(hiddenTextData);
            }
        }

        private void AddRemoteText()
        {
            Console.WriteLine("Adding a remote text message");

            string randomMessage = Messages.ElementAt(new Random().Next(0, 5));

            // SENDING SIMULATION
            NetworkData networkData = new NetworkData("Moe", randomMessage, false);
            string jsonNetworkData = JsonConvert.SerializeObject(networkData);

            Console.WriteLine("JSON Network Data:");
            Console.WriteLine(jsonNetworkData);

            NetworkData receivedNetworkData = JsonConvert.DeserializeObject<NetworkData>(jsonNetworkData);

            if (!receivedNetworkData.ImageMessage)
            {
                TextData visibleTextData = new TextData(receivedNetworkData.Name, receivedNetworkData.Data);
                TextData hiddenTextData = new TextData(receivedNetworkData.Name, receivedNetworkData.Data, Visibility.Hidden);

                UserData.Add(hiddenTextData);
                RemoteData.Add(visibleTextData);
            }
        }

        private void AddUserImage()
        {
            Console.WriteLine("Adding a user image");

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = "Pictures";
            openFileDialog.Filter = "Image files (*.jpg;*.jpeg)|*.jpg;*.jpeg;";
            openFileDialog.RestoreDirectory = true;

            string fileName = "";
            BitmapImage bitmapImage = new BitmapImage();

            openFileDialog.ShowDialog(Application.Current.MainWindow);
            fileName = openFileDialog.FileName;

            if (!String.IsNullOrEmpty(fileName))
            {
                bitmapImage.BeginInit();
                bitmapImage.UriSource = new Uri(fileName);
                bitmapImage.EndInit();

                // SENDING SIMULATION
                // BitmapImage to bitmap
                Bitmap bitmap = BitmapImageToBitmap(bitmapImage);

                // Bitmap to bit array
                ImageConverter converter = new ImageConverter();
                byte[] bitmapBytes = (byte[])converter.ConvertTo(bitmap, typeof(byte[]));

                // Bitmap bytes to string
                string bitmapString = Convert.ToBase64String(bitmapBytes);

                // Bitmap in a networkdata object
                NetworkData networkData = new NetworkData("Victor", bitmapString, true);
                string jsonNetworkData = JsonConvert.SerializeObject(networkData);

                Console.WriteLine("JSON Network Data:");
                Console.WriteLine(jsonNetworkData);

                NetworkData receivedNetworkData = JsonConvert.DeserializeObject<NetworkData>(jsonNetworkData);

                if (receivedNetworkData.ImageMessage)
                {
                    byte[] receivedBitmapBytes = Convert.FromBase64String(receivedNetworkData.Data);

                    Bitmap receivedBitmap = (Bitmap)converter.ConvertFrom(receivedBitmapBytes);
                    BitmapImage receivedBitmapImage = BitmapToBitmapImage(receivedBitmap);

                    ImageData visibleImage = new ImageData(receivedNetworkData.Name, receivedBitmapImage);
                    ImageData hiddenImage = new ImageData(receivedNetworkData.Name, receivedBitmapImage, Visibility.Hidden);

                    RemoteData.Add(hiddenImage);
                    UserData.Add(visibleImage);
                } 
            }
        }

        private void AddRemoteImage()
        {
            Console.WriteLine("Adding a remote image");

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = "Pictures";
            openFileDialog.Filter = "Image files (*.jpg;*.jpeg)|*.jpg;*.jpeg;";
            openFileDialog.RestoreDirectory = true;

            string fileName = "";
            BitmapImage bitmapImage = new BitmapImage();

            openFileDialog.ShowDialog(Application.Current.MainWindow);
            fileName = openFileDialog.FileName;
            
            if (!String.IsNullOrEmpty(fileName))
            {
                bitmapImage.BeginInit();
                bitmapImage.UriSource = new Uri(fileName);
                bitmapImage.EndInit();

                // SENDING SIMULATION
                // BitmapImage to bitmap
                Bitmap bitmap = BitmapImageToBitmap(bitmapImage);

                // Bitmap to bit array
                ImageConverter converter = new ImageConverter();
                byte[] bitmapBytes = (byte[])converter.ConvertTo(bitmap, typeof(byte[]));

                // Bitmap bytes to string
                string bitmapString = Convert.ToBase64String(bitmapBytes);

                // Bitmap in a networkdata object
                NetworkData networkData = new NetworkData("Moe", bitmapString, true);
                string jsonNetworkData = JsonConvert.SerializeObject(networkData);

                Console.WriteLine("JSON Network Data:");
                Console.WriteLine(jsonNetworkData);

                NetworkData receivedNetworkData = JsonConvert.DeserializeObject<NetworkData>(jsonNetworkData);

                if (receivedNetworkData.ImageMessage)
                {
                    byte[] receivedBitmapBytes = Convert.FromBase64String(receivedNetworkData.Data);

                    Bitmap receivedBitmap = (Bitmap)converter.ConvertFrom(receivedBitmapBytes);
                    BitmapImage receivedBitmapImage = BitmapToBitmapImage(receivedBitmap);

                    ImageData visibleImage = new ImageData(receivedNetworkData.Name, receivedBitmapImage);
                    ImageData hiddenImage = new ImageData(receivedNetworkData.Name, receivedBitmapImage, Visibility.Hidden);

                    RemoteData.Add(visibleImage);
                    UserData.Add(hiddenImage);
                }
            }
        }

        private Bitmap BitmapImageToBitmap(BitmapImage bitmapImage)
        {
            using (MemoryStream outStream = new MemoryStream())
            {
                BitmapEncoder encoder = new BmpBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(bitmapImage));
                encoder.Save(outStream);
                Bitmap bitmap = new Bitmap(outStream);

                return new Bitmap(bitmap);
            }
        }

        private BitmapImage BitmapToBitmapImage(Bitmap bitmap)
        {
            using (var memory = new MemoryStream())
            {
                bitmap.Save(memory, ImageFormat.Jpeg);
                memory.Position = 0;

                BitmapImage bitmapImage = new BitmapImage();
                
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memory;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
                bitmapImage.Freeze();

                return bitmapImage;
            }
        }
    }
}
