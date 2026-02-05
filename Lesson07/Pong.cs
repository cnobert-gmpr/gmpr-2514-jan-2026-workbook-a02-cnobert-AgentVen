using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Lesson07;

public class Pong : Game {
	private const int _WindowWidth = 750, _WindowHeight = 450, ballScale = 21, playAreaWallPixelSize = 12;
	private const float initalBallSpeed = 60, initalPaddleSpeed = 240;
	private Vector2 paddleSize = new Vector2(8, 124);
	private const int paddleEdgeOffset = 25;

	private GraphicsDeviceManager _graphics;
	private SpriteBatch _spriteBatch;

	private Texture2D background, ball, leftPaddle, rightPaddle;

	private Vector2 ballPosition, ballDirection;
	private float ballSpeed;

	private Vector2 leftPaddlePosition, rightPaddlePosition, leftPaddleDirection, rightPaddleDirection;
	private float leftPaddleSpeed, rightPaddleSpeed;

	private int playAreaWallPixelOffset = (int)MathF.Floor(_WindowHeight * ((float)playAreaWallPixelSize / (float)_WindowHeight));


	internal Rectangle PlayAreaBoundingBox {
		get {
			return new Rectangle(0, playAreaWallPixelOffset, _WindowWidth, _WindowHeight - playAreaWallPixelOffset * 2);
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


		// Ball
		ballPosition = new Vector2(150, 195);
		ballSpeed = initalBallSpeed;
		ballDirection = new Vector2(-1, -1);


		// Left paddle
		leftPaddlePosition = new Vector2(
			paddleEdgeOffset,
			(float)_WindowHeight / 2 - paddleSize.Y / 2
		);
		leftPaddleDirection = Vector2.Zero;
		leftPaddleSpeed = initalPaddleSpeed;

		// Right paddle
		rightPaddlePosition = new Vector2(
			_WindowWidth - paddleEdgeOffset - paddleSize.X,
			(float)_WindowHeight / 2 - paddleSize.Y / 2
		);
		rightPaddleDirection = Vector2.Zero;
		rightPaddleSpeed = initalPaddleSpeed;


		base.Initialize();
	}

	protected override void LoadContent() {
		_spriteBatch = new SpriteBatch(GraphicsDevice);

		background = Content.Load<Texture2D>("Court");
		ball = Content.Load<Texture2D>("Ball");
		leftPaddle = Content.Load<Texture2D>("Paddle");
		rightPaddle = Content.Load<Texture2D>("Paddle");
	}

	protected override void Update(GameTime gameTime) {
		float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;


		#region Keyboard input
		KeyboardState kbState = Keyboard.GetState();

		// right paddle controls
		if (kbState.IsKeyDown(Keys.Up)) rightPaddleDirection = -Vector2.UnitY;
		else if (kbState.IsKeyDown(Keys.Down)) rightPaddleDirection = Vector2.UnitY;
		else rightPaddleDirection = Vector2.Zero;

		// left paddle controls
		if (kbState.IsKeyDown(Keys.W)) leftPaddleDirection = -Vector2.UnitY;
		else if (kbState.IsKeyDown(Keys.S)) leftPaddleDirection = Vector2.UnitY;
		else leftPaddleDirection = Vector2.Zero;
		#endregion


		#region Paddle update
		leftPaddlePosition += leftPaddleDirection * leftPaddleSpeed * dt;

		if (leftPaddlePosition.Y <= PlayAreaBoundingBox.Top) {
			leftPaddlePosition.Y = PlayAreaBoundingBox.Top;
		} else if (leftPaddlePosition.Y + paddleSize.Y >= PlayAreaBoundingBox.Bottom) {
			leftPaddlePosition.Y = PlayAreaBoundingBox.Bottom - paddleSize.Y;
		}

		rightPaddlePosition += rightPaddleDirection * rightPaddleSpeed * dt;

		if (rightPaddlePosition.Y <= PlayAreaBoundingBox.Top) {
			rightPaddlePosition.Y = PlayAreaBoundingBox.Top;
		} else if (rightPaddlePosition.Y + paddleSize.Y >= PlayAreaBoundingBox.Bottom) {
			rightPaddlePosition.Y = PlayAreaBoundingBox.Bottom - paddleSize.Y;
		}
		#endregion


		#region Ball update
		ballPosition += ballDirection * ballSpeed * dt;

		if (ballPosition.X <= PlayAreaBoundingBox.Left
		|| ballPosition.X + ballScale >= PlayAreaBoundingBox.Right) ballDirection.X *= -1;
		
		if (ballPosition.Y <= PlayAreaBoundingBox.Top
		|| ballPosition.Y + ballScale >= PlayAreaBoundingBox.Bottom) ballDirection.Y *= -1;
		#endregion


		base.Update(gameTime);
	}

	protected override void Draw(GameTime gameTime) {
		GraphicsDevice.Clear(Color.CornflowerBlue);

		_spriteBatch.Begin();
		
		_spriteBatch.Draw(background, new Rectangle(0, 0, _WindowWidth, _WindowHeight), Color.White);


		// Ball
		Rectangle ballRect = new Rectangle(
			(int)ballPosition.X, (int)ballPosition.Y, ballScale, ballScale);
		_spriteBatch.Draw(ball, ballRect, Color.White);


		// Paddles

		Rectangle leftPaddleRect = new Rectangle(
			(int)leftPaddlePosition.X, (int)leftPaddlePosition.Y, (int)paddleSize.X, (int)paddleSize.Y);
		_spriteBatch.Draw(leftPaddle, leftPaddleRect, Color.White);

		Rectangle rightPaddleRect = new Rectangle(
			(int)rightPaddlePosition.X, (int)rightPaddlePosition.Y, (int)paddleSize.X, (int)paddleSize.Y);
		_spriteBatch.Draw(rightPaddle, rightPaddleRect, Color.White);


		_spriteBatch.End();

		base.Draw(gameTime);
	}
}
