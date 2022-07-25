using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.CompilerServices;

public delegate void PropertyChangedEventHandler(PropertyChangedEventArgs e);
public delegate void NotifyCollectionChangedEventHandler(
    PropertyChangedEventArgs property,
    NotifyCollectionChangedEventArgs e);

public class BaseModel
{
    public int InstanceId { get; set; }

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) =>
        PropertyChanged?.Invoke(new PropertyChangedEventArgs(propertyName));
    
    protected bool SetProperty<T>(ref T storage, T value, bool ignoreNullOrEmpty = true,
        bool ignoreUpdateSameValue = true, [CallerMemberName] string propertyName = null)
    {
        // need to update view depend of default value for something data (e.t. asset bundle content)
        if (ignoreUpdateSameValue && Equals(storage, value) ||
            ignoreNullOrEmpty && string.IsNullOrEmpty(propertyName))
            return false;

        storage = value;
        OnPropertyChanged(propertyName);
        return true;
    }
}