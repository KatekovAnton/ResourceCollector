using System;
using Microsoft.Xna.Framework;
namespace ResourceCollectorXNA
{
#if WINDOWS || XBOX
    static class Program
    {[STAThread]
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (MyGame game = new MyGame())
            {
                game.Run();
            }
        }
    }
#endif


    /*

    public abstract class Animation
    {
        public abstract Microsoft.Xna.Framework.Matrix[] GetMatrices(float frame);
        public bool isTransition;
    }
    public abstract class FullAnim:Animation
    {
        Matrix[][] matrices;
        
        public override Matrix[] GetMatrices(float frame)
        {
            return matrices[Convert.ToInt32(frame)];
        }
    }

    public class BoneFrame
    {
        float frameNumber;
        Matrix frameMatrix;
    }
    public class BoneAnim
    {
        int boneNumber;
        BoneFrame[] frames;
    }
    public abstract class KeyFrameAnim : Animation
    {
        public float length;
        BoneAnim[] animation;
        public override Matrix[] GetMatrices(float frame)
        {
            //проинтерполировать кости по кадрам и получить анимации
            return null;
        }
    }
    
    public class CharacterEvent
    {
        public string eventName;
        DateTime createdTime;
        public CharacterEvent(string _eventname)
        {
            eventName = _eventname;
            createdTime = DateTime.Now;
        }
    }
    public class NodeEvent
    {
        public CharacterEvent neededevent;
        public AnimationNode associatedNode;
    }
    public class AnimationNode
    {
        public string animationName;
        public NodeEvent[] NodeEvents;

        public Animation animation;
    }
    public class AnimationGraph
    {
        public AnimationNode[] nodes;
        public AnimationNode currentNode;
        public AnimationGraph( AnimationNode[] _nodes)
        {
            nodes = _nodes;
            currentNode = nodes[0];
        }
        public void Advance(CharacterEvent _event)
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
    }


    public class CharacterPart
    {
        AnimationGraph graph;
        public void ReceiveEvent(CharacterEvent _event)
        {
            graph.Advance(_event);
        }
    }

    public class Character
    {
        CharacterPart[] parts;
        public void ReceiveEvent(CharacterEvent _event)
        {
            foreach (CharacterPart p in parts)
            {
                p.ReceiveEvent(_event);
            }
        }
    }

    //0 1.56     3.15   25.7556  100 
    public class npc
    {
        float curentframe;
        KeyFrameAnim anim;
        bool forward;
        Matrix[] matricesforshader;
        public void doframe(float time)
        {
            if (forward)
                curentframe += time;
            else
                curentframe -= time;

            if (curentframe < 0)
                curentframe += anim.length;
            else if (curentframe > anim.length)
                curentframe -= anim.length;

            matricesforshader = anim.GetMatrices(curentframe);
        }
    }*/
}

