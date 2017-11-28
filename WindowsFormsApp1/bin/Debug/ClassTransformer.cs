using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
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
    }//endofclass



    public class ClassTransformer  
    {
        public string OldName { get; set; }
        public string NewName { get; set; }

        public string Source { get; set; }
        public List<string> SourceAsLines { get; set; } = new List<string>();

        public List<string> TargetLines { get; set; } = new List<string>();

        public List<string> MakeLines { get; set; } = new List<string>();

        public void Transform()
        {
            SourceAsLines.AddRange(Source.Split(
                new[] { Environment.NewLine },
                StringSplitOptions.None
            ));

            foreach (var l in SourceAsLines)
            {

              
                bool result = false;

                if (ClassName(l)) result = true;

                if (l.Replace(" ","") == "{")
                {
                   // TargetLines.Add("{");
                    result = true;
                }

                if (l.Replace(" ", "") == "}")
                {
                  
                    result = true;
                }

                if (DateTimeVariable(l, "string", @"Value = string.Empty, Original = string.Empty")) result = true;

                if (DateTimeVariable(l, "double", @"Value = 0.0, Original = 0.0")) result = true;

                if (DateTimeVariable(l, @"double?", @"Value = 0.0, Original = 0.0")) result = true;

                if (DateTimeVariable(l, "int", @"Value = 0, Original = 0")) result = true;

                if (DateTimeVariable(l, @"int?", @"Value = 0, Original = 0")) result = true;

                if (DateTimeVariable(l,"DateTime", @"Value = DateTime.Today,Original = DateTime.Today")) result = true;

                if (DateTimeVariable(l, @"DateTime?", @"Value = DateTime.Today,Original = DateTime.Today")) result = true;

                if (DateTimeVariable(l, "bool", @"Value = false, Original = false")) result = true;

                if (DateTimeVariable(l, @"bool?", @"Value = false, Original = false")) result = true;

                if (!result)
                    TargetLines.Add(l);
            }

            TargetLines.Add("");
            TargetLines.Add("");


            TargetLines.Add("public void Make("+ OldName +" test)");

            TargetLines.Add("{");

            foreach (var line in MakeLines)
            {
                TargetLines.Add(line);
            }

            TargetLines.Add("}//endofmake");

            TargetLines.Add("");
            TargetLines.Add("");

            TargetLines.Add("public static ExtRangeCollection<" + NewName + "> MakeCollection(List<" + OldName +"> records)");
            TargetLines.Add("{");
            TargetLines.Add("var newData = new ExtRangeCollection<" + NewName + ">();");
            TargetLines.Add("foreach (var rec in records)");
            TargetLines.Add("{");
            TargetLines.Add("var e = new " + NewName + "();");
            TargetLines.Add("e.Make(rec);");
            TargetLines.Add("newData.AddItem(e);");
            TargetLines.Add("}");
            TargetLines.Add("return newData;");
            TargetLines.Add("}//endofMakeCollection");
            TargetLines.Add("");
            TargetLines.Add("");
            TargetLines.Add("}//endofclass");

        }


        private bool ClassName(string l)
        {
            if (l.Contains("public class "))
            {
                Regex regex = new Regex(@"(?<=\bclass )(\w+)");
                Match match = regex.Match(l);

                if (match.Success)
                {                 
                    this.OldName = match.Value;
                    this.NewName = match.Value + "BO";

                    TargetLines.Add(@"public class " + NewName + " : ModelBase<" + NewName + ">");                    
                    TargetLines.Add(@"{");
                    TargetLines.Add(@"");
                    TargetLines.Add(@"public " + NewName + "()");

                    TargetLines.Add(@"{");

                    TargetLines.Add(@"");
                    TargetLines.Add(@"this.Validator = e =>");
                    TargetLines.Add(@"{");
                    TargetLines.Add(@"");
                    TargetLines.Add(@"};");

                    TargetLines.Add(@"IsNew = true;");
                    TargetLines.Add(@"IsDirty = true;");
                    TargetLines.Add(@"IsValid = false;");
                    TargetLines.Add(@"}");
                    TargetLines.Add(@"");

                    return true;
                }
            }
            return false;
        }
     
 

        private bool DateTimeVariable(string l, string searchToken, string defaultValue)
        {
            string padding = "      ";

            l = l.Replace(@"?", "questionmark");
            searchToken = searchToken.Replace(@"?", "questionmark");

            if (l.Contains("public "+ searchToken + " "))
            {
                Regex regex = new Regex(@"(?<=\bpublic "+ searchToken + @" )(\w+)");
                Match match = regex.Match(l);

                if (match.Success)
                {

                    var neLine = padding + @"public Property<" + searchToken + "> " + match.Value +
                                 " { get; set; } = new Property<" + searchToken +
                                 ">(){"+ defaultValue+"};";

                    neLine = neLine.Replace("questionmark", @"?");
                    searchToken = searchToken.Replace("questionmark", @"?");


                    TargetLines.Add(neLine);

                    //this.ID = Property<int>.Make(test.ID);
                    var makeLine = padding+ @"this."+ match.Value + " = Property<" + searchToken + ">.Make(test." + match.Value+ ");";

                    MakeLines.Add(makeLine);
                    return true;
                }
            }



            return false;
        }

      
    }
}