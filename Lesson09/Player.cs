using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Lesson09;

public class Player {
	private const int SPEED = 150, JUMP_VELOCITY = -130;

	private enum AnimationState { Idle, Jumping, Walking }
	private AnimationState animationState;

	private bool isFacingForward = true;

	private Vector2 position, velocity, size;
	private float speed;

	private SimpleAnimation idleAnim, walkAnim, jumpAnim, currAnim;

	private Rectangle gameBoundingBox;

	internal Rectangle BoundingBox { get => new Rectangle(position.ToPoint(), size.ToPoint()); }

	internal Vector2 Velocity { get => velocity; }


	public Player(Vector2 constrPosition, Rectangle constrGameBoundingBox) {
		position = constrPosition;
		gameBoundingBox = constrGameBoundingBox;

		velocity = Vector2.Zero;
		size = new Vector2(35, 34);
		speed = (float)SPEED;
	}

	internal void Initialize() {
		animationState = AnimationState.Idle;
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

		velocity.Y += Platformer.GRAVITY * dt;

		position += velocity * dt;

		if (MathF.Abs(velocity.Y) > Platformer.GRAVITY * dt) {
			animationState = AnimationState.Jumping;
			currAnim = jumpAnim;
			currAnim.Reset();
		}

		switch (animationState) {
			case AnimationState.Idle:
				break;
			case AnimationState.Jumping:
				break;
			case AnimationState.Walking:
				break;
		}
	}

	internal void Draw(SpriteBatch spriteBatch) {
		switch (animationState) {
			case AnimationState.Idle:
			case AnimationState.Jumping:
			case AnimationState.Walking:
				SpriteEffects spriteEffects = isFacingForward ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

				currAnim?.Draw(spriteBatch, position, spriteEffects);

				break;
		}
	}


	internal void Walk(float direction) {
		velocity.X = direction * speed;

		bool prevIsFacingForward = isFacingForward;
		if (velocity.X != 0) isFacingForward = velocity.X > 0;

		if (animationState == AnimationState.Idle) {
			currAnim = walkAnim;
			currAnim.Reset();
			animationState = AnimationState.Walking;
		}

		if (prevIsFacingForward != isFacingForward)
			currAnim.Reset();
	}

	internal void VMove(float direction) {
		velocity.Y = direction * speed;
	}

	internal void Jump() {
		if (animationState != AnimationState.Jumping)
			velocity.Y = JUMP_VELOCITY;
	}

	internal void Stop() {
		velocity = Vector2.Zero;

		if (animationState != AnimationState.Idle) {
			currAnim = idleAnim;
			currAnim.Reset();
			animationState = AnimationState.Idle;
		}
	}

	internal void SnapToSurface(Rectangle boundingBox) {
		if (animationState == AnimationState.Jumping) {
			position.Y = boundingBox.Top - size.Y + 1;
			velocity.Y = 0;
			animationState = AnimationState.Walking;
		}
	}

	internal void Grounded(Rectangle boundingBox, float dt) {
		velocity.Y -= Platformer.GRAVITY * dt;
	}


	internal void PushForce(Vector2 pushDirection, float dt) {
		position += pushDirection * dt;
	}
}