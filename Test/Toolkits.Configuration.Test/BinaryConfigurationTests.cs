using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using Toolkits.Configuration;

namespace Toolkits.Configuration.Tests
{
    [TestClass()]
    public class BinaryConfigurationTests
    {
        IConfiguration configuration_Binary = default!;

        [TestInitialize]
        public void SetupBinary()
        {
            configuration_Binary = ConfigurationFactory.GetConfiguration(
                "xmlText.bin",
                ConfigurationType.Binary
            );
        }

        [TestMethod()]
        public void GetTest_Binary1()
        {
            var value = configuration_Binary.Get("property3.p31.p312", new sssss());

            Assert.AreEqual("jerry", value.name);
        }

        [Serializable]
        public class sssss
        {
            public string name { get; set; } = "jerry";
            public string name1 { get; set; } = "jerry1";
            public string name2 { get; set; } = "jerry2";
        }
    }
}
