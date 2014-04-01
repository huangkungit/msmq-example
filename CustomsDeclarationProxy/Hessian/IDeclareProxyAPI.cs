using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CustomsDeclarationProxy.Hessian
{
    public interface IDeclareProxyAPI
    {
        Boolean sendDeclMessage(string messageId, string outId, int sendType, string messageDetail);

        String getResponseMessageByMessageId(string messageId, int type);
    }
}
