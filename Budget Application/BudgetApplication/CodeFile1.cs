using System;

namespace BudgetApplication.Model
{
    public class Category
    {
        private String _name;
        private Group _group;

        public Category(Group group, String name = "Default Category")
        {
            _group = group;
            _name = String.Copy(name);
        }

        public String Name
        {
            get
            {
                return String.Copy(_name);
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _name = String.Copy(value);
                }
            }
        }

        public Group Group
        {
            get; set;
        }
    }

    public class Group
    {
        private String _name;
        private bool _isIncome;

        public Group(bool isIncome = false, String name = "Default Group")
        {
            _isIncome = isIncome;
            _name = String.Copy(name);
        }

        public String Name
        {
            get
            {
                return String.Copy(_name);
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _name = String.Copy(value);
                }
            }
        }

        public bool IsIncome
        {
            get; set;
        }
    }
}