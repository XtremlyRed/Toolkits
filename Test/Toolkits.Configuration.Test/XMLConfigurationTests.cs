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
    public class XMLConfigurationTests
    {
        IConfiguration configuration_XML = default!;

        [TestInitialize]
        public void SetupXML()
        {
            configuration_XML = ConfigurationFactory.CreateConfiguration(
                "xmlText.xml",
                ConfigurationType.Xml
            );
        }

        [TestMethod()]
        public void GetTest_XML1()
        {
            var value = configuration_XML.Get("property3.p31.p312", new sssss());

            Assert.AreEqual("jerry", value.name);
        }

        public class sssss
        {
            public string name { get; set; } = "jerry";
            public string name1 { get; set; } = "jerry1";
            public string name2 { get; set; } = "jerry2";
        }
    }
}
