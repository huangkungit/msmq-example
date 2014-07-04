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
        public static string getOutIdFromResp(XmlDocument doc, CustomsMessageType cmt)
        {
            //XmlNamespaceManager xmlns = new XmlNamespaceManager(doc.NameTable);
           // xmlns.AddNamespace("nm", cu.getNameSpaceByMsgType(cmt));

            
            string outDeclNo = doc.SelectSingleNode(cu.getOutIdPath(cmt)).InnerText;
            return outDeclNo;

        }

        public static string getRecMsgIdFromResp(XmlDocument doc, CustomsMessageType cmt)
        {
           // XmlNamespaceManager xmlns = new XmlNamespaceManager(doc.NameTable);
           // xmlns.AddNamespace("nm", cu.getNameSpaceByMsgType(cmt));
            
            string recMsgId = doc.SelectSingleNode(cu.getRecMsgIdPathByMsgType(cmt)).InnerText;
            return recMsgId;

        }

    }
}