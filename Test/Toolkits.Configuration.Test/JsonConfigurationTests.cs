using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Toolkits.Configuration;

namespace Toolkits.Configuration.Tests
{
    [TestClass()]
    public class JsonConfigurationTests
    {
        IConfiguration configuration_JSON = default!;

        [TestInitialize]
        public void SetupJSON()
        {
            configuration_JSON = ConfigurationFactory.CreateConfiguration(
                "jsonTest.json",
                ConfigurationType.Json
            );
        }

        [TestMethod()]
        public void GetTest_Json1()
        {
            var value = configuration_JSON.Get("property1", 100);

            Assert.AreEqual(100, value);
        }

        [TestMethod()]
        public void GetTest_Json2()
        {
            var value = configuration_JSON.Get("property2.p11.p111", 100);

            Assert.AreEqual(100, value);
        }
    }
}
