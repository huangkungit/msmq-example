using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CustomsDeclarationProxy.Dao
{
    public class MessageRespDao
    {
        public void insertData(resp_message rm)
        {
            using (var ctx = new cdiEntities())
            {
                ctx.AddToresp_message(rm);
                ctx.SaveChanges();
            }
        }

        public String queryMessageDetail(string id, int type)
        {

            using (var ctx = new cdiEntities())
            {
               
                var results = ctx.resp_message.SingleOrDefault(c => c.send_message_id == id && c.message_type == type);

                ctx.SaveChanges();
                if (results == null)
                {
                    return null;
                }
                return results.message_detail;
            }
        }

        public string getRespMsgDetailByMsgId(string msgId, int type)
        {
            using (var ctx = new cdiEntities())
            {

                var results = ctx.resp_message.SingleOrDefault(c => c.send_message_id == msgId && c.message_type == type);

                ctx.SaveChanges();
                if (results == null)
                {
                    return null;
                }

                return results.message_detail;
            }


        }

        public void updateRespMsgDetail(string outId, string msgDetail, string recMsgId ,int type)
        {
            using (var ctx = new cdiEntities())
            {
                var results = ctx.resp_message.Where(c => c.out_decl_no == outId && c.message_type == type);

                foreach (resp_message c in results)
                {
                    c.message_detail = msgDetail;
                    c.receive_message_id = recMsgId;
                }
                ctx.SaveChanges();
            }
        }


    }
}