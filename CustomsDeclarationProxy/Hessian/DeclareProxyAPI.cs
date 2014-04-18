using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using System.Transactions;
using hessiancsharp.server;
using System.Messaging;
using CustomsDeclarationProxy.Message;
using CustomsDeclarationProxy.Log;
using CustomsDeclarationProxy.Service;
using CustomsDeclarationProxy.Factory;
using CustomsDeclarationProxy.Constant;
namespace CustomsDeclarationProxy.Hessian
{
    public class DeclareProxyAPI : CHessianHandler, IDeclareProxyAPI
    {

        private MessageDeclService messageDeclService = MessageDeclService.createInstance();
        private MessageRespService messageRespService = MessageRespService.createInstance();
        private MsgQueueFactory mqFactory = new MsgQueueFactory();
        private MsgQueue mq = null;

        public Boolean sendDeclMessage(string messageId, string outId, int sendType, string messageDetail)
        {
            if (messageId == null && messageId == ""
                && outId == null && outId == ""
                && messageDetail == null && messageDetail == ""
                && sendType == 0)
            {
                return false;
            }
            MessageQueueTransaction msgTransaction = new MessageQueueTransaction();

            try
            {
                mq = mqFactory.CreateMsgQueueFactory(sendType);
                
                
                using (TransactionScope scope = new TransactionScope())
                {

                    msgTransaction.Begin();
                    XmlDocument xmldoc = new XmlDocument();

                    xmldoc.LoadXml(messageDetail);
                    messageDetail = xmldoc.InnerXml;

                    Logger.Debug(messageDetail);

                    messageDeclService.createDeclMessage(messageId, outId, sendType, messageDetail);
                    messageRespService.createResponseMessage(messageId, outId, sendType);

                    mq.SendMessage(xmldoc, msgTransaction, messageId);

                    scope.Complete();
                    msgTransaction.Commit();                  
                    return true;

                }
            } catch (Exception e)
            {
                msgTransaction.Abort();
                if ((int)CustomsDeclarationProxy.Constant.CustomsMessageType.MANIFEST== sendType)
                {
                    Logger.Error("shipmentPackId:" + outId + "send and insert manifest message failed!", e);
                }
                else if ((int)CustomsDeclarationProxy.Constant.CustomsMessageType.ORDER == sendType)
                {
                    Logger.Error("shipmentPackId:" + outId + "send and insert order message failed!", e);
                }
                return false;
            }

        }

        public String getResponseMessageByMessageId(string msgId, int type)
        {
           return  messageRespService.getRespMsgDetailByMsgId(msgId, type);

        }


    }  
    
}