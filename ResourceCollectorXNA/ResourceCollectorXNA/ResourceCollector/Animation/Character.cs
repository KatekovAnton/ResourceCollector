using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

namespace ResourceCollector
{
    public class CharacterEvent // событие обрабатываемое анимацией
    {
        public string eventName;
        public DateTime createdTime;

        public CharacterEvent(string _eventname)
        {
            eventName = _eventname;
            createdTime = DateTime.Now;
        }

        public override string ToString()
        {
            return this.eventName;
        }
    }

    public class CharacterPart //класс частей персонажа(например верх и низ)
    {
        public AnimationGraph animgraph;
        public void ReceiveEvent(CharacterEvent _event)
        {
            animgraph.Advance(_event);
        }
    }

    public class Character // класс перса
    {
        public CharacterPart[] parts;// список частей
        public SkeletonWithAddInfo Skeleton
        {
            get;
            protected set;
        }
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
    }
}
