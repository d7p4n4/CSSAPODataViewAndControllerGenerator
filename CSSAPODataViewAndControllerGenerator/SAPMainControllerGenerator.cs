using Ac4yClassModule.Class;
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

        public Ac4yClass Type { get; set; }

        private const string TemplateExtension = ".jsT";

        private const string ODataURLMask = "#odataUrl#";
        private const string SearchFieldMask = "#searchField#";
        private const string SortFieldMask = "#sortField#";
        private const string TableIdMask = "#tableId#";
        private const string FormViewIdMask = "#formViewId#";
        private const string ControllerNameMask = "#controllerName#";
        private const string UpdatedGroupMask = "#updatedGroup#";
        private const string PropertyNameMask = "#propertyName#";
        private const string CreatedEntitiesMask = "#createdEntities#";
        private const string DefaultValueMask = "#defaultValue#";

        private const string CreatedPropertiesTemplate = "\"#propertyName#\": #defaultValue#,";


        public Dictionary<string, string> FormaKonverziok = new Dictionary<string, string>()
        {
            { "Int32", "int32" },
            { "Decimal", "" },
            { "Money", "integer" },
            { "Float", "float" },
            { "Int64", "int64" },
            { "VarChar", "" },
            { "Char", "" },
            { "VarBinary", "byte[]" },
            { "DateTime", "date-time" },
            { "Date", "date" },
            { "TinyInt", "" },
            { "Bit", "" },
            { "String", "" },
            { "Double", "double" },
            { "Boolean", "" }
        };

        public bool GetConvertedType(string type, Dictionary<string, string> dictionary)
        {
            string result;
            try
            {
                if (dictionary.TryGetValue(type, out result))
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception exception)
            {
                result = "";
            }
            return false;
        } // GetConvertedType

        #endregion members
        public string ReadIntoString(string fileName)
        {

            string textFile = "SAP\\MainController\\" + fileName + TemplateExtension;

            return File.ReadAllText(textFile);

        } // ReadIntoString

        public void WriteOut(string text, string fileName, string outputPath)
        {
            System.IO.Directory.CreateDirectory(outputPath + "sources");
            File.WriteAllText(outputPath + "sources\\" + EntityName + "\\sources\\" + fileName + ".js", text);

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
                        .Replace(ControllerNameMask, EntityName)
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
            string createdProperties = "";

            foreach(Ac4yProperty ac4yProperty in Type.PropertyList)
            {
                if(!ac4yProperty.Name.Equals("Id") && !GetConvertedType(ac4yProperty.TypeName, FormaKonverziok))
                {
                    if(ac4yProperty.TypeName.Equals("Boolean"))
                    {
                        createdProperties = createdProperties + "\n" + CreatedPropertiesTemplate
                                                                            .Replace(PropertyNameMask, ac4yProperty.Name)
                                                                            .Replace(DefaultValueMask, "false");
                    } else if(ac4yProperty.TypeName.Equals("DateTime"))
                    {
                        createdProperties = createdProperties + "\n" + CreatedPropertiesTemplate
                                                                            .Replace(PropertyNameMask, ac4yProperty.Name)
                                                                            .Replace(DefaultValueMask, "\"1990-01-01T00:00:00\"");
                    } else
                    {
                        createdProperties = createdProperties + "\n" + CreatedPropertiesTemplate
                                                                            .Replace(PropertyNameMask, ac4yProperty.Name)
                                                                            .Replace(DefaultValueMask, "\"\"");
                    }

                }
                    
            }

            return ReadIntoString("methodsWithTableCreate")
                    .Replace(FormViewIdMask, EntityName)
                    .Replace(ODataURLMask, ODataURL)
                    .Replace(SortFieldMask, SortField)
                    .Replace(SearchFieldMask, SearchField)
                    .Replace(TableIdMask, TableId)
                    .Replace(UpdatedGroupMask, EntityName + "Group")
                    .Replace(CreatedEntitiesMask, createdProperties)
                    ;
        }

        internal void Generate(object p)
        {
            throw new NotImplementedException();
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

        public SAPMainControllerGenerator Generate(Ac4yClass type)
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
