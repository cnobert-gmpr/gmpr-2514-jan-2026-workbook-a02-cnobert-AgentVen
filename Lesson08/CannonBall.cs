using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Lesson08;

public class CannonBall : Projectile {
	private const float TRAIL_SPAWN_TIME = 0.03f;
	private const int MAX_TRAIL_POSITIONS = 12;

	private Texture2D texture;

	private List<Vector2> trailPositions;
	private float trailTimer;

	internal bool IsSpent { get => projectileState == ProjectileState.Spent; }
	

	internal override void Initialize(float initSpeed, Rectangle initGameBoundingBox) {
		base.Initialize(initSpeed, initGameBoundingBox);

		dimensions = new Point(4, 4);

		trailPositions = new List<Vector2>();
		trailTimer = 0;
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

				trailTimer += dt;
				if (trailTimer >= TRAIL_SPAWN_TIME) {
					trailTimer = 0;
					trailPositions.Insert(0, position);

					if (trailPositions.Count > MAX_TRAIL_POSITIONS)
						trailPositions.RemoveAt(trailPositions.Count - 1);
				}

				// Out-of-bounds
				if(!BoundingBox.Intersects(gameBoundingBox)) {
					projectileState = ProjectileState.Spent;
					trailPositions.Clear();
				}
				
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
				spriteBatch.Draw(texture, position, Color.White);

				DrawTrail(spriteBatch);

				break;
			case ProjectileState.Spent:
				break;
		}
	}


	internal void Reset() {
		if (projectileState != ProjectileState.Idle)
			projectileState = ProjectileState.Idle;
	}

	internal override bool HasCollidedWith(Rectangle otherBoundingBox) {
		if (base.HasCollidedWith(otherBoundingBox)) {
			trailPositions.Clear();

			return true;
		}
		return false;
	}

	private void DrawTrail(SpriteBatch spriteBatch) {
		for (int i = 0; i < trailPositions.Count; i++) {
			float alpha = 1f - ((float)(i + 1) / (trailPositions.Count + 1));
			float scale = 1f - (i * 0.1f);
			if (scale < 0.2f) scale = 0.2f;

			Vector2 trailPosition = trailPositions[i];
			Vector2 origin = new Vector2(texture.Width / 2, texture.Height / 2);
			Vector2 centeredPosition = trailPosition + new Vector2(texture.Width / 2f, texture.Height / 2f);

			spriteBatch.Draw(texture, centeredPosition, null, Color.Gray * (alpha * 0.5f),
				0f, origin, scale, SpriteEffects.None, 0f);
		}
	}
}