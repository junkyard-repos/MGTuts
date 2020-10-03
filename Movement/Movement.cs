using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Movement
{
  public class Movement : Game
  {
    GraphicsDeviceManager _graphics;
    SpriteBatch _spriteBatch;
    float animationSpeed;
    int height;
    int width;
    int segments;
    double animationTime = 0;
    int playerFrame;
    string playerState;
    SpriteEffects playerDirection = SpriteEffects.None;
    float floor;
    Vector2 playerPosition;
    float playerSpeed = 100.0f;

    Texture2D playerIdleTexture;
    Texture2D playerRunTexture;
    Texture2D playerJumpTexture;
    Texture2D playerFallTexture;
    private bool playerIsJumping = false;
    private bool playerIsGrounded = false;

    public Movement()
    {
      _graphics = new GraphicsDeviceManager(this);
      Content.RootDirectory = "Content";
      IsMouseVisible = true;
    }

    protected override void Initialize()
    {
      animationSpeed = 1;
      playerState = "idle";
      playerPosition = new Vector2(50, 50);
      floor = _graphics.PreferredBackBufferHeight / 3 - 32;


      base.Initialize();
    }

    protected override void LoadContent()
    {
      _spriteBatch = new SpriteBatch(GraphicsDevice);

      playerIdleTexture = Content.Load<Texture2D>("rogue noir/animations/png/player_idle");
      playerRunTexture = Content.Load<Texture2D>("rogue noir/animations/png/player_run");
      playerJumpTexture = Content.Load<Texture2D>("rogue noir/animations/png/player_jump");
      playerFallTexture = Content.Load<Texture2D>("rogue noir/animations/png/player_jump_midair");

      height = playerIdleTexture.Height;
      width = playerIdleTexture.Width;
      segments = playerIdleTexture.Width / playerIdleTexture.Height;
    }

    protected override void Update(GameTime gameTime)
    {
      if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
        Exit();

      animationTime += gameTime.ElapsedGameTime.TotalSeconds;
      if (animationTime >= 0.07)
      {
        if (playerFrame < segments - 1)
        {
          playerFrame++;
        }
        else
        {
          playerFrame = 0;
        }
        animationTime = 0;
      }

      if (Keyboard.GetState().IsKeyDown(Keys.D))
      {
        playerState = "run-right";
        playerDirection = SpriteEffects.None;
        playerPosition.X = MathHelper.Clamp(playerPosition.X += playerSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds, -8, _graphics.PreferredBackBufferWidth / 3 - 24);
      }
      else if (Keyboard.GetState().IsKeyDown(Keys.A))
      {
        playerState = "run-left";
        playerDirection = SpriteEffects.FlipHorizontally;
        playerPosition.X = MathHelper.Clamp(playerPosition.X -= playerSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds, -8, _graphics.PreferredBackBufferWidth / 3 - 24);
      }
      else
      {
        playerState = "idle";
      }

      if (Keyboard.GetState().IsKeyDown(Keys.Space))
      {
        playerIsJumping = true;
        playerIsGrounded = false;
        playerState = "jumping";
        playerPosition.Y = MathHelper.Clamp(playerPosition.Y -= playerSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds, -4, floor);
      }
      else if (Keyboard.GetState().IsKeyUp(Keys.Space) && !playerIsGrounded) // todo: playerIsGrounded
      {
        playerPosition.Y = MathHelper.Clamp(playerPosition.Y += playerSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds, -4, floor);
        if (playerPosition.Y >= floor)
        {
          playerIsJumping = false;
          playerIsGrounded = true;
          playerState = "idle";
        }
        else
        {
          playerState = "falling";
        }
      }




      base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
      GraphicsDevice.Clear(Color.CornflowerBlue);

      _spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, Matrix.CreateScale(3, 3, 1));



      DrawPlayer();

      _spriteBatch.End();

      base.Draw(gameTime);
    }

    private void DrawPlayer()
    {
      switch (playerState)
      {
        case "run-right":
          _spriteBatch.Draw(playerRunTexture, playerPosition, new Rectangle(playerFrame * 32, 0, 32, 32), Color.White, 0, Vector2.Zero, Vector2.One, playerDirection, 0);
          break;
        case "run-left":
          _spriteBatch.Draw(playerRunTexture, playerPosition, new Rectangle(playerFrame * 32, 0, 32, 32), Color.White, 0, Vector2.Zero, Vector2.One, playerDirection, 0);
          break;
        case "jumping":
          _spriteBatch.Draw(playerJumpTexture, playerPosition, new Rectangle(7 * 32, 0, 32, 32), Color.White, 0, Vector2.Zero, Vector2.One, playerDirection, 0);
          break;
        case "falling":
          _spriteBatch.Draw(playerFallTexture, playerPosition, new Rectangle(playerFrame * 32, 0, 32, 32), Color.White, 0, Vector2.Zero, Vector2.One, playerDirection, 0);
          break;
        default:
          _spriteBatch.Draw(playerIdleTexture, playerPosition, new Rectangle(playerFrame * 32, 0, 32, 32), Color.White, 0, Vector2.Zero, Vector2.One, playerDirection, 0);
          break;
      }
    }
  }
}
