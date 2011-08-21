using System;

namespace ResourceCollectorXNA.Engine.Logic
{
    public class Level:GameScene{
        public void ObjectAdded(PivotObject newObject) {
            Console.WriteLine("ObjectAdded");
        }


        public void Cleared() {
            Console.WriteLine("Cleared");
        }
    }
}
