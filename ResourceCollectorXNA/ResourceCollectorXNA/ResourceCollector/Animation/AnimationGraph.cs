using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ResourceCollector
{
    public class NodeEvent //событие которое возможно из конретного узла фрафа анимаций
    {
        public CharacterEvent neededevent; // необходимое событие
        public AnimationNode parent;// ссылка на владельца 
        public AnimationNode associatedNode; //узел к которому ведет событие 
        public String description;// описание 
        public Object tag;// 

        public NodeEvent(String _description, AnimationNode _parent, AnimationNode _associatedNode, CharacterEvent _neededevent)
        {
            description = _description;
            associatedNode = _associatedNode;
            neededevent = _neededevent;
            parent = _parent;

        }

        public NodeEvent()
        {
        }

        public override string ToString()
        {
            return description;
        }
    }
    public class AnimationNode //узел графа анимаций
    {
        public NodeEvent[] NodeEvents;
        public Animation animation;
        public Object Tag;

        public AnimationNode(Animation _animation)
        {
            animation = _animation;
        }

        public AnimationNode()
        {
        }

        public override string ToString()
        {
            return animation.name;
        }
    }
    public class AnimationGraph // граф анимаций
    {
        public String description;
        public AnimationNode[] nodes; // массив узлов
        public AnimationNode currentNode; // текущий узел

        public AnimationGraph(AnimationNode[] _nodes)
        {
            nodes = _nodes;
            description = "";
            if (nodes != null)
            {
                currentNode = nodes[0];
            }
        }

        public AnimationGraph()
        {

        }

        public void Advance(CharacterEvent _event) // обработка перехода к слет узлу графа анмаций по событию
        {
            for (int i = 0; i < currentNode.NodeEvents.Length; i++)
            {
                if (currentNode.NodeEvents[i].neededevent.eventName.CompareTo(_event.eventName) == 0)
                {
                    currentNode = currentNode.NodeEvents[i].associatedNode;
                    return;
                }
            }
        }

        public static void AnimationGraffToStream(AnimationGraph AnimGraf, System.IO.BinaryWriter bw)
        {
            bw.Write(AnimGraf.description);
            bw.Write(AnimGraf.nodes.Length);
            for (int i = 0; i < AnimGraf.nodes.Length; i++)
            {
                bw.Write(AnimGraf.nodes[i].animation.type);
                if (AnimGraf.nodes[i].animation.type == 0)
                {
                    FullAnim fa = (FullAnim)AnimGraf.nodes[i].animation;
                    bw.Write(fa.name);
                    bw.Write(fa.BonesCount);
                    bw.Write(fa.isTransition);
                    bw.Write((double)fa.length);
                    FullAnim.MtrxToStream(bw, fa.matrices);
                }
                if (AnimGraf.nodes[i].NodeEvents != null)
                {
                    bw.Write(AnimGraf.nodes[i].NodeEvents.Length);
                    for (int j = 0; j < AnimGraf.nodes[i].NodeEvents.Length; j++)
                    {
                        bw.Write(AnimGraf.nodes[i].NodeEvents[j].description);
                        bw.Write((int)AnimGraf.nodes[i].NodeEvents[j].parent.Tag);
                        bw.Write((int)AnimGraf.nodes[i].NodeEvents[j].associatedNode.Tag);
                        bw.Write(AnimGraf.nodes[i].NodeEvents[j].neededevent.eventName);

                    }
                }
                else
                {
                    bw.Write(0);
                }
            }
        }

        public static AnimationGraph AnimationGraffFromStream(System.IO.BinaryReader br)
        {
            int lenth = 0;
            AnimationGraph AGrf = new AnimationGraph();
            AGrf.description = br.ReadString();
            lenth = br.ReadInt32();
            AGrf.nodes = new AnimationNode[lenth];
            for (int i = 0; i < lenth; i++)
            {
                int type = br.ReadInt32();
                if (type == 0)
                {
                    FullAnim fa = new FullAnim();
                    fa.type = type;
                    fa.name = br.ReadString();
                    //br.Write(fa.name);
                    fa.BonesCount = br.ReadInt32();
                    //br.Write(fa.BonesCount);
                    fa.isTransition = br.ReadBoolean();
                    //br.Write(fa.isTransition);
                    fa.length = (float)br.ReadDouble();
                    //br.Write(fa.length);
                    fa.matrices = FullAnim.MtrxFromStream(br);
                    //MtrxToStream(br, fa.matrices);
                    AGrf.nodes[i] = new AnimationNode(fa);
                    //AGrf.nodes[i].animation = fa;
                }
                int NodeEventsLength = br.ReadInt32();
                //bw.Write(AnimGraf.nodes[i].NodeEvents.Length);
                if (NodeEventsLength > 0)
                {
                    AGrf.nodes[i].NodeEvents = new NodeEvent[NodeEventsLength];
                    for (int j = 0; j < NodeEventsLength; j++)
                    {
                        AGrf.nodes[i].NodeEvents[j] = new NodeEvent();
                        //bw.Write(AnimGraf.nodes[i].NodeEvents[j].description);
                        AGrf.nodes[i].NodeEvents[j].description = br.ReadString();
                        //bw.Write((int)AnimGraf.nodes[i].NodeEvents[j].parent.Tag);
                        AGrf.nodes[i].NodeEvents[j].parent = new AnimationNode();
                        AGrf.nodes[i].NodeEvents[j].parent.Tag = br.ReadInt32();
                        //bw.Write((int)AnimGraf.nodes[i].NodeEvents[j].associatedNode.Tag);
                        AGrf.nodes[i].NodeEvents[j].associatedNode = new AnimationNode();
                        AGrf.nodes[i].NodeEvents[j].associatedNode.Tag = br.ReadInt32();
                        //bw.Write(AnimGraf.nodes[i].NodeEvents[j].neededevent.eventName);
                        AGrf.nodes[i].NodeEvents[j].neededevent = new CharacterEvent(br.ReadString());
                    }
                }
            }
            loadNodeEventsByTag(AGrf);
            return AGrf;
        }

        public static void loadNodeEventsByTag(AnimationGraph AGrf)
        {
            for (int i = 0; i < AGrf.nodes.Length; i++)
            {
                if (AGrf.nodes[i].NodeEvents != null)
                {
                    for (int j = 0; j < AGrf.nodes[i].NodeEvents.Length; j++)
                    {
                        AGrf.nodes[i].NodeEvents[j].parent = AGrf.nodes[(int)AGrf.nodes[i].NodeEvents[j].parent.Tag];
                        AGrf.nodes[i].NodeEvents[j].associatedNode = AGrf.nodes[(int)AGrf.nodes[i].NodeEvents[j].associatedNode.Tag];

                    }
                }
            }
        }
    }
}
