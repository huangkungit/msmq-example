using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CustomsDeclarationProxy.Constant;
using CustomsDeclarationProxy.Message;

namespace CustomsDeclarationProxy.Factory
{
    public class MsgQueueFactory
    {
        public MsgQueue CreateMsgQueueFactory(int type)
        {
            if (type == (int)MessageType.GOODS)
            {
                return new MsgQueue().Createqueue("FormatName:Direct=TCP:121.52.213.102\\private$\\LITB_GOODS_APL");

            }
            else if(type == (int)MessageType.MANIFEST)
            {
                return new MsgQueue().Createqueue("FormatName:Direct=TCP:121.52.213.102\\private$\\LITB_DECL_APL");

            }
            else if (type == (int)MessageType.ORDER)
            {
                return new MsgQueue().Createqueue("FormatName:Direct=TCP:121.52.213.102\\private$\\LITB_ORDER_APL");

            }
            else
            {
                throw new Exception("No Such Type MsgQueue!");   
            }

        }
    }
}