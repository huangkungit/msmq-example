using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Messaging;
using System.Xml;
using System.Transactions;
using CustomsDeclarationProxy.Dao;
using CustomsDeclarationProxy.Log;




namespace CustomsDeclarationProxy.Message
{
    public class MsgQueue
    {

        private String queuePath;

        public String QueuePath
        {
            get { return queuePath; }
            set { queuePath = value; }
        }

        private MessageQueue mq; 

        

        public MsgQueue Createqueue(string path)
        {
            try
            {

                QueuePath = path;
                mq = new MessageQueue(path);
                mq.Formatter = new XmlMessageFormatter(new Type[] { typeof(XmlDocument) });
                return this;

            }
            catch (MessageQueueException e)
            {
                Logger.Error("connect messageQueue: " + QueuePath + "fail!", e);
            }

            return null;

        }


        public void SendMessage(XmlDocument xmldoc, MessageQueueTransaction msgTransaction, string messageId)
        {
            try
            {

                System.Messaging.Message myMessage = new System.Messaging.Message();               
                myMessage.Body = xmldoc;
                myMessage.Label = messageId;
                mq.Send(myMessage, msgTransaction);       
                Logger.Info(messageId + "send message success！");

            }
            catch (Exception e)
            {
                Logger.Error("send message fail!", e);
                throw e;

            }
        }


        public void SendEncryptMessage(string encryptMsg, MessageQueueTransaction msgTransaction, string messageId)
        {
            try
            {

                System.Messaging.Message myMessage = new System.Messaging.Message();
                myMessage.Formatter = new XmlMessageFormatter(new Type[] { typeof(string) });
                myMessage.Body = encryptMsg;
                myMessage.Label = messageId;
                mq.Send(myMessage, msgTransaction);
                Logger.Info(messageId + "send encrypt message success！");

            }
            catch (Exception e)
            {
                Logger.Error("send encrypt message fail!", e);
                throw e;

            }
        }


        public System.Messaging.Message ReceiveMessage(MessageQueueTransaction msgTransaction)
        {

            System.Messaging.Message msg = new System.Messaging.Message();
          
            try
            {
                msg = mq.Receive(TimeSpan.FromMilliseconds(1000), msgTransaction);
                Logger.Info("receive message successful!");
                return msg; 
            }
            catch (MessageQueueException e) 
            {
                if (e.MessageQueueErrorCode != MessageQueueErrorCode.IOTimeout)
                {
                 
                    throw e;
                }
                return null;
            }
                             
        }

        public void ClearMessage()
        {
            if (MessageQueue.Exists(QueuePath))
            {
                MessageQueue queue = new MessageQueue(QueuePath);
                queue.Purge();
            }
        }

    }
}
