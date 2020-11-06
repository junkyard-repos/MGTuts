using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Riemer
{
  class Game2 : Game
  {
    private GraphicsDeviceManager _graphics;
    private GraphicsDevice _device;
    private SpriteBatch _spriteBatch;

    private Effect _effect;
    private VertexPositionColor[] _tri1;
    private VertexPositionColor[] _tri2;

    private Effect _testEffect;
    private Texture2D _foxHead;

    private void SetUpVertices()
    {
      _tri1 = new VertexPositionColor[3];
      _tri2 = new VertexPositionColor[3];

      _tri1[0].Position = new Vector3(-1f, -1f, 0f);
      _tri1[0].Color = Color.Red;
      _tri1[1].Position = new Vector3(-1f, 1f, 0f);
      _tri1[1].Color = Color.Green;
      _tri1[2].Position = new Vector3(1f, 1f, 0f);
      _tri1[2].Color = Color.Yellow;

      _tri2[0].Position = new Vector3(-1f, -1f, 0f);
      _tri2[0].Color = Color.Blue;
      _tri2[1].Position = new Vector3(1f, 1f, 0f);
      _tri2[1].Color = Color.Pink;
      _tri2[2].Position = new Vector3(1f, -1f, 0f);
      _tri2[2].Color = Color.GreenYellow;
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
    }

    protected override void Update(GameTime gameTime)
    {
      if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
                Keyboard.GetState().IsKeyDown(Keys.Escape))
      {
        Exit();
      }
      base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
      GraphicsDevice.Clear(Color.DarkSlateBlue);

      _spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, Matrix.CreateScale(3, 3, 1));
      _spriteBatch.Draw(_foxHead, new Rectangle(15, 15, 32, 32), null, Color.White, 0, Vector2.Zero, new SpriteEffects(), 1);
      _spriteBatch.End();

      _effect.CurrentTechnique = _effect.Techniques["Pretransformed"];

      foreach (EffectPass pass in _effect.CurrentTechnique.Passes)
      {
        pass.Apply();
      }

      _device.DrawUserPrimitives(PrimitiveType.TriangleList, _tri1, 0, 1, VertexPositionColor.VertexDeclaration);
      _device.DrawUserPrimitives(PrimitiveType.TriangleList, _tri2, 0, 1, VertexPositionColor.VertexDeclaration);

      _spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, Matrix.CreateScale(3, 3, 1));
      _spriteBatch.Draw(_foxHead, new Rectangle(15, 15, 32, 32), null, Color.White, 0, Vector2.Zero, new SpriteEffects(), 1);
      _spriteBatch.End();

      _spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, Matrix.CreateScale(3, 3, 1));
      _testEffect.Parameters["filterColor"].SetValue(Color.Red.ToVector4());
      _testEffect.CurrentTechnique.Passes[0].Apply();
      _spriteBatch.Draw(_foxHead, new Rectangle(75, 15, 32, 32), null, Color.White, 0, Vector2.Zero, new SpriteEffects(), 1);
      _spriteBatch.End();

      base.Draw(gameTime);
    }
  }
}
