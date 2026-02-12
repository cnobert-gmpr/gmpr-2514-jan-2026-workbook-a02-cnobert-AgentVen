using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Lesson07;

public class Ball {
	private Vector2 position, direction;
	private float speed, scale;

	private Texture2D texture;

	private Rectangle PlayAreaBoundingBox;


	internal Rectangle BoundingBox { 
		get => new Rectangle(position.ToPoint(), new Vector2(scale).ToPoint()); 
	}

	internal void Initialize(Vector2 ballPosition, Vector2 ballDirection, float ballSpeed, float ballScale, 
	Rectangle playAreaBoundingBox) {
		position = ballPosition;
		direction = ballDirection;
		speed = ballSpeed;
		scale = ballScale;
		PlayAreaBoundingBox = playAreaBoundingBox;
	}

	internal void LoadContent(ContentManager content) {
		texture = content.Load<Texture2D>("Ball");
	}

	internal void Update(GameTime gameTime) {
		float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

		position += direction * speed * dt;

		if (position.X <= PlayAreaBoundingBox.Left
		|| position.X + scale >= PlayAreaBoundingBox.Right) direction.X *= -1;
		
		if (position.Y <= PlayAreaBoundingBox.Top
		|| position.Y + scale >= PlayAreaBoundingBox.Bottom) direction.Y *= -1;
	}

	internal void Draw(SpriteBatch spriteBatch) {
		spriteBatch.Draw(texture, BoundingBox, Color.White);
	}


	internal void ProcessCollision(Rectangle otherBoundingBox) {
		if (BoundingBox.Intersects(otherBoundingBox)) {
			// TODO)) Need timed debounce

			Rectangle interect = Rectangle.Intersect(BoundingBox, otherBoundingBox);

			if (interect.Width > interect.Height) direction *= -Vector2.UnitY;
			else direction *= -Vector2.UnitX;
		}
	}
}