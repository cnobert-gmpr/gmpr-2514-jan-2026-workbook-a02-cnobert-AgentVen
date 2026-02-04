using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Lesson07;

public class Pong : Game {
	private const int _WindowWidth = 750, _WindowHeight = 450, ballScale = 21;

	private GraphicsDeviceManager _graphics;
	private SpriteBatch _spriteBatch;

	private Texture2D _background, _ball, _leftPaddle, _rightPaddle;

	private Vector2 ballPosition, ballDirection;
	private float ballSpeed;

	private Vector2 leftPaddlePosition, rightPaddlePosition, paddleSize;
	private float leftPaddleSpeed, rightPaddleSpeed;

	private int playAreaWallPixelOffset = (int)MathF.Floor(_WindowHeight * (2f/75f));


	internal Rectangle PlayAreaBoundingBox {
		get {
			return new Rectangle(
				0, playAreaWallPixelOffset, _WindowWidth, _WindowHeight - playAreaWallPixelOffset * 2
			);
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

		ballPosition = new Vector2(150, 195);
		ballSpeed = 60;
		ballDirection = new Vector2(-1, -1);

		paddleSize = new Vector2(12, 150);
		float paddleOffsetXPosition = 25;
		leftPaddlePosition = new Vector2(
			paddleOffsetXPosition,
			(float)_WindowHeight / 2 - paddleSize.Y / 2
		);
		rightPaddlePosition = new Vector2(
			_WindowWidth - paddleOffsetXPosition - paddleSize.X,
			(float)_WindowHeight / 2 - paddleSize.Y / 2
		);
		leftPaddleSpeed = 60;
		rightPaddleSpeed = 60;

		base.Initialize();
	}

	protected override void LoadContent() {
		_spriteBatch = new SpriteBatch(GraphicsDevice);

		_background = Content.Load<Texture2D>("Court");
		_ball = Content.Load<Texture2D>("Ball");
		_leftPaddle = Content.Load<Texture2D>("Paddle");
		_rightPaddle = Content.Load<Texture2D>("Paddle");
	}

	protected override void Update(GameTime gameTime) {
		float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
		
		ballPosition += ballDirection * ballSpeed * dt;
		if (ballPosition.X <= PlayAreaBoundingBox.Left
		|| ballPosition.X + ballScale >= PlayAreaBoundingBox.Right) ballDirection.X *= -1;
		if (ballPosition.Y <= PlayAreaBoundingBox.Top
		|| ballPosition.Y + ballScale >= PlayAreaBoundingBox.Bottom) ballDirection.Y *= -1;

		base.Update(gameTime);
	}

	protected override void Draw(GameTime gameTime) {
		GraphicsDevice.Clear(Color.CornflowerBlue);

		_spriteBatch.Begin();
		
		_spriteBatch.Draw(_background, new Rectangle(0, 0, _WindowWidth, _WindowHeight), Color.White);

		Rectangle ballRect = new Rectangle(
			(int)ballPosition.X, (int)ballPosition.Y, ballScale, ballScale);
		_spriteBatch.Draw(_ball, ballRect, Color.White);

		Rectangle leftPaddleRect = new Rectangle(
			(int)leftPaddlePosition.X, (int)leftPaddlePosition.Y, (int)paddleSize.X, (int)paddleSize.Y);
		_spriteBatch.Draw(_leftPaddle, leftPaddleRect, Color.White);
		Rectangle rightPaddleRect = new Rectangle(
			(int)rightPaddlePosition.X, (int)rightPaddlePosition.Y, (int)paddleSize.X, (int)paddleSize.Y);
		_spriteBatch.Draw(_rightPaddle, rightPaddleRect, Color.White);

		_spriteBatch.End();

		base.Draw(gameTime);
	}
}
