using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace YoutubeDown.Library
{
    public abstract class PropertyChangedBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChanged<T>(ref T storage, T value, [CallerMemberName] string name = null)
        {
            if (EqualityComparer<T>.Default.Equals(storage, value)) return;

            storage = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        protected void NotifyPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
