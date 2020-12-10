using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MySetup
{
    [RunInstaller(true)]
    public partial class InstallerSetup : System.Configuration.Install.Installer
    {
        public InstallerSetup()
        {
            InitializeComponent();
        }

        public override void Install(System.Collections.IDictionary stateSaver)
        {
            base.Install(stateSaver);
        }

        public override void Commit(IDictionary savedState)
        {
            base.Commit(savedState);

            try
            {
                GetFile();
            }
            catch (Exception e)
            {
                MessageBox.Show("Error: " + e.Message);
                base.Rollback(savedState);
            }
        }

        public override void Rollback(IDictionary savedState)
        {
            base.Rollback(savedState);

            try
            {
                //MessageBox.Show("Rollback");
                //GetFile();
            }
            catch (Exception e)
            {
                MessageBox.Show("Error: " + e.Message);
                base.Rollback(savedState);
            }

        }

        public override void Uninstall(IDictionary savedState)
        {
            base.Uninstall(savedState);
        }

        private void GetFile()
        {
            MessageBox.Show(Context.Parameters["RUN"]);
            try
            {
                string ROOTDIRECTORY = Context.Parameters["ROOTDIRECTORY"];
                string LIBRARYPATH = Context.Parameters["LIBRARYPATH"];
                string PLANOBJECTNAMESPACE = Context.Parameters["PLANOBJECTNAMESPACE"];
                string CLASSNAMELIST = Context.Parameters["CLASSNAMELIST"];
                string FORMTITLE = Context.Parameters["FORMTITLE"];
                string PAGETITLE = Context.Parameters["PAGETITLE"];
                string ODATAURL = Context.Parameters["ODATAURL"];
                List<string> classNameList = CLASSNAMELIST.Split(',').ToList();
                string path = Context.Parameters["assemblyPath"].Replace("MySetup.exe", "");

                new AppsettingsGenerator()
                {
                    OutputPath = path
                    ,
                    RootDirectory = ROOTDIRECTORY
                    ,
                    InputPath = path
                    ,
                    FormTitle = FORMTITLE
                    ,
                    PageTitle = PAGETITLE
                    ,
                    OdataURL = ODATAURL
                    ,
                    LibraryPath = LIBRARYPATH
                    ,
                    PlanObjectNamespace = PLANOBJECTNAMESPACE
                }
                    .Generate();

                new BatFileGenerator()
                {
                    OutputPath = path
                    ,
                    InputPath = path
                    ,
                    GeneratorDirectory = path
                }
                    .Generate();


                new ParameterGenerator()
                {
                    OutputPath = path + "Config\\"
                    ,
                    InputPath = path
                    ,
                    ClassNames = classNameList
                    ,
                    Namespace = PLANOBJECTNAMESPACE
                }
                    .Generate();

                //new RunBatFile()
                //{
                //    Path = path
                //    ,
                //    Run = RUN
                //}
                //    .RunBat();

                //MessageBox.Show(path);

            }
            catch (Exception exception)
            {
                MessageBox.Show("Error> " + exception.Message + "\n\n" + exception.StackTrace);
            }
        }

        private void AddConfigurationFileDetails()
        {
        }
    }
}