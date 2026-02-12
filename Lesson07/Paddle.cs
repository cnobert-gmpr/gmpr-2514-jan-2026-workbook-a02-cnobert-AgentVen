using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Lesson07;

public class Paddle {
	private Vector2 position, direction, size;
	private float speed;

	private Texture2D texture;

	private Rectangle PlayAreaBoundingBox;


	internal Rectangle BoundingBox {
		get { return new Rectangle(position.ToPoint(), size.ToPoint()); }
	}

	internal Vector2 Direction { set => direction = value; }


	internal void Initialize(Vector2 paddlePosition, Vector2 paddleSize,
	float paddleSpeed, Rectangle playAreaBoundingBox) {
		position = paddlePosition;
		direction = Vector2.Zero;
		size = paddleSize;
		speed = paddleSpeed;
		PlayAreaBoundingBox = playAreaBoundingBox;
	}

	internal void LoadContent(ContentManager content) {
		texture = content.Load<Texture2D>("Paddle");
	}

	internal void Update(GameTime gameTime) {
		float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

		position += direction * speed * dt;

		if (position.Y <= PlayAreaBoundingBox.Top) {
			position.Y = PlayAreaBoundingBox.Top;
		} else if (position.Y + size.Y >= PlayAreaBoundingBox.Bottom) {
			position.Y = PlayAreaBoundingBox.Bottom - size.Y;
		}
	}

	internal void Draw(SpriteBatch spriteBatch) {
		spriteBatch.Draw(texture, BoundingBox, Color.White);
	}
}