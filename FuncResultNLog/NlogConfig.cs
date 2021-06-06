using System.Collections.Generic;
using System.Linq;
using NLog;
using NLog.Config;
using NLog.LayoutRenderers;
using NLog.Layouts;
using NLog.Targets;

namespace FuncResultNLog
{
    public class NlogConfig
    {
        private LoggingConfiguration _Config = new LoggingConfiguration();

        private JsonLayout _Json = new JsonLayout();


        public NlogConfig(string jsonFilePath, bool enableConsoleResult)
        {
            //Target 2
            var consoleTarget = new ColoredConsoleTarget();
            _Config.AddTarget("console", consoleTarget);
            var fileTarget = new FileTarget("ResultBaseFileTarget");
            _Config.AddTarget(fileTarget);

            // Step 3. Set target properties 
            // basic layout
            fileTarget.FileName = jsonFilePath;


            _Json.Attributes.Add(new JsonAttribute("Level", "${level:upperCase=true}"));
            _Json.Attributes.Add(new JsonAttribute("LongDate", "${longdate}"));
            _Json.Attributes.Add(new JsonAttribute("LogerName", "${logger}"));
            _Json.Attributes.Add(new JsonAttribute("Message", "${message}"));

            // Custom Properties
            _Json.Attributes.Add(new JsonAttribute("AssemblyName", "${event-properties:item=AssemblyName}", true));
            _Json.Attributes.Add(new JsonAttribute("MemberName", "${event-properties:item=MemberName}", true));
            _Json.Attributes.Add(new JsonAttribute("FilePath", "${event-context:item=FilePath}", true));
            _Json.Attributes.Add(new JsonAttribute("LineNumber", "${event-properties:item=LineNumber}", false));
            _Json.Attributes.Add(new JsonAttribute("Details", "${event-context:item=Details}", true));

            _Json.Attributes.Add(new JsonAttribute("Exception", new JsonLayout
            {
                Attributes =
                {
                    new JsonAttribute("toString", "${exception:format=toString}"),
                     new JsonAttribute("Data", "${exception:format=Data}"),
                     
                    new JsonAttribute("InnerException", new JsonLayout
                    {
                        Attributes =
                        {
                             new JsonAttribute("toString",
                                              "${exception:format=:innerFormat=toString:MaxInnerExceptionLevel=5:InnerExceptionSeparator=#}"),
                             new JsonAttribute("Data",
                                              "${exception:format=:innerFormat=Data:MaxInnerExceptionLevel=5:InnerExceptionSeparator=#}"),
                            
                        }
                    }, false)
                }
            }, false));

            

            fileTarget.Layout = _Json;
            consoleTarget.Layout = _Json;

            // Step 4. Define rules
            if( enableConsoleResult )
            {
                var rule1 = new LoggingRule( "*", LogLevel.Trace, consoleTarget );
                _Config.LoggingRules.Add( rule1 );
            }

            var rule2 = new LoggingRule("*", LogLevel.Trace, fileTarget);
            _Config.LoggingRules.Add(rule2);

            // Step 5. Activate the configuration
            LogManager.Configuration = _Config;

        }

        public NlogConfig AddWindowsIdentity()
        {
            _Json.Attributes.Add(new JsonAttribute("WindowsIdentity", "${windows-identity}", true));
            return this;
        }

        public NlogConfig AddMachineName()
        {
            _Json.Attributes.Add(new JsonAttribute("Machine", "${machinename}"));
            
            return this;
        }

        public NlogConfig AddTime(bool universalTime)
        {
            _Json.Attributes.Add(new JsonAttribute("Time", $"${{time:universalTime={universalTime}}}"));
            return this;
        }

        public NlogConfig AddTicks()
        {
            _Json.Attributes.Add(new JsonAttribute("Ticks", "${ticks}"));
            return this;
        }

        public NlogConfig AddShortDate(bool universalTime)
        {
            _Json.Attributes.Add(new JsonAttribute("ShortDate", $"${{shortdate:universalTime={universalTime}}}"));
            return this;
        }

        public NlogConfig AddProcessData(List<ProcessInfoProperty> listOfProcessInfo )
        {

            var attr = new JsonLayout();
            attr.Attributes.Add(new JsonAttribute("ProcessTime", "${processtime}"));
             attr.Attributes.Add(new JsonAttribute("Processname", "${processname:fullName=true}"));                
            attr.Attributes.Add(new JsonAttribute("Processid", "${processid}"));                
           
            listOfProcessInfo.ForEach(f => attr.Attributes.Add(new JsonAttribute(f.ToString(), $"${{processinfo:property={f.ToString()}}}")));
            
            _Json.Attributes.Add(new JsonAttribute("ProcessData", attr, false));
            return this;
        }



        public NlogConfig AddThread()
        {
            _Json.Attributes.Add(new JsonAttribute("ThreadId", "${threadid}", false));
            _Json.Attributes.Add(new JsonAttribute("ThreadName", "${threadname}", false));
            return this;
        }

        public NlogConfig AddStackTrace(StackTraceFormat format = StackTraceFormat.DetailedFlat, int topFrames = 3, int skipFrames = 0)
        {
            _Json.Attributes.Add(new JsonAttribute("stacktrace", $"${{stacktrace:format={format.ToString()}:topFrames={topFrames}:skipFrames={skipFrames}:separator=#}}", false));
            return this;
        }

        //public NlogConfig AddRegistry(IEnumerable<RegistryLayoutRenderer> registrys)
        //{
        //    registrys?.ToList().ForEach(f =>
        //    {
        //        _Json.Attributes.Add(new JsonAttribute("Registrys", new JsonLayout
        //        {
        //            Attributes =
        //        {
        //        new JsonAttribute("Key", f.Key),
        //        new JsonAttribute("Value", f.Value),
        //        new JsonAttribute("Datos", $"${{registry:value={f.Value.ToString().Replace("'","")}:key={f.Key.ToString().Replace("'","")}:defaultValue={f.DefaultValue?.ToString()}:view={f.View.ToString()}:requireEscapingSlashesInDefaultValue={f.RequireEscapingSlashesInDefaultValue.ToString()}}}", true)
        //        }
        //        }, false));
        //    });

        //    return this;
        //}


        public NlogConfig AddCallSiteLine(int skipFrames)
        {
            _Json.Attributes.Add(new JsonAttribute("CallSiteLine", $"${{callsite-linenumber:skipFrames={skipFrames}}}"));
            return this;
        }


        public NlogConfig AddCallSite(CallSiteLayoutRenderer callSiteParam)
        {
            _Json.Attributes.Add(new JsonAttribute("CallSite", $"${{callsite:ClassName={callSiteParam.ClassName.ToString().Replace("'", "")}:fileName={callSiteParam.FileName.ToString().Replace("'", "")}:includeSourcePath={callSiteParam.IncludeSourcePath.ToString().Replace("'", "")}:methodName = {callSiteParam.MethodName.ToString().Replace("'", "")}:cleanNamesOfAnonymousDelegates = {callSiteParam.CleanNamesOfAnonymousDelegates.ToString().Replace("'", "")}:skipFrames = {callSiteParam.SkipFrames.ToString().Replace("'", "")}}}"));
            return this;
        }

        




    }
}
