using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace PixelShaders
{
  public class Game1 : Game
  {
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    private Texture2D _foxHead;
    private Effect _testEffect;

    public Game1()
    {
      _graphics = new GraphicsDeviceManager(this);
      _graphics.GraphicsProfile = GraphicsProfile.HiDef;

      _graphics.ApplyChanges();
      Content.RootDirectory = "Content";
      IsMouseVisible = true;
    }

    protected override void Initialize()
    {
      // TODO: Add your initialization logic here

      base.Initialize();
    }

    protected override void LoadContent()
    {
      _spriteBatch = new SpriteBatch(GraphicsDevice);

      _foxHead = Content.Load<Texture2D>("2d/FoxHeadWithShadow");
      _testEffect = Content.Load<Effect>("Shaders/Test");
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

      _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, Matrix.CreateScale(4, 4, 1));
      _spriteBatch.Draw(_foxHead, new Rectangle(15,15, 32,32), null, Color.White, 0, Vector2.Zero, new SpriteEffects(), 1);
      _spriteBatch.End();

      base.Draw(gameTime);
    }
  }
}
