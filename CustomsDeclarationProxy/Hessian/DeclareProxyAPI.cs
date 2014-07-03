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
using CustomsDeclarationProxy.Util;
using System.Text;
using CustomsDeclarationProxy.Config;

namespace CustomsDeclarationProxy.Hessian
{
    public class DeclareProxyAPI : CHessianHandler, IDeclareProxyAPI
    {

        private MessageDeclService messageDeclService = MessageDeclService.createInstance();
        private MessageRespService messageRespService = MessageRespService.createInstance();
        private ConfigUtil configUtil = ConfigUtil.createInstance();
        private MsgQueueFactory mqFactory = new MsgQueueFactory();
        private MsgQueue mq = null;

        public Boolean sendDeclMessage(string messageId, string outId, int sendType, string messageDetail, int place)
        {
            if (messageId == null && messageId == ""
                && outId == null && outId == ""
                && messageDetail == null && messageDetail == ""
                && sendType == 0
                && place == 0)
            {
                return false;
            }
            MessageQueueTransaction msgTransaction = new MessageQueueTransaction();

            try
            {
                mq = mqFactory.CreateMsgQueueFactory(sendType, place);
                
                
                using (TransactionScope scope = new TransactionScope())
                {

                    msgTransaction.Begin();
                    XmlDocument xmldoc = new XmlDocument();

                    

                    if (place == (int)SendPlace.GOVERNMENT)
                    {

                        String key = configUtil.getGovPwd();
                        messageDetail = AESUtil.AesEncoding(xmldoc.InnerXml, key, Encoding.UTF8);
                        xmldoc.Load(messageDetail);
                        mq.SendEncryptMessage(messageDetail, msgTransaction, messageId);
                    }
                    else
                    {
                       
                        xmldoc.LoadXml(messageDetail);
                        mq.SendMessage(xmldoc, msgTransaction, messageId);
                    }
                    Logger.Debug(messageDetail);

                    messageDeclService.createDeclMessage(messageId, outId, sendType, messageDetail,place);
                    messageRespService.createResponseMessage(messageId, outId, sendType,place);

                   

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

        public String getResponseMessageByMessageId(string msgId, int type, int place)
        {
           return  messageRespService.getRespMsgDetailByMsgId(msgId, type, place);

        }


    }  
    
}