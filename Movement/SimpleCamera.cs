using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Movement
{
  public static class BasicCamera
  {
    private static Vector2 position = Vector2.Zero;
    public static Vector2 viewPortSize = Vector2.Zero;
    public static Rectangle worldRectangle = new Rectangle(0, 0, 0, 0);
    public static int Zoom = 1;

    public static void UpdatePosition(Vector2 value)
    {
      position = new Vector2(
        MathHelper.Clamp(value.X, 0, (worldRectangle.Width - viewPortSize.X) / Zoom),
        MathHelper.Clamp(value.Y, 0, (worldRectangle.Height - viewPortSize.Y) / Zoom)
      );
      Console.WriteLine(position);
    }


    public static Vector2 Position
    {
      get { return position; }

    }


  }

  public class SimpleCamera : Game
  {
    GraphicsDeviceManager _graphics;
    SpriteBatch _spriteBatch;

    Texture2D _spriteSheet;

    public SimpleCamera()
    {
      _graphics = new GraphicsDeviceManager(this);
      Content.RootDirectory = "Content";
      IsMouseVisible = true;
    }

    protected override void Initialize()
    {
      _graphics.PreferredBackBufferWidth = 800;
      _graphics.PreferredBackBufferHeight = 200;
      _graphics.ApplyChanges();

      BasicCamera.Zoom = 4;
      BasicCamera.viewPortSize = new Vector2(_graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight);

      base.Initialize();
    }

    protected override void LoadContent()
    {
      _spriteBatch = new SpriteBatch(GraphicsDevice);

      _spriteSheet = Content.Load<Texture2D>("Minivania/s4m_ur4i_minivania_tilemap");
      BasicCamera.worldRectangle = new Rectangle(0, 0, _spriteSheet.Width * BasicCamera.Zoom, _spriteSheet.Height * BasicCamera.Zoom);
      base.LoadContent();
    }

    protected override void Update(GameTime gameTime)
    {
      if (Keyboard.GetState().IsKeyDown(Keys.W))
      {
        BasicCamera.UpdatePosition(BasicCamera.Position + new Vector2(0, -1f));
      }
      if (Keyboard.GetState().IsKeyDown(Keys.A))
      {
        BasicCamera.UpdatePosition(BasicCamera.Position + new Vector2(-1f, 0));
      }
      if (Keyboard.GetState().IsKeyDown(Keys.S))
      {
        BasicCamera.UpdatePosition(BasicCamera.Position + new Vector2(0, 1f));
      }
      if (Keyboard.GetState().IsKeyDown(Keys.D))
      {
        BasicCamera.UpdatePosition(BasicCamera.Position + new Vector2(1f, 0));
      }


      base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
      GraphicsDevice.Clear(Color.Black);

      _spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, Matrix.CreateScale(BasicCamera.Zoom, BasicCamera.Zoom, 1));

      _spriteBatch.Draw(_spriteSheet, Vector2.Zero, new Rectangle((int)BasicCamera.Position.X, (int)BasicCamera.Position.Y, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight), Color.White);

      _spriteBatch.End();

      base.Draw(gameTime);
    }
  }
}
