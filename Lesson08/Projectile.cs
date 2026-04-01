using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Lesson08;

public abstract class Projectile {
	protected enum ProjectileState { Idle, Airborne, Spent }
	protected ProjectileState projectileState;

	protected Vector2 position, direction;
	protected float speed;

	protected Rectangle gameBoundingBox;

	internal bool CanInstantiate { get => projectileState == ProjectileState.Idle; }


	internal virtual void Initialize(float initSpeed, Rectangle initGameBoundingBox) {
		speed = initSpeed;
		gameBoundingBox = initGameBoundingBox;

		position = Vector2.Zero;
		direction = Vector2.Zero;

		projectileState = ProjectileState.Idle;
	}

	internal abstract void LoadContent(ContentManager content);
	internal abstract void Update(GameTime gameTime);
	internal abstract void Draw(SpriteBatch spriteBatch);


	internal void Instantiate(Vector2 instPosition, Vector2 instDirection) {
		if (!CanInstantiate) return;

		position = instPosition;
		direction = instDirection;

		projectileState = ProjectileState.Airborne;
	}

	internal abstract bool HasCollidedWith(Rectangle otherBoundingBox);
}