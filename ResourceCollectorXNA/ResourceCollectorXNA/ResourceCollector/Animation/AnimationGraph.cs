using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ResourceCollector
{
    public class NodeEvent                                  //событие которое возможно для конкретного узла графа анимаций- ребро графа анимаций
    {
        public string neededEvent;                          // необходимое событие
        public AnimationNode parentNode;                        // ссылка на владельца 
        public AnimationNode associatedNode;                // узел к которому ведет событие 
        public string description;                          // описание 

        public string parentName;
        public string associatedNodeName; 

        public NodeEvent(string _description, AnimationNode _parent, AnimationNode _associatedNode, string _neededevent)
        {
            description = _description;
            associatedNode = _associatedNode;
            neededEvent = _neededevent;
            parentNode = _parent;


            parentName = parentNode.name;
            associatedNodeName = associatedNode.name;
        }

        public NodeEvent()
        {
        }

        public override string ToString()
        {
            return description;
        }

        public static void NodeEventToStream(NodeEvent node, System.IO.BinaryWriter bw)
        {
            bw.WritePackString(node.neededEvent);
            bw.WritePackString(node.description);

            bw.WritePackString(node.parentNode.name);
            bw.WritePackString(node.associatedNode.name);
        }

        public static NodeEvent NodeEventFromStream(System.IO.BinaryReader br)
        {
            NodeEvent @event = new NodeEvent();
            @event.neededEvent = br.ReadPackString();
            @event.description = br.ReadPackString();

            @event.parentName = br.ReadPackString();
            @event.associatedNodeName = br.ReadPackString();
            return @event;
        }
    }

    public class AnimationNode                              // узел графа анимаций
    {
        public string name
        {
            get;
            private set;
        }

        public NodeEvent[] NodeEvents;                      // исходящие рёбра
        public Animation animation;                         // соответствующая узлу анимация

        public AnimationNode(string _name, Animation _animation)
        {
            SetName(_name);
            animation = _animation;
        }

        public AnimationNode(string _name)
        {
            SetName(_name);
        }

        public void SetName(string _name)
        {
            name = _name;
            if (!(name[name.Length - 1] == '\0'))
                name += "\0";
        }

        public AnimationNode Advance(CharacterEvent _event) // обработка перехода к слет узлу графа анмаций по событию
        {
            for (int i = 0; i < NodeEvents.Length; i++)
                if (NodeEvents[i].neededEvent.CompareTo(_event.eventName) == 0)
                    return NodeEvents[i].associatedNode;
                
            return this;
        }

        public static void AnimationNodeToStream(AnimationNode node, System.IO.BinaryWriter bw)
        {
            bw.WritePackString(node.name);
            bw.Write(node.animation.type);
            switch (node.animation.type)
            {
                case 0:
                    FullAnimation.FullAnimationToStream(bw, node.animation as FullAnimation);
                    break;
                default:
                    break;
            }
            bw.Write(node.NodeEvents.Length);
            for (int i = 0; i < node.NodeEvents.Length; i++)
            {
                NodeEvent.NodeEventToStream(node.NodeEvents[i], bw);
            }
        }

        public static AnimationNode AnimationNodeFromStream(System.IO.BinaryReader br)
        {
            AnimationNode node = new AnimationNode(br.ReadPackString());
            int animType = br.ReadInt32();

            switch (animType)
            {
                case 0:
                    node.animation= FullAnimation.FullAnimationFromStream(br);
                    break;
                default:
                    break;
            }
            node.NodeEvents = new NodeEvent[br.ReadInt32()];
            for (int i = 0; i < node.NodeEvents.Length; i++)
            {
                NodeEvent.NodeEventFromStream(br);
            }
            return node;
        }

        public override string ToString()
        {
            return name;
        }
    }
    public class AnimationGraph                             // граф анимаций
    {
        public string description
        {
            get;
            private set;
        }

        public void SetDescription(string _description)
        {
            description = _description;
            if (!(description[description.Length - 1] == '\0'))
                description += "\0";
        }

        public List<AnimationNode> nodes;                       // массив узлов

        public AnimationGraph(AnimationNode[] _nodes)
        {
            nodes = new List<AnimationNode>(_nodes);
            description = "new_graph\0";
        }

        public AnimationGraph()
        {
            nodes = new List<AnimationNode>();
        }

        public AnimationNode FindNodeWithName(string nodeName)
        {
            for (int i = 0; i < nodes.Count; i++)
            {
                AnimationNode currentNode = nodes[i];
                if (currentNode != null && currentNode.name == nodeName)
                    return currentNode;
            }
            return null;
        }

        public void AttachNode(AnimationNode newNode)
        {
            for (int i = 0; i < nodes.Count; i++)
                if (newNode.name.CompareTo(nodes[i].name) == 0)
                    return;

            nodes.Add(newNode);
        }

        public static void AnimationGraphToStream(AnimationGraph AnimGraph, System.IO.BinaryWriter bw)
        {
            bw.WritePackString(AnimGraph.description);
            bw.Write(AnimGraph.nodes.Count);
            for (int i = 0; i < AnimGraph.nodes.Count; i++)
                AnimationNode.AnimationNodeToStream(AnimGraph.nodes[i], bw);
        }

        public static AnimationGraph AnimationGraphFromStream(System.IO.BinaryReader br)
        {
            AnimationGraph AGrf = new AnimationGraph();
            AGrf.description = br.ReadPackString();
            int lenth = br.ReadInt32();
            AGrf.nodes = new List<AnimationNode>();
            for (int i = 0; i < lenth; i++)
            {
                AnimationNode node = AnimationNode.AnimationNodeFromStream(br);
                AGrf.nodes.Add(node);
            }
            for (int i = 0; i < AGrf.nodes.Count; i++)
                for (int j = 0; j < AGrf.nodes[i].NodeEvents.Length; j++)
                {
                    AGrf.nodes[i].NodeEvents[j].associatedNode = AGrf.FindNodeWithName(AGrf.nodes[i].NodeEvents[j].associatedNodeName);
                    AGrf.nodes[i].NodeEvents[j].parentNode = AGrf.FindNodeWithName(AGrf.nodes[i].NodeEvents[j].parentName);
                }
            return AGrf;
        }
    }
}
