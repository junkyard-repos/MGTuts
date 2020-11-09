using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Riemer
{
  internal class Game2 : Game
  {
    private GraphicsDeviceManager _graphics;
    private GraphicsDevice _device;
    private SpriteBatch _spriteBatch;

    private GameWindow gameWindow;
    private SwapChainRenderTarget target;

    private Effect _effect;
    private VertexPositionColor[] _tri1;
    private VertexPositionColor[] _tri2;

    private float _angle = 0f;

    private Matrix _viewMatrix;
    private Matrix _projectionMatrix;

    private Effect _testEffect;
    private Texture2D _foxHead;

    [Test("Hi there!")]
    private void CreateGameWindow()
    {
      gameWindow = GameWindow.Create(this, 300, 300);
      gameWindow.Position = (Vector2.One * 300).ToPoint();
      target = new SwapChainRenderTarget(GraphicsDevice, gameWindow.Handle, 300, 300);

      var f = System.Windows.Forms.Control.FromHandle(gameWindow.Handle).FindForm();
      f.Show();

      target.Present();
    }

    private void UpdateTriPos(float x, float y)
    {
      _tri1[0].Position = new Vector3(_tri1[0].Position.X + x, _tri1[0].Position.Y + y, 0f);
      _tri1[1].Position = new Vector3(_tri1[1].Position.X + x, _tri1[1].Position.Y + y, 0f);
      _tri1[2].Position = new Vector3(_tri1[2].Position.X + x, _tri1[2].Position.Y + y, -5f);
    }

    private void UpdateCameraPos(float x, float y)
    {
      var t = _viewMatrix.Translation;
      _viewMatrix = Matrix.CreateLookAt(new Vector3(t.X + x, t.Y + y, 50), new Vector3(0, 0, 0), new Vector3(0, 1, 0));
    }

    private void SetUpVertices()
    {
      _tri1 = new VertexPositionColor[3];

      _tri1[0].Position = new Vector3(0f, 0f, 0f);
      _tri1[0].Color = Color.Red;
      _tri1[1].Position = new Vector3(10f, 10f, 0f);
      _tri1[1].Color = Color.Yellow;
      _tri1[2].Position = new Vector3(10f, 0f, -5f);
      _tri1[2].Color = Color.Green;
    }

    private void SetUpCamera()
    {
      _viewMatrix = Matrix.CreateLookAt(new Vector3(0, 0, 50), new Vector3(0, 0, 0), new Vector3(0, 1, 0));
      //_viewMatrix = Matrix.CreateLookAt(new Vector3(0, 0, -50), new Vector3(0, 0, 0), new Vector3(0, 1, 0));
      _projectionMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, _device.Viewport.AspectRatio, 1.0f, 300.0f);
    }

    public Game2()
    {
      _graphics = new GraphicsDeviceManager(this);

      Content.RootDirectory = "Content";

      IsMouseVisible = true;
    }

    protected override void Initialize()
    {
      _graphics.PreferredBackBufferWidth = 500;
      _graphics.PreferredBackBufferHeight = 500;
      _graphics.IsFullScreen = false;
      _graphics.ApplyChanges();

      SetUpVertices();

      base.Initialize();
    }

    protected override void LoadContent()
    {
      _spriteBatch = new SpriteBatch(GraphicsDevice);
      _device = _graphics.GraphicsDevice;
      _effect = Content.Load<Effect>("Series 1/effects");
      _testEffect = Content.Load<Effect>("Shaders/Test");
      _foxHead = Content.Load<Texture2D>("2d/FoxHeadWithShadow");

      RasterizerState rs = new RasterizerState();
      rs.CullMode = CullMode.None;
      _device.RasterizerState = rs;

      SetUpCamera();

      CreateGameWindow();
    }

    protected override void Update(GameTime gameTime)
    {
      if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
                Keyboard.GetState().IsKeyDown(Keys.Escape))
      {
        Exit();
      }

      float x = 0;
      float y = 0;

      if (Keyboard.GetState().IsKeyDown(Keys.W))
      {
        y += 1;
      }
      if (Keyboard.GetState().IsKeyDown(Keys.S))
      {
        y -= 1;
      }
      if (Keyboard.GetState().IsKeyDown(Keys.A))
      {
        x -= 1;
      }
      if (Keyboard.GetState().IsKeyDown(Keys.D))
      {
        x += 1;
      }

      _angle += 0.005f;

      //UpdateTriPos(x, y);
      //UpdateCameraPos(x, y);

      base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
      GraphicsDevice.SetRenderTarget(target);
      GraphicsDevice.Clear(Color.DarkSlateBlue);
      //GraphicsDevice.Present();

      _spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, Matrix.CreateScale(3, 3, 1));
      _spriteBatch.Draw(_foxHead, new Rectangle(15, 15, 32, 32), null, Color.White, 0, Vector2.Zero, new SpriteEffects(), 1);
      _spriteBatch.End();

      GraphicsDevice.SetRenderTarget(null);

      //_effect.CurrentTechnique = _effect.Techniques["Pretransformed"];
      _effect.CurrentTechnique = _effect.Techniques["ColoredNoShading"];

      _effect.Parameters["xView"].SetValue(_viewMatrix);
      _effect.Parameters["xProjection"].SetValue(_projectionMatrix);
      //_effect.Parameters["xWorld"].SetValue(Matrix.Identity);
      //Matrix worldMatrix = Matrix.CreateRotationY(3 * _angle);
      Matrix worldMatrix = Matrix.CreateTranslation(-20.0f / 3.0f, -10.0f / 3.0f, 0) * Matrix.CreateRotationZ(_angle);
      _effect.Parameters["xWorld"].SetValue(worldMatrix);

      foreach (EffectPass pass in _effect.CurrentTechnique.Passes)
      {
        pass.Apply();
      }

      _device.DrawUserPrimitives(PrimitiveType.TriangleList, _tri1, 0, 1, VertexPositionColor.VertexDeclaration);
      //_device.DrawUserPrimitives(PrimitiveType.TriangleList, _tri2, 0, 1, VertexPositionColor.VertexDeclaration);

      _spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, Matrix.CreateScale(3, 3, 1));
      _spriteBatch.Draw(_foxHead, new Rectangle(15, 15, 32, 32), null, Color.White, 0, Vector2.Zero, new SpriteEffects(), 1);
      _spriteBatch.End();

      _spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, Matrix.CreateScale(3, 3, 1));
      _testEffect.Parameters["filterColor"].SetValue(Color.Red.ToVector4());
      _testEffect.CurrentTechnique.Passes[0].Apply();
      _spriteBatch.Draw(_foxHead, new Rectangle(75, 15, 32, 32), null, Color.White, 0, Vector2.Zero, new SpriteEffects(), 1);
      _spriteBatch.End();

      target.Present();

      base.Draw(gameTime);
    }
  }
}