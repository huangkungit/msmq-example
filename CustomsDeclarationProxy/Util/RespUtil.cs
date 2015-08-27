using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using CustomsDeclarationProxy.Log;
using CustomsDeclarationProxy.Config;
using CustomsDeclarationProxy.Constant;

namespace CustomsDeclarationProxy.Util
{
    public class RespUtil
    {
        public static ConfigUtil cu = ConfigUtil.createInstance();
        public static string getOutIdFromResp(XmlDocument doc, CustomsMessageType cmt, SendPlace sp)
        {

            string des = getTheSendPlace(sp);
            string outDeclNo = doc.SelectSingleNode(cu.getOutIdPath(cmt,des)).InnerText;
            return outDeclNo;

        }

        public static string getRecMsgIdFromResp(XmlDocument doc, CustomsMessageType cmt, SendPlace sp)
        {

            string des = getTheSendPlace(sp);
            string recMsgId = doc.SelectSingleNode(cu.getRecMsgIdPathByMsgType(cmt, des)).InnerText;
            return recMsgId;

        }

        public static string getTheSendPlace(SendPlace sp)
        {
            String des = "";
            if (sp == SendPlace.CUSTOMS)
            {
                des = "customs";
            }
            else if (sp == SendPlace.GOVERNMENT)
            {
                des = "government";
            }
            return des;
            
        }

    }
}
