using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Lesson08;

public class CannonBall {
	private const float TRAIL_SPAWN_TIME = 0.03f;
	private const int MAX_TRAIL_POSITIONS = 12;

	private enum CannonBallState { Idle, Shot, Used }
	private CannonBallState currState = CannonBallState.Idle;

	private Texture2D texture;
	private Vector2 position, direction;
	private float speed;

	private List<Vector2> trailPositions;
	private float trailTimer;

	private Rectangle gameBoundingBox;

	internal Rectangle BoundingBox {
		get => new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height);
	}

	internal bool CanInstantiate { get => currState == CannonBallState.Idle; }
	

	internal void Initialize(float initSpeed, Rectangle initGameBoundingBox) {
		speed = initSpeed;
		gameBoundingBox = initGameBoundingBox;

		position = Vector2.Zero;
		direction = Vector2.Zero;

		trailPositions = new List<Vector2>();
		trailTimer = 0;
	}

	internal void LoadContent(ContentManager content) {
		texture = content.Load<Texture2D>("CannonBall");
	}

	internal void Update(GameTime gameTime) {
		float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

		switch (currState) {
			case CannonBallState.Idle:
				break;
			case CannonBallState.Shot:
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
					currState = CannonBallState.Used;
					trailPositions.Clear();
				}
				
				break;
			case CannonBallState.Used:
				break;
		}
	}

	internal void Draw(SpriteBatch spriteBatch) {
		switch (currState) {
			case CannonBallState.Idle:
				break;
			case CannonBallState.Shot:
				spriteBatch.Draw(texture, position, Color.White);

				DrawTrail(spriteBatch);

				break;
			case CannonBallState.Used:
				break;
		}
	}

	internal void Instantiate(Vector2 instPosition, Vector2 instDirection) {
		if (!CanInstantiate) return;

		position = instPosition;
		direction = instDirection;

		currState = CannonBallState.Shot;
	}

	internal bool ProcessCollision(Rectangle otherBoundingBox) {
		if (currState == CannonBallState.Shot && BoundingBox.Intersects(otherBoundingBox)) {
			currState = CannonBallState.Used;
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