using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;
using System.Collections.Generic;

using CatFishing_Windows.Models;
using System;
using System.Timers;

namespace CatFishing_Windows
{

    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class MainGame : Game
    {
        readonly GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        //游戏信息
        private GameInfo ThisGame = new GameInfo();

        //猫
        private Character Cat = new Character();
        //鱼
        private Character Fish = new Character();

        private Texture2D Title; //标题页面
        private Texture2D Ending; //结束画面
        private Texture2D Tip; //新手指引
        private Texture2D LOGO;
        private List<Texture2D> Background; //场景
        private List<Texture2D> Wire; //鱼线

        private Texture2D Tool; //道具栏
        private Texture2D Sign; //道具对应的介绍

        private Texture2D Food; //食物
        private double FoodTime = 0; //上一次偷食的GameTime
        private List<Point> FoodPoints = new List<Point>(); //食物坐标
        
        private Texture2D MousePoint; //鼠标
        
        private Song BackGroundMusic; //背景音乐

        
        
        public MainGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            ThisGame.NowGameState = GameStates.Loading; //游戏状态初始为开始页面
            ThisGame.Counter = 0;

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            #region 加载 猫 资源
            //加载所有猫贴图
            List<Texture2D> CatList = new List<Texture2D>()
            {
                Content.Load<Texture2D>("cat1"),
                Content.Load<Texture2D>("cat2"),
                Content.Load<Texture2D>("cat3"),
                Content.Load<Texture2D>("cat4")
            };
            //定义猫猫动画
            Animation CatA0 = new Animation();//动画1
            CatA0.AddFrame(CatList[0], 0.1);
            CatA0.AddFrame(CatList[1], 0.1);
            CatA0.AddFrame(CatList[2], 0.5);
            CatA0.AddFrame(CatList[3], 0.1);
            Cat.CharacterAnimation.Add(CatA0);
            //猫位置
            Cat.Position = new Rectangle(0, 5,560,285);
            #endregion
            #region 加载并播放背景音乐
            BackGroundMusic = Content.Load<Song>("bgm");
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Play(BackGroundMusic);
            #endregion
            #region 加载 鱼 资源
            //加载所有鱼贴图
            string[] fishList = { "fish1", "fish2", "fish3", "fish1-2", "fish2-2" };
            List<Texture2D> FishList = new List<Texture2D>();
            foreach(var i in fishList) FishList.Add(Content.Load<Texture2D>(i));
            //自定义鱼动画
            Animation FishA0 = new Animation();//动画1
            FishA0.AddFrame(FishList[0], 0.5);
            FishA0.AddFrame(FishList[3], 0.5);

            Animation FishA1 = new Animation();//动画2
            FishA1.AddFrame(FishList[1], 0.5);
            FishA1.AddFrame(FishList[4], 0.5);

            Animation FishA2 = new Animation();//动画3
            FishA2.AddFrame(FishList[2], 0.5);

            Fish.CharacterAnimation.Add(FishA0);
            Fish.CharacterAnimation.Add(FishA1);
            Fish.CharacterAnimation.Add(FishA2);

            Fish.Position = new Rectangle(400, 300, 74, 32);
            #endregion

            Background = new List<Texture2D>
            {
                 Content.Load<Texture2D>("bg"),
                 Content.Load<Texture2D>("bg2")
            };

            Wire = new List<Texture2D>()
            {
                Content.Load<Texture2D>("wire"),
                Content.Load<Texture2D>("wire2")
            };
            
            MousePoint = Content.Load<Texture2D>("mc");

            Food = Content.Load<Texture2D>("food");
            Tool = Content.Load<Texture2D>("tool");
            Sign = Content.Load<Texture2D>("sign");
            Title = Content.Load<Texture2D>("title");
            Ending = Content.Load<Texture2D>("end");
            Tip = Content.Load<Texture2D>("tip");

            LOGO = Content.Load<Texture2D>("xsyd");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            MouseState mouseState = Mouse.GetState(); //获取鼠标状态
            KeyboardState keyboardState = Keyboard.GetState(); //获取键盘状态

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || keyboardState.IsKeyDown(Keys.Escape))
                Exit();

            Cat.Update(gameTime);
            Fish.Update(gameTime);

