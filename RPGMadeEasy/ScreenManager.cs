using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace RPGMadeEasy
{
    public class ScreenManager
    {
        private static ScreenManager instance;
        public Vector2 Dimensions { get; private set; }

        public static ScreenManager Instance
        {
            get 
            {
                if (instance == null)
                {
                    instance = new ScreenManager();
                }

                return instance;
            }
        }

        public ScreenManager()
        {
            Dimensions = new Vector2(640, 480);
        }

        public void LoadContent(ContentManager content)
        {

        }

        public void UnloadContent() { }

        public void Update()
        {

        }

        public void Draw(SpriteBatch spriteBatch) { }

        
    }
}
