using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using CustomsDeclarationProxy.Constant;
using System.Configuration;

namespace CustomsDeclarationProxy.Config
{
    public class ConfigUtil
    {
        private XmlDocument doc;

        private volatile static ConfigUtil configUtil = null;
        private static readonly object lockHelper = new object();

        private ConfigUtil()
        {
              doc=new XmlDocument();
              string configurl = ConfigurationManager.AppSettings["configurl"].ToString();
              doc.Load(System.Web.HttpContext.Current.Server.MapPath(configurl));
        }

        public static ConfigUtil createInstance()
        {
            if (configUtil == null)
            {
                lock (lockHelper)
                {
                    if (configUtil == null)
                        configUtil = new ConfigUtil();
                }
            }
            return configUtil;
        }

        public String getDeclMsgQueueAddr(String str){
            return doc.SelectSingleNode("config/" + str).InnerText;
        }

        public String getGovPwd()
        {
            return doc.SelectSingleNode("config/sendHelper/government/password").InnerText;
        }

        public String getRespMsgQueueAddr(String str)
        {
            return doc.SelectSingleNode("config/" + str).InnerText;
        }

        /*
        public String getNameSpaceByMsgType(CustomsMessageType cmt)
        {
            if (cmt == CustomsMessageType.GOODS)
            {
                return doc.SelectSingleNode("config/responseHelper/goods/ns").InnerText;
            }
            else if (cmt == CustomsMessageType.MANIFEST)
            {
                return doc.SelectSingleNode("config/responseHelper/manifest/ns").InnerText;
            }
            else if (cmt == CustomsMessageType.ORDER)
            {
                return doc.SelectSingleNode("config/responseHelper/order/ns").InnerText;
            }

            return "";
        }
        */


        public String getOutIdPath(CustomsMessageType cmt)
        {
           
            if (cmt == CustomsMessageType.GOODS)
            {
                return doc.SelectSingleNode("config/responseHelper/customs/goods/outId").InnerText;
            }
            else if (cmt == CustomsMessageType.MANIFEST)
            {
                return doc.SelectSingleNode("config/responseHelper/customs/manifest/outId").InnerText;
            }
            else if (cmt == CustomsMessageType.ORDER)
            {
                return doc.SelectSingleNode("config/responseHelper/customs/order/outId").InnerText;
            }
                

            return "";
        }


        public String getRecMsgIdPathByMsgType(CustomsMessageType cmt)
        {
            if (cmt == CustomsMessageType.GOODS)
            {
                return doc.SelectSingleNode("config/responseHelper/customs/goods/recMsgId").InnerText;
            }
            else if (cmt == CustomsMessageType.MANIFEST)
            {
                return doc.SelectSingleNode("config/responseHelper/customs/manifest/recMsgId").InnerText;
            }
            else if (cmt == CustomsMessageType.ORDER)
            {
                return doc.SelectSingleNode("config/responseHelper/customs/order/recMsgId").InnerText;
            }

            return "";
        }
    } 
}