using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnDataApi.DataModel;
using System.Net;
using System.Net.Http;
using System.Xml;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.IO;
using System.Xml.Serialization;
using System.Text;

namespace UnDataApi.Services
{
    public class UnDataService
    {
        public List<DataFlow> DataFlows { get; set; }

        public List<DataStructure> DataStructures { get; set; }


        public HttpClient UnApiClient = new HttpClient();

        public UnDataService()
        {

            DataFlows = GetDataFlows().Result;
            DataStructures = GetAllDataStructures().Result;

        }

        public async Task<List<DataStructure>> GetAllDataStructures()
        {
            HttpResponseMessage response = await UnApiClient.GetAsync("http://data.un.org/ws/rest/datastructure/");
            List<DataStructure> localStrucs = new List<DataStructure>();
            if (response.IsSuccessStatusCode)
            {
                Stream stream = response.Content.ReadAsStreamAsync().Result;
                localStrucs = ProcessStrucXml(stream, localStrucs);
            }

            if (localStrucs == null)
            {
                throw new Exception("Error getting DataFlows");
            }
            return localStrucs;
        }

        public List<DataStructure> ProcessStrucXml(Stream stream, List<DataStructure> localStrucs)
        {
            XmlReader reader = XmlReader.Create(stream);
            reader.ReadToDescendant("DataStructure");
            DataStructure struc = new DataStructure();
            do
            {
                //List<Dimension> dimList;
                //List<DataModel.Attribute> attList;
                //List<Measure> measures;
                switch (reader.NodeType)
                {
                    case XmlNodeType.Element:
                        switch (reader.Name)
                        {
                            case "DataStructure":
                                if (struc != null && struc.Id != string.Empty && struc.Id != null) localStrucs.Add(struc);
                                struc = new DataStructure();
                                struc.AttributeList = new List<DataModel.Attribute>();
                                struc.MeasuresList = new List<Measure>();
                                struc.DimensionList = new List<Dimension>();
                                while (reader.MoveToNextAttribute())
                                {
                                    switch (reader.Name)
                                    {
                                        case "id":
                                            struc.Id = reader.Value;
                                            break;

                                        case "version":
                                            struc.Version = reader.Value;
                                            break;

                                        case "agencyID":
                                            struc.AgencyId = reader.Value;
                                            break;

                                        case "isFinal":
                                            bool isfinal;
                                            bool isParsed = bool.TryParse(reader.Value, out isfinal);
                                            struc.IsFinal = isfinal;
                                            break;
                                    }

                                }
                                break;
                            case "Dimension":
                                {
                                    Dimension dim = new Dimension();
                                    struc.DimensionList.Add(dim);
                                    
                                    while (reader.MoveToNextAttribute())
                                    {
                                        switch (reader.Name)
                                        {
                                            case "id":
                                                dim.Id = reader.Value;
                                                break;

                                            case "position":
                                                int val = 0;
                                                bool isParsed = int.TryParse(reader.Value, out val);
                                                dim.Position = val;
                                                break;
                                        }

                                    }
                                    reader.ReadToFollowing("ConceptIdentity");
                                    var read = reader.ReadInnerXml();
                                    dim.ConceptId = GetConceptFromDom(XmlReader.Create(new MemoryStream(Encoding.UTF8.GetBytes(read))));
                                    reader.ReadToFollowing("LocalRepresentation");
                                    read = reader.ReadInnerXml();
                                    dim.Representation = GetRepFromDom(XmlReader.Create(new MemoryStream(Encoding.UTF8.GetBytes(read))));
                                    reader.ReadToFollowing("EnumerationFormat");
                                    dim.Representation.Format = new EnumerationFormat();
                                    while (reader.MoveToNextAttribute())
                                    {
                                        switch (reader.Name)
                                        {
                                            case "textType":
                                                dim.Representation.Format.TextType = reader.Value;
                                                break;

                                            case "maxLength":
                                                int val = 0;
                                                bool isParsed = int.TryParse(reader.Value, out val);
                                                dim.Representation.Format.MaxLength = val;
                                                break;
                                            case "minLength":
                                                int val2 = 0;
                                                bool isParsed2 = int.TryParse(reader.Value, out val2);
                                                dim.Representation.Format.MinLength = val2;
                                                break;
                                        }

                                    }
                                    break;
                                }
                            case "PrimaryMeasure":
                                {
                                    Measure measure = new Measure();
                                    struc.MeasuresList.Add(measure);
                                    
                                    while (reader.MoveToNextAttribute())
                                    {
                                        switch (reader.Name)
                                        {
                                            case "id":
                                                measure.Id = reader.Value;
                                                break;

                                        }
                                    }
                                    reader.ReadToFollowing("ConceptIdentity");
                                    var read = reader.ReadInnerXml();
                                    measure.ConceptId = GetConceptFromDom(XmlReader.Create(new MemoryStream(Encoding.UTF8.GetBytes(read))));
                                    reader.ReadToFollowing("LocalRepresentation");
                                    read = reader.ReadInnerXml();
                                    measure.LocalRep = GetRepFromDom(XmlReader.Create(new MemoryStream(Encoding.UTF8.GetBytes(read))));
                                    reader.ReadToFollowing("EnumerationFormat");
                                    measure.LocalRep.Format = new EnumerationFormat();
                                    while (reader.MoveToNextAttribute())
                                    {
                                        switch (reader.Name)
                                        {
                                            case "textType":
                                                measure.LocalRep.Format.TextType = reader.Value;
                                                break;

                                            case "maxLength":
                                                int val = 0;
                                                bool isParsed = int.TryParse(reader.Value, out val);
                                                measure.LocalRep.Format.MaxLength = val;
                                                break;
                                            case "minLength":
                                                int val2 = 0;
                                                bool isParsed2 = int.TryParse(reader.Value, out val2);
                                                measure.LocalRep.Format.MinLength = val2;
                                                break;
                                        }

                                    }
                                    break;
                                }
                            case "Attribute":
                                {
                                    DataModel.Attribute attribute = new DataModel.Attribute();
                                    struc.AttributeList.Add(attribute);
                                    while (reader.MoveToNextAttribute())
                                    {
                                        switch (reader.Name)
                                        {
                                            case "id":
                                                attribute.Id = reader.Value;
                                                break;
                                            case "assignmentStatus":
                                                attribute.AssignmentStatus = reader.Value;
                                                break;

                                        }
                                    }
                                    reader.ReadToFollowing("ConceptIdentity");
                                    var read = reader.ReadInnerXml();
                                    attribute.ConceptId = GetConceptFromDom(XmlReader.Create(new MemoryStream(Encoding.UTF8.GetBytes(read))));
                                    reader.ReadToFollowing("LocalRepresentation");
                                    read = reader.ReadInnerXml();
                                    attribute.LocalRep = GetRepFromDom(XmlReader.Create(new MemoryStream(Encoding.UTF8.GetBytes(read))));
                                    reader.ReadToFollowing("EnumerationFormat");
                                    attribute.LocalRep.Format = new EnumerationFormat();
                                    while (reader.MoveToNextAttribute())
                                    {
                                        switch (reader.Name)
                                        {
                                            case "textType":
                                                attribute.LocalRep.Format.TextType = reader.Value;
                                                break;

                                            case "maxLength":
                                                int val = 0;
                                                bool isParsed = int.TryParse(reader.Value, out val);
                                                attribute.LocalRep.Format.MaxLength = val;
                                                break;
                                            case "minLength":
                                                int val2 = 0;
                                                bool isParsed2 = int.TryParse(reader.Value, out val2);
                                                attribute.LocalRep.Format.MinLength = val2;
                                                break;
                                        }

                                    }
                                    reader.ReadToFollowing("AttributeRelationship");
                                    reader.ReadToDescendant("Ref");
                                    while (reader.MoveToNextAttribute())
                                    {
                                        switch (reader.Name)
                                        {
                                            case "id":
                                                attribute.AttributeRelationship = reader.Value;
                                                break;
                                        }

                                    }
                                    break;
                                }

                        }

                        break;
                    case XmlNodeType.Text:
                        struc.Name = reader.Value;
                        break;
                    case XmlNodeType.EndElement:
                        Console.Write("</{0}>", reader.Name);
                        break;
                }
            } while (reader.Read());
            if (struc != null && struc.Id != string.Empty && struc.Id != null) localStrucs.Add(struc);
            reader.Close();
            return localStrucs;
        }

