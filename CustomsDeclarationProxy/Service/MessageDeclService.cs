using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using CustomsDeclarationProxy.Dao;

namespace CustomsDeclarationProxy.Service
{
    public class MessageDeclService
    {

       private volatile static MessageDeclService messageDeclService = null;
       private static readonly object lockHelper = new object();
       private MessageDeclService() { }
       public static MessageDeclService createInstance()
       {
           if (messageDeclService == null)
           {
               lock (lockHelper)
               {
                   if (messageDeclService == null)
                       messageDeclService = new MessageDeclService();
               }
           }
           return messageDeclService;

       }

       private MessageDeclDao mdd = new MessageDeclDao();

       public void createDeclMessage(string messageId, string outId, int type, string messageDetailXml, int place)
       {
           decl_message dm = new decl_message();

           dm.out_decl_no = outId;
           dm.send_message_id = messageId;
           dm.message_detail = messageDetailXml;
           dm.message_type = type;
           dm.send_place = place;
           mdd.InsertData(dm);

       }

    }
}