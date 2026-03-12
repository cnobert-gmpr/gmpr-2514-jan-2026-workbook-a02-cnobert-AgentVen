using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Lesson08;

public class CannonBall {
	private enum CannonBallState { None, Idle, Shot }
	private CannonBallState currState  = CannonBallState.Idle;

	private Texture2D texture;
	private Vector2 position, direction;
	private float speed;

	private Rectangle gameBoundingBox;

	internal Rectangle BoundingBox {
		get => new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height);
	}
	

	internal void Initialize(float initSpeed, Rectangle initGameBoundingBox) {
		speed = initSpeed;
		gameBoundingBox = initGameBoundingBox;
	}

	internal void LoadContent(ContentManager content) {
		texture = content.Load<Texture2D>("CannonBall");
	}

	internal void Update(GameTime gameTime) {
		float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

		switch (currState) {
			case CannonBallState.None:
				break;
			case CannonBallState.Idle:
				break;
			case CannonBallState.Shot:
				position += direction * speed * dt;

				if (BoundingBox.Bottom < gameBoundingBox.Top)
					currState = CannonBallState.None;
				
				break;
		}
	}

	internal void Draw(SpriteBatch spriteBatch) {
		switch (currState) {
			case CannonBallState.None:
				break;
			case CannonBallState.Idle:
				break;
			case CannonBallState.Shot:
				spriteBatch.Draw(texture, position, Color.White);

				break;
		}
	}

	internal void Instantiate(Vector2 instPosition, Vector2 instDirection) {
		if (currState == CannonBallState.Shot) return;

		position = instPosition;
		direction = instDirection;

		currState = CannonBallState.Shot;
	}
}