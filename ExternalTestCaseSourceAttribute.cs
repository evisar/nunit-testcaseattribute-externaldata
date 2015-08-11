using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace NunitTestCaseSourceExternal
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class ExternalTestCaseSourceAttribute : TestCaseSourceAttribute
    {
        [SetUpFixture]
        private class DataTestCase : IDisposable
        {
            static object _sync = new object();

            [ThreadStatic]
            internal static string[] Args;
            [ThreadStatic]
            internal static ITestCaseSource DataSource;

            public IEnumerable TestCases
            {
                get
                {
                    lock (_sync)
                    {
                        return DataSource.GetTestCases(Args);
                    }
                }

            }

            [TearDown]
            public void Release()
            {
                DataSource = null;
                Args = null;
            }

            public void Dispose()
            {
                Release();
            }
        }

        public ExternalTestCaseSourceAttribute(Type dataSourceType, [CallerFilePath] string classPath = null, [CallerMemberName] string methodName = null)
            : base(typeof(DataTestCase), "TestCases")
        {
            DataTestCase.DataSource = Activator.CreateInstance(dataSourceType) as ITestCaseSource;
            DataTestCase.Args = new string[] { classPath, methodName };
        }

        public ExternalTestCaseSourceAttribute([CallerFilePath] string classPath = null, [CallerMemberName] string methodName = null)
            : base(typeof(DataTestCase), "TestCases")
        {
            Type dataSourceType = Type.GetType(ConfigurationManager.AppSettings["TestDataSourceType"]);
            DataTestCase.DataSource = Activator.CreateInstance(dataSourceType) as ITestCaseSource;
            DataTestCase.Args = new string[] { classPath, methodName }; 
        }
    }
}
