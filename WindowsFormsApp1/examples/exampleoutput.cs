using System;
using DataObjects;

namespace DtoParser
{
    public class IncomeEditDTO2 : ModelBase<IncomeEditDTO2>
    {

        public IncomeEditDTO2()
        {
            this.Validator = e =>
            {
                if (string.IsNullOrEmpty(this.Description.Value))
                {
                    this.Description.Errors.Add("Description is NULL or Empty");
                }

                if (this.BudgetNet.Value <= 0.0)
                {
                    this.BudgetNet.Errors.Add("Budget should be greater than 0.0");
                }

                if (this.StartDate.Value > DateTime.Today.AddYears(1))
                {
                    this.StartDate.Errors.Add("Start Date should be before 2018");
                }            

                if (this.SODate.Value > DateTime.Today.AddYears(1))
                {
                    this.SODate.Errors.Add("SO Date should be before 2018");
                }


            };

            IsNew = true;
            IsDirty = true;
            IsValid = false;

        }

        public Property<int> IDFIELD { get; set; } = new Property<int>(){Value = 0,Original = 0};

        public Property<bool> IsProject { get; set; } = new Property<bool>() { Value = false, Original = false };

        public Property<string> Description { get; set; }

        public Property<DateTime?> StartDate { get; set; } = new Property<DateTime?>() { Value = DateTime.Today, Original = DateTime.Today };

        public Property<double> BudgetNet { get; set; }

        public Property<DateTime?> SODate { get; set; }
      



    }
}