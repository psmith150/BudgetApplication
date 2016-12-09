using System;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BudgetApplication.Model;
using System.ComponentModel;
using System.Windows.Data;
using System.Collections.ObjectModel;
using System.Xml;

namespace BudgetApplication
{
    public class MainViewModel
    {
        private ObservableCollection<Transaction> _transactions;
        private ObservableCollection<Category> _categories;
        private ObservableCollection<Group> _groups;
        private ObservableCollection<PaymentMethod> _paymentMethods;
        public MainViewModel()
        {
            LoadData();
            _canExecute = true;
        }

        #region Common to all tabs

        public ObservableCollection<Group> Groups
        {
            get
            {
                return _groups;
            }
            private set
            {

            }
        }

        public bool AddGroup(Group group)
        {
            foreach (Group currGroup in _groups)
            {
                if (currGroup.Name.Equals(group))
                {
                    return false;
                }
            }
            if (String.IsNullOrEmpty(group.Name))
            {
                return false;
            }
            _groups.Add(group);
            return true;
        }

        public ObservableCollection<Category> Categories
        {
            get
            {
                return _categories;
            }
            private set
            {

            }
        }

        /// <summary>
        /// Adds a category with the specified name to the category list
        /// </summary>
        /// <param name="categoryName"></param>
        /// <returns></returns>
        public bool AddCategory(Category category)
        {
            foreach (Category currCategory in _categories)
            {
                if (currCategory.Name.Equals(category))
                {
                    return false;
                }
            }
            if (String.IsNullOrEmpty(category.Name))
            {
                return false;
            }
            _categories.Add(category);
            return true;
        }

        public bool IsValidCategory(string categoryName)
        {
            foreach (Category currCategory in _categories)
            {
                if (currCategory.Name.Equals(categoryName))
                {
                    return true;
                }
            }
            return false;
        }

        public ObservableCollection<PaymentMethod> PaymentMethods
        {
            get
            {
                return _paymentMethods;
            }
            private set
            {

            }
        }

        public bool AddPaymentMethod(PaymentMethod paymentMethod)
        {
            _paymentMethods.Add(paymentMethod);
            return true;
        }
        #endregion

        #region Budget tab
        #endregion

        #region Spending tab
        #endregion

        #region Comparison tab
        #endregion

        #region Transaction tab

        public ObservableCollection<Transaction> Transactions
        {
            get
            {
                return _transactions;
            }
            private set
            {

            }
        }

        public bool AddTransaction(Transaction transaction)
        {
            //TODO: verify valid transaction
            _transactions.Add(transaction);
            return true;
        }
        #endregion

        private ICommand _saveData_ClickCommand;
        private ICommand _loadData_ClickCommand;
        public ICommand SaveData_ClickCommand
        {
            get
            {
                return _saveData_ClickCommand ?? (_saveData_ClickCommand = new CommandHandler(() => SaveData(), _canExecute));
            }
        }

        public ICommand LoadData_ClickCommand
        {
            get
            {
                return _loadData_ClickCommand ?? (_loadData_ClickCommand = new CommandHandler(() => LoadData(), _canExecute));
            }
        }
        private bool _canExecute;

