using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using System.Threading;
using System.Transactions;
using System.Xml;
using System.Configuration;
using CustomsDeclarationProxy.Log;
using CustomsDeclarationProxy.Message;
using CustomsDeclarationProxy.Service;
using CustomsDeclarationProxy.Util;
using CustomsDeclarationProxy.Constant;
using CustomsDeclarationProxy.MessageThread;
using CustomsDeclarationProxy.Config;



namespace CustomsDeclarationProxy
{
    public class Global : System.Web.HttpApplication
    {
        

        void Application_Start(object sender, EventArgs e)
        {

            //在运行程序开始时运行的代码
            Logger.Info("The CustomsDeclarationProxy Start...");

            //ConfigUtil cf = ConfigUtil.createInstance();
           // MsgQueue customsGoodsRespQueue = new MsgQueue().Createqueue(cf.getDeclMsgQueueAddr("customsGoodsRespUrl"));
            //Thread test = new Thread(new ThreadStart(new MsgListen(customsGoodsRespQueue, CustomsMessageType.GOODS, SendPlace.GOVERNMENT).ThreadProc));
            
            MessageReceiveThread.startThread();
          

        }


        void Application_End(object sender, EventArgs e)
        {
           MessageReceiveThread.stopThread();
            Logger.Info("The CustomsDeclarationProxy Stop...");
        }

        void Application_Error(object sender, EventArgs e)
        {
            // 在出现未处理的错误时运行的代码

        }

        void Session_Start(object sender, EventArgs e)
        {
            // 在新会话启动时运行的代码

        }

        void Session_End(object sender, EventArgs e)
        {
            // 在会话结束时运行的代码。 
            // 注意: 只有在 Web.config 文件中的 sessionstate 模式设置为
            // InProc 时，才会引发 Session_End 事件。如果会话模式设置为 StateServer 
            // 或 SQLServer，则不会引发该事件。

        }

    }
}
