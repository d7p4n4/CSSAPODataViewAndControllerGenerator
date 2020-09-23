using Ac4yClassModule.Class;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CSSAPODataViewAndControllerGenerator
{
    class SAPFormViewGenerator
    {

        #region members

        public string TemplatePath { get; set; }
        public string TemplateSubPath { get; set; }
        public string OutputPath { get; set; }
        public string FormTitle { get; set; }
        public string PageTitle { get; set; }

        public Ac4yClass Type { get; set; }

        private const string TemplateExtension = ".xmlT";

        private const string PageTitleMask = "#pageTitle#";
        private const string FormTitleMask = "#formTitle#";
        private const string PropertyNameMask = "#propertyName#";

        #endregion members

        public string ReadIntoString(string fileName)
        {

            string textFile = TemplatePath + TemplateSubPath + fileName + TemplateExtension;

            return File.ReadAllText(textFile);

        } // ReadIntoString

        public void WriteOut(string text, string fileName, string outputPath)
        {
            System.IO.Directory.CreateDirectory(outputPath + "sources");
            File.WriteAllText(outputPath + "sources\\" + fileName + ".xml", text);

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
                        .Replace(PageTitleMask, PageTitle)
                        ;

        }

        public string GetFoot()
        {
            return
                ReadIntoString("Foot")
                        ;

        }


        private string GetContent()
        {
            string text = ReadIntoString("content");
            string returnText = "";

            for (int i = 0; i < Type.PropertyList.Count; i++)
            {
                if (Type.PropertyList[i].Name.Equals("Id"))
                {
                }
                else
                { 
                    returnText += text.Replace(PropertyNameMask, Type.PropertyList[i].Name);
                }
            }

            return returnText;
        }

        private string GetFormHead()
        {
            return ReadIntoString("formHead")
                    .Replace(FormTitleMask, FormTitle);
        }


        public SAPFormViewGenerator Generate()
        {

            string result = null;

            result += GetHead();

            result += GetFormHead();

            result += GetContent();

            result += GetFoot();

            WriteOut(result, "Form.view", OutputPath);

            return this;

        } // Generate


        public SAPFormViewGenerator Generate(Ac4yClass type)
        {

            Type = type;

            return Generate();

        } // Generate

        public void Dispose()
        {
            //throw new NotImplementedException();
        }

    }
}
