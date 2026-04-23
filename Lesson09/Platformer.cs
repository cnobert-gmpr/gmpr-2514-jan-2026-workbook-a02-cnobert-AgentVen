using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using static Lesson09.Collider;

namespace Lesson09;

public class Platformer : Game {
	private const int WINDOW_WIDTH = 550, WINDOW_HEIGHT = 400;
	
	internal const float GRAVITY = 100;

	private readonly GraphicsDeviceManager graphicsDeviceManager;
	private SpriteBatch spriteBatch;

	private Player _player;

	private Collider ground;
	private List<Platform> _platforms;

	private Rectangle gameBoundingBox = new Rectangle(0, 0, WINDOW_WIDTH, WINDOW_HEIGHT);


	public Platformer() {
		graphicsDeviceManager = new GraphicsDeviceManager(this);
		Content.RootDirectory = "Content";
		IsMouseVisible = true;
	}

	protected override void Initialize() {
		graphicsDeviceManager.PreferredBackBufferWidth = WINDOW_WIDTH;
		graphicsDeviceManager.PreferredBackBufferHeight = WINDOW_HEIGHT;
		graphicsDeviceManager.ApplyChanges();

		_player = new Player(new Vector2(50, 50), gameBoundingBox);
		_player.Initialize();

		ground = new Collider(
			new Vector2(0, WINDOW_HEIGHT - 100), new Vector2(WINDOW_WIDTH, 1),
			ColliderType.Top);

		_platforms = new List<Platform>();
		_platforms.Add(new Platform(new Vector2(100, 200), new Vector2(70, 10)));
		_platforms.Add(new Platform(new Vector2(100, 150), new Vector2(70, 10)));
		_platforms.Add(new Platform(new Vector2(100, 100), new Vector2(70, 10)));
		_platforms.Add(new Platform(new Vector2(400, 250), new Vector2(70, 10)));
		_platforms.Add(new Platform(new Vector2(300, 50), new Vector2(70, 10)));
		
		base.Initialize();
	}

	protected override void LoadContent() {
		spriteBatch = new SpriteBatch(GraphicsDevice);

		_player.LoadContent(Content);

		ground.LoadContent(GraphicsDevice);

		foreach (Platform platform in _platforms) platform.LoadContent(GraphicsDevice);
	}

	protected override void Update(GameTime gameTime) {
		#region Input
		KeyboardState keyboardState = Keyboard.GetState();

		if (keyboardState.IsKeyDown(Keys.Left)) _player.Walk(-1);
		else if (keyboardState.IsKeyDown(Keys.Right)) _player.Walk(1);
		else _player.Stop();

		if (keyboardState.IsKeyDown(Keys.Space)) _player.Jump();
		#endregion

		_player.Update(gameTime);

		ground.PlayerCollision(_player, gameTime);

		foreach (Platform platform in _platforms) platform.PlayerCollisions(_player, gameTime);

		base.Update(gameTime);
	}

	protected override void Draw(GameTime gameTime) {
		GraphicsDevice.Clear(Color.CornflowerBlue);

		spriteBatch.Begin();

		_player.Draw(spriteBatch);

		ground.Draw(spriteBatch);

		foreach (Platform platform in _platforms) platform.Draw(spriteBatch);

		spriteBatch.End();

		base.Draw(gameTime);
	}
}
