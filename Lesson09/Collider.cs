using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Lesson09;

public class Collider {
	public enum ColliderType { Top, Right, Bottom, Left }
	private ColliderType colliderType;

	private Texture2D pixel;

	private Vector2 position, size;

	internal Rectangle BoundingBox {
		get => new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y);
	}


	public Collider(Vector2 constrPosition, Vector2 constrSize, ColliderType constrColliderType) {
		position = constrPosition;
		size = constrSize;
		colliderType = constrColliderType;
	}

	internal void LoadContent(GraphicsDevice graphicsDevice) {
		Color visualliztionColour = Color.White;

		switch (colliderType) {
			case ColliderType.Top:
				visualliztionColour = Color.Blue;

				break;
			case ColliderType.Right:
				visualliztionColour = Color.Red;

				break;
			case ColliderType.Bottom:
				visualliztionColour = Color.Yellow;

				break;
			case ColliderType.Left:
				visualliztionColour = Color.Green;

				break;
		}

		if (pixel == null) {
			pixel = new Texture2D(graphicsDevice, 1, 1);
			pixel.SetData([visualliztionColour]);
		}
	}

	internal void Draw(SpriteBatch spriteBatch) {
		spriteBatch.Draw(pixel, BoundingBox, Color.White);
	}


	internal void PlayerCollision(Player player, GameTime gameTime) {
		float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

		if (BoundingBox.Intersects(player.BoundingBox)) {
			switch (colliderType) {
				case ColliderType.Top:
					player.Land(BoundingBox);
					player.Grounded(BoundingBox, dt);

					break;
				case ColliderType.Right:
					if (player.Velocity.X < 0)
						player.Walk(0);

					break;
				case ColliderType.Bottom:
					if (player.Velocity.Y < 0)
						player.VMove(0);

					break;
				case ColliderType.Left:
					if (player.Velocity.X > 0)
						player.Walk(0);

					break;
			}
		}
	}
}