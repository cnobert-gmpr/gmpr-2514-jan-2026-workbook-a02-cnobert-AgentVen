using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Lesson08;

public class Cannon {
	private const int TOTAL_CANNON_BALLS = 10;
	private const float RELOAD_TIME = 5f;

	private enum CannonState { Intact, Destroyed };
	private CannonState cannonState;

	private SimpleAnimation moveAnimation;
	private SimpleAnimation[] explosionAnimations;
	
	private Vector2 position, direction;
	private Point dimensions;
	private float speed;

	private CannonBall[] _cannonBalls;
	private int cannonBallsLeft;
	private float reloadingTimer;

	private Rectangle gameBoundingBox;

	internal Rectangle BoundingBox {
		get => new Rectangle(
			(int)position.X, (int)position.Y,
			(int)moveAnimation.FrameDimensions.X, (int)moveAnimation.FrameDimensions.Y
		);
	}

	internal Vector2 Direction {
		set {
			direction = value;
			moveAnimation.Reverse = direction.X < 0;
		}
	}

	internal bool IsIntact { get => cannonState == CannonState.Intact; }

	internal int CannonBallsLeft { get => cannonBallsLeft; }

	internal bool IsReloading { get => reloadingTimer < RELOAD_TIME; }


	internal void Initialize(Vector2 initPosition, float initSpeed, Rectangle initGameBoundingBox) {
		position = initPosition;
		speed = initSpeed;
		gameBoundingBox = initGameBoundingBox;

		cannonState = CannonState.Intact;

		_cannonBalls = new CannonBall[TOTAL_CANNON_BALLS];
		for (int i = 0; i < TOTAL_CANNON_BALLS; i++) {
			_cannonBalls[i] = new CannonBall();
			_cannonBalls[i].Initialize(50f, gameBoundingBox);
		}

		cannonBallsLeft = TOTAL_CANNON_BALLS;
		reloadingTimer = 5f;

		explosionAnimations = new SimpleAnimation[5];
	}

	internal void LoadContent(ContentManager content) {
		Texture2D texture = content.Load<Texture2D>("Cannon");
		dimensions = new Point(texture.Width / 4, texture.Height);
		moveAnimation = new SimpleAnimation(texture, dimensions.X, dimensions.Y, 4, 2f);

		foreach (CannonBall cannonBall in _cannonBalls) cannonBall.LoadContent(content);

		texture = content.Load<Texture2D>("Poof");
		for (int i = 0; i < 5; i++)
			explosionAnimations[i] = 
				new SimpleAnimation(texture, texture.Width / 8, texture.Height, 8, 4f);
	}

	internal void Update(GameTime gameTime) {
		float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

		if (reloadingTimer < RELOAD_TIME) reloadingTimer += dt;

		switch (cannonState) {
			case CannonState.Intact:
				position += direction * speed * dt;

				if (BoundingBox.Left < gameBoundingBox.Left)
					position.X = gameBoundingBox.Left;
				else if (BoundingBox.Right > gameBoundingBox.Right)
					position.X = gameBoundingBox.Right - BoundingBox.Width;

				if (direction != Vector2.Zero) moveAnimation.Update(gameTime);

				break;
			case CannonState.Destroyed:
				foreach (SimpleAnimation explosionAnimation in explosionAnimations)
					explosionAnimation.Update(gameTime);
				
				break;
		}

		foreach (CannonBall cannonBall in _cannonBalls) cannonBall.Update(gameTime);
	}

	internal void Draw(SpriteBatch spriteBatch, SpriteFont textFont) {
		switch (cannonState) {
			case CannonState.Intact:
				moveAnimation?.Draw(spriteBatch, position, SpriteEffects.None);

				break;
			case CannonState.Destroyed:
				moveAnimation?.Draw(spriteBatch, position, SpriteEffects.None);

				explosionAnimations[0].Draw(spriteBatch, 
					position + new Vector2(12, -2), SpriteEffects.None);
				explosionAnimations[1].Draw(spriteBatch, 
					position + new Vector2(15, 11), SpriteEffects.None);
				explosionAnimations[2].Draw(spriteBatch, 
					position + new Vector2(1, 36), SpriteEffects.None);
				explosionAnimations[3].Draw(spriteBatch, 
					position + new Vector2(10, 26), SpriteEffects.None);
				explosionAnimations[4].Draw(spriteBatch, 
					position + new Vector2(20, 33), SpriteEffects.None);
				break;
		}

		foreach (CannonBall cannonBall in _cannonBalls) cannonBall.Draw(spriteBatch);

		if (reloadingTimer >= RELOAD_TIME)
			spriteBatch.DrawString(textFont,
				(cannonBallsLeft < 10? "0" : "") + cannonBallsLeft + "/" + TOTAL_CANNON_BALLS,
				new Vector2(gameBoundingBox.Right - 53, gameBoundingBox.Bottom - 20), Color.White);
	}


	internal void FireCannonBall() {
		if (cannonState == CannonState.Destroyed) return;

		foreach (CannonBall cannonBall in _cannonBalls) {
			if (!cannonBall.CanInstantiate) continue;

			cannonBall.Instantiate(new Vector2(
				BoundingBox.Center.X - cannonBall.BoundingBox.Width / 2f, 
				BoundingBox.Top - cannonBall.BoundingBox.Height), -Vector2.UnitY);
			
			cannonBallsLeft -= 1;
			return;
		}
	}

	internal void ReloadCannonBalls() {
		if (cannonState == CannonState.Destroyed) return;

		foreach (CannonBall cannonBall in _cannonBalls) if (!cannonBall.IsSpent) return;

		foreach (CannonBall cannonBall in _cannonBalls) cannonBall.Reset();

		reloadingTimer = 0;
		cannonBallsLeft = 10;
	}

	internal bool ACannonBallHasCollidedWith(Rectangle otherBoundingBox) {
		foreach (CannonBall cannonBall in _cannonBalls)
			if (cannonBall.HasCollidedWith(otherBoundingBox)) return true;
		
		return false;
	}

	internal void Destroy() {
		if (cannonState == CannonState.Destroyed) return;

		cannonState = CannonState.Destroyed;
	}
}