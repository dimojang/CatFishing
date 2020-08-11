using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatFishing_Windows.Models
{
    class Character
    {
        /// <summary>
        /// 动画列表
        /// </summary>
        public List<Animation> CharacterAnimation { get; set; } = new List<Animation>();
        
        /// <summary>
        /// 当前激活的动画序号
        /// </summary>
        public int AnimationIndex { get; set; } = 0;

        /// <summary>
        /// 角色状态
        /// </summary>
        public double CharacterState { get; set; } = 1;

        /// <summary>
        /// 角色大小和位置
        /// </summary>
        public Rectangle Position { get; set; }

        /// <summary>
        /// 绘制精灵
        /// </summary>
        /// <param name="spriteBatch">画布</param>
        /// <param name="color">通道色</param>
        public void Draw(SpriteBatch spriteBatch, Color color)
        {
            spriteBatch.Draw(CharacterAnimation[AnimationIndex].CurrentTexture, 
                Position, 
                color);
        }

        /// <summary>
        /// 更新动画帧列表
        /// </summary>
        /// <param name="gameTime">距上次更新画布的时间</param>
        public void Update(GameTime gameTime)
        {
            CharacterAnimation[AnimationIndex].Update(gameTime);
        }
    }
}
