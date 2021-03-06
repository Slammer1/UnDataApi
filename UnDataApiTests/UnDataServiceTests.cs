using NUnit.Framework;
using UnDataApi.Services;
using UnDataApi.DataModel;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System;

namespace Tests
{
    public class UnDataServiceTests
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
            int count = service.AllDataFlows.Count;
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
            DataFlow firstFlow = flows[0];
            Assert.AreEqual(firstFlow.Id, "DF_UNData_UNFCC");
            Assert.AreEqual(firstFlow.Name, "SDMX_GHG_UNDATA");
            Assert.AreEqual(firstFlow.AgencyId, "ESTAT");
            Assert.AreEqual(firstFlow.Version, "1.0");
            Assert.AreEqual(firstFlow.IsFinal, true);
            Assert.AreEqual(firstFlow.Structure.Class, "DataStructure");
            Assert.AreEqual(firstFlow.Structure.Id, "DSD_GHG_UNDATA");
            Assert.AreEqual(firstFlow.Structure.AgencyId, "UNSD");
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
            Assert.AreEqual(lastFlow.Structure.Class, "DataStructure");
            Assert.AreEqual(lastFlow.Structure.Id, "WDI");
            Assert.AreEqual(lastFlow.Structure.AgencyId, "WB");
        }

        [Test]
        public void GetSingleDataStructureTest1()
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
            lastFlow.GetDataStructure();
        }
        /// <summary>
        /// Tests the getting of all data structures. Getting all the data structures at
        /// once won't be a typical use case, but it is still a good test.
        /// </summary>
        [Test]
        public void DataStructureTest1()
        {
            UnDataService service = new UnDataService();
            string xmlToParse = "";
            try
            {
                xmlToParse = File.ReadAllText("Files\\TestStrucXml.txt", Encoding.GetEncoding("ISO-8859-1"));
            }
            catch (FileNotFoundException e)
            {
                throw new FileNotFoundException("The TestStrucXml.txt file did not get copied to the " +
                    "output directory correctly. Please click on the file and, in the properties," +
                    "adjust the build action and it should be copied correctly because the properties" +
                    "have been changed. That or do a clean build.");
            }
            Stream stream = new MemoryStream(Encoding.UTF8.GetBytes(xmlToParse));
            List<DataStructure> strucs = new List<DataStructure>();
            strucs = service.ProcessStrucXml(stream, strucs);

            DataStructure struc = strucs[0];
            Assert.AreEqual(struc.Id, "NA_MAIN");
            Assert.AreEqual(struc.AgencyId, "ESTAT");
            Assert.AreEqual(struc.Version, "1.9");
            Assert.AreEqual(struc.IsFinal, true);
            Assert.AreEqual(struc.Name, "NA Main Aggregates");
            Dimension dim = struc.DimensionList[0];
            Assert.AreEqual(dim.Id, "FREQ");
            Assert.AreEqual(dim.Position, 1);
            dim = struc.DimensionList[14];
            Assert.AreEqual(dim.Id, "TIME_PERIOD");
            Assert.AreEqual(dim.Position, 15);
            Measure measure = struc.MeasuresList[0];
            Assert.AreEqual(measure.Id, "OBS_VALUE");
            ConceptIdentity measIdentity = measure.ConceptId;
            Assert.AreEqual(measIdentity.Id, "OBS_VALUE");
            Assert.AreEqual(measIdentity.Class, "Concept");
            Assert.AreEqual(measIdentity.ParentId, "CS_NA");
            UnDataApi.DataModel.Attribute attribute = struc.AttributeList[0];
            Assert.AreEqual(attribute.Id, "OBS_STATUS");
            Assert.AreEqual(attribute.AssignmentStatus, "Mandatory");
            AttributeRelationship rel = attribute.AttributeRelationship;
            Assert.AreEqual(rel.MeasureDimensions[0], "OBS_VALUE");
            attribute = struc.AttributeList[4];
            Assert.AreEqual(attribute.Id, "REF_PERIOD_DETAIL");
            Assert.AreEqual(attribute.AssignmentStatus, "Conditional");
            rel = attribute.AttributeRelationship;
            Assert.AreEqual(rel.MeasureDimensions[0], "FREQ");
            Assert.AreEqual(rel.MeasureDimensions[5], "COUNTERPART_SECTOR");
            Assert.AreEqual(rel.MeasureDimensions[13], "TRANSFORMATION");
            Assert.AreEqual(rel.MeasureDimensions.Count, 14);

        }
    }
}