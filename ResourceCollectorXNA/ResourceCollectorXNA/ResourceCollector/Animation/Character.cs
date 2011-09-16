using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

namespace ResourceCollector
{
    public class CharacterEvent                     // событие обрабатываемое анимацией
    {
        public string eventName;
        public DateTime createdTime;

        public CharacterEvent(string _eventname)
        {
            eventName = _eventname;
            createdTime = DateTime.Now;
        }

        public bool CompareTo(CharacterEvent tmp)
        {
            if (this.eventName.CompareTo(tmp.eventName) == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static bool operator == (CharacterEvent _event1, CharacterEvent _event2)
        {
            return _event1.eventName.CompareTo(_event2.eventName) == 0;
        }

        public static bool operator != (CharacterEvent _event1, CharacterEvent _event2)
        {
            return _event1.eventName.CompareTo(_event2.eventName) != 0;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override string ToString()
        {
            return this.eventName;
        }
    }

    public class CharacterPart                          //класс частей персонажа (например верх и низ)
    {
        public AnimationGraph animGraph;

        public CharacterPart()
        { }
    }

    public class Character                              // класс чарактера
    {
        public CharacterPart[] parts;                   // список частей

        public SkeletonWithAddInfo skeleton;

        public Character()
        { }
    }

    public class CharacterUnit
    {
        public Character _baseCharacter;
        public AnimationNode[] _currentNodes;           // текущие узлы

        public void ReceiveEvent(CharacterEvent _event)
        {
            foreach(AnimationNode node in _currentNodes)
                node.Advance(_event);
        }
    }
}
