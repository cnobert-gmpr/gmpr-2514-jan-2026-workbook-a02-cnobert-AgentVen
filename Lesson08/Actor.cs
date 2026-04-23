using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Lesson08;

public class Actor {
	protected Vector2 position, direction;
	protected float speed;

	protected Rectangle gameBoundingBox;
}