using Ac4yClassModule.Class;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CSSAPODataViewAndControllerGenerator
{
    class SAPIndexHTMLGenerator
    {

        #region members

        public string OutputPath { get; set; }
        public string Title { get; set; }
        public string EntityName { get; set; }


        private const string TemplateExtension = ".htmlT";

        private const string TitleMask = "#title#";
        private const string PagesMask = "#pages#";
        private const string PageNameMask = "#pageName#";

        #endregion members

        public string ReadIntoString(string fileName)
        {

            string textFile = "SAP\\indexHTML\\" + fileName + TemplateExtension;

            return File.ReadAllText(textFile);

        } // ReadIntoString

        public void WriteOut(string text, string fileName, string outputPath)
        {
            File.WriteAllText(outputPath + fileName + ".html", text);

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
                        .Replace(TitleMask, Title)
                        ;

        }

        public string GetFoot()
        {
            return
                ReadIntoString("Foot")
                        ;

        }

        private string GetBody()
        {
            return ReadIntoString("body")
                        .Replace(PageNameMask, EntityName)
                ;
        }

        public SAPIndexHTMLGenerator Generate()
        {

            string result = null;

            result += GetHead();

            result += GetBody();

            result += GetFoot();

            WriteOut(result, "index", OutputPath + "\\sources\\" + EntityName + "\\");

            return this;

        } // Generate



        public void Dispose()
        {
            //throw new NotImplementedException();
        }

    }
}
