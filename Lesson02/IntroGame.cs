using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Lesson02;

public class IntroGame : Game {
	private GraphicsDeviceManager _graphics;
	private SpriteBatch _spriteBatch;

	private Texture2D _pixel;

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
		// TODO: Add your update logic here

		base.Update(gameTime);
	}

	protected override void Draw(GameTime gameTime) {
		GraphicsDevice.Clear(Color.Black);

		_spriteBatch.Begin();

		Rectangle rect = new Rectangle(100, 150, 80, 50);
		_spriteBatch.Draw(_pixel, rect, Color.White);

		_spriteBatch.End();

		base.Draw(gameTime);
	}
}
