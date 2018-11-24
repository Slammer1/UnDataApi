using NUnit.Framework;
using UnDataApi.Services;
using UnDataApi.DataModel;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System;

namespace Tests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {

        }

        /// <summary>
        /// First test just makes sure that we have at least 12 dataflows. It tests the full query to the actual
        /// query to the database so I don't want to get too specific with the tests as the dataflows on the web
        /// api provided by undata may change. Further tests will use a saved version of the xml pulled from 
        /// the website in its current condition.
        /// </summary>
        [Test]
        public void DataFlowTest1()
        {
            UnDataService service = new UnDataService();
            int count = service.DataFlows.Count;
            Assert.Greater(count, 11);
        }

        [Test]
        public void DataFlowTest2()
        {
            UnDataService service = new UnDataService();
            string xmlToParse = "";
            try
            {
                xmlToParse = File.ReadAllText("Files\\TestXml.txt");
            }
            catch(FileNotFoundException  e)
            {
                throw new FileNotFoundException("The TestXml.txt file did not get copied to the " +
                    "output directory correctly. Please click on the file and, in the properties," +
                    "adjust the build action and it should be copied correctly because the properties" +
                    "have been changed. That or do a clean build.");
            }
            Stream stream = new MemoryStream(Encoding.UTF8.GetBytes(xmlToParse));
            List<DataFlow> flows = new List<DataFlow>();
            flows = service.ProcessXml(stream, flows);
            DataFlow firstFlow = flows[0];
            Assert.AreEqual(firstFlow.Id, "DF_UNData_UNFCC");
            Assert.AreEqual(firstFlow.Name, "SDMX_GHG_UNDATA");
            Assert.AreEqual(firstFlow.AgencyId, "ESTAT");
            Assert.AreEqual(firstFlow.Version, "1.0");
            Assert.AreEqual(firstFlow.IsFinal, true);
            var x = 0;
        }

        [Test]
        public void DataFlowTest3()
        {
            UnDataService service = new UnDataService();
            string xmlToParse = "";
            try
            {
                xmlToParse = File.ReadAllText("Files\\TestXml.txt");
            }
            catch (FileNotFoundException e)
            {
                throw new FileNotFoundException("The TestXml.txt file did not get copied to the " +
                    "output directory correctly. Please click on the file and, in the properties," +
                    "adjust the build action and it should be copied correctly because the properties" +
                    "have been changed. That or do a clean build.");
            }
            Stream stream = new MemoryStream(Encoding.UTF8.GetBytes(xmlToParse));
            List<DataFlow> flows = new List<DataFlow>();
            flows = service.ProcessXml(stream, flows);
            DataFlow secondFlow = flows[1];
            Assert.AreEqual(secondFlow.Id, "NASEC_IDCFINA_A");
            Assert.AreEqual(secondFlow.Name, "Annual financial accounts");
            Assert.AreEqual(secondFlow.AgencyId, "ESTAT");
            Assert.AreEqual(secondFlow.Version, "1.9");
            Assert.AreEqual(secondFlow.IsFinal, true);
        }

        [Test]
        public void DataFlowTest4()
        {
            UnDataService service = new UnDataService();
            string xmlToParse = "";
            try
            {
                xmlToParse = File.ReadAllText("Files\\TestXml.txt");
            }
            catch (FileNotFoundException e)
            {
                throw new FileNotFoundException("The TestXml.txt file did not get copied to the " +
                    "output directory correctly. Please click on the file and, in the properties," +
                    "adjust the build action and it should be copied correctly because the properties" +
                    "have been changed. That or do a clean build.");
            }
            Stream stream = new MemoryStream(Encoding.UTF8.GetBytes(xmlToParse));
            List<DataFlow> flows = new List<DataFlow>();
            flows = service.ProcessXml(stream, flows);
            DataFlow thirdFlow = flows[2];
            Assert.AreEqual(thirdFlow.Id, "NASEC_IDCFINA_Q");
            Assert.AreEqual(thirdFlow.Name, "Quarterly financial accounts");
            Assert.AreEqual(thirdFlow.AgencyId, "ESTAT");
            Assert.AreEqual(thirdFlow.Version, "1.9");
            Assert.AreEqual(thirdFlow.IsFinal, true);
        }

        [Test]
        public void DataFlowTest5()
        {
            UnDataService service = new UnDataService();
            string xmlToParse = "";
            try
            {
                xmlToParse = File.ReadAllText("Files\\TestXml.txt");
            }
            catch (FileNotFoundException e)
            {
                throw new FileNotFoundException("The TestXml.txt file did not get copied to the " +
                    "output directory correctly. Please click on the file and, in the properties," +
                    "adjust the build action and it should be copied correctly because the properties" +
                    "have been changed. That or do a clean build.");
            }
            Stream stream = new MemoryStream(Encoding.UTF8.GetBytes(xmlToParse));
            List<DataFlow> flows = new List<DataFlow>();
            flows = service.ProcessXml(stream, flows);
            DataFlow nextToLastFlow = flows[10];
            Assert.AreEqual(nextToLastFlow.Id, "DF_UNDATA_WPP");
            Assert.AreEqual(nextToLastFlow.Name, "World Population Prospects");
            Assert.AreEqual(nextToLastFlow.AgencyId, "UNSD");
            Assert.AreEqual(nextToLastFlow.Version, "2.0");
            Assert.AreEqual(nextToLastFlow.IsFinal, false);
        }

        [Test]
        public void DataFlowTest6()
        {
            UnDataService service = new UnDataService();
            string xmlToParse = "";
            try
            {
                xmlToParse = File.ReadAllText("Files\\TestXml.txt");
            }
            catch (FileNotFoundException e)
            {
                throw new FileNotFoundException("The TestXml.txt file did not get copied to the " +
                    "output directory correctly. Please click on the file and, in the properties," +
                    "adjust the build action and it should be copied correctly because the properties" +
                    "have been changed. That or do a clean build.");
            }
            Stream stream = new MemoryStream(Encoding.UTF8.GetBytes(xmlToParse));
            List<DataFlow> flows = new List<DataFlow>();
            flows = service.ProcessXml(stream, flows);
            DataFlow lastFlow = flows[11];
            Assert.AreEqual(lastFlow.Id, "DF_UNDATA_WDI");
            Assert.AreEqual(lastFlow.Name, "WB World Development Indicators");
            Assert.AreEqual(lastFlow.AgencyId, "WB");
            Assert.AreEqual(lastFlow.Version, "1.0");
            Assert.AreEqual(lastFlow.IsFinal, true);
        }
    }
}