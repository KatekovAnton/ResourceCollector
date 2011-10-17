using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
namespace ResourceCollectorXNA.Engine.Logic.BehaviourModel
{
    public abstract class ObjectBehaviourModel
    {
        public bool moved;
        public Matrix PreviousPosition;
        public Matrix CurrentPosition;
        public Matrix globalpose;

        /// <summary>
        /// установить позицию объекта
        /// </summary>
        /// <param name="GlobalPoseMatrix"></param>
        /// <param name="Additionaldata"></param>
        public abstract void SetGlobalPose(Matrix GlobalPoseMatrix, object Additionaldata);

        /// <summary>
        /// запоминаем что надо
        /// </summary>
        public void BeginDoFrame()
        {
            PreviousPosition = globalpose;
            moved = false;
        }
        
        /// <summary>
        /// передвинуть объект (в его локальных координатах)
        /// </summary>
        /// <param name="displacement"></param>
        public abstract void Move(Vector3 displacement);

        /// <summary>
        /// симулярует
        /// </summary>
        /// <param name="gametime"></param>
        public abstract void DoFrame(GameTime gametime);

        /// <summary>
        /// завершаем всё и копируем параматеры в выходной интерфейс
        /// </summary>
        public void EndDoFrame()
        {
            globalpose = CurrentPosition;
        }
    }
}
