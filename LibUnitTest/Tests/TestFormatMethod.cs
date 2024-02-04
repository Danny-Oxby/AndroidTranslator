using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibUnitTest.Tests
{
    [TestClass]
    public class TestFormatMethod
    {
        [AssemblyInitialize]
        public static void CreateTables(TestContext context)
        {
            //create new table for testing
            SqlLibrary.CreateTables.CreateDefaultTables();
        }

        [AssemblyCleanup]
        public static void DestroyTables()
        {
            //clean the table for next test
            SqlLibrary.CreateTables.DropTables();
        }
    }
}
