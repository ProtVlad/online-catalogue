using System.ComponentModel;

public class Curs : INotifyPropertyChanged
{
    private string _numeCurs;
    private string _descriere;

    public int Id { get; set; }

    public string NumeCurs
    {
        get => _numeCurs;
        set
        {
            if (_numeCurs != value)
            {
                _numeCurs = value;
                OnPropertyChanged(nameof(NumeCurs)); // Notify UI
            }
        }
    }

    public string Descriere
    {
        get => _descriere;
        set
        {
            if (_descriere != value)
            {
                _descriere = value;
                OnPropertyChanged(nameof(Descriere)); // Notify UI
            }
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
