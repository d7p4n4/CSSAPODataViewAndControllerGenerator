using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CSSAPODataViewAndControllerGenerator
{
    class SAPFormControllerGenerator
    {

        #region members

        public string OutputPath { get; set; }
        public string ODataURL { get; set; }
        public string EntityName { get; set; }


        private const string TemplateExtension = ".jsT";

        private const string ODataURLMask = "#odataUrl#";
        private const string EntityNameMask = "#entityName#";
        private const string ControllerNameMask = "#controllerName#";

        #endregion members

        public string ReadIntoString(string fileName)
        {

            string textFile = "SAP\\FormController\\" + fileName + TemplateExtension;

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
                        .Replace(ControllerNameMask, EntityName + "Controller")
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
                    .Replace(ODataURLMask, ODataURL)
                    .Replace(EntityNameMask, EntityName);
        }

        public SAPFormControllerGenerator Generate()
        {

            string result = null;

            result += GetHead();

            result += GetMethods();

            result += GetFoot();

            WriteOut(result, EntityName + "Form.controller", OutputPath);

            return this;

        } // Generate



        public void Dispose()
        {
            //throw new NotImplementedException();
        }

    }
}