using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Lesson09;

public class Platformer : Game {
	private const int WINDOW_WIDTH = 550, WINDOW_HEIGHT = 400;
	
	internal const float GRAVITY = 60f;

	private readonly GraphicsDeviceManager graphics;
	private SpriteBatch spriteBatch;

	private Player _player;

	private Rectangle gameBoundingBox = new Rectangle(0, 0, WINDOW_WIDTH, WINDOW_HEIGHT);


	public Platformer() {
		graphics = new GraphicsDeviceManager(this);
		Content.RootDirectory = "Content";
		IsMouseVisible = true;
	}

	protected override void Initialize() {
		graphics.PreferredBackBufferWidth = WINDOW_WIDTH;
		graphics.PreferredBackBufferHeight = WINDOW_HEIGHT;
		graphics.ApplyChanges();

		_player = new Player(new Vector2(50, 50), gameBoundingBox);
		_player.Initialize();

		base.Initialize();
	}

	protected override void LoadContent() {
		spriteBatch = new SpriteBatch(GraphicsDevice);

		_player.LoadContent(Content);
	}

	protected override void Update(GameTime gameTime) {
		KeyboardState keyboardState = Keyboard.GetState();
		if (keyboardState.IsKeyDown(Keys.Left)) _player.Walk(-1);
		else if (keyboardState.IsKeyDown(Keys.Right)) _player.Walk(1);
		else _player.Stop();

		_player.Update(gameTime);

		base.Update(gameTime);
	}

	protected override void Draw(GameTime gameTime) {
		GraphicsDevice.Clear(Color.CornflowerBlue);

		spriteBatch.Begin();

		_player.Draw(spriteBatch);

		spriteBatch.End();

		base.Draw(gameTime);
	}
}
