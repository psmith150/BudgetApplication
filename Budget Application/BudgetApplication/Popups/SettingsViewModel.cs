using BudgetApplication.Services;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Forms;
using MessageBox = System.Windows.MessageBox;

namespace BudgetApplication.Popups
{
    public class SettingsViewModel : PopupViewModel
    {
        #region Commands
        public ICommand SaveCommand { get; private set; }
        public ICommand ExitPopupCommand {get; private set;}
        public ICommand SelectDirectoryCommand { get; private set; }
        #endregion

        public SettingsViewModel(SessionService session) : base(session)
        {
            this.SaveCommand = new RelayCommand(() => this.Save());
            this.ExitPopupCommand = new RelayCommand(() => this.Exit());
            this.SelectDirectoryCommand = new RelayCommand(() => this.SelectDirectory());
        }

        public override void Initialize(object param)
        {
            this.DefaultDirectory = Properties.Settings.Default.DefaultDirectory;
            this.savingNeeded = false;
        }

        public override void Deinitialize()
        {
        }

        #region Public Properties
        private string _defaultDirectory;
        public string DefaultDirectory
        {
            get
            {
                return this._defaultDirectory;
            }
            set
            {
                this.Set(ref this._defaultDirectory, value);
                this.savingNeeded = true;
            }
        }
        #endregion

        #region Private Fields
        private bool savingNeeded = false;
        #endregion

        #region Private Methods
        private void SelectDirectory()
        {
            FolderBrowserDialog dirDialog = new FolderBrowserDialog();
            dirDialog.RootFolder = Environment.SpecialFolder.MyDocuments;
            DialogResult result = dirDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                this.DefaultDirectory = dirDialog.SelectedPath;
            }
        }
        private void Exit()
        {
            if (this.savingNeeded)
            {
                MessageBoxResult result = MessageBox.Show("Settings have not been saved, are you sure you want to exit?", "Discard Changes?", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                    this.ClosePopup(null);
            }
            else
            {
                this.ClosePopup(null);
            }
        }

        private void Save()
        {
            Properties.Settings.Default.DefaultDirectory = this.DefaultDirectory;
            Properties.Settings.Default.Save();
            this.ClosePopup(null);
        }
        #endregion
    }
}
