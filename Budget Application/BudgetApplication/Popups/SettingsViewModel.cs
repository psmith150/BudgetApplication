using BudgetApplication.Services;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BudgetApplication.Popups
{
    public class SettingsViewModel : PopupViewModel
    {
        private int _SampleSetting;
        public int SampleSetting
        {
            get
            {
                return this._SampleSetting;
            }
            set
            {
                this.Set(ref this._SampleSetting, value);
            }
        }

        public ICommand SaveCommand { get; private set; }

        public SettingsViewModel(SessionService session) : base(session)
        {
            this.SaveCommand = new RelayCommand(() => this.Save());
        }

        public override void Initialize(object param)
        {
            this.SampleSetting = Properties.Settings.Default.SampleSetting;
        }

        public override void Deinitialize()
        {
        }

        private void Save()
        {
            Properties.Settings.Default.SampleSetting = this.SampleSetting;
            Properties.Settings.Default.Save();
            this.ClosePopup(null);
        }
    }
}
