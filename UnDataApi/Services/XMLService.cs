using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using UnDataApi.DataModel;

namespace UnDataApi.Services
{
    /// <summary>
    /// This class should handle all of the XML parsing for the services. Want to move all XML operations out
    /// of the other services so they don't have to handle XML (or include the System.Xml or associated classes,
    /// for the sake of good design practices).
    /// </summary>
    public static class XMLService
    {

        public static List<DataFlow> InitializeFlowsFromXml(Stream stream)
        {
            List<DataFlow> localFlows = new List<DataFlow>();
            XmlReader reader = XmlReader.Create(stream);
            reader.ReadToDescendant("Dataflow");
            DataFlow flow = new DataFlow();
            do
            {
                switch (reader.NodeType)
                {
                    case XmlNodeType.Element:
                        switch (reader.Name)
                        {
                            case "Dataflow":
                                if (flow != null && flow.Id != string.Empty && flow.Id != null) localFlows.Add(flow);
                                flow = new DataFlow();
                                while (reader.MoveToNextAttribute())
                                {
                                    switch (reader.Name)
                                    {
                                        case "id":
                                            flow.Id = reader.Value;
                                            break;

                                        case "version":
                                            flow.Version = reader.Value;
                                            break;

                                        case "agencyID":
                                            flow.AgencyId = reader.Value;
                                            break;

                                        case "isFinal":
                                            bool isfinal;
                                            bool isParsed = bool.TryParse(reader.Value, out isfinal);
                                            flow.IsFinal = isfinal;
                                            break;
                                    }

                                }
                                break;
                            case "Structure":
                                Structure structure = new Structure();
                                flow.Structure = structure;
                                XmlReader inner = XmlReader.Create(new MemoryStream(Encoding.UTF8.GetBytes(reader.ReadInnerXml())));
                                inner.ReadToDescendant("Ref");
                                while (inner.MoveToNextAttribute())
                                {
                                    switch (inner.Name)
                                    {
                                        case "id":
                                            structure.Id = inner.Value;
                                            break;

                                        case "version":
                                            structure.Version = inner.Value;
                                            break;

                                        case "agencyID":
                                            structure.AgencyId = inner.Value;
                                            break;

                                        case "package":
                                            structure.Package = inner.Value;
                                            break;
                                        case "class":
                                            structure.Class = inner.Value;
                                            break;
                                    }

                                }
                                break;
                        }

                        break;
                    case XmlNodeType.Text:
                        flow.Name = reader.Value;
                        break;
                    case XmlNodeType.EndElement:
                        Console.Write("</{0}>", reader.Name);
                        break;
                }
            } while (reader.Read());
            if (flow != null && flow.Id != string.Empty && flow.Id != null) localFlows.Add(flow);
            reader.Close();
            return localFlows;
        }

        internal static DataSet ProcessQuery(Stream stream)
        {
            List<DataSeries> seriesList = new List<DataSeries>();
            DataSet localSet = new DataSet();
            localSet.DataSeries = seriesList;
            XmlReader reader = XmlReader.Create(stream);
            reader.ReadToDescendant("message:DataSet");
            DataSeries series = new DataSeries();
            do
            {
                switch (reader.NodeType)
                {
                    case XmlNodeType.Element:
                        switch (reader.Name)
                        {
                            case "generic:SeriesKey":
                                series = new DataSeries();
                                while(reader.ReadToDescendant("generic:Value"))
                                {
                                    do
                                    {
                                        string key = "", value = "";
                                        while (reader.MoveToNextAttribute())
                                        {

                                            switch (reader.Name)
                                            {
                                                case "id":
                                                    key = reader.Value;
                                                    break;

                                                case "value":
                                                    value = reader.Value;
                                                    series.Dimensions.Add(key, value);
                                                    break;
                                            }

                                        }
                                    } while (reader.ReadToNextSibling("generic:Value"));
                                    
                                }
                                seriesList.Add(series);
                                break;
                            case "generic:Obs":
                                DataPoint point = new DataPoint();
                                while (reader.ReadToDescendant("generic:ObsDimension"))
                                {
                                    while (reader.MoveToNextAttribute())
                                    {
                                        switch (reader.Name)
                                        {
                                            case "id":
                                                point.ObsDimension = reader.Value;
                                                break;

                                            case "value":
                                                point.Key = reader.Value;
                                                break;
                                        }

                                    }
                                }
                                while (reader.ReadToNextSibling("generic:ObsValue"))
                                {
                                    while (reader.MoveToNextAttribute())
                                    {
                                        switch (reader.Name)
                                        {
                                           
                                            case "value":
                                                point.Value = reader.Value;
                                                series.DataPoints.Add(point);
                                                break;
                                        }

                                    }
                                }
                                break;
                        }

                        break;
                    case XmlNodeType.Text:
                        break;
                }
            } while (reader.Read());
            reader.Close();
            return localSet;
        }

