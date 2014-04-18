using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CustomsDeclarationProxy.Constant;
using CustomsDeclarationProxy.Message;
using CustomsDeclarationProxy.Config;
namespace CustomsDeclarationProxy.Factory
{
    public class MsgQueueFactory
    {
        public ConfigUtil configUtil = ConfigUtil.createInstance();
        public MsgQueue CreateMsgQueueFactory(int type)
        {
            if (type == (int)CustomsMessageType.GOODS)
            {
                return new MsgQueue().Createqueue(configUtil.getDeclMsgQueueAddr("goodsDeclUrl"));

            }
            else if(type == (int)CustomsMessageType.MANIFEST)
            {
                String str = configUtil.getDeclMsgQueueAddr("manifestDeclUrl");
                return new MsgQueue().Createqueue(configUtil.getDeclMsgQueueAddr("manifestDeclUrl"));

            }
            else if (type == (int)CustomsMessageType.ORDER)
            {           
                return new MsgQueue().Createqueue(configUtil.getDeclMsgQueueAddr("orderDeclUrl"));

            }
            else
            {
                throw new Exception("No Such Type MsgQueue!");   
            }

        }
    }
}