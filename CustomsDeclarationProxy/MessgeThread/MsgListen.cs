using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Transactions;
using System.Messaging;
using System.Xml;
using System.Text;
using CustomsDeclarationProxy.Message;
using CustomsDeclarationProxy.Constant;
using CustomsDeclarationProxy.MessageThread;
using CustomsDeclarationProxy.Log;
using CustomsDeclarationProxy.Util;
using CustomsDeclarationProxy.Service;
using CustomsDeclarationProxy.Config;

namespace CustomsDeclarationProxy.MessageThread
{
    public class MsgListen
    {
        MsgQueue respQueue;

        CustomsMessageType cmt;

        SendPlace sendPlace;

        MessageRespService msgRespService = MessageRespService.createInstance();

        private ConfigUtil configUtil = ConfigUtil.createInstance();

        public MsgListen(MsgQueue respQueue, CustomsMessageType cmt, SendPlace sp)
        {
            this.respQueue = respQueue;
            this.cmt = cmt;
            this.sendPlace = sp;
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
                            if (sendPlace == SendPlace.GOVERNMENT)
                            {
                                String key = configUtil.getGovPwd();
                                xmlmsg = AESUtil.AesDecoding(xmlmsg, key, Encoding.UTF8);
                            }
                            doc.LoadXml(xmlmsg);

                            string outId = "";
                            string recMsgId = "";
                            try
                            {
                                outId = RespUtil.getOutIdFromResp(doc, cmt);
                                recMsgId = RespUtil.getRecMsgIdFromResp(doc, cmt);
                            }
                            catch (Exception e)
                            {
                                Logger.Error("Can't get the outId or recMsgId from RespMsg", e);
                            }
   
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