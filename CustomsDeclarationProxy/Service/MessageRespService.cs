using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CustomsDeclarationProxy.Dao;

namespace CustomsDeclarationProxy.Service
{
    public class MessageRespService
    {
        MessageRespDao mrd = new MessageRespDao();

        private volatile static MessageRespService messageResponseService = null;
        private static readonly object lockHelper = new object();
        private MessageRespService() { }

        public static MessageRespService createInstance()
        {
            if(messageResponseService == null)
            {
                lock(lockHelper)
                {
                    if(messageResponseService==null)
                        messageResponseService = new MessageRespService();
                
                }
            }
            return messageResponseService;
        }

        public void createResponseMessage(string sendMessageId, string outId, int msgType)
        {
            resp_message rm = new resp_message();
            rm.send_message_id = sendMessageId;
            rm.message_type = msgType;
            rm.out_decl_no = outId;
            rm.send_place = 1;
            mrd.insertData(rm);

        }

        public String queryMessageDetail(string id, int type)
        {
            return mrd.queryMessageDetail(id, type);
        }

        public void updateRespMsgDetail(string outId, string msgDetail, string recMsgId, int type)
        {
            mrd.updateRespMsgDetail(outId, msgDetail,recMsgId, type);
        }

        public string getRespMsgDetailByMsgId(string msgId, int type)
        {
            return mrd.getRespMsgDetailByMsgId(msgId, type);
        }
    }
}