        internal static Dictionary<string, string> InitializeCodesFromXml(Stream stream)
        {
            XmlDocument document = new XmlDocument();
            document.Load(stream);
            Dictionary<string, string> codes = new Dictionary<string, string>();
            string key = "";
            string value = "";
            XmlElement root = document.DocumentElement;
            XmlNodeList nodes = root.ChildNodes;
            foreach (XmlNode node in nodes)
            {
                if (node.Name == "Structures")
                {
                    nodes = node.ChildNodes;
                    break;
                }
            }
            foreach (XmlNode node in nodes)
            {
                if (node.Name == "Codelists")
                {
                    nodes = node.ChildNodes;
                    break;
                }
            }
            foreach (XmlNode node in nodes)
            {
                if (node.Name == "Codelist")
                {
                    nodes = node.ChildNodes;
                    break;
                }
            }
            foreach (XmlNode node in nodes)
            {
                if (node.Name == "Code")
                {
                    foreach (XmlAttribute attribute in node.Attributes)
                    {
                        switch (attribute.Name)
                        {
                            case "id":
                                key = attribute.Value;
                                break;
                        }

                    }
                    value = GetValue(node);
                }
                if (node.Name == "Name")
                {
                    codes.Add("CodeListName", node.InnerText);
                    continue;
                }
                codes.Add(key, value);
            }
            return codes;
        }

        private static string GetValue(XmlNode node)
        {
            foreach (XmlNode child in node.ChildNodes)
            {
                if (child.Name == "Name")
                {
                    return child.InnerText;
                }
            }
            throw new Exception("Error trying to find the codelist name.");
        }

        public static List<DataStructure> InitializeDataStrucsFromXml(Stream stream)
        {
            XmlDocument document = new XmlDocument();
            document.Load(stream);
            List<DataStructure> structures = new List<DataStructure>();
            XmlElement root = document.DocumentElement;
            XmlNodeList nodes = root.ChildNodes;
            foreach(XmlNode node in nodes)
            {
                if(node.Name == "Structures")
                {
                    nodes = node.ChildNodes;
                    break;
                }
            }
            foreach (XmlNode node in nodes)
            {
                if (node.Name == "DataStructures")
                {
                    nodes = node.ChildNodes;
                    break;
                }
            }
            foreach (XmlNode node in nodes)
            {
                structures.Add(GetDataStructure(node));
            }

            return structures;
        }

        public static DataStructure GetDataStructure(XmlNode node)
        {
            DataStructure structure = new DataStructure();
            foreach(XmlAttribute attribute in node.Attributes)
            {
                switch(attribute.Name)
                {
                    case "id":
                        structure.Id = attribute.Value;
                        break;
                    case "isFinal":
                        bool isFinal;
                        bool isParsed = bool.TryParse(attribute.Value, out isFinal);
                        structure.IsFinal = isFinal;
                        break;
                    case "agencyID":
                        structure.AgencyId = attribute.Value;
                        break;
                    case "version":
                        structure.Version = attribute.Value;
                        break;

                }

            }
            XmlNodeList nodes = node.ChildNodes;
            foreach(XmlNode childNode in node.ChildNodes)
            {
                if(childNode.Name == "Name")
                {
                    structure.Name = childNode.InnerText;
                }
                if (childNode.Name == "DataStructureComponents")
                {
                    nodes = childNode.ChildNodes;
                }

            }
            XmlNodeList dimensionNodes = null;
            XmlNodeList measureNodes = null;
            XmlNodeList attributeNodes = null;
            foreach (XmlNode node2 in nodes)
            {
                if (node2.Name == "DimensionList")
                {
                    dimensionNodes = node2.ChildNodes;
                }
                if (node2.Name == "MeasureList")
                {
                    measureNodes = node2.ChildNodes;
                }
                if (node2.Name == "AttributeList")
                {
                    attributeNodes = node2.ChildNodes;
                }

            }
            structure.DimensionList = ProcessDimensions(dimensionNodes);
            structure.AttributeList = ProcessAttributes(attributeNodes);
            structure.MeasuresList = ProcessMeasures(measureNodes);

            return structure;

        }

