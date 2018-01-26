using System;
using DataObjects;

namespace DtoParser
{
    public class IncomeEditDTOSource2BO : ModelBase<IncomeEditDTOSource2BO>
    {

        public IncomeEditDTOSource2BO()
        {

            this.Validator = e =>
            {

            };
            IsNew = true;
            IsDirty = true;
            IsValid = false;
        }

        public Property<DateTime?> SODate { get; set; } = new Property<DateTime?>() { Value = DateTime.Today, Original = DateTime.Today };

        public Property<int> ID { get; set; } = new Property<int>() { Value = 0, Original = 0 };

        public Property<DateTime> StartDate { get; set; } = new Property<DateTime>() { Value = DateTime.Today, Original = DateTime.Today };


        public Property<bool> IsProject { get; set; } = new Property<bool>() { Value = false, Original = false };

        public Property<string> Description { get; set; } = new Property<string>() { Value = string.Empty, Original = string.Empty };

        public Property<double> BudgetNet { get; set; } = new Property<double>() { Value = 0.0, Original = 0.0 };





        public void Make(IncomeEditDTOSource2 test)
        {
            this.SODate = Property<DateTime?>.Make(test.SODate);
            this.ID = Property<int>.Make(test.ID);
            this.StartDate = Property<DateTime>.Make(test.StartDate);
            this.IsProject = Property<bool>.Make(test.IsProject);
            this.Description = Property<string>.Make(test.Description);
            this.BudgetNet = Property<double>.Make(test.BudgetNet);
        }//endofmake
    }
}