﻿using Ac4yClassModule.Class;
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
        private const string APPSETTINGS_TITLE = "TITLE";
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
            persistenceLista.Add(typeof(Colors));

            new SAPIndexHTMLGenerator()
            {
                TemplatePath = Config[APPSETTINGS_TEMPLATEPATH]
                ,
                TemplateSubPath = Config[APPSETTINGS_SAPINDEXHTMLSUBPATH]
                ,
                OutputPath = Config[APPSETTINGS_SAPINDEXHTMLOUTPUTPATH]
                ,
                Title = Config[APPSETTINGS_TITLE]
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