        #region Saving and Opening files
        public void SaveData()
        {
            using (FileStream file = new FileStream("data.xml", FileMode.Create))
            {
                using (StreamWriter stream = new StreamWriter(file))
                {
                    XmlWriterSettings settings = new XmlWriterSettings();
                    settings.Indent = true;
                    using (XmlWriter writer = XmlWriter.Create(stream, settings))
                    {
                        writer.WriteStartDocument();
                        writer.WriteStartElement("Data");

                        writer.WriteStartElement("Groups");
                        foreach (Group group in _groups)
                        {
                            writer.WriteStartElement("Group");
                            writer.WriteElementString("Name", group.Name);
                            writer.WriteElementString("IsIncome", group.IsIncome.ToString());
                            writer.WriteEndElement();
                        }
                        writer.WriteEndElement();

                        writer.WriteStartElement("Categories");
                        foreach (Category category in _categories)
                        {
                            //MessageBox.Show(String.Format("{0}", category.Name));
                            writer.WriteStartElement("Category");
                            writer.WriteElementString("Name", category.Name);
                            writer.WriteElementString("Group", category.Group.Name);
                            writer.WriteEndElement();
                        }
                        writer.WriteEndElement();


                        writer.WriteStartElement("PaymentMethods");
                        foreach (PaymentMethod payment in _paymentMethods)
                        {
                            //MessageBox.Show(String.Format("{0}", category.Name));
                            if(payment.PaymentType() == PaymentMethod.Type.CreditCard)
                            {
                                CreditCard card = payment as CreditCard;
                                writer.WriteStartElement("CreditCard");
                                writer.WriteElementString("Name", card.Name);
                                writer.WriteElementString("CreditLimit", card.CreditLimit.ToString());
                            }
                            else if (payment.PaymentType() == PaymentMethod.Type.CheckingAccount)
                            {
                                CheckingAccount account = payment as CheckingAccount;
                                writer.WriteStartElement("CheckingAccount");
                                writer.WriteElementString("Name", account.Name);
                            }
                            else
                            {
                                throw new XmlException("Invalid payment method: " + payment.Name);
                            }
                            writer.WriteEndElement();
                        }
                        writer.WriteEndElement();


                        writer.WriteStartElement("Transactions");
                        foreach (Transaction transaction in _transactions)
                        {
                            //MessageBox.Show(String.Format("{0}", category.Name));
                            writer.WriteStartElement("Transaction");
                            writer.WriteElementString("Date", transaction.Date.ToString());
                            writer.WriteElementString("Item", transaction.Item);
                            writer.WriteElementString("Payee", transaction.Payee);
                            writer.WriteElementString("Amount", transaction.Amount.ToString());
                            writer.WriteElementString("Comment", transaction.Comment);
                            writer.WriteElementString("Category", transaction.Category.Name);
                            writer.WriteElementString("PaymentMethod", transaction.PaymentMethod.Name);
                            writer.WriteEndElement();
                        }
                        writer.WriteEndElement();

                        writer.WriteEndElement();
                        writer.WriteEndDocument();
                    }
                }

            }
        }

