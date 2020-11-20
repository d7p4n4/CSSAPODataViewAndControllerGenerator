using Ac4yClassModule.Class;
using Ac4yClassModule.Service;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CSSAPODataViewAndControllerGenerator
{
    class SAPTableViewGenerator
    {

        #region members

        public string OutputPath { get; set; }
        public string EntityName { get; set; }
        public string TableId { get; set; }

        public Ac4yClass Type { get; set; }

        private const string TemplateExtension = ".xmlT";

        private const string EntityNameMask = "#entityName#";
        private const string TableIdMask = "#tableId#";
        private const string PropertyNameMask = "#propertyName#";
        private const string PropertyPathMask = "#propertyPath#";
        private const string ControllerNameMask = "#controllerName#";
        private const string HeaderTextMask = "#headerText#";
        private const string ComboboxEntityMask = "#comboboxEntity#";
        private const string InputFieldMask = "#inputField#";
        private const string UpdateGroupMask = "#updateGroup#";

        public Dictionary<string, string> InputMezoKonverziok = new Dictionary<string, string>()
        {
            { "TEXTBOX", "<Input id=\"#propertyName#Id\" value=\"{#propertyName#}\" valueLiveUpdate=\"true\" />" },
            { "CHECKBOX", "<CheckBox id=\"#propertyName#Id\" selected=\"{path: '#propertyName#'}\" />" },
            { "NODEF", "<DateTimePicker id=\"#propertyName#Id\" value=\"{path: '#propertyName#'}\" valueFormat=\"yyyy-MM-ddTHH:mm:ss\" displayFormat=\"yyyy-MM-ddTHH:mm:ss\" />" },
            { "COMBOBOX", "<ComboBox id=\"#propertyName#Id\" items=\"{/#comboboxEntity#}\" selectedKey=\"{#propertyName#}\">\n " +
                         "   <core:Item key=\"{Name}\" text=\"{Name}\" />\n " +
                         "</ComboBox>" }
           // { "COMBOBOX", "<Input id=\"#propertyName#Id\" value=\"{#propertyName#}\" valueLiveUpdate=\"true\" />" }
        };

        #endregion members

        public string GetInputField(string type)
        {
            string result = "";
            if (type == null)
            {
                type = "TEXTBOX";
            }
            try
            {
                result = InputMezoKonverziok[type];
            }
            catch (Exception exception)
            {
                result = "nodeftype (" + type + ")";
            }

            return result;
        } // GetConvertedType

        public string ReadIntoString(string fileName)
        {

            string textFile = "SAP\\MainViewXML\\" + fileName + TemplateExtension;

            return File.ReadAllText(textFile);

        } // ReadIntoString

        public void WriteOut(string text, string fileName, string outputPath)
        {
            System.IO.Directory.CreateDirectory(outputPath + "sources");
            File.WriteAllText(outputPath + "sources\\" + Type.Name + "\\sources\\" + fileName + ".xml", text);

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

            return ReadIntoString("head")
                    .Replace(ControllerNameMask, Type.Name)
                    .Replace(HeaderTextMask, Type.Name + " entity")
                        ;

        }

        public string GetFoot()
        {
            return
                ReadIntoString("foot")
                        ;

        }


        private string GetTableItem()
        {
            string returnText = "";
            Ac4yClassHandler ac4yClassHandler = new Ac4yClassHandler();

            for (int i = 0; i < Type.PropertyList.Count; i++)
            {
                string text = ReadIntoString("tableItem");

                if (Type.PropertyList[i].Name.Equals("Id"))
                {
                }
                else
                {
                    text = text.Replace(InputFieldMask, GetInputField(Type.PropertyList[i].WidgetType))
                                .Replace(PropertyNameMask, Type.PropertyList[i].Name)
                                ;

                    if (Type.PropertyList[i].WidgetType.Equals("COMBOBOX"))
                    {
                        text = text.Replace(ComboboxEntityMask, ac4yClassHandler.GetAc4yComboboxEntityName(Type.PropertyList[i].PropertyInfo));
                    };

                    returnText += text;
                }
            }

            return returnText;
        }

        private string GetTableCells()
        {
            return ReadIntoString("tableCells");
        }

        private string GetTableColumn()
        {
            string text = ReadIntoString("tableColumns");
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

        private string GetTableHead()
        {
            return ReadIntoString("tableHead")
                    .Replace(TableIdMask, TableId)
                    .Replace(EntityNameMask, EntityName)
                    .Replace(UpdateGroupMask, EntityName + "Group");
        }


        public SAPTableViewGenerator Generate()
        {

            string result = null;

            result += GetHead();

            result += GetTableHead();

            result += GetTableColumn();

            result += GetTableCells();

            result += GetTableItem();

            result += GetFoot();

            WriteOut(result, Type.Name + "Main.view", OutputPath);

            return this;

        } // Generate


        public SAPTableViewGenerator Generate(Ac4yClass type)
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
