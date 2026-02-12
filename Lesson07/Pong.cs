using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Lesson07;

public class Pong : Game {
	private const int _WindowWidth = 750, _WindowHeight = 450, playAreaWallPixelSize = 12;

	private GraphicsDeviceManager _graphics;
	private SpriteBatch _spriteBatch;

	private Ball _ball;
	private Paddle _leftPaddle, _rightPaddle;

	private Texture2D backgroundTexture;

	private int playAreaWallPixelOffset = (int)MathF.Floor(_WindowHeight * ((float)playAreaWallPixelSize / (float)_WindowHeight));


	internal Rectangle PlayAreaBoundingBox {
		get {
			return new Rectangle(
				0, playAreaWallPixelOffset, _WindowWidth, _WindowHeight - playAreaWallPixelOffset * 2);
		}
	}


	public Pong() {
		_graphics = new GraphicsDeviceManager(this);
		Content.RootDirectory = "Content";
		IsMouseVisible = true;
	}

	protected override void Initialize() {
		_graphics.PreferredBackBufferWidth = _WindowWidth;
		_graphics.PreferredBackBufferHeight = _WindowHeight;
		_graphics.ApplyChanges();

		_ball = new Ball();
		_ball.Initialize(new Vector2(150, 195), new Vector2(-1, -1), 60, 21, PlayAreaBoundingBox);

		#region Paddle initialize
		float paddleEdgeOffset = 25;
		Vector2 paddleSize = new Vector2(8, 124);

		// Left paddle
		_leftPaddle = new Paddle();
		_leftPaddle.Initialize(new Vector2(
			paddleEdgeOffset,
			(float)_WindowHeight / 2 - paddleSize.Y / 2
		), paddleSize, 240, PlayAreaBoundingBox);

		// Right paddle
		_rightPaddle = new Paddle();
		_rightPaddle.Initialize(new Vector2(
			_WindowWidth - paddleEdgeOffset - paddleSize.X,
			(float)_WindowHeight / 2 - paddleSize.Y / 2
		), paddleSize, 240, PlayAreaBoundingBox);
		#endregion

		base.Initialize();
	}

	protected override void LoadContent() {
		_spriteBatch = new SpriteBatch(GraphicsDevice);

		backgroundTexture = Content.Load<Texture2D>("Court");
		_leftPaddle.LoadContent(this.Content);
		_rightPaddle.LoadContent(this.Content);
		_ball.LoadContent(this.Content);
	}

	protected override void Update(GameTime gameTime) {
		_ball.Update(gameTime);

		KeyboardState kbState = Keyboard.GetState();

		// right paddle controls
		if (kbState.IsKeyDown(Keys.Up)) _rightPaddle.Direction = -Vector2.UnitY;
		else if (kbState.IsKeyDown(Keys.Down)) _rightPaddle.Direction = Vector2.UnitY;
		else _rightPaddle.Direction = Vector2.Zero;

		_rightPaddle.Update(gameTime);

		// left paddle controls
		if (kbState.IsKeyDown(Keys.W)) _leftPaddle.Direction = -Vector2.UnitY;
		else if (kbState.IsKeyDown(Keys.S)) _leftPaddle.Direction = Vector2.UnitY;
		else _leftPaddle.Direction = Vector2.Zero;

		_leftPaddle.Update(gameTime);

		base.Update(gameTime);
	}

	protected override void Draw(GameTime gameTime) {
		GraphicsDevice.Clear(Color.CornflowerBlue);

		_spriteBatch.Begin();
		
		_spriteBatch.Draw(backgroundTexture, new Rectangle(0, 0, _WindowWidth, _WindowHeight), Color.White);

		_ball.Draw(_spriteBatch);

		_leftPaddle.Draw(_spriteBatch);
		_rightPaddle.Draw(_spriteBatch);

		_spriteBatch.End();

		base.Draw(gameTime);
	}
}
