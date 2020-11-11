using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CSSAPODataViewAndControllerGenerator
{
    class SAPMainControllerGenerator
    {

        #region members

        public string OutputPath { get; set; }
        public string ODataURL { get; set; }
        public string SearchField { get; set; }
        public string SortField { get; set; }
        public string TableId { get; set; }
        public string EntityName { get; set; }


        private const string TemplateExtension = ".jsT";

        private const string ODataURLMask = "#odataUrl#";
        private const string SearchFieldMask = "#searchField#";
        private const string SortFieldMask = "#sortField#";
        private const string TableIdMask = "#tableId#";
        private const string FormViewIdMask = "#formViewId#";

        #endregion members

        public string ReadIntoString(string fileName)
        {

            string textFile = "SAP\\MainController\\" + fileName + TemplateExtension;

            return File.ReadAllText(textFile);

        } // ReadIntoString

        public void WriteOut(string text, string fileName, string outputPath)
        {
            System.IO.Directory.CreateDirectory(outputPath + "sources");
            File.WriteAllText(outputPath + "sources\\" + EntityName + "\\" + fileName + ".js", text);

        }

        public string GetNameWithLowerFirstLetter(String Code)
        {
            return
                char.ToLower(Code[0])
                + Code.Substring(1)
                ;

        } // GetNameWithLowerFirstLetter
        public string GetNameWithUpperFirstLetter(String Code)
        {
            return
                char.ToUpper(Code[0])
                + Code.Substring(1)
                ;

        } // GetNameWithUpperFirstLetter

        public string GetHead()
        {

            return ReadIntoString("Head")
                        ;

        }

        public string GetFoot()
        {
            return
                ReadIntoString("Foot")
                        ;

        }

        private string GetMethods()
        {
            return ReadIntoString("methods")
                    .Replace(FormViewIdMask, EntityName)
                    .Replace(ODataURLMask, ODataURL)
                    .Replace(SortFieldMask, SortField)
                    .Replace(SearchFieldMask, SearchField)
                    .Replace(TableIdMask, TableId);
        }

        public SAPMainControllerGenerator Generate()
        {

            string result = null;

            result += GetHead();

            result += GetMethods();

            result += GetFoot();

            WriteOut(result, EntityName + "Main.controller", OutputPath);

            return this;

        } // Generate



        public void Dispose()
        {
            //throw new NotImplementedException();
        }

    }
}
