using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Lesson08;

public class Mosquito {
	private enum MosquitoState { Alive, Dying, Dead };
	private MosquitoState currState = MosquitoState.Alive;

	private SimpleAnimation animationAlive, animationPoofing;

	private Vector2 position, direction;
	private float speed;

	private Rectangle gameBoundingBox;

	internal Rectangle BoundingBox {
		get => new Rectangle(
			(int)position.X, (int)position.Y, (int)animationAlive.FrameDimensions.X, (int)animationAlive.FrameDimensions.Y);
	}
	
	internal bool Alive { get => currState == MosquitoState.Alive; }
	

	internal void Initialize(Vector2 initPosition, float initSpeed, Vector2 initDirection, 
	Rectangle initGameBoundingBox) {
		position = initPosition;
		speed = initSpeed;
		direction = initDirection;
		gameBoundingBox = initGameBoundingBox;
	}

	internal void LoadContent(ContentManager content) {
		Texture2D texture = content.Load<Texture2D>("Mosquito");
		animationAlive = new SimpleAnimation(texture, texture.Width / 11, texture.Height, 11, 8f);
		animationAlive.Paused = false;

		texture = content.Load<Texture2D>("Poof");
		animationPoofing = new SimpleAnimation(texture, texture.Width / 8, texture.Height, 8, 4f);
	}
	
	internal void Update(GameTime gameTime) {
		float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

		switch (currState) {
			case MosquitoState.Alive:
				position += direction * speed * dt;

				if(BoundingBox.Left < gameBoundingBox.Left || BoundingBox.Right > gameBoundingBox.Right)
					direction.X *= -1;
				
				animationAlive.Update(gameTime);

				break;
			case MosquitoState.Dying:
				animationPoofing.Update(gameTime);
				if (animationPoofing.DonePlayingOnce) currState = MosquitoState.Dead;
				
				break;
			case MosquitoState.Dead:
				break;
		}
	}

	internal void Draw(SpriteBatch spriteBatch) {
		switch (currState) {
			case MosquitoState.Alive:
				animationAlive.Draw(spriteBatch, position, SpriteEffects.None);

				break;
			case MosquitoState.Dying:
				animationPoofing.Draw(spriteBatch, BoundingBox.Center.ToVector2(), SpriteEffects.None);

				break;
			case MosquitoState.Dead:
				break;
		}
	}

	internal void Kill() {
		if (!Alive) return;

		currState = MosquitoState.Dying;
		animationPoofing.Looping = false;
	}
}