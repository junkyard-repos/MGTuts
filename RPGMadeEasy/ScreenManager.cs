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
        public ContentManager Content { get; private set; }

        GameScreen currentScreen;

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
            Dimensions = new Vector2(1280, 720);
            currentScreen = new SplashScreen();
        }

        public void LoadContent(ContentManager content)
        {
            Content = new ContentManager(content.ServiceProvider, "Content");
            currentScreen.LoadContent();
        }

        public void UnloadContent() 
        {
            currentScreen.UnloadContent();
        }

        public void Update(GameTime gameTime)
        {
            currentScreen.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch) 
        {
            currentScreen.Draw(spriteBatch);
        }

        
    }
}
