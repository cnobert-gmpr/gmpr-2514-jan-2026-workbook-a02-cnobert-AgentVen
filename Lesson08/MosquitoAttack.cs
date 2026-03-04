using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Lesson08;

public class MosquitoAttack : Game {
	private const int windowWidth = 550, windowHeight = 400;

	private GraphicsDeviceManager _graphics;
	private SpriteBatch _spriteBatch;

	private Texture2D backgroundTexture;
	private SpriteFont font;

	Cannon _cannon;


	public MosquitoAttack() {
		_graphics = new GraphicsDeviceManager(this);
		Content.RootDirectory = "Content";
		IsMouseVisible = true;
	}

	protected override void Initialize() {
		_graphics.PreferredBackBufferWidth = windowWidth;
		_graphics.PreferredBackBufferHeight = windowHeight;
		_graphics.ApplyChanges();

		_cannon = new Cannon();
		_cannon.Initialize(new Vector2(50, 325));

		base.Initialize();
	}

	protected override void LoadContent() {
		_spriteBatch = new SpriteBatch(GraphicsDevice);

		backgroundTexture = Content.Load<Texture2D>("Background");
		font = Content.Load<SpriteFont>("SystemArialFont");

		_cannon.LoadContent(Content);
	}

	protected override void Update(GameTime gameTime) {
		_cannon.Update(gameTime);

		base.Update(gameTime);
	}

	protected override void Draw(GameTime gameTime) {
		GraphicsDevice.Clear(Color.CornflowerBlue);

		_spriteBatch.Begin();

		_spriteBatch.Draw(backgroundTexture, Vector2.Zero, Color.White);

		_cannon.Draw(_spriteBatch);

		_spriteBatch.End();

		base.Draw(gameTime);
	}
}
