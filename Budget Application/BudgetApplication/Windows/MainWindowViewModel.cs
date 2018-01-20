using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using BudgetApplication.Screens;
using BudgetApplication.Services;
using GalaSoft.MvvmLight.CommandWpf;
using Microsoft.Win32;
using System.IO;
using System.Xml.Serialization;
using BudgetApplication.Model;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace BudgetApplication.Windows
{
    public class MainWindowViewModel : BaseViewModel
    {
        #region Commands
        public ICommand NavigateToScreenCommand { get; set; }
        public ICommand ToggleEventLogVisibiliyCommand { get; set; }
        public ICommand SaveDataCommand { get; set; }
        public ICommand LoadDataCommand { get; set; }
        #endregion

        #region Constructor
        public MainWindowViewModel(NavigationService navigationService, SessionService session) : base(session)
        {
            this.NavigationService = navigationService;

            this.NavigateToScreenCommand = new RelayCommand<Type>((viewModel) => this.NavigateToScreen(viewModel));
            this.ToggleEventLogVisibiliyCommand = new RelayCommand(this.ShowDebugWindow);
            this.SaveDataCommand = new RelayCommand(() => SaveData());
            this.LoadDataCommand = new RelayCommand(() => LoadData());

            // Set the starting page
            this.NavigationService.NavigateTo<BudgetViewModel>();
        }
        #endregion

        #region Public Properties
        private NavigationService _NavigationService;
        public NavigationService NavigationService
        {
            get
            {
                return this._NavigationService;
            }
            private set
            {
                this.Set(ref this._NavigationService, value);
            }
        }
        #endregion

        #region Private Fields
        private string currentFilePath;
        #endregion

        #region Private Methods
        private void NavigateToScreen(object param)
        {
            var viewModelType = param as Type;
            if (viewModelType != null)
            {
                this.NavigationService.NavigateTo(viewModelType);
            }
        }

        private void ShowDebugWindow()
        {
            //if (Application.Current.Windows.OfType<DebugWindow>().Any())
            //{
            //    Application.Current.Windows.OfType<DebugWindow>().First().Activate();
            //}
            //else
            //{
            //    new DebugWindow().Show();
            //}
        }

        #region File Handling
        /// <summary>
        /// Creates a new data file with no data in the current filepath
        /// </summary>
        private void InitNewFile()
        {
            //Debug.WriteLine("Creating new file at " + completeFilePath);
            //using (FileStream file = new FileStream(completeFilePath, FileMode.Create))
            //{
            //    using (StreamWriter stream = new StreamWriter(file))
            //    {
            //        //Create the DataWrapper object and add the apprpriate data
            //        XmlSerializer dataSerializer = new XmlSerializer(typeof(DataWrapper));
            //        DataWrapper data = new DataWrapper();
            //        data.Groups = new MyObservableCollection<Group>();
            //        data.PaymentMethods = new MyObservableCollection<PaymentMethod>();
            //        data.Transactions = new MyObservableCollection<Transaction>();
            //        List<decimal[]> budgetData = new List<decimal[]>(); //Easiest to just store values in order
            //        foreach (MoneyGridRow row in _budgetValues)
            //        {
            //            budgetData.Add(new decimal[12]);
            //        }
            //        data.BudgetValues = budgetData;

            //        dataSerializer.Serialize(stream, data); //Saves the data using the attributes defined in each class
            //    }
            //}
        }

        /// <summary>
        /// Saves the current data in the current filepath
        /// </summary>
        private void SaveData()
        {
            try
            {
                using (FileStream file = new FileStream(this.currentFilePath, FileMode.Open))
                {
                    using (StreamWriter stream = new StreamWriter(file))
                    {
                        //Create the DataWrapper object and add the apprpriate data
                        XmlSerializer dataSerializer = new XmlSerializer(typeof(DataWrapper));
                        DataWrapper data = new DataWrapper();
                        data.Groups = this.Session.Groups;
                        data.PaymentMethods = this.Session.PaymentMethods;
                        data.Transactions = this.Session.Transactions;
                        List<decimal[]> budgetData = new List<decimal[]>(); //Easiest to just store values in order
                        foreach (MoneyGridRow row in this.Session.BudgetValues)
                        {
                            budgetData.Add(row.Values.Values);
                        }
                        data.BudgetValues = budgetData;

                        dataSerializer.Serialize(stream, data); //Saves the data using the attributes defined in each class
                    }
                }
            }
            catch (IOException ex)
            {
                Debug.WriteLine($"Error saving to file {this.currentFilePath}\n" + ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                Debug.WriteLine($"Error writing XML to file {this.currentFilePath}\n" + ex.Message);
            }
        }

        /// <summary>
        /// Loads data from the current filepath
        /// </summary>
        private void LoadData()
        {
            try
            {
                var fileSearch = new OpenFileDialog();
                fileSearch.InitialDirectory = @"C:\";
                fileSearch.Filter = "XML File (*.xml) | *.xml";
                fileSearch.FilterIndex = 2;
                fileSearch.RestoreDirectory = true;
                fileSearch.ShowDialog();
                this.currentFilePath = fileSearch.FileName.ToString();

                if (string.IsNullOrEmpty(this.currentFilePath) == false)
                {
                    this.Session.LoadDataFromFile(this.currentFilePath);
                }
            }
            catch (IOException ex)
            {
                Debug.WriteLine($"Error loading from file {this.currentFilePath}\n" + ex.Message);
            }
        }
        #endregion
        #endregion
    }
}