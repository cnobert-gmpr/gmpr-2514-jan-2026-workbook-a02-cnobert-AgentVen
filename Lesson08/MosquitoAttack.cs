using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Lesson08;

public class MosquitoAttack : Game {
	private const int WINDOW_WIDTH = 550, WINDOW_HEIGHT = 400;
	private const int TOTAL_MOSQUITOES = 10;

	private GraphicsDeviceManager _graphics;
	private SpriteBatch _spriteBatch;

	private Texture2D backgroundTexture;
	private SpriteFont font;

	private enum GameState { Playing, Paused, Stopped }
	private GameState gameState;

	private KeyboardState currKeyboardState, prevKeyboardState;

	private Cannon _cannon;
	private Mosquito[] _mosquitoes;

	private Rectangle BoundingBox { get => new Rectangle(0, 0, WINDOW_WIDTH, WINDOW_HEIGHT); }

	public MosquitoAttack() {
		_graphics = new GraphicsDeviceManager(this);
		Content.RootDirectory = "Content";
		IsMouseVisible = true;
	}

	protected override void Initialize() {
		_graphics.PreferredBackBufferWidth = WINDOW_WIDTH;
		_graphics.PreferredBackBufferHeight = WINDOW_HEIGHT;
		_graphics.ApplyChanges();

		_cannon = new Cannon();
		_cannon.Initialize(new Vector2(50, 325), 150, BoundingBox);

		#region Mosquitoes
		_mosquitoes = new Mosquito[TOTAL_MOSQUITOES];
		for (int i = 0; i < TOTAL_MOSQUITOES; i++) _mosquitoes[i] = new Mosquito();

		Random mosquitoRandom = new Random();
		foreach (Mosquito mosquito in _mosquitoes) {
			Vector2 direction = mosquitoRandom.Next(1, 3) == 2? -Vector2.UnitX : Vector2.UnitX;
			Vector2 position = new Vector2(
				mosquitoRandom.Next(1, WINDOW_WIDTH - 50),
				mosquitoRandom.Next(1, 151)
			);
			float speed = mosquitoRandom.Next(150, 251);

			mosquito.Initialize(position, speed, direction, BoundingBox);
		}
		#endregion

		gameState = GameState.Playing;

		base.Initialize();
	}

	protected override void LoadContent() {
		_spriteBatch = new SpriteBatch(GraphicsDevice);

		backgroundTexture = Content.Load<Texture2D>("Background");
		font = Content.Load<SpriteFont>("SystemArialFont");

		_cannon.LoadContent(Content);
		foreach (Mosquito mosquito in _mosquitoes) mosquito.LoadContent(Content);
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
				if (Pressed(Keys.Space)) _cannon.Fire();
				#endregion

				_cannon.Update(gameTime);
				foreach (Mosquito mosquito in _mosquitoes) {
					mosquito.Update(gameTime);
					if (mosquito.Alive && _cannon.ProcessCollision(mosquito.BoundingBox))
						mosquito.Kill();
				}

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
				foreach (Mosquito mosquito in _mosquitoes) mosquito.Draw(_spriteBatch);

				break;
			case GameState.Paused:
				_cannon.Draw(_spriteBatch);
				foreach (Mosquito mosquito in _mosquitoes) mosquito.Draw(_spriteBatch);

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
