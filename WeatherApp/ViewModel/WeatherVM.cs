using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherApp.Model;
using WeatherApp.ViewModel.Command;
using WeatherApp.ViewModel.Helpers;

namespace WeatherApp.ViewModel
{
    // WeatherWindow.xaml ile çalışacak.
    // INotifyPropertyChanged model değiştiğinde görünümün de değişmesini sağlar. Tersi de geçerlidir.
    public class WeatherVM : INotifyPropertyChanged
    {
        private string _query;
        public string Query {
            get { return _query; } 
            set 
            { 

                _query = value;
                OnPropertyChanged("Query");

            }
        }

        public ObservableCollection<City> Cities { get; set; }
        private CurrentConditions _currentConditions;

        public CurrentConditions CurrentConditions
        {
            get { return _currentConditions; }
            set 
            { 
                _currentConditions = value;
                OnPropertyChanged("CurrentConditions");
            }
        }

        private City _selectedCity;

        public City SelectedCity
        {
            get { return _selectedCity; }
            set
            {
                _selectedCity = value;
                OnPropertyChanged("SelectedCity");
            }
        }

        public SearchCommand SearchCommand {  get; set; }

        public WeatherVM()
        {
            if(DesignerProperties.GetIsInDesignMode(new System.Windows.DependencyObject()))
            {
                SelectedCity = new City
                {
                    LocalizedName = "Antalya"
                };
                CurrentConditions = new CurrentConditions
                {
                    WeatherText = "Yagisli",
                    Temperature = new Temperature
                    {
                        Metric = new Units
                        {
                            Value = 17
                        }
                    }
                };
            }
            SearchCommand = new SearchCommand(this);
            Cities = new ObservableCollection<City>();
            
        }

        public async void MakeQuery()
        {
            var cities = await AccuWeatherHelper.GetCities(Query);

            Cities.Clear();
            foreach(var city in cities) 
            {
                Cities.Add(city);
            }
            
        }
        
        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

 

}
