using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Lesson07;

public class Ball {
	private Vector2 position, direction;
	private float speed, scale;

	private Texture2D texture;

	private Rectangle PlayAreaBoundingBox;


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
		Rectangle rect = new Rectangle(
			(int)position.X, (int)position.Y, (int)scale, (int)scale);
		spriteBatch.Draw(texture, rect, Color.White);
	}
}