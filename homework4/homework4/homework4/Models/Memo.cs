using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;
using System.Runtime.Serialization;
using Windows.Storage;
using System.IO;

namespace homework4.Model
{
    [DataContract]
    public class Memo : INotifyPropertyChanged
    {
        [DataMember(Order = 0)]
        public string id { set; get; }
        private string title;
        [DataMember(Order = 1)]
        public string Title
        {
            get { return title; }
            set { title = value; OnPropertyChanged("Title"); }
        }
        private string description;
        [DataMember(Order = 2)]
        public string Description
        {
            get { return description; }
            set { description = value; OnPropertyChanged("Description"); }
        }
        private DateTimeOffset time;
        [DataMember(Order = 3)]
        public DateTimeOffset Time
        {
            get { return time; }
            set { time = value; OnPropertyChanged("Time"); }
        }
        private bool completed;
        [DataMember(Order = 4)]
        public bool Completed
        {
            get { return completed; }
            set { completed = value; OnPropertyChanged("Completed"); }
        }
        private BitmapImage img;
        [DataMember(Order = 5)]
        public string token { set; get; }
        public StorageFile imgFile;
        public BitmapImage Img
        {
            get { return img; }
            set { img = value; OnPropertyChanged("Img"); }
        }
        public Memo(string title, string description, BitmapImage img, DateTimeOffset time, StorageFile imgFile)
        {
            this.id = Guid.NewGuid().ToString();
            this.title = title;
            this.description = description;
            this.img = img;
            this.time = time;
            this.imgFile = imgFile;
            this.completed = false;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}
