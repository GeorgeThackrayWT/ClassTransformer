using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace DtoParser
{   
    public class ClassTransformer  
    {
        public string OldName { get; set; }
        public string NewName { get; set; }

        public string Source { get; set; }
        public List<string> SourceAsLines { get; set; } = new List<string>();

        public List<string> TargetLines { get; set; } = new List<string>();

        public List<string> MakeLines { get; set; } = new List<string>();

        public List<string> ConvertToDtoLines { get; set; } = new List<string>();

        public List<string> CloneLines { get; set; } = new List<string>();

        public List<string> PropertyRecord { get; set; }= new List<string>();

        public List<string> ResetLines { get; set; } = new List<string>();


        public void CreateDtoClass()
        {
            var padding = "      ";

            SourceAsLines.AddRange(Source.Split(
                new[] { Environment.NewLine },
                StringSplitOptions.None
            ));



            foreach (var l in SourceAsLines)
            {
                bool result = false;

                MakeNames(l,"");



                if (MakeDtoLine(l, "string")) result = true;

                if (MakeDtoLine(l, "double")) result = true;

                if (MakeDtoLine(l, @"double?")) result = true;

                if (MakeDtoLine(l, "int")) result = true;

                if (MakeDtoLine(l, @"int?")) result = true;

                if (MakeDtoLine(l, "DateTime")) result = true;

                if (MakeDtoLine(l, @"DateTime?")) result = true;

                if (MakeDtoLine(l, "bool")) result = true;

                if (MakeDtoLine(l, @"bool?")) result = true;

                if (MakeDtoLine(l, @"ComboBoxValue")) result = true;

                if (MakeDtoLine(l, @"CATPlotObj")) result = true;

                if (MakeDtoLine(l, @"CATPlotObjBool")) result = true;

                if (l.Contains("public class"))
                {
                    TargetLines.Add("public class " + OldName + " : BaseDto, IRecord ");
                }
                else
                {
                    if (l.Trim() != "}")
                    {
                        if (l.Trim() != "{")
                            TargetLines.Add(padding + l.TrimStart());
                        else
                            TargetLines.Add(l.TrimStart());
                    }
                }


            }

            TargetLines.AddRange(CreateCloneMethod(this.CloneLines, OldName));

            TargetLines.Add("}");

        }


        public void CreateEditListClass()
        {
            SourceAsLines.AddRange(Source.Split(
                new[] { Environment.NewLine },
                StringSplitOptions.None
            ));
            var constructorCreated = false;



            foreach (var l in SourceAsLines)
            {


                bool result = false;

                MakeNames(l, "EditList");

                if (!constructorCreated)
                {
                    constructorCreated = MakeListConstructor(OldName, NewName);
                    continue;
                }


                if (l.Replace(" ", "") == "{")
                {
                    // TargetLines.Add("{");
                    result = true;
                }

                if (l.Replace(" ", "") == "}")
                {

                    result = true;
                }

                if (MakeListLine(l, "string")) result = true;

                if (MakeListLine(l, "double")) result = true;

                if (MakeListLine(l, @"double?")) result = true;

                if (MakeListLine(l, "int")) result = true;

                if (MakeListLine(l, @"int?")) result = true;

                if (MakeListLine(l, "DateTime")) result = true;

                if (MakeListLine(l, @"DateTime?")) result = true;

                if (MakeListLine(l, "bool")) result = true;

                if (MakeListLine(l, @"bool?")) result = true;

                if (MakeListLine(l, "ComboBoxValue")) result = true;

                if (MakeListLine(l, "CATPlotObj")) result = true;

                if (MakeListLine(l, "CATPlotObjBool")) result = true;

                if (!result)
                    TargetLines.Add(l);
            }

            TargetLines.Add("");
            TargetLines.Add("");

            TargetLines.Add("public int RecordId => Id;");

            TargetLines.Add("");
            TargetLines.Add("");


            TargetLines.AddRange(CreateConvertToDto(ConvertToDtoLines, OldName));

            TargetLines.Add("");
            TargetLines.Add("");


            TargetLines.AddRange(CreateMakeFromExistingMethod(MakeLines, NewName));


            TargetLines.Add("");
            TargetLines.Add("");


            TargetLines.AddRange(CreateMakeMethod(MakeLines, OldName,false));

            TargetLines.Add("");
            TargetLines.Add("");

            TargetLines.AddRange(CreateListResetChangesMethod(ResetLines, OldName));

            TargetLines.Add("");
            TargetLines.Add("");


            TargetLines.AddRange(CreateListMakeCollectionMethod());

            TargetLines.Add("}//endofclass");
        }

        public void CreateEditClass()
        {
            SourceAsLines.AddRange(Source.Split(
                new[] { Environment.NewLine },
                StringSplitOptions.None
            ));

            var constructorCreated = false;

            foreach (var l in SourceAsLines)
            {

              
                bool result = false;

                MakeNames(l,"Edit");
                

                if (!constructorCreated)
                {
                    constructorCreated = MakeConstructor(OldName, NewName);
                    continue;
                }


                if (l.Replace(" ","") == "{")
                {
                   // TargetLines.Add("{");
                    result = true;
                }

                if (l.Replace(" ", "") == "}")
                {
                  
                    result = true;
                }

                if (MakePropertyLine(l, "string", @"Value = string.Empty, Original = string.Empty")) result = true;

                if (MakePropertyLine(l, "double", @"Value = 0.0, Original = 0.0")) result = true;

                if (MakePropertyLine(l, @"double?", @"Value = 0.0, Original = 0.0")) result = true;

                if (MakePropertyLine(l, "int", @"Value = 0, Original = 0")) result = true;

                if (MakePropertyLine(l, @"int?", @"Value = 0, Original = 0")) result = true;

                if (MakePropertyLine(l,"DateTime", @"Value = DateTime.Today,Original = DateTime.Today")) result = true;

                if (MakePropertyLine(l, @"DateTime?", @"Value = DateTime.Today,Original = DateTime.Today")) result = true;

                if (MakePropertyLine(l, "bool", @"Value = false, Original = false")) result = true;

                if (MakePropertyLine(l, @"bool?", @"Value = false, Original = false")) result = true;

                if (MakePropertyLine(l, @"ComboBoxValue", @"Value = null, Original = null")) result = true;

                if (MakePropertyLine(l, @"CATPlotObj", @"Value = null, Original = null")) result = true;

                if (MakePropertyLine(l, @"CATPlotObjBool", @"Value = null, Original = null")) result = true;

                if (!result)
                    TargetLines.Add(l);
            }

            TargetLines.Add("");
            TargetLines.Add("");

            TargetLines.Add("public int RecordId => Id.Value;");

            TargetLines.Add("");
            TargetLines.Add("");


            TargetLines.AddRange(CreateConvertToDto(ConvertToDtoLines, OldName));

            TargetLines.Add("");
            TargetLines.Add("");


            TargetLines.AddRange(CreateMakeFromExistingMethod(MakeLines, NewName));


            TargetLines.Add("");
            TargetLines.Add("");


            TargetLines.AddRange(CreateMakeMethod(MakeLines, OldName,true));


            TargetLines.Add("");
            TargetLines.Add("");


            TargetLines.AddRange(CreateEditResetChangesMethod(ResetLines, OldName));

            TargetLines.Add("");
            TargetLines.Add("");


            TargetLines.AddRange(CreateEditMakeCollectionMethod());

            TargetLines.Add("}//endofclass");

        }

        private List<string> CreateConvertToDto(List<string> makeLines, string paramType)
        {
            var returnLines = new List<string> { "public "+paramType+" ConvertToDto()", "{" };

            returnLines.Add(@"var returnVal = _dto.Clone();");

            returnLines.AddRange(makeLines);

            returnLines.Add("return returnVal;");

            returnLines.Add("}//ConvertToDto");

            return returnLines;
        }

        private List<string> CreateMakeMethod(List<string> makeLines, string paramType, bool editClass)
        {
            var padding = "      ";
            var returnLines = new List<string> { "public void Make(" + paramType + " test)", "{" };

            returnLines.AddRange(makeLines);

            if (editClass)
            {
                returnLines.Add(padding + @"this.Id = Property<int>.Make(test.Id);");
            }
            else
            { 
                returnLines.Add(padding + @"_original.Id = test.Id;");
                returnLines.Add(padding + @"_current.Id = test.Id;");
            }

            returnLines.Add(padding + @"_dto = test;");

            returnLines.Add("}//endoffirstmake");

            return returnLines;
        }
        
        private List<string> CreateEditMakeCollectionMethod()
        {
            var returnLines = new List<string>
            {
                "   public static ExtRangeCollection<" + this.NewName + "> MakeCollection(List<" + this.OldName +"> records)",
                "   {",
                "",
                "       var newData = new ExtRangeCollection<" + this.NewName + ">();",
                "",
                "       foreach (var rec in records)",
                "       {",
                "           var e = new " + this.NewName + "();",
                "           e.Make(rec);",
                "           newData.AddItem(e);",
                "       }",
                "",
                "       return newData;",
                "   }"
            };

           
            return returnLines;
        }

        private List<string> CreateListMakeCollectionMethod()
        {
            var returnLines = new List<string>
            {
                "   public static ExtRangeCollection<" + this.NewName + "> MakeCollection(List<" + this.OldName +"> records)",
                "   {",
                "",
                "       var newData = new ExtRangeCollection<" + this.NewName + ">();",
                "",
                "       foreach (var rec in records)",
                "       {",
                "           var e = new " + this.NewName + "();",
                "           e.Make(rec);",
                "           newData.AddItem(e);",
                "       }",
                "",
                "       return newData;",
                "   }"
            };


            return returnLines;
        }


        private List<string> CreateListResetChangesMethod(List<string> makeLines, string paramType)
        {
            var returnLines = new List<string> { "public void ResetChanges()", "{" };

            returnLines.Add(@"this._current = this._original;");

            returnLines.Add("}//endofResetChanges");

            return returnLines;
        }

        private List<string> CreateEditResetChangesMethod(List<string> makeLines, string paramType)
        {
            var returnLines = new List<string> { "public void ResetChanges()", "{" };
            
            returnLines.AddRange(makeLines);

            returnLines.Add("}//endofResetChanges");

            return returnLines;
        }

        private List<string> CreateCloneMethod(List<string> makeLines, string paramType)
        {
            var padding = "      ";

            var returnLines = new List<string>
            {
                padding+"public " + paramType + " Clone()",
                padding+"{",
                padding+" return new " + paramType + "() {"
            };


            returnLines.AddRange(makeLines);

            returnLines.Add(padding + "};");

            returnLines.Add(padding + "}//endofclonemethods");

            return returnLines;
        }

        private List<string> CreateMakeFromExistingMethod(List<string> makeLines, string paramType)
        {

            //var t1 = (IncomeEdit)existingRecord;

            //var temp = t1.ConvertToDto();

            //temp.Id = id;

            //Make(temp);

            var padding = " ";

            var returnLines = new List<string>
            {
                "public void MakeFromExisting(object existingRecord, int id)",
                "{",
                padding + @"var t1 = (" + paramType + ")existingRecord;",
                padding + "var temp = t1.ConvertToDto();",
                padding + "temp.Id = id;",
                padding + "Make(temp);",
                "}//MakeFromExisting"
            };
            
            //returnLines.AddRange(MakeLines);
            
            return returnLines;
        }


        private void MakeNames(string l, string postfix)
        {
            Regex regex = new Regex(@"(?<=\bclass )(\w+)");
            Match match = regex.Match(l);

            var oldName = "";
            var newName = "";

            if (match.Success)
            {
                oldName = match.Value;

                
                newName = match.Value.Replace("Dto","") + postfix;

                this.OldName = oldName;
                this.NewName = newName;
            }
            
        }

        private bool MakeConstructor(string oldName, string newName)
        {
            var padding = "      ";

            TargetLines.AddRange(new List<string>{
                @"public class " + NewName + " : PropertyBase<" + NewName + ">, IDuplicate",
                @"{",
                @"",
                padding+ @"private " + OldName + " _dto;",
                @"",
                @"",
                padding+ @"public " + NewName + "()",
                padding+ @"{",
                padding+ @"",
                padding + padding+ @"this.Validator = e =>",
                padding + padding+ @"{",
                padding + padding+ @"",
                padding + padding+ @"};",

                padding + padding + @"IsNew = true;",
                padding + padding + @"IsDirty = true;",
                padding + padding + @"IsValid = false;",
                padding+ @"}//endofconstructor",
                @""
            });
            
            return true;
        }

        private bool MakeListConstructor(string oldName, string newName)
        {
            var padding = "      ";

            TargetLines.AddRange(new List<string>
            {
                @"public class " + NewName + " : ListObj<" + OldName + ">, INotifyPropertyChanged, IRecord, IDuplicate",
                @"{",
                @"",
                padding+ @"private " + OldName + " _dto;",
                @"",
                @"",
                padding+ @"public " + NewName + "()",
                padding+ @"{",
                padding+ @"",
                padding+ @"this.Validator = e =>",
                padding+ @"{",
                padding+ @"  IsValid = !(e.Errors.Count > 0);",
                padding+ @"};",
                padding+ @"",
                padding+ @"",
                padding+ @"",
                padding+ @"}//endofconstructor",
                @""
            });

         

            return true;
        }

        private bool MakePropertyLine(string currentLine, string searchToken, string defaultValue)
        {
            string padding = "      ";

            currentLine = currentLine.Replace(@"?", "questionmark");
            searchToken = searchToken.Replace(@"?", "questionmark");

            if (currentLine.Contains("public "+ searchToken + " "))
            {
                Regex regex = new Regex(@"(?<=\bpublic "+ searchToken + @" )(\w+)");
                Match match = regex.Match(currentLine);

                if (match.Success)
                {

                    PropertyRecord.Add(match.Value);

                    var neLine = padding + @"public Property<" + searchToken + "> " + match.Value +
                                 " { get; set; } = new Property<" + searchToken +
                                 ">(){"+ defaultValue+"};";

                    neLine = neLine.Replace("questionmark", @"?");
                    searchToken = searchToken.Replace("questionmark", @"?");


                    TargetLines.Add(neLine);

                    //this.ID = Property<int>.Make(test.ID);
                    var makeLine = padding+ @"this."+ match.Value + " = Property<" + searchToken + ">.Make(test." + match.Value+ ");";

                    MakeLines.Add(makeLine);

                    //ConvertToDtoLines

                    var convertLine = padding + "returnVal." + match.Value + " = " + match.Value + ".Value;";

                    ConvertToDtoLines.Add(convertLine);


                    var cloneLines = padding + "" + match.Value + " = this." + match.Value + ";";


                    CloneLines.Add(cloneLines);


                    var resetLine = padding + "" + match.Value + ".Revert();";

                    ResetLines.Add(resetLine);
                    
                    return true;
                }
            }



            return false;
        }

        private bool MakeListLine(string currentLine, string searchType)
        {
            string padding = "      ";

            currentLine = currentLine.Replace(@"?", "questionmark");
            searchType = searchType.Replace(@"?", "questionmark");

            if (currentLine.Contains("public " + searchType + " "))
            {
                Regex regex = new Regex(@"(?<=\bpublic " + searchType + @" )(\w+)");
                Match match = regex.Match(currentLine);

                if (match.Success)
                {

                    PropertyRecord.Add(match.Value);

                    var neLine = padding + @"public " + searchType + " " + match.Value +
                                 " { get => _current." + match.Value +
                                 "; set { _current." + match.Value + " = value; OnPropertyChanged(); } }";

                    neLine = neLine.Replace("questionmark", @"?");
                    searchType = searchType.Replace("questionmark", @"?");


                    TargetLines.Add(neLine);

                    //public bool Completed { get => _current.Completed; set { _current.Completed = value; OnPropertyChanged(); } }
                   
                    MakeLines.Add(padding + @"_original." + match.Value + " = test." + match.Value + ";");
                    MakeLines.Add(padding + @"_current." + match.Value + " = test." + match.Value + ";");


                    var convertLine = padding + "returnVal." + match.Value + " = this." + match.Value + ";";

                    ConvertToDtoLines.Add(convertLine);

                    var cloneLines = padding + "" + match.Value + " = this." + match.Value + ";";

                    var resetLine = padding + "" + match.Value + ".Revert();";

                    CloneLines.Add(cloneLines);

                    return true;
                }
            }



            return false;
        }

        private bool MakeDtoLine(string currentLine, string searchType)
        {
            string padding = "      ";

            currentLine = currentLine.Replace(@"?", "questionmark");
            searchType = searchType.Replace(@"?", "questionmark");

            if (currentLine.Contains("public " + searchType + " "))
            {
                Regex regex = new Regex(@"(?<=\bpublic " + searchType + @" )(\w+)");
                Match match = regex.Match(currentLine);

                if (match.Success)
                {

                    PropertyRecord.Add(match.Value);

                    var neLine = padding + @"public " + searchType + " " + match.Value +
                                 " { get => _current." + match.Value +
                                 "; set { _current." + match.Value + " = value; OnPropertyChanged(); } }";

                    neLine = neLine.Replace("questionmark", @"?");
                    searchType = searchType.Replace("questionmark", @"?");


                    CloneLines.Add(padding + padding+ match.Value + " = this." + match.Value + ",");
              
                    return true;
                }
            }



            return false;
        }
    }
}