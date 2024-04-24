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
            configuration_XML = ConfigurationFactory.GetConfiguration(
                "xmlText.xml",
                ConfigurationType.Xml
            );
        }

        [TestMethod()]
        public void GetTest_XML1()
        {
            var value = configuration_XML.Get("property1", new sssss());

            configuration_XML.Set("p1.p2.p21", DateTime.Now);
            configuration_XML.Set("p1.p2.p22", DateTime.Now);
            configuration_XML.Set("p1.p2.p23", DateTime.Now);
            configuration_XML.Set("p1.p2.p24", DateTime.Now);
            configuration_XML.Set("p1.p3", DateTime.Now);

            Assert.AreEqual("jerry", value.name);
        }

        [TestMethod()]
        public void GetTest_XML2()
        {
            var value = configuration_XML.Get("propert2.p32.p312", new sssss());

            Assert.AreEqual("jerry", value.name);
        }

        [TestMethod()]
        public void GetTest_XML3()
        {
            var value = configuration_XML.Get("propert2.p33", 100);

            Assert.AreEqual(100, value);
        }

        [TestMethod()]
        public void SetTest_XML1()
        {
            configuration_XML.Set("propert3.p32.p312", new sssss() { Index = int.MaxValue });

            var value = configuration_XML.Get<int>("propert3.p32.p312.Index");

            Assert.AreEqual(int.MaxValue, value);
        }

        [TestMethod()]
        public void SetTest_XML2()
        {
            var nag = "2354565";

            configuration_XML.Set(
                "propert4.p32.p312",
                new sssss() { Index = int.MaxValue, name = nag }
            );

            var value = configuration_XML.Get<string>("propert4.p32.p312.name");

            Assert.AreEqual(nag, value);
        }

        [TestMethod()]
        public void SetTest_XML3()
        {
            var dateTime = new DateTime(2024, 04, 24);

            configuration_XML.Set(
                "propert5.p32.p312",
                new sssss() { Index = int.MaxValue, Time = dateTime }
            );

            var value = configuration_XML.Get<DateTime>("propert5.p32.p312.Time");

            Assert.AreEqual(dateTime, value);
        }

        public class sssss
        {
            public DateTime Time { get; set; } = DateTime.Now;

            public int Index { get; set; } = 100;

            public string name { get; set; } = "jerry";
        }
    }
}
