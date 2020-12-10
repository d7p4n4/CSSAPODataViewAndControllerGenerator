using Ac4yClassModule.Service;
using Ac4yUtilityContainer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace CSSAPODataViewAndControllerGenerator
{
    public class RunWithDll
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

        private string Argument { get; set; }
        private Assembly _library { get; set; }

        CSODataGeneratorParameter Parameter { get; set; }

        public RunWithDll(string args)
        {
            Argument = args;
        }

        public RunWithDll() { }

        public void Run()
        {
            _library = Assembly.LoadFile(
                    LibraryPath

                );

            Parameter =
                (CSODataGeneratorParameter)
                new Ac4yUtility().Xml2ObjectFromFile(
                        ParameterPath
                        + ParameterFileName
                        , typeof(CSODataGeneratorParameter)
                    );

            foreach (PlanObjectReference planObject in Parameter.PlanObjectReferenceList)
            {
                planObject.classType = _library.GetType(
                                                    planObject.namespaceName
                                                    + planObject.className
                                                    );

            }

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
                foreach (PlanObjectReference planObject in Parameter.PlanObjectReferenceList)
                {
                    Directory.CreateDirectory(OutputPath + "sources\\" + planObject.className);
                    Directory.CreateDirectory(OutputPath + "sources\\" + planObject.className + "\\sources");

                    new SAPTableViewGenerator()
                    {
                        OutputPath = OutputPath
                    ,
                        EntityName = planObject.className
                    ,
                        TableId = TableId
                    }
                    .Generate(new Ac4yClassHandler().GetAc4yClassFromType(planObject.classType));

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
                        EntityName = planObject.className
                    }
                        .Generate(new Ac4yClassHandler().GetAc4yClassFromType(planObject.classType));


                    new SAPFormViewGenerator()
                    {
                        OutputPath = OutputPath
                    ,
                        FormTitle = FormTitle
                    ,
                        PageTitle = PageTitle
                    }
                    .Generate(new Ac4yClassHandler().GetAc4yClassFromType(planObject.classType));


                    new SAPFormControllerGenerator()
                    {
                        OutputPath = OutputPath
                        ,
                        ODataURL = OdataUrl
                        ,
                        EntityName = planObject.className
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
                        EntityName = planObject.className
                    }
                                        .Generate();
                    /*
                    new NginxFileGenerator()
                    {
                        OutputPath = OutputPath
                                        ,
                        DomainName = Config[APPSETTINGS_DOMAINNAME]
                                        ,
                        EntityName = planObject.className
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
