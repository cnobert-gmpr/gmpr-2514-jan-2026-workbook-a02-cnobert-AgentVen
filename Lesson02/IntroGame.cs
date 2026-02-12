using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Lesson02;

public class IntroGame : Game {
	private GraphicsDeviceManager _graphics;
	private SpriteBatch _spriteBatch;

	private Texture2D _pixel;

	private float _xPosition = 100, _yPosition = 150;
	private float _speed = 150;
	private int _width = 80, _height = 50;

	public IntroGame() {
		_graphics = new GraphicsDeviceManager(this);
		Content.RootDirectory = "Content";
		IsMouseVisible = true;
	}

	protected override void Initialize() {
		// TODO: Add your initialization logic here

		base.Initialize();
	}

	protected override void LoadContent() {
		_spriteBatch = new SpriteBatch(GraphicsDevice);

		_pixel = new Texture2D(GraphicsDevice, 1, 1);
		_pixel.SetData(new [] {Color.White});
	}

	protected override void Update(GameTime gameTime) {
		base.Update(gameTime);

		_xPosition += _speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
	}

	protected override void Draw(GameTime gameTime) {
		GraphicsDevice.Clear(Color.Black);

		_spriteBatch.Begin();

		Rectangle rect = new Rectangle((int)_xPosition, (int)_yPosition, _width, _height);
		_spriteBatch.Draw(_pixel, rect, Color.White);

		_spriteBatch.End();

		base.Draw(gameTime);
	}
}
