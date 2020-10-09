using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace XNABook_4
{
  public class Game6 : Game
  {
    private GraphicsDeviceManager graphics;
    private SpriteBatch spriteBatch;

    Texture2D spriteSheet;
    Texture2D titleScreen;
    SpriteFont pericles14;

    // Temporary Demo Code Begin
    Sprite tempSprite;
    Sprite tempSprite2;
    // Temporary Demo Code End

    public Game6()
    {
      graphics = new GraphicsDeviceManager(this);
      Content.RootDirectory = "Content";
      IsMouseVisible = true;
    }

    protected override void Initialize()
    {
      graphics.PreferredBackBufferWidth = 800;
      graphics.PreferredBackBufferHeight = 600;
      graphics.ApplyChanges();

      base.Initialize();
    }

    protected override void LoadContent()
    {
      spriteBatch = new SpriteBatch(GraphicsDevice);

      spriteSheet = Content.Load<Texture2D>(@"Textures\SpriteSheet");
      titleScreen = Content.Load<Texture2D>(@"Textures\TitleScreen");
      pericles14 = Content.Load<SpriteFont>(@"Fonts\Pericles14");

      // Temporary Demo Code Begin
      tempSprite = new Sprite(
          new Vector2(100, 100),
          spriteSheet,
          new Rectangle(0, 64, 32, 32),
          Vector2.Zero);

      tempSprite2 = new Sprite(
          new Vector2(200, 200),
          spriteSheet,
          new Rectangle(0, 160, 32, 32),
          Vector2.Zero);

      // Temporary Demo Code End

      Camera.WorldRectangle = new Rectangle(0, 0, 1600, 1600);
      Camera.ViewPortWidth = 800;
      Camera.ViewPortHeight = 600;

      TileMap.Initialize(spriteSheet);

      Player.Initialize(
    spriteSheet,
      new Rectangle(0, 64, 32, 32),
      6,
      new Rectangle(0, 96, 32, 32),
      1,
      new Vector2(300, 300)
    );

    }

    protected override void Update(GameTime gameTime)
    {
      // Allows the game to exit
      if (GamePad.GetState(PlayerIndex.One).Buttons.Back ==
          ButtonState.Pressed)
        this.Exit();

      Player.Update(gameTime);

      base.Update(gameTime);
    }


    protected override void Draw(GameTime gameTime)
    {
      GraphicsDevice.Clear(Color.CornflowerBlue);
      spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, Matrix.CreateScale(1, 1, 1));
      TileMap.Draw(spriteBatch);
      Player.Draw(spriteBatch);
      spriteBatch.End();

      base.Draw(gameTime);
    }
  }
}
