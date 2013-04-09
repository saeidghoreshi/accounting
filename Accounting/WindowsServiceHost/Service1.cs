using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.ServiceModel;
using Accounting.Definition.Contracts.v1;

namespace WindowsServiceHost
{
    public partial class AService6 : ServiceBase
    {
        ServiceHost host = new ServiceHost(typeof(IContracts));
        public AService6()
        {   
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            host.Open();
        }

        protected override void OnStop()
        {
            host.Close();
        }
    }
}
