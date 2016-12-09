using System;

namespace BudgetApplication.Model
{
    public class Category
    {
        private String _name;
        private Group _group;

        public Category(Group group, String name = "New Category")
        {
            _group = group;
            _name = String.Copy(name);
            if (group == null)
            {
                throw new ArgumentException("Null group");
            }
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
            get
            {
                return _group;
            }
        }

        public bool IsIncome()
        {
            return _group.IsIncome;
        }

        public override string ToString()
        {
            return Name;
        }
    }

    public class Group
    {
        private String _name;
        private bool _isIncome;

        public Group(bool isIncome = false, String name = "New Group")
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
            get
            {
                return _isIncome;
            }
            set
            {
                _isIncome = value;
            }
        }

        public override string ToString()
        {
            return Name;
        }
    }
}