using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Lesson08;

public class Cannon {
	private SimpleAnimation animation;
	
	private Vector2 position, direction;
	private Point dimensions;
	private float speed;

	private CannonBall _ball;

	private Rectangle gameBoundingBox;

	internal Rectangle BoundingBox {
		get => new Rectangle(
			(int)position.X,
			(int)position.Y,
			(int)animation.FrameDimensions.X,
			(int)animation.FrameDimensions.Y
		);
	}

	internal Vector2 Direction {
		set {
			direction = value;
			if (direction.X < 0) animation.Reverse = true;
			else animation.Reverse = false;
		}
	}


	internal void Initialize(Vector2 initPosition, float initSpeed, Rectangle initGameBoundingBox) {
		position = initPosition;
		speed = initSpeed;
		gameBoundingBox = initGameBoundingBox;

		_ball = new CannonBall();
		_ball.Initialize(50f, gameBoundingBox);
	}

	internal void LoadContent(ContentManager content) {
		Texture2D texture = content.Load<Texture2D>("Cannon");
		dimensions = new Point(texture.Width / 4, texture.Height);
		animation = new SimpleAnimation(texture, dimensions.X, dimensions.Y, 4, 2f);

		_ball.LoadContent(content);
	}

	internal void Update(GameTime gameTime) {
		float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

		position += direction * speed * dt;
		if (BoundingBox.Left < gameBoundingBox.Left) position.X = gameBoundingBox.Left;
		else if (BoundingBox.Right > gameBoundingBox.Right) position.X = gameBoundingBox.Right - BoundingBox.Width;
		
		if (direction != Vector2.Zero) animation.Update(gameTime);

		_ball.Update(gameTime);
	}

	internal void Draw(SpriteBatch spriteBatch) {
		animation?.Draw(spriteBatch, position, SpriteEffects.None);

		_ball.Draw(spriteBatch);
	}

	internal void Fire() {
		_ball.Instantiate(position + new Vector2(BoundingBox.Width / 2f - 2, 0), -Vector2.UnitY);
	}
}