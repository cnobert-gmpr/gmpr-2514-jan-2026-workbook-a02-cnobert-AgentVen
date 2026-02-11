using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Lesson07;

public class Pong : Game {
	private const int _WindowWidth = 750, _WindowHeight = 450, playAreaWallPixelSize = 12;
	private const float initalPaddleSpeed = 240;
	private Vector2 paddleSize = new Vector2(8, 124);
	private const int paddleEdgeOffset = 25;

	private GraphicsDeviceManager _graphics;
	private SpriteBatch _spriteBatch;

	private Ball _ball;

	private Texture2D backgroundTexture, leftPaddleTexture, rightPaddleTexture;

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

		_ball = new Ball();
		_ball.Initialize(new Vector2(150, 195), new Vector2(-1, -1), 60, 21, PlayAreaBoundingBox);

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

		backgroundTexture = Content.Load<Texture2D>("Court");
		leftPaddleTexture = Content.Load<Texture2D>("Paddle");
		rightPaddleTexture = Content.Load<Texture2D>("Paddle");
		_ball.LoadContent(this.Content);
	}

	protected override void Update(GameTime gameTime) {
		float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

		_ball.Update(gameTime);

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

		base.Update(gameTime);
	}

	protected override void Draw(GameTime gameTime) {
		GraphicsDevice.Clear(Color.CornflowerBlue);

		_spriteBatch.Begin();
		
		_spriteBatch.Draw(backgroundTexture, new Rectangle(0, 0, _WindowWidth, _WindowHeight), Color.White);

		_ball.Draw(_spriteBatch);

		// Paddles

		Rectangle leftPaddleRect = new Rectangle(
			(int)leftPaddlePosition.X, (int)leftPaddlePosition.Y, (int)paddleSize.X, (int)paddleSize.Y);
		_spriteBatch.Draw(leftPaddleTexture, leftPaddleRect, Color.White);

		Rectangle rightPaddleRect = new Rectangle(
			(int)rightPaddlePosition.X, (int)rightPaddlePosition.Y, (int)paddleSize.X, (int)paddleSize.Y);
		_spriteBatch.Draw(rightPaddleTexture, rightPaddleRect, Color.White);


		_spriteBatch.End();

		base.Draw(gameTime);
	}
}
