using BudgetApplication.Services;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using BudgetApplication.Base.AbstractClasses;
using BudgetApplication.Base.EventArgs;
using BudgetApplication.Base.Enums;
using System.Windows.Forms;

namespace BudgetApplication.Popups
{
    public class SettingsViewModel : PopupViewModel
    {
        #region Commands
        public ICommand SaveCommand { get; private set; }
        public ICommand ExitPopupCommand {get; private set;}
        public ICommand SelectDirectoryCommand { get; private set; }
        #endregion

        public SettingsViewModel(SessionService session, MessageViewerBase messageViewer) : base(session)
        {
            this.SaveCommand = new RelayCommand(() => this.Save());
            this.ExitPopupCommand = new RelayCommand(() => this.Exit());
            this.SelectDirectoryCommand = new RelayCommand(() => this.SelectDirectory());

            this._messageViewer = messageViewer;
        }

        public override void Initialize(object param)
        {
            this.DefaultDirectory = Properties.Settings.Default.DefaultDirectory;
            this.ApplicationTheme = Properties.Settings.Default.Theme;
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
        private string _ApplicationTheme;
        public string ApplicationTheme
        {
            get
            {
                return this._ApplicationTheme;
            }
            set
            {
                this.Set(ref this._ApplicationTheme, value);
                this.savingNeeded = true;
                this.ChangeSkin();
            }
        }
        public string[] Themes
        {
            get
            {
                string[] themes = { "Light", "Dark" };
                return themes;
            }
        }
        #endregion

        #region Private Fields
        private bool savingNeeded = false;
        private MessageViewerBase _messageViewer;
        #endregion

        #region Private Methods
        private void SelectDirectory()
        {
            FolderBrowserDialog dirDialog = new FolderBrowserDialog();
            dirDialog.RootFolder = Environment.SpecialFolder.MyComputer;
            dirDialog.ShowNewFolderButton = true;
            dirDialog.Description = "Select a folder";
            DialogResult result = dirDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                this.DefaultDirectory = dirDialog.SelectedPath;
            }
        }
        private void ChangeSkin()
        {
            switch (this.ApplicationTheme)
            {
                case "Light":
                    break;
                case "Dark":
                    break;
                default:
                    break;
            }
        }
        private async void Exit()
        {
            if (this.savingNeeded)
            {
                MessageViewerEventArgs result = await this._messageViewer.DisplayMessage("Settings have not been saved, are you sure you want to exit?", "Discard Changes?", MessageViewerButton.OkCancel);
                if (result.Result == MessageViewerResult.Ok)
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
