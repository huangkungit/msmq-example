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
        /// <summary>
        /// 通过Create方法创建使用指定路径的新消息队列
        /// </summary>
        /// <param name="queuePath"></param>
        /// 

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
                //MessageBox.Show(e.Message);
                Logger.Error("connect messageQueue: " + QueuePath + "fail!", e);
            }

            return null;

        }

        /// <summary>
        /// 连接消息队列并发送消息到队列
        /// </summary>
        public void SendMessage(XmlDocument xmldoc, MessageQueueTransaction msgTransaction, string messageId)
        {
            try
            {
                //连接到接受消息的队列
                System.Messaging.Message myMessage = new System.Messaging.Message();               
                myMessage.Body = xmldoc;
                myMessage.Label = messageId;
                //发送消息到队列中
               // mq.Send(myMessage, MessageQueueTransactionType.Automatic);
                mq.Send(myMessage, msgTransaction);       
                Logger.Info(messageId + "send message success！");

            }
            catch (Exception e)
            {
                Logger.Error("send message fail!", e);
                throw e;

            }
        }


        /// <summary>
        /// 连接消息队列并发送消息到队列(加密)
        /// </summary>
        public void SendEncryptMessage(string encryptMsg, MessageQueueTransaction msgTransaction, string messageId)
        {
            try
            {
                //连接到接受消息的队列
                System.Messaging.Message myMessage = new System.Messaging.Message();
                myMessage.Body = encryptMsg;
                myMessage.Label = messageId;
                //发送消息到队列中
                // mq.Send(myMessage, MessageQueueTransactionType.Automatic);
                mq.Send(myMessage, msgTransaction);
                Logger.Info(messageId + "send encrypt message success！");

            }
            catch (Exception e)
            {
                Logger.Error("send encrypt message fail!", e);
                throw e;

            }
        }

        /// <summary>
        /// 连接消息队列并从队列中接收消息
        /// </summary>
        public System.Messaging.Message ReceiveMessage(MessageQueueTransaction msgTransaction)
        {
            //连接到本地队列
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


        /// <summary>
        /// 清空消息
        /// </summary>
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
