using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Lesson07;

public class Paddle {
	private Vector2 position, direction = Vector2.Zero, size;
	private float speed;

	private Texture2D texture;

	private Rectangle PlayAreaBoundingBox;


	internal void Initialize(Vector2 paddlePosition, Vector2 paddleSize,
	float paddleSpeed, Rectangle playAreaBoundingBox) {
		position = paddlePosition;
		size = paddleSize;
		speed = paddleSpeed;
		PlayAreaBoundingBox = playAreaBoundingBox;
	}

	internal void LoadContent(ContentManager content) {
		texture = content.Load<Texture2D>("Paddle");
	}

	internal void Update(GameTime gameTime, Vector2 direction) {
		float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

		position += direction * speed * dt;

		if (position.Y <= PlayAreaBoundingBox.Top) {
			position.Y = PlayAreaBoundingBox.Top;
		} else if (position.Y + size.Y >= PlayAreaBoundingBox.Bottom) {
			position.Y = PlayAreaBoundingBox.Bottom - size.Y;
		}
	}

	internal void Draw(SpriteBatch spriteBatch) {
		Rectangle rect = new Rectangle(
			(int)position.X, (int)position.Y, (int)size.X, (int)size.Y);
		spriteBatch.Draw(texture, rect, Color.White);
	}
}