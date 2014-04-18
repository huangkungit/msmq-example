using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading;
using System.Transactions;
using System.Messaging;
using System.Xml;
using CustomsDeclarationProxy.Message;
using CustomsDeclarationProxy.Log;
using CustomsDeclarationProxy.Util;
using CustomsDeclarationProxy.Service;
using CustomsDeclarationProxy.Constant;
using CustomsDeclarationProxy.MessgeThread;
using CustomsDeclarationProxy.Config;
namespace CustomsDeclarationProxy.MessageThread
{
    public static class MessageReceiveThread
    {
        static private int threadNumber = 3;
        static private Thread[] receiveMessageThreadArray = new Thread[threadNumber];
        static private MsgQueue GoodsRespQueue = new MsgQueue().Createqueue(".\\private$\\LITB_GOODS_RESP_APL");
        static private MsgQueue DeclRespQueue = new MsgQueue().Createqueue(".\\private$\\LITB_DECL_RESP_APL");
        static private MsgQueue OrderRespQueue = new MsgQueue().Createqueue(".\\private$\\LITB_ORDER_RESP_APL");
        static public bool flag;
        static private MessageRespService msgRespService = MessageRespService.createInstance();

        static public void startThread()
        {
            try
            {
                flag = true;
                int counter;
                receiveMessageThreadArray[0] = new Thread(new ThreadStart(new MsgListen(GoodsRespQueue,CustomsMessageType.GOODS).ThreadProc));
                receiveMessageThreadArray[1] = new Thread(new ThreadStart(new MsgListen(DeclRespQueue, CustomsMessageType.MANIFEST).ThreadProc));
                receiveMessageThreadArray[2] = new Thread(new ThreadStart(new MsgListen(OrderRespQueue,CustomsMessageType.ORDER).ThreadProc));

                for (counter = 0; counter < threadNumber; counter++)
                {
                    receiveMessageThreadArray[counter].Start();
                }
                Logger.Info("start receive thread success!");

                ConfigUtil cu = ConfigUtil.createInstance();
                String str = cu.getDeclMsgQueueAddr("orderDeclUrl");

            }
            catch (Exception e)
            {
                Logger.Error("start receive thread failed!", e);
            }

        }

        static public void stopThread()
        {
            try
            {
                flag = false;
                int counter;
                for (counter = 0; counter < threadNumber; counter++)
                {
                    receiveMessageThreadArray[counter].Join();
                }
                Logger.Info("stop message receive thread");
            }
            catch (Exception e)
            {
                Logger.Error("stop receive thread failed!", e);
            }
        }
        
    }
}