        private static List<Measure> ProcessMeasures(XmlNodeList measureNodes)
        {
            List<Measure> measures = new List<Measure>();
            foreach(XmlNode node in measureNodes)
            {
                Measure measure = InitializeMeasure(node);
                measure.ConceptId = GetConceptFromXml(node);
                measure.LocalRep = GetRepFromXml(node);
                measures.Add(measure);

            }

            return measures;
        }

        private static Measure InitializeMeasure(XmlNode node)
        {
            Measure measure = new Measure();
            foreach (XmlAttribute xattribute in node.Attributes)
            {
                switch (xattribute.Name)
                {
                    case "id":
                        measure.Id = xattribute.Value;
                        break;
                }
            }
            return measure;
        }

        private static List<DataModel.Attribute> ProcessAttributes(XmlNodeList attributeNodes)
        {
            if(attributeNodes == null)
            {
                return null;
            }
            List<DataModel.Attribute> attributes = new List<DataModel.Attribute>();
            foreach(XmlNode node in attributeNodes)
            {
                DataModel.Attribute attribute = InitializeAttribute(node);
                attribute.ConceptId = GetConceptFromXml(node);
                attribute.LocalRep = GetRepFromXml(node);
                attribute.AttributeRelationship = GetAttRelationFromXml(node);
                attributes.Add(attribute);
            }

            return attributes;
        }

        private static AttributeRelationship GetAttRelationFromXml(XmlNode node)
        {
            AttributeRelationship relationship = new AttributeRelationship();
            relationship.MeasureDimensions = new List<string>();
            XmlNode idNode = null;
            foreach (XmlNode child in node.ChildNodes)
            {
                if (child.Name == "AttributeRelationship")
                {
                    idNode = child;
                }
            }
            foreach (XmlNode child in idNode.ChildNodes)
            {
                foreach(XmlNode n in child.ChildNodes)
                {
                    if (n.Name == "Ref")
                    {
                        foreach (XmlAttribute attribute in n.Attributes)
                        {
                            switch (attribute.Name)
                            {
                                case "id":
                                    relationship.MeasureDimensions.Add(attribute.Value);
                                    break;
                                
                            }
                        }
                    }
                }
                
            }
            
            return relationship;
        }

        private static DataModel.Attribute InitializeAttribute(XmlNode node)
        {
            DataModel.Attribute attribute = new DataModel.Attribute();
            foreach (XmlAttribute xattribute in node.Attributes)
            {
                switch (xattribute.Name)
                {
                    case "id":
                        attribute.Id = xattribute.Value;
                        break;
                    case "assignmentStatus":
                        attribute.AssignmentStatus = xattribute.Value;
                        break;
                }
            }
            return attribute;
        }

        private static List<Dimension> ProcessDimensions(XmlNodeList dimensionNodes)
        {
            List<Dimension> dimensions = new List<Dimension>();
            foreach(XmlNode node in dimensionNodes)
            {
                Dimension dim = InitializeDimension(node);
                dim.Representation = GetRepFromXml(node);
                dim.ConceptId = GetConceptFromXml(node);
                dimensions.Add(dim);

            }
            return dimensions;
        }

