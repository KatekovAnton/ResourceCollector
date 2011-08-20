using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Input;

namespace ResourceCollectorXNA.Engine.Interface
{
    public class KeyScan
    {
        public bool pressed;
        public long timePressed;
        public Keys key;
        public KeyScan(Keys _key)
        {
            timePressed = 0;
            pressed = false;
            key = _key;
        }
        public static bool operator ==(KeyScan _key1, KeyScan _key2)
        {
            return _key1.key == _key2.key;
        }
        public static bool operator !=(KeyScan _key1, KeyScan _key2)
        {
            return _key1.key != _key2.key;
        }
        public void Update()
        {
            if (KeyboardManager.currentState.IsKeyDown(key))
            {
                if (!pressed)
                {
                    pressed = true;
                    timePressed = DateTime.Now.Ticks;
                }
            }
            else
            {
                if (pressed)
                {
                    pressed = false;
                    timePressed = 0;
                }
            }
        }
        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }
    }
    public class KeyboardManager
    {
        public static KeyboardState currentState;
        public static List<KeyScan> scaningKeys;
        public static KeyScan GetScanForKey(Keys key)
        {
            KeyScan ks = null;
            for (int i = 0; i < scaningKeys.Count; i++)
                if (scaningKeys[i].key == key)
                    return scaningKeys[i];
            return new KeyScan(key);
        }
        private sealed class KeyboardManagerCreator
        {
            private static readonly KeyboardManager instance = new KeyboardManager();

            public static KeyboardManager _KeyboardManager
            {
                get { return instance; }
            }
        }
        public static KeyboardManager Manager
        {
            get { return KeyboardManagerCreator._KeyboardManager; }
        }
        
        int lastusercount;
        public void Update()
        {
            if (lastusercount != keyboardusers.Count)
            {
                //scaningKeys.Clear();
                foreach (IKeyboardUserInterface user in keyboardusers)
                {
                    List<HotKey> hotkeys = user.hotkeys();
                    foreach (HotKey k in hotkeys)
                    {
                        for (int i = 0; i < k.associatedKeys.Length; i++)
                        {
                            bool containkey = false;
                            for (int hk = 0; hk < scaningKeys.Count; hk++)
                            {
                                if (scaningKeys[hk].key == k.associatedKeys[i])
                                {
                                    containkey = true;
                                    break;
                                }
                            }
                            if (!containkey)
                                scaningKeys.Add(new KeyScan(k.associatedKeys[i]));
                        }
                    }
                }
                lastusercount = keyboardusers.Count;
            }
            currentState = Keyboard.GetState();
            for (int hk = 0; hk < scaningKeys.Count; hk++)
                scaningKeys[hk].Update();

            foreach (IKeyboardUserInterface user in keyboardusers)
            {
                List<HotKey> hotkeys = user.hotkeys();
                foreach (HotKey k in hotkeys)
                {
                    k.TryExecute();
                }
            }

            /*foreach (IKeyboardUserInterface user in keyboardusers)
            {
                if (user.IsKeyboardCaptured())
                {
                    List<HotKey> hotkeys = user.hotkeys();
                    for (int i = 0; i < hotkeys.Count; i++)
                    {
                        bool allpressed = true;

                        int ch = 0;
                        for (; ch < hotkeys[i].associatedKeys.Count; ch++)
                        {
                            if (s.IsKeyDown(hotkeys[i].associatedKeys[ch]))
                            {
                                if (hotkeys[i].pressed[ch])
                                {
                                    hotkeys[i].pressed[ch] = true;
                                }
                                else
                                {
                                    allpressed = false;
                                    hotkeys[i].pressed[ch] = true;
                                    break;
                                }
                            }
                            else
                            {
                                allpressed = false;
                                break;
                            }
                        }
                        

                        if (allpressed)
                        {
                            if (hotkeys[i].action != null)
                                hotkeys[i].action();

                            for (int c = 0; c < hotkeys[i].pressed.Length; c++)
                                hotkeys[i].pressed[c] = false;
                        }
                        else
                        {
                            for (; ch < hotkeys[i].pressed.Length; ch++)
                                hotkeys[i].pressed[ch] = false;
                        }

                    }
                }
            }*/
        }
        private List<IKeyboardUserInterface> keyboardusers;
        public static bool IsMouseCaptured
        {
            get
            {
                foreach (IKeyboardUserInterface user in Manager.keyboardusers)
                {
                    if (user.IsKeyboardCaptured())
                        return true;
                }
                return false;
            }
        }
        protected KeyboardManager()
        {
            keyboardusers = new List<IKeyboardUserInterface>();
            scaningKeys = new List<KeyScan>();
        }
        public void AddKeyboardUser(IKeyboardUserInterface newUser)
        {
            keyboardusers.Add(newUser);
        }
        public void RemoveKeyboardUser(IKeyboardUserInterface user)
        {
            keyboardusers.Remove(user);
        }
    }
}
