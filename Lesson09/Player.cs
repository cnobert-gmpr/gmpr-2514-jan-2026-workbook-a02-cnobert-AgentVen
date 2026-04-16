using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Lesson09;

public class Player {
	private enum PlayerState { Idle, Jumping, Walking }
	private PlayerState playerState;

	private bool isFacingForward = true;

	private Vector2 position, velocity, size;
	private float speed;

	private SimpleAnimation idleAnim, walkAnim, jumpAnim, currAnim;

	private Rectangle gameBoundingBox;

	internal Rectangle BoundingBox { get => new Rectangle(position.ToPoint(), size.ToPoint()); }


	public Player(Vector2 constrPosition, Rectangle constrGameBoundingBox) {
		position = constrPosition;
		gameBoundingBox = constrGameBoundingBox;

		velocity = Vector2.Zero;
		size = new Vector2(46, 40);
	}

	internal void Initialize() {
		playerState = PlayerState.Idle;
	}

	internal void LoadContent(ContentManager content) {
		// Idle: cells 30 px wide, 1/8 s per frame => 8 fps
		Texture2D idleTexture = content.Load<Texture2D>("Idle");
		int idleFrameWidth = 30;
		int idleFrameHeight = idleTexture.Height;
		int idleFrameCount = idleTexture.Width / idleFrameWidth;
		idleAnim = new SimpleAnimation(idleTexture, idleFrameWidth, idleFrameHeight, idleFrameCount, 8f);

		// Walk: cells 35 px wide, 1/8 s per frame => 8 fps
		Texture2D walkTexture = content.Load<Texture2D>("Walk");
		int walkFrameWidth = 35;
		int walkFrameHeight = walkTexture.Height;
		int walkFrameCount = walkTexture.Width / walkFrameWidth;
		walkAnim = new SimpleAnimation(walkTexture, walkFrameWidth, walkFrameHeight, walkFrameCount, 8f);

		// Jump: cells 30 px wide, 1/8 s per frame => 8 fps
		Texture2D jumpTexture = content.Load<Texture2D>("JumpOne");
		int jumpFrameWidth = 30;
		int jumpFrameHeight = jumpTexture.Height;
		int jumpFrameCount = jumpTexture.Width / jumpFrameWidth;
		jumpAnim = new SimpleAnimation(jumpTexture, jumpFrameWidth, jumpFrameHeight, jumpFrameCount, 8f);

		// After loading, make sure Initialize will have something to use
		currAnim = idleAnim;
	}

	internal void Update(GameTime gameTime) {
		float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

		currAnim?.Update(gameTime);

		velocity.Y += Platformer.GRAVITY;

		position += velocity * dt;

		if (MathF.Abs(velocity.Y) > Platformer.GRAVITY * dt) {
			playerState = PlayerState.Jumping;
			currAnim = jumpAnim;
			currAnim.Reset();
		}

		switch (playerState) {
			case PlayerState.Idle:
				break;
			case PlayerState.Jumping:
				break;
			case PlayerState.Walking:
				break;
		}
	}

	internal void Draw(SpriteBatch spriteBatch) {
		switch (playerState) {
			case PlayerState.Idle:
			case PlayerState.Jumping:
			case PlayerState.Walking:
				SpriteEffects spriteEffects = isFacingForward ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

				currAnim?.Draw(spriteBatch, position, spriteEffects);

				break;
		}
	}


	internal void Walk(float direction) {
		velocity.X = direction * speed;

		bool prevIsFacingForward = isFacingForward;
		isFacingForward = velocity.X > 0;

		if (playerState == PlayerState.Idle) {
			currAnim = walkAnim;
			currAnim.Reset();
			playerState = PlayerState.Walking;
		}

		if (prevIsFacingForward != isFacingForward)
			currAnim.Reset();
	}

	internal void Stop() {
		velocity = Vector2.Zero;

		if (playerState != PlayerState.Idle) {
			currAnim = idleAnim;
			currAnim.Reset();
			playerState = PlayerState.Idle;
		}
	}
}