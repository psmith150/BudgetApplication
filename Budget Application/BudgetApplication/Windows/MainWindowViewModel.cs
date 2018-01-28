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
using BudgetApplication.Popups;
using System.Collections.Specialized;
using System.Collections.ObjectModel;

namespace BudgetApplication.Windows
{
    public class MainWindowViewModel : BaseViewModel
    {
        #region Commands
        public ICommand NavigateToScreenCommand { get; private set; }
        public ICommand ToggleEventLogVisibiliyCommand { get; private set; }
        public ICommand SaveDataCommand { get; private set; }
        public ICommand SaveDataAsCommand { get; private set; }
        public ICommand LoadDataCommand { get; private set; }
        public ICommand NewFileCommand { get; private set; }
        public ICommand OpenGroupsAndCategoriesCommand { get; private set; }
        public ICommand OpenPaymentMethodsCommand { get; private set; }
        public ICommand OpenSettingsCommand { get; private set; }
        public ICommand ChangeYearCommand { get; private set; }
        public ICommand OpenRecentFileCommand { get; private set; }
        #endregion

        #region Constructor
        public MainWindowViewModel(NavigationService navigationService, SessionService session) : base(session)
        {
            this.NavigationService = navigationService;
            this.NavigateToScreenCommand = new RelayCommand<Type>((viewModel) => this.NavigateToScreen(viewModel));
            this.OpenGroupsAndCategoriesCommand = new RelayCommand(() => this.OpenGroupsAndCategories());
            this.OpenPaymentMethodsCommand = new RelayCommand(() => this.OpenPaymentMethods());
            this.ToggleEventLogVisibiliyCommand = new RelayCommand(this.ShowDebugWindow);
            this.SaveDataCommand = new RelayCommand(() => this.SaveData());
            this.SaveDataAsCommand = new RelayCommand(() => this.SaveAs());
            this.LoadDataCommand = new RelayCommand(() => this.LoadData());
            this.NewFileCommand = new RelayCommand(() => this.NewData());
            this.OpenSettingsCommand = new RelayCommand(() => this.OpenSettings());
            this.ChangeYearCommand = new RelayCommand(() => this.ChangeYear());
            this.OpenRecentFileCommand = new RelayCommand<string>((s) => this.OpenRecentFile(s));

            //Load list of recent files
            this.LastFiles = new ObservableCollection<string>();
            foreach (string file in Properties.Settings.Default.LastFiles)
            {
                this.LastFiles.Add(file);
            }
            this.LastFiles.CollectionChanged += ((o, e) => SaveLastFiles());
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
        private ObservableCollection<string> _lastFiles;
        public ObservableCollection<string> LastFiles
        {
            get
            {
                return _lastFiles;
            }
            private set
            {
                this.Set(ref this._lastFiles, value);
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

        private async void OpenGroupsAndCategories()
        {
            await this.NavigationService.OpenPopup<GroupAndCategoriesViewModel>();
        }

        private async void OpenPaymentMethods()
        {
            await this.NavigationService.OpenPopup<PaymentMethodsViewModel>();
        }

        private async void OpenSettings()
        {
            await this.NavigationService.OpenPopup<SettingsViewModel>();
        }

        private async void ChangeYear()
        {
            await this.NavigationService.OpenPopup<ChangeYearViewModel>();
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
        /// Saves the current data in the current filepath
        /// </summary>
        private async void SaveData()
        {

            if (string.IsNullOrEmpty(this.currentFilePath) == false)
            {
                UpdateLastFilesList(this.currentFilePath);
                await this.Session.SaveDataToFile(this.currentFilePath);
            }
        }

        private async void SaveAs()
        {
            try
            {
                var fileSearch = new SaveFileDialog();
                fileSearch.InitialDirectory = Properties.Settings.Default.DefaultDirectory;
                fileSearch.Filter = "XML File (*.xml) | *.xml";
                fileSearch.FilterIndex = 2;
                fileSearch.RestoreDirectory = true;
                fileSearch.ShowDialog();
                this.currentFilePath = fileSearch.FileName.ToString();

                if (string.IsNullOrEmpty(this.currentFilePath) == false)
                {
                    UpdateLastFilesList(this.currentFilePath);
                    await this.Session.SaveDataToFile(this.currentFilePath);
                }
            }
            catch (IOException ex)
            {
                Debug.WriteLine($"Error loading from file {this.currentFilePath}\n" + ex.Message);
            }
        }

        /// <summary>
        /// Loads data from the current filepath
        /// </summary>
        private async void LoadData()
        {
            try
            {
                var fileSearch = new OpenFileDialog();
                fileSearch.InitialDirectory = Properties.Settings.Default.DefaultDirectory;
                fileSearch.Filter = "XML File (*.xml) | *.xml";
                fileSearch.FilterIndex = 2;
                fileSearch.RestoreDirectory = true;
                fileSearch.ShowDialog();
                this.currentFilePath = fileSearch.FileName.ToString();

                if (string.IsNullOrEmpty(this.currentFilePath) == false)
                {
                    UpdateLastFilesList(this.currentFilePath);
                    await this.Session.LoadDataFromFile(this.currentFilePath);
                }
            }
            catch (IOException ex)
            {
                Debug.WriteLine($"Error loading from file {this.currentFilePath}\n" + ex.Message);
            }
        }

        private async void NewData()
        {
            try
            {
                var fileSearch = new SaveFileDialog();
                fileSearch.InitialDirectory = Properties.Settings.Default.DefaultDirectory;
                fileSearch.Filter = "XML File (*.xml) | *.xml";
                fileSearch.FilterIndex = 2;
                fileSearch.RestoreDirectory = true;
                fileSearch.ShowDialog();
                this.currentFilePath = fileSearch.FileName.ToString();

                if (string.IsNullOrEmpty(this.currentFilePath) == false)
                {
                    UpdateLastFilesList(this.currentFilePath);
                    await this.Session.CreateNewFile(this.currentFilePath);
                }
            }
            catch (IOException ex)
            {
                Debug.WriteLine($"Error loading from file {this.currentFilePath}\n" + ex.Message);
            }
        }

        private void SaveLastFiles()
        {
            Properties.Settings.Default.LastFiles = new StringCollection();
            foreach (string file in this.LastFiles)
            {
                Properties.Settings.Default.LastFiles.Add(file);
            }
            Properties.Settings.Default.Save();
        }

        private async void OpenRecentFile(string filePath)
        {
            Debug.WriteLine("Opening file " + filePath);
            try
            {
                if (File.Exists(filePath))
                {
                    this.currentFilePath = filePath;
                    UpdateLastFilesList(filePath);
                    await this.Session.LoadDataFromFile(this.currentFilePath);
                }
                else
                {
                    MessageBox.Show("File no longer exists; removing from list.", "File not found", MessageBoxButton.OK);
                    for (int i=0; i< this.LastFiles.Count; i++)
                    {
                        if (this.LastFiles[i].Equals(filePath))
                            this.LastFiles.RemoveAt(i);
                    }
                }
            }
            catch (IOException ex)
            {
                Debug.WriteLine($"Error loading from file {this.currentFilePath}\n" + ex.Message);
            }
        }

        private void UpdateLastFilesList(string file)
        {
            bool found = false;
            for (int i=0; i< this.LastFiles.Count; i++)
            {
                if (this.LastFiles[i].Equals(file))
                {
                    this.LastFiles.Move(i, 0);
                    found = true;
                    break;
                }
            }
            if (!found)
            {
                this.LastFiles.Insert(0,file);
            }
            if (this.LastFiles.Count > 10)
            {
                this.LastFiles.RemoveAt(10);
            }
        }
        #endregion
        #endregion
    }
}