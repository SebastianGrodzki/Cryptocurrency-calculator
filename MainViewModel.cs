using Calculator.WpfApp.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Calculator.WpfApp.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private string _screenVal;
        private List<string> _availableOperations = new
            List<string> {  "+", "-", "/", "*" };
        private DataTable _dataTable = new DataTable();
        private bool _isLastSignAnOperation;

        public MainViewModel()
        {
            ScreenVal = "0";
            AddNumberCommand = new RelayCommand(AddNumber);
            AddOperationCommand = new RelayCommand(AddOperation, CanAddOperation);
            ClearScreenCommand = new RelayCommand(ClearScreen);
            GetResultCommand = new RelayCommand(GetResult, CanGetResult);
        }

        private bool CanGetResult(object obj) =>! _isLastSignAnOperation;

        private bool CanAddOperation(object obj) => !_isLastSignAnOperation;

        private void GetResult(object obj)
        {
            var result = Math.Round(Convert.ToDouble (_dataTable.Compute(ScreenVal.Replace(",", "."), "")), 2);

            ScreenVal = result.ToString();
        }

        private void ClearScreen(object obj)
        {
            ScreenVal = "0";

            _isLastSignAnOperation = false;
        }

        private void AddOperation(object obj)
        {
            var operation = obj as string;

            ScreenVal += operation;

            _isLastSignAnOperation = true;
        }

        private void AddNumber(object obj)
        {
            var number = obj as string;

            if (ScreenVal == "0" && number != ",") //jaki zostal przekazany parametr do tej metody
                ScreenVal = string.Empty;
            else if (number == "," && _availableOperations.Contains(ScreenVal.Substring
                (ScreenVal.Length - 1)))          //czy na ekranie wyswietlane zero i liczba ktora zostala przekazana rozna od przecinka,
                number = "0,";                                  //jesli tak screenval empty)
        
            ScreenVal += number;

            _isLastSignAnOperation = false;
        
        }

        public ICommand AddNumberCommand { get; set; }

        public ICommand AddOperationCommand { get; set; }

        public ICommand ClearScreenCommand { get; set; }
        
        public ICommand GetResultCommand { get; set; }

        public string ScreenVal
        {
            get { return _screenVal; }
            set
            {
                _screenVal = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
