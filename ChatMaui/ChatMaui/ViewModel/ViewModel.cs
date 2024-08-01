using Microsoft.Maui.Controls;
using Syncfusion.Maui.Chat;
using Syncfusion.Maui.Popup;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ChatMaui
{
    internal class ImagePickerService
    {
        internal async Task<byte[]> PickImageAsync()
        {
            try
            {
                var result = await MediaPicker.PickPhotoAsync();

                if (result != null)
                {
                    // Open the file stream
                    using (Stream stream = await result.OpenReadAsync())
                    {
                        // Copy the stream into a memory stream to avoid object disposed exception.
                        using (MemoryStream memoryStream = new MemoryStream())
                        {
                            await stream.CopyToAsync(memoryStream);
                            return memoryStream.ToArray();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions
                Console.WriteLine($"Error picking image: {ex.Message}");
            }

            return null;
        }

        internal async Task SaveImageAsync(string filePath)
        {
            try
            {
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error deleting image: {ex.Message}");
            }

            try 
            { 
                // Get the image from Gallery.
                byte[] imageBytes = await PickImageAsync();

                if (imageBytes != null)
                {
                    // Save the image bytes to a file, database, etc.
                    File.WriteAllBytes(filePath, imageBytes);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error saving image: {ex.Message}");
            }
        }
    }

    public class ViewModel : INotifyPropertyChanged
    {
        #region Fields

        /// <summary>
        /// Collection of messages in a conversation.
        /// </summary>
        private ObservableCollection<object> messages;

        private bool isOpen;

        private int imageNo;

        private readonly ImagePickerService _imagePickerService;
        /// <summary>
        /// Current user of chat.
        /// </summary>
        private Author currentUser;

        #endregion

        #region Constructor
       
        public ViewModel()
        {
            _imagePickerService = new ImagePickerService();
            this.messages = new ObservableCollection<object>();
            this.currentUser = new Author() { Name = "Nancy"};
            this.GenerateMessages();

            this.OpenGalleryCommand = new Command(OpenGalleryTapped);
        }

        #endregion

        #region Properties
        public ICommand OpenGalleryCommand
        {
            get;set;
        }

        public bool CanShowPopup
        {
            get
            {
                return isOpen;
            }
            set
            {
                isOpen = value;
                RaisePropertyChanged("CanShowPopup");
            }
        }

        /// <summary>
        /// Gets or sets the collection of messages of a conversation.
        /// </summary>
        public ObservableCollection<object> Messages
        {
            get
            {
                return this.messages;
            }

            set
            {
                this.messages = value;
                RaisePropertyChanged(nameof(this.messages));    
            }
        }

        /// <summary>
        /// Gets or sets the current user of the message.
        /// </summary>
        public Author CurrentUser
        {
            get
            {
                return this.currentUser;
            }
            set
            {
                this.currentUser = value;
                RaisePropertyChanged("CurrentUser");
            }
        }

        #endregion

        #region Events

        /// <summary>
        /// Property changed handler.
        /// </summary>
        public event PropertyChangedEventHandler? PropertyChanged;

        #endregion

        #region Methods
        private async void OpenGalleryTapped(object args)
        {
            string filePath = Path.Combine(FileSystem.AppDataDirectory, "image" + ++imageNo + ".jpg");
            
            // Get and store the image from gallery
            await _imagePickerService.SaveImageAsync(filePath);
            
            var imageSource = ImageSource.FromFile(filePath);
            if (imageSource != null)
            {
                var imageMessage = new ImageMessage()
                {
                    Source = imageSource,
                    Aspect = Aspect.AspectFill,
                    Size = new Size(150, 150),
                    Author = this.CurrentUser,
                };

                this.Messages.Add(imageMessage);
            }

            // Close the attachment popup once the image is added from gallery.
            if (this.CanShowPopup) 
            {
                this.CanShowPopup = false;
            }
        }

        /// <summary>
        /// Occurs when property is changed.
        /// </summary>
        /// <param name="propName">changed property name</param>
        public void RaisePropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }

        private void GenerateMessages()
        {
            this.messages.Add(new TextMessage()
            {
                Author = currentUser,
                Text = "Hi guys, good morning! I'm very delighted to share with you the news that our team is going to launch a new mobile application.",
            });

            this.messages.Add(new TextMessage()
            {
                Author = new Author() { Name = "Andrea", Avatar = "Andrea.png" },
                Text = "Oh! That's great.",
            });

            this.messages.Add(new TextMessage()
            {
                Author = new Author() { Name = "Harrison", Avatar = "Harrison.png" },
                Text = "That is good news.",
            });

            this.messages.Add(new TextMessage()
            {
                Author = new Author() { Name = "Margaret", Avatar = "Margaret.png" },
                Text = "Are we going to develop the app natively or hybrid?"
            });

            this.messages.Add(new TextMessage()
            {
                Author = currentUser,
                Text = "We should develop this app in .NET MAUI, since it provides native experience and perfomance as well as allowing for seamless cross-platform development.",
            });
            this.messages.Add(new TextMessage()
            {
                Author = new Author() { Name = "Margaret", Avatar = "Margaret.png" },
                Text = "I haven't heard of .NET MAUI. What's .NET MAUI?",
            });
            this.messages.Add(new TextMessage()
            {
                Author = currentUser,
                Text = ".NET MAUI is a new library that lets you build native UIs for Android, iOS, macOS, and Windows from one shared C# codebase.",
            });
        }

        #endregion
    }
}