        public void LoadData()
        {
            _groups = new ObservableCollection<Group>();
            _categories = new ObservableCollection<Category>();
            _transactions = new ObservableCollection<Transaction>();
            _paymentMethods = new ObservableCollection<PaymentMethod>();
            //MessageBox.Show("Gets here 2");
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load("data.xml");

                foreach(XmlNode node in doc.DocumentElement.ChildNodes)
                {
                    String nodeName = node.Name;

                    if (nodeName.Equals("Groups"))
                    {
                        foreach(XmlNode groupNode in node.ChildNodes)
                        {
                            String name = null;
                            bool isIncome = false;
                            foreach(XmlNode property in groupNode.ChildNodes)
                            {
                                if (property.Name.Equals("Name"))
                                {
                                    name = property.InnerText;
                                }
                                else if (property.Name.Equals("IsIncome"))
                                {
                                    isIncome = Boolean.Parse(property.InnerText);
                                }
                                else
                                {
                                    throw new XmlException("Unknown group property: " + property.Name);
                                }
                            }
                            _groups.Add(new Group(isIncome, name));
                        }
                    }
                    else if (nodeName.Equals("Categories"))
                    {
                        foreach (XmlNode categoryNode in node.ChildNodes)
                        {
                            String name = null;
                            String groupName = null;
                            Group group = null;
                            foreach (XmlNode property in categoryNode.ChildNodes)
                            {
                                if (property.Name.Equals("Name"))
                                {
                                    name = property.InnerText;
                                }
                                else if (property.Name.Equals("Group"))
                                {
                                    groupName = property.InnerText;
                                }
                                else
                                {
                                    throw new XmlException("Unknown category property: " + property.Name);
                                }
                            }
                            try
                            {
                                group = _groups.Single(x => x.Name.Equals(groupName));
                            }
                            catch (Exception ex)
                            {
                                throw new XmlException("Invalid group name: " + groupName);
                            }
                            _categories.Add(new Category(group, name));
                        }
                    }
                    else if (nodeName.Equals("PaymentMethods"))
                    {
                        foreach (XmlNode paymentNode in node.ChildNodes)
                        {
                            String name = null;
                            PaymentMethod payment = null;
                            if (paymentNode.Name.Equals("CreditCard"))
                            {
                                foreach (XmlNode property in paymentNode.ChildNodes)
                                {
                                    decimal creditLimit = 300;
                                    if (property.Name.Equals("Name"))
                                    {
                                        name = property.InnerText;
                                    }
                                    else if (property.Name.Equals("CreditLimit"))
                                    {
                                        creditLimit = Decimal.Parse(property.InnerText);
                                    }
                                    else
                                    {
                                        throw new XmlException("Unknown credit card property: " + property.Name);
                                    }
                                    payment = new CreditCard(name, creditLimit);
                                }
                            }
                            else if (paymentNode.Name.Equals("CheckingAccount"))
                            {
                                foreach (XmlNode property in paymentNode.ChildNodes)
                                {
                                    if (property.Name.Equals("Name"))
                                    {
                                        name = property.InnerText;
                                    }
                                    else
                                    {
                                        throw new XmlException("Unknown checking account property: " + property.Name);
                                    }
                                    payment = new CheckingAccount(name);
                                }
                            }
                            else
                            {
                                throw new XmlException("Unknown payment type: " + paymentNode.Name);
                            }
                            _paymentMethods.Add(payment);
                        }
                    }
                    else if (nodeName.Equals("Transactions"))
                    {
                        foreach (XmlNode transactionNode in node.ChildNodes)
                        {
                            DateTime date = DateTime.Today;
                            String item=null;
                            String payee=null;
                            Decimal amount=0;
                            String comment=null;
                            String categoryName=null;
                            Category category=null;
                            String paymentName=null;
                            PaymentMethod payment=null;

                            foreach (XmlNode property in transactionNode.ChildNodes)
                            {
                                if (property.Name.Equals("Date"))
                                {
                                    date = DateTime.Parse(property.InnerText);
                                }
                                else if (property.Name.Equals("Item"))
                                {
                                    item = property.InnerText;
                                }
                                else if (property.Name.Equals("Payee"))
                                {
                                    payee = property.InnerText;
                                }
                                else if (property.Name.Equals("Amount"))
                                {
                                    amount = Decimal.Parse(property.InnerText);
                                }
                                else if (property.Name.Equals("Comment"))
                                {
                                    comment = property.InnerText;
                                }
                                else if (property.Name.Equals("Category"))
                                {
                                    categoryName = property.InnerText;
                                    try
                                    {
                                        category = _categories.Single(x => x.Name.Equals(categoryName));
                                    }
                                    catch (Exception ex)
                                    {
                                        throw new XmlException("Invalid category name: " + categoryName, ex);
                                    }
                                }
                                else if (property.Name.Equals("PaymentMethod"))
                                {
                                    paymentName = property.InnerText;
                                    try
                                    {
                                        payment = _paymentMethods.Single(x => x.Name.Equals(paymentName));
                                    }
                                    catch (Exception ex)
                                    {
                                        throw new XmlException("Invalid payment method name: " + paymentName, ex);
                                    }
                                }
                                else
                                {
                                    throw new XmlException("Unknown transaction property: " + property.Name);
                                }
                            }
                            Transaction transaction = new Transaction();
                            transaction.Date = date;
                            transaction.Item = item;
                            transaction.Payee = payee;
                            transaction.Amount = amount;
                            transaction.Comment = comment;
                            transaction.Category = category;
                            transaction.PaymentMethod = payment;
                            _transactions.Add(transaction);
                        }
                    }
                    else
                    {
                        throw new XmlException("Unexpected node: " + nodeName);
                    }
                }
            }
            catch (FileNotFoundException ex)
            {
                
            }
        }
        #endregion
    }

    public class CommandHandler : ICommand
    {
        private Action _action;
        private bool _canExecute;
        public CommandHandler(Action action, bool canExecute)
        {
            _action = action;
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            _action();
        }
    }
}
