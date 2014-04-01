using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CustomsDeclarationProxy.Dao
{
    public class MessageDeclDao
    {
        public void InsertData(decl_message dm)
        {
            using (var ctx = new cdiEntities())
            {
                ctx.AddTodecl_message(dm);
                ctx.SaveChanges();
            }
        }

    }
}