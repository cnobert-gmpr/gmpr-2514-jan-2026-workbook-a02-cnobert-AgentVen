using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Lesson08;

public class Cannon {
	private SimpleAnimation animation;
	private Vector2 position;
	private Point dimensions;


	internal void Initialize(Vector2 initPosition) {
		position = initPosition;
	}

	internal void LoadContent(ContentManager content) {
		Texture2D texture = content.Load<Texture2D>("Cannon");
		dimensions = new Point(texture.Width / 4, texture.Height);
		animation = new SimpleAnimation(texture, dimensions.X, dimensions.Y, 4, 2f);
	}

	internal void Update(GameTime gameTime) {
		animation.Update(gameTime);
	}

	internal void Draw(SpriteBatch spriteBatch) {
		if (animation != null) animation.Draw(spriteBatch, position, SpriteEffects.None);
	}
}