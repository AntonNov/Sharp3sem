using System.IO;
using System.ServiceProcess;
using System.Threading;
using ConfigurationManager;
using DataAccessLayer.UnitOfWork;
using DataManager.Options;
using DataManager.XmlGenerator;
using ServiceLayer.DTO;
using ServiceLayer.Services;

namespace DataManager
{
    public class DataManager : ServiceBase
    {
        private System.ComponentModel.IContainer components = null;
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                components?.Dispose();
            }
            base.Dispose(disposing);
        }
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            this.ServiceName = "DataManager";
        }
        public DataManager()
        {
            var optionsManager = new OptionsManager();
            OptionsLib.PathOptions = optionsManager.GetConfiguredOptionsModel<PathOptions>();
            OptionsLib.ServiceOptions = optionsManager.GetConfiguredOptionsModel<ServiceOptions>();

            InitializeComponent();

            this.CanStop = OptionsLib.ServiceOptions.CanStop;
            this.CanPauseAndContinue = OptionsLib.ServiceOptions.CanPauseAndContinue;
            this.AutoLog = OptionsLib.ServiceOptions.AutoLog;
        }

        protected override void OnStart(string[] args)
        {
            ThreadPool.QueueUserWorkItem(async state =>
            {
                var repositories = new UnitOfWork();
                OrderService orderService = new OrderService(repositories);
                var ordersInfo = await orderService.GetOrdersInfo();
                IXmlGeneratorService<OrderInfo> xmlGenerator = new XmlGeneratorService<OrderInfo>();
                await xmlGenerator.GenerateXml(OptionsLib.PathOptions.SourceDirectory, ordersInfo);
            });
        }

        protected override void OnStop()
        {
            Thread.Sleep(1000);
        }
    }
}