        private LocalRepresentation GetRepFromDom(XmlReader inner)
        {
            LocalRepresentation rep = new LocalRepresentation();
            rep.Enumeration = GetEnumFromDom(inner);
            return rep;
        }

        private Enumeration GetEnumFromDom(XmlReader inner)
        {
            inner.ReadToDescendant("Ref");
            Enumeration enumeration = new Enumeration();
            while (inner.MoveToNextAttribute())
            {
                switch (inner.Name)
                {
                    case "id":
                        enumeration.Id = inner.Value;
                        break;


                    case "agencyID":
                        enumeration.AgencyId = inner.Value;
                        break;

                    case "package":
                        enumeration.Package = inner.Value;
                        break;
                    case "class":
                        enumeration.Class = inner.Value;
                        break;
                }

            }
            return enumeration;
        }

        private ConceptIdentity GetConceptFromDom(XmlReader inner)
        {
            inner.ReadToDescendant("Ref");
            ConceptIdentity identity = new ConceptIdentity();
            while (inner.MoveToNextAttribute())
            {
                switch (inner.Name)
                {
                    case "id":
                        identity.Id = inner.Value;
                        break;

                    case "maintainableParentID":
                        identity.ParentId = inner.Value;
                        break;

                    case "maintainableParentVersion":
                        identity.ParentVersion = inner.Value;
                        break;

                    case "agencyID":
                        identity.AgencyId = inner.Value;
                        break;

                    case "package":
                        identity.Package = inner.Value;
                        break;
                    case "class":
                        identity.Class = inner.Value;
                        break;
                }

            }
            return identity;
        }

        public async Task<List<DataFlow>> GetDataFlows()
        {
            HttpResponseMessage response = await UnApiClient.GetAsync("http://data.un.org/ws/rest/dataflow/");
            List<DataFlow> localFlows = new List<DataFlow>();
            if (response.IsSuccessStatusCode)
            {
                Stream stream = response.Content.ReadAsStreamAsync().Result;
                localFlows = ProcessXml(stream, localFlows);
            }

            if (localFlows == null)
            {
                throw new Exception("Error getting DataFlows");
            }
            return localFlows;

        }

        public List<DataFlow> ProcessXml(Stream stream, List<DataFlow> localFlows)
        {
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
        }
    }
