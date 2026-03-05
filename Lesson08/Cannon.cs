using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Lesson08;

public class Cannon {
	private SimpleAnimation animation;
	private Vector2 position, direction;
	private Point dimensions;
	private float speed;

	internal Vector2 Direction {
		set {
			direction = value;
			if (direction.X < 0) animation.Reverse = true;
			else animation.Reverse = false;
		}
	}


	internal void Initialize(Vector2 initPosition, float initSpeed) {
		position = initPosition;
		speed = initSpeed;
	}

	internal void LoadContent(ContentManager content) {
		Texture2D texture = content.Load<Texture2D>("Cannon");
		dimensions = new Point(texture.Width / 4, texture.Height);
		animation = new SimpleAnimation(texture, dimensions.X, dimensions.Y, 4, 2f);
	}

	internal void Update(GameTime gameTime) {
		float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

		position += direction * speed * dt;
		
		if (direction != Vector2.Zero) animation.Update(gameTime);
	}

	internal void Draw(SpriteBatch spriteBatch) {
		if (animation != null) animation.Draw(spriteBatch, position, SpriteEffects.None);
	}
}