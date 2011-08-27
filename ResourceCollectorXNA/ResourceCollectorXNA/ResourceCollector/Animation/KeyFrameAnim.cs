using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

namespace ResourceCollector
{
    public class BoneFrame // фрейм кости
    {
        public float frameNumber;
        public DecomposedMatrix frameDMatrix;
        public BoneFrame(float _frameNumber, DecomposedMatrix _frameDMatrix)
        {
            frameNumber = _frameNumber;
            frameDMatrix = _frameDMatrix;
        }
        public BoneFrame(float _frameNumber, Matrix _Matrix)
        {
            frameNumber = _frameNumber;
            frameDMatrix = new DecomposedMatrix(_Matrix);
        }
    }
    public class BoneAnim //анимация кости (набор всех фреймов)
    {
        int boneNumber;
        BoneFrame[] frames;
        public BoneAnim(int _boneNumber, BoneFrame[] _frames)
        {
            boneNumber = _boneNumber;
            frames = _frames;
        }
    }
    public abstract class KeyFrameAnim : Animation //анимация по ключевым кадрам
    {
        public float length;
        BoneAnim[] animation;
        public override Matrix[] GetMatrices(float frame)
        {
            //проинтерполировать кости по кадрам и получить анимации
            return null;
        }

    }
}
