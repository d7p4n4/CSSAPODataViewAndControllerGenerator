using Ac4yClassModule.Class;
using Ac4yClassModule.Service;
using CSClassLibForJavaOData;
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

        private const string APPSETTINGS_TEMPLATEPATH = "TEMPLATEPATH";

        // JAVA //

        private const string APPSETTINGS_SAPINDEXHTMLSUBPATH = "SAPINDEXHTMLSUBPATH";
        private const string APPSETTINGS_SAPMAINVIEWMLSUBPATH = "SAPMAINVIEWXMLSUBPATH";
        private const string APPSETTINGS_SAPMAINCONTROLLERSUBPATH = "SAPMAINCONTROLLERSUBPATH";
        private const string APPSETTINGS_SAPFORMVIEWXMLSUBPATH = "SAPFORMVIEWXMLSUBPATH";
        private const string APPSETTINGS_SAPFORCONTROLLERSUBPATH = "SAPFORMCONTROLLERSUBPATH";
        private const string APPSETTINGS_TITLE = "TITLE";
        private const string APPSETTINGS_FORMTITLE = "FORMTITLE";
        private const string APPSETTINGS_PAGETITLE = "PAGETITLE";
        private const string APPSETTINGS_HEADERTEXT = "HEADERTEXT";
        private const string APPSETTINGS_ENTITYNAME = "ENTITYNAME";
        private const string APPSETTINGS_TABLEID = "TABLEID";
        private const string APPSETTINGS_ODATAURL = "ODATAURL";
        private const string APPSETTINGS_COMBOBOXENTITY = "COMBOBOXENTITY";
        private const string APPSETTINGS_SAPINDEXHTMLOUTPUTPATH = "SAPINDEXHTMLOUTPUTPATH";
        



        public IConfiguration Config { get; set; }

        #endregion members

        public Program(IConfiguration config)
        {

            Config = config;

        } // Program

        public void Run()
        {
            List<Type> persistenceLista = new List<Type>();
            persistenceLista.Add(typeof(Cars));
            persistenceLista.Add(typeof(Color));

            new SAPTableViewGenerator()
            {
                TemplatePath = Config[APPSETTINGS_TEMPLATEPATH]
                ,
                TemplateSubPath = Config[APPSETTINGS_SAPMAINVIEWMLSUBPATH]
                ,
                OutputPath = Config[APPSETTINGS_SAPINDEXHTMLOUTPUTPATH]
                ,
                HeaderText = Config[APPSETTINGS_HEADERTEXT]
                ,
                EntityName = Config[APPSETTINGS_ENTITYNAME]
                ,
                TableId = Config[APPSETTINGS_TABLEID]
            }
                .Generate(new Ac4yClassHandler().GetAc4yClassFromType(typeof(Vendor)));

            new SAPMainControllerGenerator()
            {
                TemplatePath = Config[APPSETTINGS_TEMPLATEPATH]
                ,
                TemplateSubPath = Config[APPSETTINGS_SAPMAINCONTROLLERSUBPATH]
                ,
                OutputPath = Config[APPSETTINGS_SAPINDEXHTMLOUTPUTPATH]
                ,
                ODataURL = Config[APPSETTINGS_ODATAURL]
            }
                .Generate();

            new SAPFormViewGenerator()
            {
                TemplatePath = Config[APPSETTINGS_TEMPLATEPATH]
                ,
                TemplateSubPath = Config[APPSETTINGS_SAPFORMVIEWXMLSUBPATH]
                ,
                OutputPath = Config[APPSETTINGS_SAPINDEXHTMLOUTPUTPATH]
                ,
                FormTitle = Config[APPSETTINGS_FORMTITLE]
                ,
                PageTitle = Config[APPSETTINGS_PAGETITLE]
                ,
                ComboBoxEntityName = Config[APPSETTINGS_COMBOBOXENTITY]
            }
                .Generate(new Ac4yClassHandler().GetAc4yClassFromType(typeof(Vendor)));

            new SAPFormControllerGenerator()
            {
                TemplatePath = Config[APPSETTINGS_TEMPLATEPATH]
                ,
                TemplateSubPath = Config[APPSETTINGS_SAPFORCONTROLLERSUBPATH]
                ,
                OutputPath = Config[APPSETTINGS_SAPINDEXHTMLOUTPUTPATH]
                ,
                ODataURL = Config[APPSETTINGS_ODATAURL]
                ,
                EntityName = Config[APPSETTINGS_ENTITYNAME]
            }
                .Generate();
            
        } // run

        static void Main(string[] args)
        {

            try
            {
                var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
                XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));

                IConfiguration config = null;

                config = new ConfigurationBuilder()
                            .AddJsonFile("appsettings.json", true, true)
                            .Build();

                new Program(config).Run();

            }
            catch (Exception exception)
            {

                log.Error(exception.Message);
                log.Error(exception.StackTrace);

                Console.ReadLine();

            }

        } // Main

    } // Program

} // JavaGeneralas