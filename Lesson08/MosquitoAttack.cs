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

	private enum GameState { Playing, Paused, Stopped }
	private GameState gameState;

	private KeyboardState currKeyboardState, prevKeyboardState;

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
		_cannon.Initialize(new Vector2(50, 325), 150);

		gameState = GameState.Playing;

		base.Initialize();
	}

	protected override void LoadContent() {
		_spriteBatch = new SpriteBatch(GraphicsDevice);

		backgroundTexture = Content.Load<Texture2D>("Background");
		font = Content.Load<SpriteFont>("SystemArialFont");

		_cannon.LoadContent(Content);
	}

	protected override void Update(GameTime gameTime) {
		float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

		currKeyboardState = Keyboard.GetState();

		switch (gameState) {
			case GameState.Playing:
				#region input
				// Pause
				if (Pressed(Keys.Escape)) gameState = GameState.Paused;

				// Movement
				if (currKeyboardState.IsKeyDown(Keys.Left)) _cannon.Direction = -Vector2.UnitX;
				else if (currKeyboardState.IsKeyDown(Keys.Right)) _cannon.Direction = Vector2.UnitX;
				else _cannon.Direction = Vector2.Zero;

				// Shoot
				/*if (currKeyboardState.IsKeyDown(Keys.Space)) {
					// [TODO]
				}*/
				#endregion

				_cannon.Update(gameTime);

				break;
			case GameState.Paused:
				// Unpause
				if (Pressed(Keys.Escape)) gameState = GameState.Playing;

				break;
			case GameState.Stopped:
				break;
		}

		prevKeyboardState = currKeyboardState;

		base.Update(gameTime);
	}

	protected override void Draw(GameTime gameTime) {
		GraphicsDevice.Clear(Color.CornflowerBlue);

		_spriteBatch.Begin();

		_spriteBatch.Draw(backgroundTexture, Vector2.Zero, Color.White);

		switch (gameState) {
			case GameState.Playing:
				_cannon.Draw(_spriteBatch);

				break;
			case GameState.Paused:
				_cannon.Draw(_spriteBatch);

				_spriteBatch.DrawString(font, "PAUSED", new Vector2(5, 380), Color.White);

				break;
			case GameState.Stopped:
				break;
		}

		_spriteBatch.End();

		base.Draw(gameTime);
	}

	private bool Pressed(Keys key) {
		return currKeyboardState.IsKeyDown(key) && prevKeyboardState.IsKeyUp(key);
	}
}
