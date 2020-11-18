﻿using Ac4yClassModule.Class;
using Ac4yClassModule.Service;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CSSAPODataViewAndControllerGenerator
{
    class SAPFormViewGenerator
    {

        #region members

        public string OutputPath { get; set; }
        public string FormTitle { get; set; }
        public string PageTitle { get; set; }

        public Ac4yClass Type { get; set; }

        private const string TemplateExtension = ".xmlT";

        private const string PageTitleMask = "#pageTitle#";
        private const string FormTitleMask = "#formTitle#";
        private const string PropertyNameMask = "#propertyName#";
        private const string InputFieldMask = "#inputField#";
        private const string ComboboxEntityMask = "#comboboxEntity#";
        private const string FormViewIdMask = "#formViewId#";
        private const string ControllerNameMask = "#controllerName#";

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

            string textFile = "SAP\\FormViewXML\\" + fileName + TemplateExtension;

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

            return ReadIntoString("Head")
                        .Replace(PageTitleMask, PageTitle)
                        .Replace(ControllerNameMask, Type.Name + "Form")
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
            string returnText = "";
            Ac4yClassHandler ac4yClassHandler = new Ac4yClassHandler();

            for (int i = 0; i < Type.PropertyList.Count; i++)
            {
                string text = ReadIntoString("content");

                if (Type.PropertyList[i].Name.Equals("Id"))
                {
                }
                else
                {
                    text = text.Replace(InputFieldMask, GetInputField(Type.PropertyList[i].WidgetType))
                                .Replace(PropertyNameMask, Type.PropertyList[i].Name)
                                ;

                    if(Type.PropertyList[i].WidgetType.Equals("COMBOBOX"))
                    {
                        text = text.Replace(ComboboxEntityMask, ac4yClassHandler.GetAc4yComboboxEntityName(Type.PropertyList[i].PropertyInfo));
                    };

                    returnText += text;
                }
            }

            return returnText;
        }

        private string GetFormHead()
        {
            return ReadIntoString("formHead")
                    .Replace(FormTitleMask, FormTitle)
                    .Replace(FormViewIdMask, Type.Name)
                    ;
        }


        public SAPFormViewGenerator Generate()
        {

            string result = null;

            result += GetHead();

            result += GetFormHead();

            result += GetContent();

            result += GetFoot();

            WriteOut(result, Type.Name + "Form.view", OutputPath);

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
