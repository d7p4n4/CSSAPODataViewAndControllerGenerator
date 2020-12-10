using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySetup
{
    public class BatFileGenerator
    {

        #region members

        public string OutputPath { get; set; }
        public string InputPath { get; set; }
        public string GeneratorDirectory { get; set; }

        private const string TemplateExtension = ".txt";

        string generatorDirectoryMask = "#generatorDirectory#";

        #endregion members

        public string ReadIntoString(string fileName)
        {

            string textFile = InputPath + "SAP\\Bat\\" + fileName + TemplateExtension;

            return File.ReadAllText(textFile);

        } // ReadIntoString

        public void WriteOut(string text, string fileName, string outputPath)
        {
            File.WriteAllText(outputPath + fileName + ".bat", text);

        }

        public string GetNameWithLowerFirstLetter(String Code)
        {
            return
                char.ToLower(Code[0])
                + Code.Substring(1)
                ;

        } // GetNameWithLowerFirstLetter

        public string GetBatFile()
        {

            return ReadIntoString("GeneratorStart")
                        .Replace(generatorDirectoryMask, GeneratorDirectory)
                        ;

        }

        public BatFileGenerator Generate()
        {

            string result = null;

            result += GetBatFile();

            WriteOut(result, "GeneratorStart", OutputPath);

            return this;

        } // Generate

        public BatFileGenerator Generate(Type type)
        {

            return Generate();

        } // Generate

        public void Dispose()
        {
            //throw new NotImplementedException();
        }

    }

} // EFGenerala