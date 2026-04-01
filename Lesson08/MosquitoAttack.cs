using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Lesson08;

public class MosquitoAttack : Game {
	private const int WINDOW_WIDTH = 550, WINDOW_HEIGHT = 400;
	private const int TOTAL_MOSQUITOES = 10;

	private enum GameState { Start, Play, Pause, Stop }
	private GameState gameState;

	private KeyboardState currKeyboardState, prevKeyboardState;

	private GraphicsDeviceManager _graphics;
	private SpriteBatch _spriteBatch;

	private Texture2D backgroundTexture;
	private SpriteFont textFont;

	private Rectangle BoundingBox { get => new Rectangle(0, 0, WINDOW_WIDTH, WINDOW_HEIGHT); }

	private Cannon _cannon;
	private Mosquito[] _mosquitoes;

	private Random mosquitoRNG = new Random();


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

		#region Mosquitoes initization
		_mosquitoes = new Mosquito[TOTAL_MOSQUITOES];
		for (int i = 0; i < TOTAL_MOSQUITOES; i++) _mosquitoes[i] = new Mosquito();

		foreach (Mosquito mosquito in _mosquitoes) {
			mosquito.Initialize(
				new Vector2(mosquitoRNG.Next(1, WINDOW_WIDTH - 50), mosquitoRNG.Next(1, 151)),
				mosquitoRNG.Next(150, 251),
				mosquitoRNG.Next(1, 3) == 2? -Vector2.UnitX : Vector2.UnitX,
				BoundingBox
			);
		}
		#endregion

		gameState = GameState.Start;

		base.Initialize();
	}

	protected override void LoadContent() {
		_spriteBatch = new SpriteBatch(GraphicsDevice);

		backgroundTexture = Content.Load<Texture2D>("Background");
		textFont = Content.Load<SpriteFont>("SystemArialFont");

		_cannon.LoadContent(Content);
		foreach (Mosquito mosquito in _mosquitoes) mosquito.LoadContent(Content);
	}

	protected override void Update(GameTime gameTime) {
		float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

		currKeyboardState = Keyboard.GetState();

		switch (gameState) {
			case GameState.Start:
				// Start game
				if (Pressed(Keys.Space)) gameState = GameState.Play;

				break;
			case GameState.Play:
				// End conditions
				if (!_cannon.IsIntact) {
					gameState = GameState.Stop;
					break;
				} else if (_mosquitoes.All(mosqutio => mosqutio.IsAlive == false)) {
					gameState = GameState.Stop;
					break;
				}


				// Pause game
				if (Pressed(Keys.Escape)) gameState = GameState.Pause;

				// Move cannon
				if (currKeyboardState.IsKeyDown(Keys.Left)) _cannon.Direction = -Vector2.UnitX;
				else if (currKeyboardState.IsKeyDown(Keys.Right)) _cannon.Direction = Vector2.UnitX;
				else _cannon.Direction = Vector2.Zero;

				// Shoot cannon ball
				if (Pressed(Keys.Space)) _cannon.FireCannonBall();

				// Reload cannon
				if (Pressed(Keys.R) && _cannon.CannonBallsLeft == 0 && !_cannon.IsReloading)
					_cannon.ReloadCannonBalls();


				_cannon.Update(gameTime);
				foreach (Mosquito mosquito in _mosquitoes) {
					mosquito.Update(gameTime);

					if (mosquito.IsAlive) {
						if (_cannon.ACannonBallHasCollidedWith(mosquito.BoundingBox))
							mosquito.Kill();

						if (mosquitoRNG.Next(0, 1000) == 2)
							mosquito.FireBall();

						if (mosquito.FireBallHasCollidedWith(_cannon.BoundingBox))
							_cannon.Destroy();
					}
				}

				break;
			case GameState.Pause:
				// Unpause game
				if (Pressed(Keys.Escape)) gameState = GameState.Play;

				break;
			case GameState.Stop:
				_cannon.Update(gameTime);
				foreach (Mosquito mosquito in _mosquitoes) mosquito.Update(gameTime);

				break;
		}

		prevKeyboardState = currKeyboardState;

		base.Update(gameTime);
	}

	protected override void Draw(GameTime gameTime) {
		GraphicsDevice.Clear(Color.Black);

		_spriteBatch.Begin();
		
		_spriteBatch.Draw(backgroundTexture, Vector2.Zero, Color.White);

		switch (gameState) {
			case GameState.Start:
				// TODO))
				_spriteBatch.DrawString(textFont, "Press space to start", 
					new Vector2(BoundingBox.Center.X, BoundingBox.Bottom - 20), Color.White);

				break;
			case GameState.Play:
				_cannon.Draw(_spriteBatch, textFont);
				foreach (Mosquito mosquito in _mosquitoes) mosquito.Draw(_spriteBatch);

				if (_cannon.IsReloading)
					_spriteBatch.DrawString(textFont, "RELOADING",
						new Vector2(BoundingBox.Right - 115, BoundingBox.Bottom - 20), Color.White);

				break;
			case GameState.Pause:
				_cannon.Draw(_spriteBatch, textFont);
				foreach (Mosquito mosquito in _mosquitoes) mosquito.Draw(_spriteBatch);

				_spriteBatch.DrawString(textFont, "PAUSED", 
					new Vector2(BoundingBox.Left + 5, BoundingBox.Bottom - 20), Color.White);

				if (_cannon.IsReloading)
					_spriteBatch.DrawString(textFont, "RELOADING",
						new Vector2(BoundingBox.Right - 100, BoundingBox.Bottom - 20), Color.White);
				
				break;
			case GameState.Stop:
				_cannon.Draw(_spriteBatch, textFont);
				foreach (Mosquito mosquito in _mosquitoes) mosquito.Draw(_spriteBatch);

				if (_cannon.IsIntact) {
					_spriteBatch.DrawString(textFont, "YOU WIN", 
						BoundingBox.Center.ToVector2(), Color.White);
				} else {
					_spriteBatch.DrawString(textFont, "YOU LOSE", 
						BoundingBox.Center.ToVector2(), Color.White);
				}

				if (_cannon.IsReloading)
					_spriteBatch.DrawString(textFont, "RELOADING",
						new Vector2(BoundingBox.Right - 100, BoundingBox.Bottom - 20), Color.White);

				break;
		}

		_spriteBatch.End();

		base.Draw(gameTime);
	}

	private bool Pressed(Keys key) {
		return currKeyboardState.IsKeyDown(key) && prevKeyboardState.IsKeyUp(key);
	}

	private bool AreAllMosquitoesDead() {
		foreach (Mosquito mosquito in _mosquitoes)
			if (mosquito.IsAlive) return false;
		return true;
	}
}
