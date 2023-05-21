using Microsoft.UI.Xaml.Controls;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace exemp
{
    public partial class MainPage : ContentPage 
    { 
        public MainPage() 
        { 
            InitializeComponent(); 
        }
        private void AddButton_Click(object sender, EventArgs e)
        {
            textBox.Text = string.Empty;
        }
    }

    public class ItemViewModel : INotifyPropertyChanged
    {
        private string _text; 
        private bool _isDone;
        private int _number;
        private CustomColor _catcolor;
        public CustomColor CatColor
        {
            get => _catcolor;
            set
            {
                if (_catcolor == value) return;
                _catcolor = value;
                OnPropertyChanged(nameof(CatColor));
                
            }
        }

        public int Number
        {
            get => _number;
            set
            {
                if (_number == value) return;
                _number = value;
                OnPropertyChanged(nameof(Number));
            }
        }
        public string Text
        {
            get => _text;
            set 
            { 
                if (_text == value) return;
                _text = value; 
                OnPropertyChanged(nameof(Text)); }
        } 
        public bool IsDone 
        { 
            get => _isDone;
            set
            {
                if (_isDone == value) return; 
                _isDone = value; 
                OnPropertyChanged(nameof(BackgroundColor));
            }
        }

        public Color BackgroundColor => _isDone ? Color.FromArgb("#44C1AF") : Color.FromArgb("#919191");

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        
    }

    public class CustomColor
    {
        public string Name { get; set; }
        public Color Value { get; set; }
        public override string ToString()
        {
            return Name;
        }
    }

    class MainViewModel : INotifyPropertyChanged
    {
        public MainViewModel()
        {
            //SelectedColor = Colors.FirstOrDefault(White);

            AddCommand = new Command<string>(s =>
            {
                var firstDigitIndex = s.IndexOfAny("0123456789".ToCharArray());
                var lastDigitIndex = s.LastIndexOfAny("0123456789".ToCharArray());

                var number = 0;
                var word = string.Empty;

                if (firstDigitIndex != -1)
                {
                    number = (int)Int64.Parse(s.Substring(firstDigitIndex));
                    word = s[0..firstDigitIndex].TrimEnd();
                }
                else
                {
                    number = 1;
                    word = s;
                }

                var task = new ItemViewModel() { Number = number, Text = word , CatColor = SelectedColor};
                Items.Add(task);
            },
                s => !string.IsNullOrEmpty(s));

            DeleteCommand = new Command<ItemViewModel>(s =>
            {
                Items.Remove(s);
            }
            );

            DoneCommand = new Command<ItemViewModel>(s =>
            {
                s.IsDone = !s.IsDone;
            });
        }
        private CustomColor _selectedColor;
        public CustomColor SelectedColor
        {
            get => _selectedColor;
            set
            {
                if (_selectedColor == value) return;
                _selectedColor = value;
                OnPropertyChanged(nameof(SelectedColor));

            }
        }

        public ObservableCollection<ItemViewModel> Items { get; set; } = new ObservableCollection<ItemViewModel>();// {new ItemViewModel(){Text = "123"}};

        public ObservableCollection<CustomColor> Colors { get; set; } = new ObservableCollection<CustomColor>
        {
            new CustomColor() { Name = "Purple", Value = Color.FromArgb("#F4B1F7") },
            new CustomColor() { Name = "Red", Value = Color.FromArgb("#F7B1B2") },
            new CustomColor() { Name = "Blue", Value = Color.FromArgb("#B1BEF7") },
            new CustomColor() { Name = "Yellow", Value = Color.FromArgb("#F7F4B1") },
            new CustomColor() { Name = "White", Value = Color.FromArgb("#FFFFFF") },
            new CustomColor() { Name = "Green", Value = Color.FromArgb("#B1F7B3") }
        };



        public ICommand AddCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand DoneCommand { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }


}