        private static ConceptIdentity GetConceptFromXml(XmlNode node)
        {
            ConceptIdentity id = new ConceptIdentity();
            XmlNode idNode = null;
            foreach(XmlNode child in node.ChildNodes)
            {
                if(child.Name == "ConceptIdentity")
                {
                    idNode = child;
                }
            }
            foreach (XmlNode child in idNode.ChildNodes)
            {
                if (child.Name == "Ref")
                {
                    idNode = child;
                }
            }
            foreach (XmlAttribute attribute in idNode.Attributes)
            {
                switch (attribute.Name)
                {
                    case "id":
                        id.Id = attribute.Value;
                        break;
                    case "agencyID":
                        id.AgencyId = attribute.Value;
                        break;
                    case "class":
                        id.Class = attribute.Value;
                        break;
                    case "maintainableParentVersion":
                        id.ParentVersion = attribute.Value;
                        break;
                    case "maintainableParentID":
                        id.ParentId = attribute.Value;
                        break;
                    case "package":
                        id.Package = attribute.Value;
                        break;
                }
            }

            return id;
        }

        private static LocalRepresentation GetRepFromXml(XmlNode node)
        {
            LocalRepresentation rep = new LocalRepresentation();
            XmlNode repNode = null;
            foreach (XmlNode child in node.ChildNodes)
            {
                if(child.Name == "LocalRepresentation")
                {
                    repNode = child;
                }
            }
            if (repNode == null) return null;
            XmlNode formatNode = null;
            foreach (XmlNode child in repNode.ChildNodes)
            {
                if (child.Name == "Enumeration")
                {
                    repNode = child;
                }
                if (child.Name == "EnumerationFormat")
                {
                    formatNode = child;
                }
                if (child.Name == "TextFormat")
                {
                    return GetShortRep(child);
                }
            }
            foreach (XmlNode child in repNode.ChildNodes)
            {
                if (child.Name == "Ref")
                {
                    repNode = child;
                }
            }
            rep.Enumeration = new Enumeration();
            if(formatNode != null)
            {
                rep.Format = new EnumerationFormat();
                foreach (XmlAttribute attribute in formatNode.Attributes)
                {
                    switch (attribute.Name)
                    {
                        case "textType":
                            rep.Format.TextType = attribute.Value;
                            break;
                        case "maxLength":
                            int val;
                            bool isParsed = int.TryParse(attribute.Value, out val);
                            rep.Format.MaxLength = val;
                            break;
                        case "minLength":
                            int val2;
                            bool isParsed2 = int.TryParse(attribute.Value, out val2);
                            rep.Format.MinLength = val2;
                            break;

                    }
                }
            }
            
            foreach (XmlAttribute attribute in repNode.Attributes)
            {
                switch (attribute.Name)
                {
                    case "id":
                        rep.Enumeration.Id = attribute.Value;
                        break;
                    case "agencyID":
                        rep.Enumeration.AgencyId = attribute.Value;
                        break;
                    case "class":
                        rep.Enumeration.Class = attribute.Value;
                        break;
                    case "version":
                        rep.Enumeration.Version = attribute.Value;
                        break;
                    case "package":
                        rep.Enumeration.Package = attribute.Value;
                        break;
                }
            }

            return rep;
        }

        private static LocalRepresentation GetShortRep(XmlNode child)
        {
            LocalRepresentation rep = new LocalRepresentation();
            rep.Format = new EnumerationFormat();
            foreach (XmlAttribute attribute in child.Attributes)
            {
                switch (attribute.Name)
                {
                    case "textType":
                        rep.Format.TextType = attribute.Value;
                        break;
                    case "maxLength":
                        int val;
                        bool isParsed = int.TryParse(attribute.Value, out val);
                        rep.Format.MaxLength = val;
                        break;
                    case "minLength":
                        int val2;
                        bool isParsed2 = int.TryParse(attribute.Value, out val2);
                        rep.Format.MinLength = val2;
                        break;
                }
            }
            return rep;
        }

        private static Dimension InitializeDimension(XmlNode node)
        {
            Dimension dime = new Dimension();
            foreach(XmlAttribute attribute in node.Attributes)
            {
                switch (attribute.Name)
                {
                    case "id":
                        dime.Id = attribute.Value;
                        break;
                    case "position":
                        int val;
                        bool isParsed = int.TryParse(attribute.Value, out val);
                        dime.Position = val;
                        break;
                }
            }
            return dime;
        }
    }
}
