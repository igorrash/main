using System.ServiceProcess;
using Send;

namespace SendTextService{
    public partial class SendTextService : ServiceBase
    {
        public SendTextService()
        {
            InitializeComponent();
        }

        public void OnDebug()
        {
            OnStart(null);
        }

        protected override void OnStart(string[] args)
        {
            var send = new SendNotifications();
        }

        protected override void OnStop()
        {

        }
    }
}
