using Ac4yClassModule.Class;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CSSAPODataViewAndControllerGenerator
{
    class NginxFileGenerator
    {

        #region members

        public string OutputPath { get; set; }
        public string Title { get; set; }
        public string EntityName { get; set; }
        public string DomainName { get; set; }
        public int PortNumber { get; set; }
        public string IndexFilePath { get; set; } 

        private const string TemplateExtension = ".T";

        private const string PortNumberMask = "#portNumber#";
        private const string IndexFilePathMask = "#indexFilePath#";
        private const string SubdomainMask = "#subdomain#";
        private const string DomainMask = "#domain#";

        #endregion members

        public string ReadIntoString(string fileName)
        {

            string textFile = "SAP\\NginxFile\\" + fileName + TemplateExtension;

            return File.ReadAllText(textFile);

        } // ReadIntoString

        public void WriteOut(string text, string fileName, string outputPath)
        {
            File.WriteAllText(outputPath + fileName, text);

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
        
        private string GetBody()
        {
            return ReadIntoString("body")
                        .Replace(PortNumberMask, PortNumber.ToString())
                        .Replace(IndexFilePathMask, IndexFilePath + EntityName)
                        .Replace(SubdomainMask, GetNameWithLowerFirstLetter(EntityName) + "sapview")
                        .Replace(DomainMask, DomainName)
                ;
        }

        public NginxFileGenerator Generate()
        {

            string result = null;

            result += GetBody();

            string fileName = GetNameWithLowerFirstLetter(EntityName) + "sapview." + DomainName.Substring(0, DomainName.IndexOf("."));

            WriteOut(result, fileName, OutputPath + "\\sources\\" + EntityName + "\\");

            return this;

        } // Generate



        public void Dispose()
        {
            //throw new NotImplementedException();
        }

    }
}
