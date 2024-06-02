using Firebase.Database;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using BadgeConnector.Models;
using BadgeConnector.Services;
using Microsoft.EntityFrameworkCore;
using Firebase.Database.Query;

namespace BadgeConnector.ViewModel
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private LedContext context;
        private Badge selectedBadge;
        private BadgeGroup selectedBadgeGroup;
        private Message selectedMessage;
        private ObservableCollection<Badge> badges;
        private ObservableCollection<BadgeGroup> badgeGroups;
        private ObservableCollection<Message> messages;

        public MainWindowViewModel()
        {
            context = new LedContext();
            Badges = new ObservableCollection<Badge>(context.Badges.ToList());
            BadgeGroups = new ObservableCollection<BadgeGroup>(context.BadgeGroups.Include(bg => bg.Badges).ToList());
            SelectedBadge = Badges.FirstOrDefault();
            SelectedBadgeGroup = BadgeGroups.FirstOrDefault();

            InitializeFirebase();
            LoadMessagesForBadge();
            SetupFirebaseListeners();
        }

        public ObservableCollection<Badge> Badges
        {
            get { return badges; }
            set { badges = value; OnPropertyChanged(nameof(Badges)); }
        }

        public ObservableCollection<BadgeGroup> BadgeGroups
        {
            get { return badgeGroups; }
            set { badgeGroups = value; OnPropertyChanged(nameof(BadgeGroups)); }
        }

        public Badge SelectedBadge
        {
            get { return selectedBadge; }
            set
            {
                selectedBadge = value;
                OnPropertyChanged(nameof(SelectedBadge));
                LoadMessagesForBadge();
            }
        }

        public BadgeGroup SelectedBadgeGroup
        {
            get { return selectedBadgeGroup; }
            set
            {
                selectedBadgeGroup = value;
                OnPropertyChanged(nameof(SelectedBadgeGroup));
                LoadMessagesForBadgeGroup();
            }
        }

        public Message SelectedMessage
        {
            get { return selectedMessage; }
            set
            {
                selectedMessage = value;
                OnPropertyChanged(nameof(SelectedMessage));
            }
        }

        public ObservableCollection<Message> Messages
        {
            get { return messages; }
            set { messages = value; OnPropertyChanged(nameof(Messages)); }
        }

        private void LoadMessagesForBadge()
        {
            if (SelectedBadge != null)
            {
                Messages = new ObservableCollection<Message>(context.Messages
                    .Include(m => m.Tags)
                    .Where(m => m.BadgeId == SelectedBadge.BadgeId)
                    .ToList());
            }
        }

        private void LoadMessagesForBadgeGroup()
        {
            if (SelectedBadgeGroup != null)
            {
                var badgeIds = SelectedBadgeGroup.Badges.Select(b => b.BadgeId).ToList();
                Messages = new ObservableCollection<Message>(context.Messages
                    .Include(m => m.Tags)
                    .Where(m => badgeIds.Contains(m.BadgeId))
                    .ToList());
            }
        }

        public ICommand AddMessageCommand => new RelayCommand(AddMessage);
        public ICommand UpdateMessageCommand => new RelayCommand(UpdateMessage);
        public ICommand DeleteMessageCommand => new RelayCommand(DeleteMessage);
        public ICommand AssignTagsCommand => new RelayCommand(AssignTags);
        public ICommand UnassignTagsCommand => new RelayCommand(UnassignTags);
        public ICommand CopyTextToClipboardCommand => new RelayCommand(CopyTextToClipboard);
        public ICommand CopyImageToClipboardCommand => new RelayCommand(CopyImageToClipboard);
        public ICommand ScanBadgesCommand => new RelayCommand(ScanBadges);
        public ICommand ImportXmlCommand => new RelayCommand(ImportXml);
        public ICommand ImportExcelCommand => new RelayCommand(ImportExcel);
        public ICommand ImportCsvCommand => new RelayCommand(ImportCsv);
        public ICommand ExportXmlCommand => new RelayCommand(ExportXml);
        public ICommand ExportExcelCommand => new RelayCommand(ExportExcel);
        public ICommand ExportCsvCommand => new RelayCommand(ExportCsv);

        private void AddMessage(object parameter)
        {
            var newMessage = new Message
            {
                RawMessage = "New Message",
                BadgeId = SelectedBadge.BadgeId // or some appropriate value
            };

            context.Messages.Add(newMessage);
            context.SaveChanges();
            FirebaseAddMessage(newMessage);
            LoadMessagesForBadge();
        }

        private void UpdateMessage(object parameter)
        {
            if (SelectedMessage != null)
            {
                context.Messages.Update(SelectedMessage);
                context.SaveChanges();
                FirebaseUpdateMessage(SelectedMessage);
                LoadMessagesForBadge();
            }
        }

        private void DeleteMessage(object parameter)
        {
            if (SelectedMessage != null)
            {
                context.Messages.Remove(SelectedMessage);
                context.SaveChanges();
                FirebaseDeleteMessage(SelectedMessage);
                LoadMessagesForBadge();
            }
        }

        private void AssignTags(object parameter)
        {
            // Assign tags implementation
        }

        private void UnassignTags(object parameter)
        {
            // Unassign tags implementation
        }

        private void CopyTextToClipboard(object parameter)
        {
            // Copy text to clipboard implementation
        }

        private void CopyImageToClipboard(object parameter)
        {
            // Copy image to clipboard implementation
        }

        private void ScanBadges(object parameter)
        {
            // Scan badges implementation
        }

        private void ImportXml(object parameter)
        {
            // Import XML implementation
        }

        private void ImportExcel(object parameter)
        {
            // Import Excel implementation
        }

        private void ImportCsv(object parameter)
        {
            // Import CSV implementation
        }

        private void ExportXml(object parameter)
        {
            // Export XML implementation
        }

        private void ExportExcel(object parameter)
        {
            // Export Excel implementation
        }

        private void ExportCsv(object parameter)
        {
            // Export CSV implementation
        }

        private void InitializeFirebase()
        {
            App.FirebaseClient = new FirebaseClient(
                "https://dotnet-8a9fb-default-rtdb.europe-west1.firebasedatabase.app/",
                new FirebaseOptions
                {
                    AuthTokenAsyncFactory = () => Task.FromResult("your-firebase-auth-token") // Replace with actual token management
                });
        }

        private void SetupFirebaseListeners()
        {
            App.FirebaseClient
                .Child("messages")
                .AsObservable<Message>()
                .Subscribe(d =>
                {
                    if (d.EventType == Firebase.Database.Streaming.FirebaseEventType.InsertOrUpdate)
                    {
                        var message = d.Object;
                        var existingMessage = context.Messages.FirstOrDefault(m => m.MessageId == message.MessageId);

                        if (existingMessage != null)
                        {
                            context.Entry(existingMessage).CurrentValues.SetValues(message);
                        }
                        else
                        {
                            context.Messages.Add(message);
                        }

                        context.SaveChanges();
                        LoadMessagesForBadge();
                    }
                    else if (d.EventType == Firebase.Database.Streaming.FirebaseEventType.Delete)
                    {
                        var message = context.Messages.FirstOrDefault(m => m.MessageId == d.Object.MessageId);
                        if (message != null)
                        {
                            context.Messages.Remove(message);
                            context.SaveChanges();
                            LoadMessagesForBadge();
                        }
                    }
                });
        }

        private async Task FirebaseAddMessage(Message message)
        {
            await App.FirebaseClient
                .Child("messages")
                .Child(message.MessageId.ToString())
                .PutAsync(message);
        }

        private async Task FirebaseUpdateMessage(Message message)
        {
            await App.FirebaseClient
                .Child("messages")
                .Child(message.MessageId.ToString())
                .PutAsync(message);
        }

        private async Task FirebaseDeleteMessage(Message message)
        {
            await App.FirebaseClient
                .Child("messages")
                .Child(message.MessageId.ToString())
                .DeleteAsync();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}