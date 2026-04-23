using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Lesson09;

public class Platform {
	private Vector2 position, size;

	private Collider[] _colliders;
	

	public Platform(Vector2 constrPosition, Vector2 constrSize) {
		position = constrPosition;
		size = constrSize;

		_colliders = new Collider[4];
		_colliders[0] = new Collider(
			new Vector2(position.X + 3, position.Y), new Vector2(size.X - 6, 1),
			Collider.ColliderType.Top);
		_colliders[1] = new Collider(
			new Vector2(position.X + size.X - 1, position.Y + 1), new Vector2(1, size.Y - 2),
			Collider.ColliderType.Right);
		_colliders[2] = new Collider(
			new Vector2(position.X + 3, position.Y + size.Y), new Vector2(size.X - 6, 1),
			Collider.ColliderType.Bottom);
		_colliders[3] = new Collider(
			new Vector2(position.X + 1, position.Y + 1), new Vector2(1, size.Y - 2),
			Collider.ColliderType.Left);
	}

	internal void LoadContent(GraphicsDevice graphicsDevice) {
		foreach (Collider collider in _colliders) collider.LoadContent(graphicsDevice);
	}

	internal void Draw(SpriteBatch spriteBatch) {
		foreach (Collider collider in _colliders) collider.Draw(spriteBatch);
	}


	internal void PlayerCollisions(Player player, GameTime gameTime) {
		foreach (Collider collider in _colliders) collider.PlayerCollision(player, gameTime);
	}
}