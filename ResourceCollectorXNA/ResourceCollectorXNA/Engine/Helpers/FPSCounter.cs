using System;
using Microsoft.Xna.Framework;




namespace ResourceCollectorXNA.Engine.Helpers{
    public class FpsCounter{
        private TimeSpan _elapsedTime = TimeSpan.Zero;
        private int _frameCounter;
        private float _frameTime;

        private int _framesPerSecond;

        public int FramesPerSecond {
            get { return _framesPerSecond; }
        }

        public float FrameTime {
            get { return _frameTime; }
        }


        public void Update (GameTime gameTime) {
            //накапливаем время прошедшее с момента отрисовки последнего кадра
            _elapsedTime += gameTime.ElapsedGameTime;

            //если накопленное время больше секунды, то считаем кадры
            if (_elapsedTime > TimeSpan.FromSeconds (1)) {
                _elapsedTime -= TimeSpan.FromSeconds (1);
                _framesPerSecond = _frameCounter;
                _frameCounter = 0;
            }

            //увеличиваем счетчик кадров
            _frameCounter++;

            //получаем время затраченное на отрисовку одного кадра
            if (_framesPerSecond > 0) {
                _frameTime = 1000f / _framesPerSecond;
            }
        }
    }
}