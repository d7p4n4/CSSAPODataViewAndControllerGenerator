using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySetup
{
    public class ParameterGenerator
    {

        #region members

        public string OutputPath { get; set; }
        public string InputPath { get; set; }
        public string Namespace { get; set; }
        public List<string> ClassNames { get; set; }

        private const string TemplateExtension = ".txt";

        private const string NamespaceMask = "#namespace#";
        private const string ClassNameMask = "#className#";
        private const string ObjectReferencesMask = "#objectReferences#";

        #endregion members

        public string ReadIntoString(string fileName)
        {

            string textFile = InputPath + "SAP\\Parameter\\" + fileName + TemplateExtension;

            return File.ReadAllText(textFile);

        } // ReadIntoString

        public void WriteOut(string text, string fileName, string outputPath)
        {
            File.WriteAllText(outputPath + fileName + ".xml", text);

        }

        public string GetNameWithLowerFirstLetter(String Code)
        {
            return
                char.ToLower(Code[0])
                + Code.Substring(1)
                ;

        } // GetNameWithLowerFirstLetter

        public string GetParameterXml()
        {
            string objectReference = ReadIntoString("objectReference");
            string objectReferenceList = "";

            foreach (string className in ClassNames)
            {
                objectReferenceList = objectReferenceList + objectReference
                                                                   .Replace(NamespaceMask, Namespace)
                                                                   .Replace(ClassNameMask, className)
                                                                   ;
            }

            return ReadIntoString("Parameter")
                        .Replace(ObjectReferencesMask, objectReferenceList)
                        ;

        }

        public ParameterGenerator Generate()
        {

            string result = null;

            result += GetParameterXml();

            WriteOut(result, "Parameter", OutputPath);

            return this;

        } // Generate

        public ParameterGenerator Generate(Type type)
        {

            return Generate();

        } // Generate

        public void Dispose()
        {
            //throw new NotImplementedException();
        }

    }

} // EFGenerala