using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatFishing_Windows.Models
{
    class Animation
    {
        /// <summary>
        /// 动画的帧列表
        /// </summary>
        public List<AnimationFrame> Frames = new List<AnimationFrame>();

        /// <summary>
        /// 已播放时间
        /// </summary>
        TimeSpan TimeIntoAnimation;

        /// <summary>
        /// 动画持续时间
        /// </summary>
        TimeSpan Duration
        {
            get
            {
                double totalSeconds = 0;
                foreach (var frame in Frames)
                {
                    totalSeconds += frame.Duration.TotalSeconds;
                }

                return TimeSpan.FromSeconds(totalSeconds);
            }
        }

        /// <summary>
        /// 添加帧
        /// </summary>
        /// <param name="frame">帧材质</param>
        /// <param name="duration">帧持续时间</param>
        public void AddFrame(Texture2D frame, double duration)
        {
            AnimationFrame animationFrame = new AnimationFrame()
            {
                FrameTexture = frame,
                Duration = TimeSpan.FromSeconds(duration)
            };
            Frames.Add(animationFrame);
        }

        /// <summary>
        /// 计算相对播放时间
        /// 若播放时间大于序列总时间 则循环
        /// </summary>
        /// <param name="gameTime">距离上次Update所过去的时间</param>
        public void Update(GameTime gameTime)
        {
            double secondsIntoAnimation =
                TimeIntoAnimation.TotalSeconds + gameTime.ElapsedGameTime.TotalSeconds;

            ///用已播放的时间%动画总时间
            ///若 已播放时间大于总时间 则余数是多出来的秒数
            ///若 已播放时间小于总时间 则余数为 已播放时间
            double remainder = secondsIntoAnimation % Duration.TotalSeconds;

            TimeIntoAnimation = TimeSpan.FromSeconds(remainder);
        }

        /// <summary>
        /// 获取当前时间对应帧
        /// </summary>
        public Texture2D CurrentTexture
        {
            get
            {
                AnimationFrame currentFrame = null;

                // 逐帧查找对应的帧
                TimeSpan accumulatedTime = new TimeSpan();
                foreach (var frame in Frames)
                {
                    if (accumulatedTime + frame.Duration >= TimeIntoAnimation)
                    {
                        currentFrame = frame;
                        break;
                    }
                    else accumulatedTime += frame.Duration;
                }

                //如果无法找到帧 则返回最后一帧
                if (currentFrame == null)
                    currentFrame = Frames.LastOrDefault();

                return currentFrame.FrameTexture;
            }
        }
    }
}
