using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;

namespace CustomsDeclarationProxy.Util
{
    public class RespUtil
    {
        public static string getOutIdFromManifestResp(XmlDocument doc)
        {
            XmlNamespaceManager xmlns = new XmlNamespaceManager(doc.NameTable);
            xmlns.AddNamespace("nm", "urn:Declaration:datamodel:standard:CN:NJHGKJ_110:1");

            string outDeclNo = doc.SelectSingleNode("nm:XmlMessage/nm:Response/nm:EX_DECL_HEAD/nm:OUT_DECL_NO", xmlns).InnerText;

            return outDeclNo;

        }

        public static string getRecMsgIdFromManifestResp(XmlDocument doc)
        {
            XmlNamespaceManager xmlns = new XmlNamespaceManager(doc.NameTable);
            xmlns.AddNamespace("nm", "urn:Declaration:datamodel:standard:CN:NJHGKJ_110:1");

            string recMsgId = doc.SelectSingleNode("nm:XmlMessage/nm:MessageHead/nm:MessageID", xmlns).InnerText;

            return recMsgId;

        }

        public static string getOutIdFromOrderResp(XmlDocument doc)
        {
            XmlNamespaceManager xmlns = new XmlNamespaceManager(doc.NameTable);
            xmlns.AddNamespace("nm", "urn:Declaration:datamodel:standard:CN:NJHGKJ_120:1");
            string outDeclNo = doc.SelectSingleNode("nm:XmlMessage/nm:Response/nm:ORDER_FORM_HEAD/nm:ORG_ORDER_NO", xmlns).InnerText;

            return outDeclNo;

        }

        public static string getRecMsgIdFromOrderResp(XmlDocument doc)
        {
            XmlNamespaceManager xmlns = new XmlNamespaceManager(doc.NameTable);
            xmlns.AddNamespace("nm", "urn:Declaration:datamodel:standard:CN:NJHGKJ_120:1");

            string recMsgId = doc.SelectSingleNode("nm:XmlMessage/nm:MessageHead/nm:MessageID", xmlns).InnerText;

            return recMsgId;

        }

        public static string getOutIdFromGoodsResp(XmlDocument doc)
        {
            XmlNamespaceManager xmlns = new XmlNamespaceManager(doc.NameTable);
            xmlns.AddNamespace("nm", "urn:Declaration:datamodel:standard:CN:NJHGKJ_100:1");

            string outId = doc.SelectSingleNode("nm:XmlMessage/nm:Response/nm:EX_GOODS_INFO/nm:OUT_ID", xmlns).InnerText;                                 
            return outId;

        }

        public static string getRecMsgIdFromGoodsResp(XmlDocument doc)
        {
            XmlNamespaceManager xmlns = new XmlNamespaceManager(doc.NameTable);
            xmlns.AddNamespace("nm", "urn:Declaration:datamodel:standard:CN:NJHGKJ_100:1");

            string recMsgId = doc.SelectSingleNode("nm:XmlMessage/nm:MessageHead/nm:MessageID", xmlns).InnerText;

            return recMsgId;
        }



    }
}