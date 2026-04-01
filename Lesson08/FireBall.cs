using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Lesson08;

public class FireBall : Projectile {
	private Texture2D texture;

	internal Rectangle BoundingBox {
		get => new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height);
	}


	internal override void Initialize(float initSpeed, Rectangle initGameBoundingBox) {
		base.Initialize(initSpeed, initGameBoundingBox);

		projectileState = ProjectileState.Idle;
	}

	internal override void LoadContent(ContentManager content) {
		texture = content.Load<Texture2D>("CannonBall");
	}

	internal override void Update(GameTime gameTime) {
		float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

		switch (projectileState) {
			case ProjectileState.Idle:
				break;
			case ProjectileState.Airborne:
				position += direction * speed * dt;

				if(!BoundingBox.Intersects(gameBoundingBox))
					projectileState = ProjectileState.Spent;

				break;
			case ProjectileState.Spent:
				break;
		}
	}


	internal override void Draw(SpriteBatch spriteBatch) {
		switch (projectileState) {
			case ProjectileState.Idle:
				break;
			case ProjectileState.Airborne:
				spriteBatch.Draw(texture, position, Color.Red);

				break;
			case ProjectileState.Spent:
				break;
		}
	}


	internal override bool HasCollidedWith(Rectangle otherBoundingBox) {
		if (projectileState == ProjectileState.Airborne && BoundingBox.Intersects(otherBoundingBox)) {
			projectileState = ProjectileState.Spent;
			return true;
		}
		return false;
	}
}