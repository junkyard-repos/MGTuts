using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Riemer
{
  public class Game2 : Game
  {
    //Properties
    private GraphicsDeviceManager _graphics;

    private GraphicsDevice _device;
    private Effect _effect;
    private VertexPositionColor[] _vertices;
    private Matrix _viewMatrix;
    private Matrix _projectionMatrix;
    private float _angle = 0f;
    private short[] _indices;

    public Game2()
    {
      _graphics = new GraphicsDeviceManager(this);
      Content.RootDirectory = "Content";
      IsMouseVisible = true;
    }

    protected override void Initialize()
    {
      // TODO: Add your initialization logic here
      _graphics.PreferredBackBufferWidth = 500;
      _graphics.PreferredBackBufferHeight = 500;
      _graphics.IsFullScreen = false;
      _graphics.ApplyChanges();
      Window.Title = "Riemer's MonoGame Tutorials -- 3D Series 1";

      base.Initialize();
    }

    private void SetUpVertices()
    {
      _vertices = new VertexPositionColor[5]
      {
                new VertexPositionColor() {Position = new Vector3(0f, 0f, 0f), Color = Color.White},
                new VertexPositionColor() {Position = new Vector3(5f, 0f, 0f), Color = Color.White},
                new VertexPositionColor() {Position = new Vector3(10f, 0f, 0f), Color = Color.White},
                new VertexPositionColor() {Position = new Vector3(5f, 0f, -5f), Color = Color.White},
                new VertexPositionColor() {Position = new Vector3(10f, 0f, -5f), Color = Color.White}
      };
    }

    private void SetUpIndices()
    {
      _indices = new short[6] { 3, 1, 0, 4, 2, 1 };
    }

    private void SetUpCamera()
    {
      _viewMatrix = Matrix.CreateLookAt(new Vector3(0, 50, 0), new Vector3(0, 0, 0), new Vector3(0, 0, -1));
      _projectionMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, _device.Viewport.AspectRatio, 1.0f, 300.0f);
    }

    protected override void LoadContent()
    {
      // TODO: use this.Content to load your game content here
      _device = _graphics.GraphicsDevice;

      _effect = Content.Load<Effect>("Series 1/effects");

      SetUpCamera();

      SetUpVertices();
      SetUpIndices();
    }

    protected override void Update(GameTime gameTime)
    {
      if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
        Exit();

      // TODO: Add your update logic here
      _angle += 0.005f;

      base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
      _device.Clear(Color.DarkSlateBlue);

      RasterizerState rs = new RasterizerState();
      rs.CullMode = CullMode.None;
      rs.FillMode = FillMode.WireFrame;
      _device.RasterizerState = rs;

      // TODO: Add your drawing code here
      _effect.CurrentTechnique = _effect.Techniques["ColoredNoShading"];
      _effect.Parameters["xView"].SetValue(_viewMatrix);
      _effect.Parameters["xProjection"].SetValue(_projectionMatrix);
      Matrix worldMatrix = Matrix.Identity;
      _effect.Parameters["xWorld"].SetValue(worldMatrix);

      foreach (EffectPass pass in _effect.CurrentTechnique.Passes)
      {
        pass.Apply();
        _device.DrawUserIndexedPrimitives(PrimitiveType.TriangleList, _vertices, 0, _vertices.Length, _indices, 0, _indices.Length / 3, VertexPositionColor.VertexDeclaration);
      }

      base.Draw(gameTime);
    }
  }
}