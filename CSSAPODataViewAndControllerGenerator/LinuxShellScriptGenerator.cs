using Ac4yClassModule.Class;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CSSAPODataViewAndControllerGenerator
{
    class LinuxShellScriptGenerator
    {

        #region members

        public string OutputPath { get; set; }
        public string HomePath { get; set; }
        public CSODataGeneratorParameter Parameter { get; set; }
        public string IndexFilePath { get; set; }
        public string ServiceFileName { get; set; }
        public string DomainName { get; set; }

        private const string TemplateExtension = ".shT";

        private const string FoldersMask = "#folders#";
        private const string NginxFilesMask = "#nginxFiles#";
        private const string NginxFilesSymlinkMask = "#nginxFilesSymlinks#";
        private const string IndexFilePathMask = "#indexFilePath#";
        private const string ServiceFileNameMask = "#serviceFileName#";
        private const string MoveCommandTemplate = "mv #folders# #indexFilePath#";
        private const string MoveCommandNginxFilesTemplate = "mv #nginxFiles# /etc/nginx/sites-available/";
        private const string MoveCommandNginxFilesSymlinkTemplate = "ln -s /etc/nginx/sites-available/#nginxFiles# /etc/nginx/sites-enabled/#nginxFiles#";

        #endregion members

        public string ReadIntoString(string fileName)
        {

            string textFile = "SAP\\LinuxShellFile\\" + fileName + TemplateExtension;

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
            string folders = "";
            string nginxFiles = "";
            string nginxFilesSymlink = "";

            foreach (PlanObjectReference planObject in Parameter.PlanObjectReferenceList)
            {
                string nginxFileName = GetNameWithLowerFirstLetter(planObject.className + "sapview." + DomainName.Substring(0, DomainName.IndexOf(".")));
                folders = folders + "\n" + MoveCommandTemplate.Replace(FoldersMask, HomePath + planObject.className).Replace(IndexFilePathMask, IndexFilePath);
                nginxFiles = nginxFiles + "\n" 
                                + MoveCommandNginxFilesTemplate
                                    .Replace(NginxFilesMask, HomePath + planObject.className + "/" + nginxFileName)
                                    ;
                nginxFilesSymlink = nginxFilesSymlink + "\n"
                                        + MoveCommandNginxFilesSymlinkTemplate.Replace(NginxFilesMask, nginxFileName);
            }

            return ReadIntoString("body")
                        .Replace(FoldersMask, folders)
                        .Replace(NginxFilesMask, nginxFiles)
                        .Replace(NginxFilesSymlinkMask, nginxFilesSymlink)
                        .Replace(IndexFilePathMask, IndexFilePath)
                        .Replace(ServiceFileNameMask, ServiceFileName)
                ;
        }

        public LinuxShellScriptGenerator Generate()
        {

            string result = null;

            result += GetBody();

            WriteOut(result, "linuxScript.sh", OutputPath + "\\sources\\");

            return this;

        } // Generate



        public void Dispose()
        {
            //throw new NotImplementedException();
        }

    }
}
