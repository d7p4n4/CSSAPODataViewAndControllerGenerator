using Ac4yClassModule.Class;
using Ac4yClassModule.Service;
using Ac4yUtilityContainer;
using CSClassLibForJavaOData;
using CSVendorTervezoProba;
using log4net;
using log4net.Config;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace CSSAPODataViewAndControllerGenerator
{
    class Program
    {

        #region members

        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private const string APPSETTINGS_SORTFIELD = "SORTFIELD";
        private const string APPSETTINGS_FORMTITLE = "FORMTITLE";
        private const string APPSETTINGS_PAGETITLE = "PAGETITLE";
        private const string APPSETTINGS_SEARCHFIELD = "SEARCHFIELD";
        private const string APPSETTINGS_TABLEID = "TABLEID";
        private const string APPSETTINGS_ODATAURL = "ODATAURL";
        private const string APPSETTINGS_OUTPUTPATH = "OUTPUTPATH";

        private const string APPSETTINGS_LIBRARYPATH = "LIBRARYPATH";
        private const string APPSETTINGS_PLANOBJECTNAMESPACE = "PLANOBJECTNAMESPACE";
        private const string APPSETTINGS_PROJECTNAME = "PROJECTNAME";

        private const string APPSETTINGS_PARAMETERPATH = "PARAMETERPATH";
        private const string APPSETTINGS_PARAMETERFILENAME = "PARAMETERFILENAME";
        private const string APPSETTINGS_XMLPATH = "XMLPATH";

        private const string APPSETTINGS_DOMAINNAME = "DOMAINNAME";
        private const string APPSETTINGS_INDEXFILEPATH = "INDEXFILEPATH";
        private const string APPSETTINGS_PORTNUMBER = "PORTNUMBER";
        private const string APPSETTINGS_SERVICEFILENAME = "SERVICEFILENAME";
        private const string APPSETTINGS_HOMEPATH = "HOMEPATH";

        CSODataGeneratorParameter Parameter { get; set; }


        public IConfiguration Config { get; set; }

        #endregion members

        public Program(IConfiguration config)
        {

            Config = config;

        } // Program
        /*
        public void Run()
        {
            _library = Assembly.LoadFile(
                        Config[APPSETTINGS_LIBRARYPATH]
                    );

            Parameter =
                (CSODataGeneratorParameter)
                new Ac4yUtility().Xml2ObjectFromFile(
                        Config[APPSETTINGS_PARAMETERPATH]
                        + Config[APPSETTINGS_PARAMETERFILENAME]
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

                new LinuxShellScriptGenerator()
                {
                    OutputPath = Config[APPSETTINGS_OUTPUTPATH]
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

                foreach (PlanObjectReference planObject in Parameter.PlanObjectReferenceList)
                {
                    Directory.CreateDirectory(Config[APPSETTINGS_OUTPUTPATH] + "sources\\" + planObject.className);
                    Directory.CreateDirectory(Config[APPSETTINGS_OUTPUTPATH] + "sources\\" + planObject.className + "\\sources");

                    new SAPTableViewGenerator()
                    {
                        OutputPath = Config[APPSETTINGS_OUTPUTPATH] 
                    ,
                        EntityName = planObject.className
                    ,
                        TableId = Config[APPSETTINGS_TABLEID]
                    }
                    .Generate(new Ac4yClassHandler().GetAc4yClassFromType(planObject.classType));
                
                new SAPMainControllerGenerator()
                {
                    OutputPath = Config[APPSETTINGS_OUTPUTPATH]
                    ,
                    ODataURL = Config[APPSETTINGS_ODATAURL]
                    ,
                    SearchField = Config[APPSETTINGS_SEARCHFIELD]
                    ,
                    SortField = Config[APPSETTINGS_SORTFIELD]
                    ,
                    TableId = Config[APPSETTINGS_TABLEID]
                    ,
                    EntityName = planObject.className
                }
                    .Generate(new Ac4yClassHandler().GetAc4yClassFromType(planObject.classType));


                new SAPFormViewGenerator()
                {
                    OutputPath = Config[APPSETTINGS_OUTPUTPATH]
                ,
                    FormTitle = Config[APPSETTINGS_FORMTITLE]
                ,
                    PageTitle = Config[APPSETTINGS_PAGETITLE]
                }
                .Generate(new Ac4yClassHandler().GetAc4yClassFromType(planObject.classType));


                new SAPFormControllerGenerator()
                {
                    OutputPath = Config[APPSETTINGS_OUTPUTPATH]
                    ,
                    ODataURL = Config[APPSETTINGS_ODATAURL]
                    ,
                    EntityName = planObject.className
                    ,
                    FormTitle = Config[APPSETTINGS_FORMTITLE]
                }
                    .Generate();

                    new SAPIndexHTMLGenerator()
                    {
                        OutputPath = Config[APPSETTINGS_OUTPUTPATH]
                        ,
                        Title = Config[APPSETTINGS_PAGETITLE]
                        ,
                        EntityName = planObject.className
                    }
                        .Generate();

                    new NginxFileGenerator()
                    {
                        OutputPath = Config[APPSETTINGS_OUTPUTPATH]
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

                    portNumberPlus++;
                }

            } catch(Exception exception)
            {
                Console.WriteLine(exception.Message);
            }
            
        } // run
        */
        static void Main(string[] args)
        {
            Console.WriteLine("Elindultam");

            try
            {
                var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
                XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));

                IConfiguration config = null;

                config = new ConfigurationBuilder()
                            .AddJsonFile("appsettings.json", true, true)
                            .Build();
                /*
                new RunWithDll()
                {
                    OdataUrl = config[APPSETTINGS_ODATAURL]
                    ,
                    FormTitle = config[APPSETTINGS_FORMTITLE]
                    ,
                    PageTitle = config[APPSETTINGS_PAGETITLE]
                    ,
                    SearchField = config[APPSETTINGS_SEARCHFIELD]
                    ,
                    SortField = config[APPSETTINGS_SORTFIELD]
                    ,
                    LibraryPath = config[APPSETTINGS_LIBRARYPATH]
                    ,
                    OutputPath = config[APPSETTINGS_OUTPUTPATH]
                    ,
                    ParameterFileName = config[APPSETTINGS_PARAMETERFILENAME]
                    ,
                    ParameterPath = config[APPSETTINGS_PARAMETERPATH]
                    ,
                    TableId = config[APPSETTINGS_TABLEID]
                }
                    .Run();*/

                Ac4yModule ac4yClasses = (Ac4yModule) new Ac4yUtility().Xml2ObjectFromFile(config[APPSETTINGS_XMLPATH], typeof(Ac4yModule));
                new RunWithXml(ac4yClasses)
                {
                    OdataUrl = config[APPSETTINGS_ODATAURL]
                    ,
                    FormTitle = config[APPSETTINGS_FORMTITLE]
                    ,
                    PageTitle = config[APPSETTINGS_PAGETITLE]
                    ,
                    SearchField = config[APPSETTINGS_SEARCHFIELD]
                    ,
                    SortField = config[APPSETTINGS_SORTFIELD]
                    ,
                    LibraryPath = config[APPSETTINGS_LIBRARYPATH]
                    ,
                    OutputPath = config[APPSETTINGS_OUTPUTPATH]
                    ,
                    ParameterFileName = config[APPSETTINGS_PARAMETERFILENAME]
                    ,
                    ParameterPath = config[APPSETTINGS_PARAMETERPATH]
                    ,
                    TableId = config[APPSETTINGS_TABLEID]
                }
                    .Run();
            }
            catch (Exception exception)
            {

                log.Error(exception.Message);
                log.Error(exception.StackTrace);
                Console.WriteLine(exception.Message + "\n" + exception.StackTrace);

                Console.ReadLine();

            }

        } // Main

    } // Program

} // JavaGeneralas