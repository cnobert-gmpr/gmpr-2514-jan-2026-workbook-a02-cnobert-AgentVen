using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Lesson08;

public class FireBall : Projectile {
	private SimpleAnimation animation;


	internal override void Initialize(float initSpeed, Rectangle initGameBoundingBox) {
		base.Initialize(initSpeed, initGameBoundingBox);

		dimensions = new Point(4, 4);

		projectileState = ProjectileState.Idle;
	}

	internal override void LoadContent(ContentManager content) {
		Texture2D texture = content.Load<Texture2D>("Fireball");
		animation = new SimpleAnimation(texture, texture.Width / 8, texture.Height, 8, 4f) {
			Paused = false
		};
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
				animation.Draw(spriteBatch, position, SpriteEffects.None);

				break;
			case ProjectileState.Spent:
				break;
		}
	}
}