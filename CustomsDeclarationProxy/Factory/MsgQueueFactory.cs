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
            int place = 1;
            if (place == (int)SendPlace.CUSTOMS)
            {
                if (type == (int)CustomsMessageType.GOODS)
                {
                    return new MsgQueue().Createqueue(configUtil.getDeclMsgQueueAddr("customsGoodsDeclUrl"));

                }
                else if (type == (int)CustomsMessageType.MANIFEST)
                {

                    return new MsgQueue().Createqueue(configUtil.getDeclMsgQueueAddr("customsManifestDeclUrl"));

                }
                else if (type == (int)CustomsMessageType.ORDER)
                {
                    return new MsgQueue().Createqueue(configUtil.getDeclMsgQueueAddr("customsOrderDeclUrl"));

                }
                else
                {
                    throw new Exception("No Such Type customsMsgQueue!");
                }
            }
            else if (place == (int)SendPlace.GOVERNMENT)
            {
                if (type == (int)CustomsMessageType.GOODS)
                {
                    return new MsgQueue().Createqueue(configUtil.getDeclMsgQueueAddr("govGoodsDeclUrl"));

                }
                else if (type == (int)CustomsMessageType.MANIFEST)
                {

                    return new MsgQueue().Createqueue(configUtil.getDeclMsgQueueAddr("govManifestDeclUrl"));

                }
                else if (type == (int)CustomsMessageType.ORDER)
                {
                    return new MsgQueue().Createqueue(configUtil.getDeclMsgQueueAddr("govOrderDeclUrl"));

                }
                else
                {
                    throw new Exception("No Such Type govMsgQueue!");
                }

            }
            else
            {
                throw new Exception("No such sendPlace!");
            }


        }


    }
}