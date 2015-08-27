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
using CustomsDeclarationProxy.MessageThread;
using CustomsDeclarationProxy.Config;

namespace CustomsDeclarationProxy.MessageThread
{
    public static class MessageReceiveThread
    {
       
        static private int threadNumber = 6;
        static private ConfigUtil cf = ConfigUtil.createInstance();
        static private Thread[] receiveMessageThreadArray = new Thread[threadNumber];
        static private MsgQueue customsGoodsRespQueue ;
        static private MsgQueue customsDeclRespQueue ;
        static private MsgQueue customsOrderRespQueue;

        static private MsgQueue govGoodsRespQueue ;
        static private MsgQueue govDeclRespQueue;
        static private MsgQueue govOrderRespQueue;

        static public bool flag;
        static private MessageRespService msgRespService = MessageRespService.createInstance();

        static public void startThread()
        {
            try
            {
                customsGoodsRespQueue = new MsgQueue().Createqueue(cf.getDeclMsgQueueAddr("customsGoodsRespUrl"));
                customsDeclRespQueue = new MsgQueue().Createqueue(cf.getDeclMsgQueueAddr("customsManifestRespUrl"));
                customsOrderRespQueue = new MsgQueue().Createqueue(cf.getDeclMsgQueueAddr("customsOrderRespUrl"));

                govGoodsRespQueue = new MsgQueue().Createqueue(cf.getDeclMsgQueueAddr("govGoodsRespUrl"));
                govDeclRespQueue = new MsgQueue().Createqueue(cf.getDeclMsgQueueAddr("govManifestRespUrl"));
                govOrderRespQueue = new MsgQueue().Createqueue(cf.getDeclMsgQueueAddr("govOrderRespUrl"));
                flag = true;
                int counter;
                receiveMessageThreadArray[0] = new Thread(new ThreadStart(new MsgListen(customsGoodsRespQueue,CustomsMessageType.GOODS,SendPlace.CUSTOMS ).ThreadProc));
                receiveMessageThreadArray[1] = new Thread(new ThreadStart(new MsgListen(customsDeclRespQueue, CustomsMessageType.MANIFEST, SendPlace.CUSTOMS).ThreadProc));
                receiveMessageThreadArray[2] = new Thread(new ThreadStart(new MsgListen(customsOrderRespQueue, CustomsMessageType.ORDER, SendPlace.CUSTOMS).ThreadProc));

                receiveMessageThreadArray[3] = new Thread(new ThreadStart(new MsgListen(govGoodsRespQueue, CustomsMessageType.GOODS, SendPlace.GOVERNMENT).ThreadProc));
                receiveMessageThreadArray[4] = new Thread(new ThreadStart(new MsgListen(govDeclRespQueue, CustomsMessageType.MANIFEST, SendPlace.GOVERNMENT).ThreadProc));
                receiveMessageThreadArray[5] = new Thread(new ThreadStart(new MsgListen(govOrderRespQueue, CustomsMessageType.ORDER, SendPlace.GOVERNMENT).ThreadProc));

                for (counter = 0; counter < threadNumber; counter++)
                {
                    receiveMessageThreadArray[counter].Start();
                }
                Logger.Info("start receive thread success!");

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
                    Logger.Info(counter+" close sucess");
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
