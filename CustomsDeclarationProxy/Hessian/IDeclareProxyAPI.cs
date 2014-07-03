using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CustomsDeclarationProxy.Hessian
{
    public interface IDeclareProxyAPI
    {
        //place : 1(海关)，2（园区政府）
        Boolean sendDeclMessage(string messageId, string outId, int sendType, string messageDetail, int place);

        String getResponseMessageByMessageId(string messageId, int type, int place);

    }
}
