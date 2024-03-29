﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TopDownShooter
{
  public static class Progarm
  {

    [STAThread]
    static void Main()
    {
      using var game = new Game();
      game.Run();
    }
  }

  public class Game : Microsoft.Xna.Framework.Game
  {
    private GraphicsDeviceManager _graphics;

    public Game()
    {
      _graphics = new GraphicsDeviceManager(this);
      Content.RootDirectory = "Content";
      IsMouseVisible = true;
    }

    protected override void Initialize()
    {
      // TODO: Add your initialization logic here

      base.Initialize();
      MusicPlayer.Init();
      var player = MusicPlayer.Instance;
    }

    protected override void LoadContent()
    {
      Globals.Content = this.Content;
      Globals.SpriteBatch = new SpriteBatch(GraphicsDevice);

      // TODO: use this.Content to load your game content here
    }

    protected override void Update(GameTime gameTime)
    {
      if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
        Exit();

      // TODO: Add your update logic here

      base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
      GraphicsDevice.Clear(Color.CornflowerBlue);

      Globals.SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);



      Globals.SpriteBatch.End();

      base.Draw(gameTime);
    }
  }
}