            // TODO: Add your update logic here
            switch (ThisGame.NowGameState)
            {
                case GameStates.Loading:
                    if (gameTime.TotalGameTime.TotalSeconds - ThisGame.Counter > 2) ThisGame.NowGameState = GameStates.Starting;
                    break;
                case GameStates.Running:
                    #region Running
                    if (ThisGame.NowGameState == GameStates.Running)
                    {
                        //左键放置饲料
                        if (mouseState.LeftButton == ButtonState.Pressed && gameTime.TotalGameTime.TotalSeconds - FoodTime > 1)
                        {
                            if (480.0 - (double)mouseState.Y <= ((double)mouseState.X - 16.0) * (71.0 / 219.0) + 107.0 || 480.0 - (double)mouseState.Y <= ((double)mouseState.X - 16.0) * (231.0 / 268.0) - (17223.0 / 134.0))
                            {
                                FoodPoints.Add(new Point(mouseState.X - 16, mouseState.Y - 16));
                                FoodTime = gameTime.TotalGameTime.TotalSeconds;
                            }
                        }

                        //空格下钩
                        if (Keyboard.GetState().IsKeyDown(Keys.Space))
                        {
                            if ((Fish.Position.X < 523 && Fish.Position.X + 74 > 547) && (283 > Fish.Position.Y && 283 < Fish.Position.Y + 32) && Fish.CharacterState > 0) 
                            {
                                Fish.CharacterState = 0;
                                Fish.AnimationIndex = 2;
                                Fish.Position = new Rectangle(512, 195, 32, 74);
                                ThisGame.Counter = gameTime.TotalGameTime.TotalSeconds;
                            }
                        }
                        if (Fish.CharacterState == 0)
                        {
                            if (gameTime.TotalGameTime.TotalSeconds - ThisGame.Counter > 1) ThisGame.NowGameState = GameStates.Ended;
                        }

                        //吃饲料
                        if (FoodPoints.Count > 0 && Fish.CharacterState > 0)
                        {
                            if (Math.Abs(Fish.Position.X - FoodPoints[FoodPoints.Count - 1].X) <= 15 && Math.Abs(Fish.Position.Y - FoodPoints[FoodPoints.Count - 1].Y) <= 15)
                            {
                                FoodPoints.RemoveAt(FoodPoints.Count - 1);
                                return;
                            }

                            //如果食物在鱼的右边，那么 IsFishRight=true
                            if (Fish.Position.X > FoodPoints[FoodPoints.Count - 1].X)
                                Fish.AnimationIndex = 0;
                            else
                                Fish.AnimationIndex = 1;

                            Fish.Position = new Rectangle(
                                Fish.Position.X + (FoodPoints[FoodPoints.Count - 1].X - Fish.Position.X) / 8,
                                Fish.Position.Y + (FoodPoints[FoodPoints.Count - 1].Y - Fish.Position.Y) / 8, 
                                74, 
                                32);
                        }
                    }
                    #endregion
                    break;
                case GameStates.Starting:
                    #region Starting
                    if (mouseState.LeftButton == ButtonState.Pressed)
                    {
                        ThisGame.NowGameState = GameStates.Running;
                    }
                    #endregion
                    break;
                case GameStates.Ended:
                    break;
            }
            
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            spriteBatch.Begin();

            switch (ThisGame.NowGameState)
            {
                case GameStates.Loading:
                    spriteBatch.Draw(LOGO, new Vector2(0.0f, 0.0f), Color.White);
                    break;
                case GameStates.Running:
                    #region Running
                    //画背景
                    spriteBatch.Draw(Fish.CharacterState == 0 ? Background[1] : Background[0],
                        new Rectangle(0, 0, Window.ClientBounds.Width, Window.ClientBounds.Height),
                        Color.White);
                    //画饲料
                    for (int i = 0; i < FoodPoints.Count; i++)
                        spriteBatch.Draw(Food,
                            new Rectangle(FoodPoints[i], new Point(16, 16)),
                            Color.White);
                    //画猫
                    Cat.Draw(spriteBatch, Color.White);
                    //画鱼线
                    spriteBatch.Draw(Fish.CharacterState == 0 ? Wire[1] : Wire[0],
                        new Vector2(523, 15),
                        Color.White);
                    //画鱼
                    Fish.Draw(spriteBatch, Color.White);
                    //画道具
                    spriteBatch.Draw(Tip, 
                        new Vector2(800 - 64, 480 - 32), 
                        Color.White);
                    spriteBatch.Draw(Tool,
                        new Rectangle(800 - 48, 0, 48, 48),
                        gameTime.TotalGameTime.TotalSeconds - FoodTime > 1 ? Color.White : Color.Gray);
                    if (Mouse.GetState().X > 800 - 48 && Mouse.GetState().Y < 48)
                        spriteBatch.Draw(Sign,
                            new Rectangle(800 - 150 - (800 - Mouse.GetState().X), 480 - 98 - (480 - Mouse.GetState().Y) + 98, 150, 98),
                            Color.White);
                    #endregion
                    break;
                case GameStates.Starting:
                    #region Starting
                    //画背景
                    spriteBatch.Draw(Fish.CharacterState == 0 ? Background[1] : Background[0],
                        new Rectangle(0, 0, Window.ClientBounds.Width, Window.ClientBounds.Height),
                        Color.White);
                    //画猫
                    Cat.Draw(spriteBatch, Color.White);
                    //画鱼线
                    spriteBatch.Draw(Fish.CharacterState == 0 ? Wire[1] : Wire[0],
                        new Vector2(523, 15),
                        Color.White);
                    //画 标题页面
                    spriteBatch.Draw(Title, new Vector2(0, 0), Color.White);
                    #endregion
                    break;
                case GameStates.Ended:
                    spriteBatch.Draw(Ending, new Vector2(0, 0), Color.White);
                    break;
            }
            
            //画鼠标
            spriteBatch.Draw(MousePoint,
                new Rectangle(Mouse.GetState().X, Mouse.GetState().Y, 32, 32),
                gameTime.TotalGameTime.TotalSeconds - FoodTime > 1 ? Color.White : Color.Gray);
            

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
