using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NunitTestCaseSourceExternal
{
    public interface ITestCaseSource
    {
        IEnumerable GetTestCases(params object[] args);
    }
}
