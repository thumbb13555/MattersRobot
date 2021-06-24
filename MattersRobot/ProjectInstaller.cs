using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.Threading.Tasks;
using System.ServiceProcess;

namespace MattersRobot
{
    [RunInstaller(true)]
    public partial class ProjectInstaller : System.Configuration.Install.Installer
    {
        public ProjectInstaller()
        {
            InitializeComponent();
        }

        private void serviceProcessInstaller1_AfterInstall(object sender, InstallEventArgs e)
        {
            //System.ServiceProcess.ServiceController sc = new System.ServiceProcess.ServiceController("MattersRobot");
            //if (sc != null) sc.Start();
        }

        private void serviceInstaller1_AfterInstall(object sender, InstallEventArgs e)
        {
            
        }
    }
}
