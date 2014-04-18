using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Transactions;
using System.Messaging;
using System.Xml;
using CustomsDeclarationProxy.Message;
using CustomsDeclarationProxy.Constant;
using CustomsDeclarationProxy.MessageThread;
using CustomsDeclarationProxy.Log;
using CustomsDeclarationProxy.Util;
using CustomsDeclarationProxy.Service;

namespace CustomsDeclarationProxy.MessgeThread
{
    public class MsgListen
    {
        MsgQueue respQueue;

        CustomsMessageType cmt;

        MessageRespService msgRespService = MessageRespService.createInstance();

        public MsgListen(MsgQueue respQueue, CustomsMessageType cmt)
        {
            this.respQueue = respQueue;
            this.cmt = cmt;
        }

        public void ThreadProc()
        {
            while (MessageReceiveThread.flag)
            {
                MessageQueueTransaction msgTransaction = new MessageQueueTransaction();
                try
                {
                    msgTransaction.Begin();
                    using (TransactionScope respTransaction = new TransactionScope())
                    {
                        System.Messaging.Message msg = respQueue.ReceiveMessage(msgTransaction);

                        if (msg == null)
                        {
                            msgTransaction.Commit();
                            respTransaction.Complete();
                            continue;
                        }

                        if ((XmlDocument)msg.Body != null)
                        {
                            string xmlmsg = "";
                            xmlmsg = ((XmlDocument)msg.Body).InnerXml; //get the message;
                            XmlDocument doc = new XmlDocument();
                            doc.LoadXml(xmlmsg);
                            string outId = RespUtil.getOutIdFromResp(doc,cmt);
                            string recMsgId = RespUtil.getRecMsgIdFromResp(doc,cmt);
   
                            msgRespService.updateRespMsgDetail(outId, xmlmsg, recMsgId, (int)cmt);                       

                            respTransaction.Complete();
                            msgTransaction.Commit();
                            Logger.Debug(xmlmsg);
                        }

                    }
                }
                catch (Exception e)
                {
                    msgTransaction.Abort();
                    Logger.Error("response message receive / update failed", e);
                }
            }


        }
    }
}