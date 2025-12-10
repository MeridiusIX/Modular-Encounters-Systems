using Sandbox.Common.ObjectBuilders.Definitions;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using VRageMath;

namespace ModularEncountersSystems.Files {


	public class Route {



        [XmlElement("Node")]
        public List<NodeData> Nodes { get; set; } = new List<NodeData>();

        [XmlIgnore]
        public List<Vector3D> NodePositions;


        [XmlIgnore]
        public string name = "";

        public void Init()
        {
            NodePositions = new List<Vector3D>();
            foreach (var node in Nodes)
            {
                NodePositions.Add(new Vector3D(node.WorldX, node.WorldY, node.WorldZ));
            }
        }
    }

    public class NodeData
    {
        [XmlAttribute("id")]
        public int Id { get; set; }

        [XmlElement("WorldX")]
        public double WorldX { get; set; }

        [XmlElement("WorldY")]
        public double WorldY { get; set; }

        [XmlElement("WorldZ")]
        public double WorldZ { get; set; }

    }


}
