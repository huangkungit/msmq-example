using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading;
using System.Transactions;
using System.Xml;
using CustomsDeclarationProxy.Message;
using CustomsDeclarationProxy.Log;
using CustomsDeclarationProxy.Util;
using CustomsDeclarationProxy.Service;
using CustomsDeclarationProxy.Constant;

namespace CustomsDeclarationProxy.MessageThread
{
    public static class MessageReceiveThread
    {
        static private int threadNumber = 3;
        static private Thread[] receiveMessageThreadArray = new Thread[threadNumber];
        static private MsgQueue GoodsRespQueue = new MsgQueue().Createqueue(".\\private$\\LITB_GOODS_RESP_APL");
        static private MsgQueue DeclRespQueue = new MsgQueue().Createqueue(".\\private$\\LITB_DECL_RESP_APL");
        static private MsgQueue OrderRespQueue = new MsgQueue().Createqueue(".\\private$\\LITB_ORDER_RESP_APL");
        static private bool flag;
        static private MessageRespService msgRespService = MessageRespService.createInstance();

        static public void startThread()
        {
            try
            {
                flag = true;
                int counter;
                receiveMessageThreadArray[0] = new Thread(new ThreadStart(DeclRespMsgListen));
                receiveMessageThreadArray[1] = new Thread(new ThreadStart(GoodsRespMsgListen));
                receiveMessageThreadArray[2] = new Thread(new ThreadStart(OrderRespMsgListen));

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
                }
                Logger.Info("stop message receive thread");
            }
            catch (Exception e)
            {
                Logger.Error("stop receive thread failed!", e);
            }
        }

        static private void DeclRespMsgListen()
        {
            while (flag)
            {
                try
                {
                    using (TransactionScope commodityTransaction = new TransactionScope())
                    {
                        System.Messaging.Message msg = DeclRespQueue.ReceiveMessage();
                        if (msg == null)
                        {
                            commodityTransaction.Complete();
                            continue;
                        }

                        
                        if ((XmlDocument)msg.Body != null)
                        {
                            string xmlmsg = "";
                            xmlmsg = ((XmlDocument)msg.Body).InnerXml; //get the message;
                            XmlDocument doc = new XmlDocument();
                            doc.LoadXml(xmlmsg);                         

                            string outId = RespUtil.getOutIdFromManifestResp(doc);
                            string recMsgId = RespUtil.getRecMsgIdFromManifestResp(doc);

                            msgRespService.updateRespMsgDetail(outId, xmlmsg, recMsgId, (int)MessageType.MANIFEST);

                            commodityTransaction.Complete();
                            Logger.Debug(xmlmsg);
                        }
                        
                    }
                }
                catch (Exception e)
                {
                    Logger.Error("decl response message receive / update failed", e);
                }

            }

        }

        static private void GoodsRespMsgListen()
        {
            while (flag)
            {
                try
                {
                    using (TransactionScope goodsTransaction = new TransactionScope())
                    {
                        System.Messaging.Message msg = GoodsRespQueue.ReceiveMessage();

                        if (msg == null)
                        {
                            goodsTransaction.Complete();
                            continue;
                        }

                        
                        if ((XmlDocument)msg.Body != null)
                        {
                            string xmlmsg = "";
                            xmlmsg = ((XmlDocument)msg.Body).InnerXml; //get the message;
                            XmlDocument doc = new XmlDocument();
                            doc.LoadXml(xmlmsg);
                                

                            string outId = RespUtil.getOutIdFromGoodsResp(doc);
                            string recMsgId = RespUtil.getRecMsgIdFromGoodsResp(doc);

                            msgRespService.updateRespMsgDetail(outId, doc.InnerXml, recMsgId,(int)MessageType.GOODS);

                            goodsTransaction.Complete();
                            Logger.Info("outId:"+outId);
                            Logger.Info("recMsgId:"+ recMsgId);
                            Logger.Info(xmlmsg);
                        }
                    }
                }
                catch (Exception e)
                {
                    Logger.Error("goods response message receive / update failed", e);
                }

            }

        }

        static private void OrderRespMsgListen()
        {
            while (flag)
            {
                try
                {
                    using (TransactionScope orderTransaction = new TransactionScope())
                    {
                        System.Messaging.Message msg = OrderRespQueue.ReceiveMessage();

                        if (msg == null)
                        {
                            orderTransaction.Complete();
                            continue;
                        }


                        if ((XmlDocument)msg.Body != null)
                        {
                            string xmlmsg = "";
                            xmlmsg = ((XmlDocument)msg.Body).InnerXml; //get the message;
                            Logger.Debug(xmlmsg);
                            XmlDocument doc = new XmlDocument();
                            doc.LoadXml(xmlmsg);


                            string outId = RespUtil.getOutIdFromOrderResp(doc);
                            string recMsgId = RespUtil.getRecMsgIdFromOrderResp(doc);

                            msgRespService.updateRespMsgDetail(outId, doc.InnerXml, recMsgId, (int)MessageType.ORDER);

                            orderTransaction.Complete();
                            Logger.Debug("outId:" + outId);
                            Logger.Debug("recMsgId:" + recMsgId);
                            Logger.Debug(xmlmsg);
                        }
                    }
                }
                catch (Exception e)
                {
                    Logger.Error("order response message receive / update failed", e);
                }

            }

        }
   
        
    }
}