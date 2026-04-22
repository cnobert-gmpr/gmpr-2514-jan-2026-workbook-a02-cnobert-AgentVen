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
	private Collider[] platform01;

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

		ground = new Collider(new Vector2(0, WINDOW_HEIGHT - 100), new Vector2(WINDOW_WIDTH, 1), 
			ColliderType.Top);

		platform01 = new Collider[4];
		platform01[0] = new Collider(new Vector2(160, 230), new Vector2(80, 1), ColliderType.Top);
		platform01[1] = new Collider(new Vector2(250, 230), new Vector2(1, 20), ColliderType.Right);
		platform01[2] = new Collider(new Vector2(160, 250), new Vector2(80, 1), ColliderType.Bottom);
		platform01[3] = new Collider(new Vector2(150, 230), new Vector2(1, 20), ColliderType.Left);
		
		base.Initialize();
	}

	protected override void LoadContent() {
		spriteBatch = new SpriteBatch(GraphicsDevice);

		_player.LoadContent(Content);

		ground.LoadContent(GraphicsDevice);

		foreach (Collider collider in platform01) collider.LoadContent(GraphicsDevice);
	}

	protected override void Update(GameTime gameTime) {
		#region Input
		KeyboardState keyboardState = Keyboard.GetState();

		if (keyboardState.IsKeyDown(Keys.Left)) _player.Walk(-1);
		else if (keyboardState.IsKeyDown(Keys.Right)) _player.Walk(1);
		else _player.Stop();

		if (keyboardState.IsKeyDown(Keys.Space)) _player.Jump();
		#endregion

		ground.PlayerCollision(_player, gameTime);

		foreach (Collider collider in platform01) collider.PlayerCollision(_player, gameTime);

		_player.Update(gameTime);

		base.Update(gameTime);
	}

	protected override void Draw(GameTime gameTime) {
		GraphicsDevice.Clear(Color.CornflowerBlue);

		spriteBatch.Begin();

		_player.Draw(spriteBatch);

		ground.Draw(spriteBatch);

		foreach (Collider collider in platform01) collider.Draw(spriteBatch);

		spriteBatch.End();

		base.Draw(gameTime);
	}
}
