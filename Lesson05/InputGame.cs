using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Lesson05;

public class InputGame : Game {
	private GraphicsDeviceManager _graphics;
	private SpriteBatch _spriteBatch;

	private SpriteFont _font;
	private string _message = "fish";

	private KeyboardState _kbPreviousState, _kbCurrentState;

	public InputGame() {
		_graphics = new GraphicsDeviceManager(this);
		Content.RootDirectory = "Content";
		IsMouseVisible = true;
	}

	protected override void Initialize() {
		_kbPreviousState = Keyboard.GetState();

		base.Initialize();
	}

	protected override void LoadContent() {
		_spriteBatch = new SpriteBatch(GraphicsDevice);
		_font = Content.Load<SpriteFont>("SystemArialFont");
	}

	protected override void Update(GameTime gameTime) {
		_kbCurrentState = Keyboard.GetState();

		_message = "";
		#region arrow keys
		if (_kbCurrentState.IsKeyDown(Keys.Up)) {
			_message += " " + Keys.Up.ToString();
		}
		if (_kbCurrentState.IsKeyDown(Keys.Left)) {
			_message += " " + Keys.Left.ToString();
		}
		if (_kbCurrentState.IsKeyDown(Keys.Right)) {
			_message += " " + Keys.Right.ToString();
		}
		if (_kbCurrentState.IsKeyDown(Keys.Down)) {
			_message += " " + Keys.Down.ToString();
		}
		#endregion
		if (IsKeyPressed(Keys.Space)) {
			_message += "\n";
			_message += "Space pressed\n";
			_message += "----------------------------------------\n";
			_message += "----------------------------------------\n";
			_message += "----------------------------------------\n";
			_message += "----------------------------------------\n";
			_message += "----------------------------------------\n";
			_message += "----------------------------------------\n";
			_message += "----------------------------------------\n";
		} else if (_kbCurrentState.IsKeyDown(Keys.Space)) {
			_message += "\n";
			_message += "Space held";
		} else if (_kbPreviousState.IsKeyDown(Keys.Space)) {
			_message += "\n";
			_message += "Space released\n";
			_message += "++++++++++++++++++++++++++++++++++++++++\n";
			_message += "++++++++++++++++++++++++++++++++++++++++\n";
			_message += "++++++++++++++++++++++++++++++++++++++++\n";
			_message += "++++++++++++++++++++++++++++++++++++++++\n";
			_message += "++++++++++++++++++++++++++++++++++++++++\n";
			_message += "++++++++++++++++++++++++++++++++++++++++\n";
			_message += "++++++++++++++++++++++++++++++++++++++++\n";
		}

		_kbPreviousState = _kbCurrentState;
		base.Update(gameTime);
	}

	protected override void Draw(GameTime gameTime) {
		GraphicsDevice.Clear(Color.CornflowerBlue);

		_spriteBatch.Begin();
		_spriteBatch.DrawString(_font, _message, Vector2.Zero, Color.White);
		_spriteBatch.End();

		base.Draw(gameTime);
	}

	private bool IsKeyHeld(Keys key) {
		return _kbCurrentState.IsKeyDown(key);
	}

	private bool IsKeyPressed(Keys key) {
		return _kbPreviousState.IsKeyUp(key) && _kbCurrentState.IsKeyDown(key);
	}
}