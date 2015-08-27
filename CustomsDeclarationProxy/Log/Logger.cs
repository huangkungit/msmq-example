using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Reflection;
using log4net;


 
[assembly: log4net.Config.XmlConfigurator(ConfigFile = "log4Net.config", Watch = true)]  
namespace CustomsDeclarationProxy.Log 
{  
 
    public static class Logger  
    {  
        private static ILog log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);  
  
        public static ILog GetILog  
        {  
            get  
            {  
                return log;  
            }  
        }  

        public static void Error(object message, Exception ex)  
        {  
            log.Error(message, ex);  
        }  
 
        public static void Fatal(object message, Exception ex)  
        {  
            log.Fatal(message, ex);  
        }  
  
        public static void Info(object message)  
        {  
            log.Info(message);  
        }  

        public static void Debug(object message)  
        {  
            log.Debug(message);  
        }  
 
        public static void Warn(object message)  
        {  
            log.Warn(message);  
        }  
    }  
}  
