using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatFishing_Windows.Models
{
    class AnimationFrame
    {
        //帧贴图
        public Texture2D FrameTexture { get; set; }

        //帧持续时间
        public TimeSpan Duration { get; set; }
    }
}
