using Ac4yClassModule.Class;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CSSAPODataViewAndControllerGenerator
{
    class RunWithXml
    {
        public string LibraryPath { get; set; }
        public string ParameterPath { get; set; }
        public string ParameterFileName { get; set; }
        public string OutputPath { get; set; }
        public string TableId { get; set; }
        public string SearchField { get; set; }
        public string OdataUrl { get; set; }
        public string PageTitle { get; set; }
        public string FormTitle { get; set; }
        public string SortField { get; set; }

        private Ac4yModule Ac4yModule { get; set; }

        public RunWithXml(Ac4yModule ac4yModule)
        {
            Ac4yModule = ac4yModule;
        }

        public RunWithXml() { }

        public void Run()
        {

            try
            {
                int portNumberPlus = 0;
                /*
                new LinuxShellScriptGenerator()
                {
                    OutputPath = OutputPath
                                ,
                    HomePath = Config[APPSETTINGS_HOMEPATH]
                                ,
                    IndexFilePath = Config[APPSETTINGS_INDEXFILEPATH]
                                ,
                    ServiceFileName = Config[APPSETTINGS_PROJECTNAME]
                                ,
                    Parameter = Parameter
                                ,
                    DomainName = Config[APPSETTINGS_DOMAINNAME]

                }
                                .Generate();
                */
                foreach(Ac4yClass planObject in Ac4yModule.ClassList)
                {
                    Directory.CreateDirectory(OutputPath + "sources\\" + planObject.Name);
                    Directory.CreateDirectory(OutputPath + "sources\\" + planObject.Name + "\\sources");

                    new SAPTableViewGenerator()
                    {
                        OutputPath = OutputPath
                    ,
                        EntityName = planObject.Name
                    ,
                        TableId = TableId
                    }
                    .Generate(planObject);

                    new SAPMainControllerGenerator()
                    {
                        OutputPath = OutputPath
                        ,
                        ODataURL = OdataUrl
                        ,
                        SearchField = SearchField
                        ,
                        SortField = SortField
                        ,
                        TableId = TableId
                        ,
                        EntityName = planObject.Name
                    }
                        .Generate(planObject);


                    new SAPFormViewGenerator()
                    {
                        OutputPath = OutputPath
                    ,
                        FormTitle = FormTitle
                    ,
                        PageTitle = PageTitle
                    }
                    .Generate(planObject);


                    new SAPFormControllerGenerator()
                    {
                        OutputPath = OutputPath
                        ,
                        ODataURL = OdataUrl
                        ,
                        EntityName = planObject.Name
                        ,
                        FormTitle = FormTitle
                    }
                        .Generate();

                    new SAPIndexHTMLGenerator()
                    {
                        OutputPath = OutputPath
                                        ,
                        Title = PageTitle
                                        ,
                        EntityName = planObject.Name
                    }
                                        .Generate();
                    /*
                    new NginxFileGenerator()
                    {
                        OutputPath = OutputPath
                                        ,
                        DomainName = Config[APPSETTINGS_DOMAINNAME]
                                        ,
                        EntityName = planObject.Name
                                        ,
                        PortNumber = Int16.Parse(Config[APPSETTINGS_PORTNUMBER]) + portNumberPlus
                                        ,
                        IndexFilePath = Config[APPSETTINGS_INDEXFILEPATH]
                    }
                                        .Generate();
                    */
                    portNumberPlus++;
                }

            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }

        }
    }
}
