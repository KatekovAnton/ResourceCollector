using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ResourceCollector
{
    public class NodeEvent                                  //событие которое возможно для конкретного узла графа анимаций- ребро графа анимаций
    {
        public string neededEvent
        {
            get;
            private set;
        }                          // необходимое событие
        public AnimationNode parentNode;                        // ссылка на владельца 
        public AnimationNode associatedNode;                // узел к которому ведет событие 
        public string description
        {
            get;
            private set;
        }// описание 

        public string parentName
        {
            get;
            private set;
        }
        public string associatedNodeName
        {
            get;
            private set;
        } 

        public NodeEvent(string _description, AnimationNode _parent, AnimationNode _associatedNode, string _neededevent)
        {
            SetDescription( _description);
            associatedNode = _associatedNode;
            parentNode = _parent;


            SetNeededEvevntName(_neededevent);
            SetParentNodeName(parentNode.name);
            SetAssocNodeName(associatedNode.name);
        }

        public void SetNeededEvevntName(string _name)
        {
            neededEvent = _name;
            if (!(neededEvent[neededEvent.Length - 1] == '\0'))
                neededEvent += "\0";
        }

        public void SetDescription(string _description)
        {
            description = _description;
            if (!(description[description.Length - 1] == '\0'))
                description += "\0";
        }

        public void SetParentNodeName(string _name)
        {
            parentName = _name;
            if (!(parentName[parentName.Length - 1] == '\0'))
                parentName += "\0";
        }

        public void SetAssocNodeName(string _name)
        {
            associatedNodeName = _name;
            if (!(associatedNodeName[associatedNodeName.Length - 1] == '\0'))
                associatedNodeName += "\0";
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

        public int index;
        public List<NodeEvent> nodeEvents;                      // исходящие рёбра
        public Animation animation;                         // соответствующая узлу анимация

        public AnimationNode(string _name, Animation _animation)
        {
            SetName(_name);
            nodeEvents = new List<NodeEvent>();
            animation = _animation;
        }

        public AnimationNode(string _name)
        {
            SetName(_name);
            nodeEvents = new List<NodeEvent>();
        }

        public void SetName(string _name)
        {
            name = _name;
            if (!(name[name.Length - 1] == '\0'))
                name += "\0";
        }

        public AnimationNode Advance(CharacterEvent _event) // обработка перехода к слет узлу графа анмаций по событию
        {
            for (int i = 0; i < nodeEvents.Count; i++)
                if (nodeEvents[i].neededEvent.CompareTo(_event.eventName) == 0)
                    return nodeEvents[i].associatedNode;
                
            return this;
        }

        public static void AnimationNodeToStream(AnimationNode node, System.IO.BinaryWriter bw)
        {
            bw.Write(node.index);
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
            bw.Write(node.nodeEvents.Count);
            for (int i = 0; i < node.nodeEvents.Count; i++)
            {
                NodeEvent.NodeEventToStream(node.nodeEvents[i], bw);
            }
        }

        public static AnimationNode AnimationNodeFromStream(System.IO.BinaryReader br)
        {
            int index = br.ReadInt32();
            AnimationNode node = new AnimationNode(br.ReadPackString());
            node.index = index; // br.ReadInt32();
            int animType = br.ReadInt32();

            switch (animType)
            {
                case 0:
                    node.animation= FullAnimation.FullAnimationFromStream(br);
                    break;
                default:
                    break;
            }
            int count = br.ReadInt32();
            node.nodeEvents = new List<NodeEvent>();
            for (int i = 0; i < count; i++)
            {
                node.nodeEvents.Add( NodeEvent.NodeEventFromStream(br));
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

        // массив узлов (анимация + её имя(название == идентификатор))
        public List<AnimationNode> nodes;         

        //индексы костей полного скелета, которые анимирует 
        //этот граф. это не правильно но так проще =(
        public int[] indexes;
        public AnimationGraph(AnimationNode[] _nodes, int[] _indexes)
        {
            nodes = new List<AnimationNode>(_nodes);
            description = "new_graph\0";

            indexes = new int[_indexes.Length];
            _indexes.CopyTo(indexes, 0);
        }

        public AnimationGraph(string _description, int []_indexes)
        {
            SetDescription(_description);
            nodes = new List<AnimationNode>();
            indexes = new int[_indexes.Length];
            _indexes.CopyTo(indexes, 0);
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
            bw.Write(AnimGraph.indexes.Length);
            for (int i = 0; i < AnimGraph.indexes.Length; i++)
                bw.Write(AnimGraph.indexes[i]);

            bw.WritePackString(AnimGraph.description);
            bw.Write(AnimGraph.nodes.Count);
            for (int i = 0; i < AnimGraph.nodes.Count; i++)
                AnimationNode.AnimationNodeToStream(AnimGraph.nodes[i], bw);
        }

        public static AnimationGraph AnimationGraphFromStream(System.IO.BinaryReader br)
        {
            int[] indexes = new int[br.ReadInt32()];
            for (int i = 0; i < indexes.Length; i++)
                indexes[i] = br.ReadInt32();
            string description = br.ReadPackString();
            AnimationGraph AGrf = new AnimationGraph(description, indexes);
     
            int lenth = br.ReadInt32();
            AGrf.nodes = new List<AnimationNode>();
            for (int i = 0; i < lenth; i++)
            {
                AnimationNode node = AnimationNode.AnimationNodeFromStream(br);
                AGrf.nodes.Add(node);
            }
            for (int i = 0; i < AGrf.nodes.Count; i++)
                for (int j = 0; j < AGrf.nodes[i].nodeEvents.Count; j++)
                {
                    AGrf.nodes[i].nodeEvents[j].associatedNode = AGrf.FindNodeWithName(AGrf.nodes[i].nodeEvents[j].associatedNodeName);
                    AGrf.nodes[i].nodeEvents[j].parentNode = AGrf.FindNodeWithName(AGrf.nodes[i].nodeEvents[j].parentName);
                }
            return AGrf;
        }

        public override string ToString()
        {
            return this.description.Remove(this.description.Length - 1);
        }
    }
}
