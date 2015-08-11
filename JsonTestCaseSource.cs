using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tsa.automation.sts2015.common;
using tsa.automation.sts2015.WCF;

namespace NunitTestCaseSourceExternal
{
    public class JsonTestCaseSource : ITestCaseSource
    {
        public System.Collections.IEnumerable GetTestCases(params object[] args)
        {
            string classPath = args.First().ToString();
            string methodName = args.Last().ToString();

            var testFolder = Path.GetFileNameWithoutExtension(classPath);

            string parentFilePath = string.Format("TestData\\{0}\\template.json", ConfigurationManager.AppSettings["Environment"]);
            JObject parentTemplate = null;
            if (File.Exists(parentFilePath))
            {
                var json = File.ReadAllText(parentFilePath);
                parentTemplate = (JObject)JsonConvert.DeserializeObject(json);
            }

            string baseFilePath = string.Format("TestData\\{0}\\{1}\\template.json", ConfigurationManager.AppSettings["Environment"], testFolder);
            JObject template = parentTemplate;
            if (File.Exists(baseFilePath))
            {
                var json = File.ReadAllText(baseFilePath);
                template = (JObject)JsonConvert.DeserializeObject(json);
                template.Merge(parentTemplate);
            }
            
            string filePath = string.Format("TestData\\{0}\\{1}\\{2}.json", ConfigurationManager.AppSettings["Environment"], testFolder, methodName);
            if (File.Exists(filePath))
            {
                var json = File.ReadAllText(filePath);
                var testCases = (IEnumerable)JsonConvert.DeserializeObject(json);

                foreach(JObject obj in testCases)
                {
                    if(template!=null)
                    {
                        obj.Merge(template);
                    }
                    yield return obj.ToString();
                }
            }
            else
            {
                yield return template.ToString();
            }
        }
    }
}
