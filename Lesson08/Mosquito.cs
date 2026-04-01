using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Lesson08;

public class Mosquito {
	private enum MosquitoState { Alive, Dying, Dead };
	private MosquitoState mosquitoState;

	private SimpleAnimation aliveAnimation, dyingAnimation;

	private Vector2 position, direction;
	private float speed;

	private Rectangle gameBoundingBox;

	private FireBall _fireBall;

	internal Rectangle BoundingBox {
		get => new Rectangle(
			(int)position.X, (int)position.Y,
			(int)aliveAnimation.FrameDimensions.X, (int)aliveAnimation.FrameDimensions.Y
		);
	}

	internal bool IsAlive { get => mosquitoState == MosquitoState.Alive; }
	

	internal void Initialize(Vector2 initPosition, float initSpeed, Vector2 initDirection, 
	Rectangle initGameBoundingBox) {
		position = initPosition;
		speed = initSpeed;
		direction = initDirection;
		gameBoundingBox = initGameBoundingBox;

		mosquitoState = MosquitoState.Alive;

		_fireBall = new FireBall();
		_fireBall.Initialize(50f, gameBoundingBox);
	}

	internal void LoadContent(ContentManager content) {
		Texture2D texture = content.Load<Texture2D>("Mosquito");
		aliveAnimation = new SimpleAnimation(texture, texture.Width / 11, texture.Height, 11, 8f) {
			Paused = false
		};

		texture = content.Load<Texture2D>("Poof");
		dyingAnimation = new SimpleAnimation(texture, texture.Width / 8, texture.Height, 8, 4f);

		_fireBall.LoadContent(content);
	}
	
	internal void Update(GameTime gameTime) {
		float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

		switch (mosquitoState) {
			case MosquitoState.Alive:
				position += direction * speed * dt;
				if (BoundingBox.Left < gameBoundingBox.Left || BoundingBox.Right > gameBoundingBox.Right)
					direction.X *= -1;
				
				aliveAnimation.Update(gameTime);

				break;
			case MosquitoState.Dying:
				dyingAnimation.Update(gameTime);
				if (dyingAnimation.DonePlayingOnce) mosquitoState = MosquitoState.Dead;

				break;
			case MosquitoState.Dead:
				break;
		}

		_fireBall.Update(gameTime);
	}

	internal void Draw(SpriteBatch spriteBatch) {
		switch (mosquitoState) {
			case MosquitoState.Alive:
				aliveAnimation.Draw(spriteBatch, position, SpriteEffects.None);

				break;
			case MosquitoState.Dying:
				dyingAnimation.Draw(spriteBatch, BoundingBox.Center.ToVector2(), SpriteEffects.None);

				break;
			case MosquitoState.Dead:
				break;
		}

		_fireBall.Draw(spriteBatch);
	}
	

	internal void FireBall() {
		if (mosquitoState != MosquitoState.Alive || !_fireBall.CanInstantiate) return;

		_fireBall.Instantiate(new Vector2(
			BoundingBox.Center.X - _fireBall.BoundingBox.Width / 2f, 
			BoundingBox.Top - _fireBall.BoundingBox.Height), Vector2.UnitY);
	}

	internal bool FireBallHasCollidedWith(Rectangle otherBoundingBox) {
		if (_fireBall.HasCollidedWith(otherBoundingBox)) return true;
		
		return false;
	}

	internal void Kill() {
		if (!IsAlive) return;

		mosquitoState = MosquitoState.Dying;
		dyingAnimation.Looping = false;
	}
}