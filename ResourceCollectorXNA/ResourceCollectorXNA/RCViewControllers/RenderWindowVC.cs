using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

namespace ResourceCollectorXNA.RCViewControllers
{

    public delegate void SetVector3(Vector3? translation);
    public delegate void SetQuaternion(Quaternion? rotation);
    public delegate void SetVoid();
    public delegate void SetTexts(string[] args);

//    public delegate string[] GetValues();


    public class RenderWindowVC
    {
        public SetTexts setMove;
        public SetTexts setRotationSame;
        public SetTexts setRotationLocal;
        public SetVoid setSelect;
        public SetTexts setRelative;
        //public GetValues newValues;
        public RenderWindowVC(RenderWindow rw)
        {
            setMove = new SetTexts(rw.setMove);
            setRelative = new SetTexts(rw.setRelative);
            setRotationSame = new SetTexts(rw.setRotationSame);
            setRotationLocal = new SetTexts(rw.setRotationLocal);
            setSelect = new SetVoid(rw.setSelect);
        }
    }
}
