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
            configuration_JSON = ConfigurationFactory.GetConfiguration(
                "jsonTest.json",
                ConfigurationType.Json
            );
        }

        [TestMethod()]
        public void GetTest_Json1()
        {
            var value = configuration_JSON.Get("property1", 100);

            configuration_JSON.Set("p1.p2.p21", DateTime.Now);
            configuration_JSON.Set("p1.p2.p22", DateTime.Now);
            configuration_JSON.Set("p1.p2.p23", DateTime.Now);
            configuration_JSON.Set("p1.p2.p24", DateTime.Now);
            configuration_JSON.Set("p1.p3", DateTime.Now);

            Assert.AreEqual(100, value);
        }

        [TestMethod()]
        public void GetTest_Json2()
        {
            var value = configuration_JSON.Get("property2.p11.p111", 100);

            Assert.AreEqual(100, value);
        }

        [TestMethod()]
        public void GetTest_Json3()
        {
            var value = configuration_JSON.Get("property3.p11.p111", new sssss());

            Assert.AreEqual("jerry", value.name);
        }

        [TestMethod()]
        public void GetTest_Json4()
        {
            var value = configuration_JSON.Get("property3.p11.p112", new sssss());
            var value2 = configuration_JSON.Get<string>("property3.p11.p112.name1");

            Assert.AreEqual("jerry1", value2);
        }

        public class sssss
        {
            public string name { get; set; } = "jerry";
            public string name1 { get; set; } = "jerry1";
            public string name2 { get; set; } = "jerry2";
        }
    }
}
