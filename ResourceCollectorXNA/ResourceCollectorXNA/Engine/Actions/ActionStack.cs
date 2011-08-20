using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ResourceCollectorXNA.Engine.Actions
{
    public class ActionStack
    {
        private List<EditorAction> actions;
        private int maxstacksize;
        public void clear()
        {
            actions.Clear();
        }
        public ActionStack(int stacksize=20)
        {
            actions = new List<EditorAction>();
            maxstacksize = stacksize;
        }
        public int Count
        {
            get
            {
                return actions.Count;
            }
        }
        public void AddNewAction(EditorAction comletedAction)
        {
            ConsoleWindow.TraceMessage("added new action " + comletedAction.ToString());
            actions.Add(comletedAction);
            if (actions.Count > maxstacksize)
            {
                actions[0].onActionDeleted();
                actions.RemoveAt(0);
            }
        }
        public EditorAction RemoveLastAction()
        {
            if (actions.Count > 0)
            {
                EditorAction action = actions[actions.Count - 1];
                actions.RemoveAt(actions.Count - 1);
                return action;
            }
            return null;
        }
        ~ActionStack()
        {
            actions.Clear();
        }
    }
}
