using System;

namespace DtoParser
{
    public class IncomeEditDTOSource2
    {
        public int ID { get; set; }

        public DateTime StartDate { get; set; }
      
        public bool IsProject { get; set; }

        public string Description { get; set; }
        
        public double BudgetNet { get; set; }
                
        public DateTime? SODate { get; set; }
                
    }
}