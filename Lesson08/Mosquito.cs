using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Lesson08;

public class Mosquito {
	private SimpleAnimation animation;

	private Vector2 position, direction;
	private float speed;

	private Rectangle gameBoundingBox;

	internal Rectangle BoundingBox {
		get => new Rectangle(
			(int)position.X,
			(int)position.Y,
			(int)animation.FrameDimensions.X,
			(int)animation.FrameDimensions.Y
		);
	}
	

	internal void Initialize(Vector2 initPosition, float initSpeed, Vector2 initDirection, 
	Rectangle initGameBoundingBox) {
		position = initPosition;
		speed = initSpeed;
		direction = initDirection;
		gameBoundingBox = initGameBoundingBox;
	}

	internal void LoadContent(ContentManager content) {
		Texture2D texture = content.Load<Texture2D>("Mosquito");

		animation = new SimpleAnimation(texture, texture.Width / 11, texture.Height, 11, 8f);
		animation.Paused = false;
	}

	internal void Update(GameTime gameTime) {
		float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

		position += direction * speed * dt;

		if(BoundingBox.Left < gameBoundingBox.Left || BoundingBox.Right > gameBoundingBox.Right)
			direction.X *= -1;
		
		animation.Update(gameTime);
	}

	internal void Draw(SpriteBatch spriteBatch) {
		animation.Draw(spriteBatch, position, SpriteEffects.None);
	}
}