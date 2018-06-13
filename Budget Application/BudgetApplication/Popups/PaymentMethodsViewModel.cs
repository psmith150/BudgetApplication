using BudgetApplication.Base.AbstractClasses;
using BudgetApplication.Model;
using BudgetApplication.Services;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace BudgetApplication.Popups
{
    public class PaymentMethodsViewModel : PopupViewModel
    {
        #region Commands
        public ICommand AddPaymentMethodCommand { get; set; }
        public ICommand RemovePaymentMethodCommand { get; set; }
        public ICommand SaveCommand { get; private set; }
        public ICommand CancelCommand { get; private set; }
        #endregion

        public PaymentMethodsViewModel(NavigationService navigation, SessionService session, MessageViewerBase messageViewer) : base(session)
        {
            this.navigationService = navigation;
            this.PaymentMethods = session.PaymentMethods;
            this.AddPaymentMethodCommand = new RelayCommand(() => { this.ShowPopup = true; });
            this.RemovePaymentMethodCommand = new RelayCommand(() => RemovePaymentMethod());
            this.SaveCommand = new RelayCommand(() => Save());
            this.CancelCommand = new RelayCommand(() => { this.ShowPopup = false; });

            this._messageViewer = messageViewer;
        }

        public override void Initialize(object param)
        {
            this.SelectedPaymentMethodIndex = 0;
        }

        public override void Deinitialize()
        {
        }

        #region Public Properties
        private MyObservableCollection<PaymentMethod> _paymentMethods;
        public MyObservableCollection<PaymentMethod> PaymentMethods
        {
            get
            {
                return _paymentMethods;
            }
            private set
            {
                _paymentMethods = value;
            }
        }

        private int _selectedPaymentMethodIndex;
        public int SelectedPaymentMethodIndex
        {
            get
            {
                return _selectedPaymentMethodIndex;
            }
            set
            {
                this.Set(ref this._selectedPaymentMethodIndex, value);
            }
        }

        private PaymentMethod _selectedPaymentMethod;
        public PaymentMethod SelectedPaymentMethod
        {
            get
            {
                return _selectedPaymentMethod;
            }
            set
            {
                this.Set(ref this._selectedPaymentMethod, value);
            }
        }

        private bool _showPopup = false;
        public bool ShowPopup
        {
            get
            {
                return _showPopup;
            }
            set
            {
                this.Set(ref this._showPopup, value);
            }
        }

        public Array PaymentTypes
        {
            get
            {
                return Enum.GetValues(typeof(PaymentMethod.Type));
            }
        }

        private string _paymentName;
        public string PaymentName
        {
            get
            {
                return _paymentName;
            }
            set
            {
                this.Set(ref this._paymentName, value);
            }
        }

        /// <summary>
        /// The chosen payment type.
        /// </summary>
        private PaymentMethod.Type _paymentType;
        public PaymentMethod.Type PaymentType
        {
            get
            {
                return _paymentType;
            }
            set
            {
                this.Set(ref this._paymentType, value);
            }
        }
        #endregion

        #region Private Fields
        private readonly NavigationService navigationService;
        MessageViewerBase _messageViewer;
        #endregion

        #region Private Methods
        /// <summary>
        /// Removes an existing payment method from the collection
        /// </summary>
        /// <param name="paymentMethod">The payment method to remove</param>
        private void RemovePaymentMethod()
        {
            try
            {
                this.PaymentMethods.Remove(this.SelectedPaymentMethod);
                this.SelectedPaymentMethodIndex = this.SelectedPaymentMethodIndex > 0 ? this.SelectedPaymentMethodIndex - 1 : 0;
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Could not find specified payment method " + this.SelectedPaymentMethod.Name, ex);
            }
        }

        private async void Save()
        {
            PaymentMethod payment;
            switch (this.PaymentType)   //Determines which type of payment to produce.
            {
                case PaymentMethod.Type.CreditCard:
                    payment = new CreditCard(this.PaymentName);
                    break;
                case PaymentMethod.Type.CheckingAccount:
                    payment = new CheckingAccount(this.PaymentName);
                    break;
                default:
                    payment = null;
                    break;
            }
            PaymentMethod checkMethod = this.PaymentMethods.FirstOrDefault(x => x.Name.Equals(payment.Name));
            if (checkMethod != null)
            {
                await this._messageViewer.DisplayMessage("Payment method of the same name already exists; please choose another.", "Duplicate payment method");
            }
            else
                this.PaymentMethods.Add(payment);
            this.ShowPopup = false;
        }
        #endregion
    }
}
