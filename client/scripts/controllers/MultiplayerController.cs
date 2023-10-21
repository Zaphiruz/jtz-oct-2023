using Godot;
using System.Collections.Generic;

public partial class MultiplayerController : Control
{
	private Server server;
	private SceneManager sceneManager;
	private DataManager dataManager;

	private MultiplayerApi multiplayer;

	private Button startGameButton;
	private LineEdit userNameLineEdit;
	private LineEdit passwordLineEdit;
	private string userName;
	private string password;

	public override void _Ready()
	{
		server = Server.GetInstance(this);
		multiplayer = server.multiplayer;
		sceneManager = SceneManager.GetInstance(this);
		dataManager = DataManager.GetInstance(this);

		startGameButton = GetNode<Button>("VBoxContainer/MarginContainer2/StartGameButton");
		startGameButton.Pressed += this.onStartGameButtonDown;
		startGameButton.Disabled = true;
		userNameLineEdit = GetNode<LineEdit>("VBoxContainer/MarginContainer/VBoxContainer/UserNameLineEdit");
		userNameLineEdit.TextChanged += OnUserNameUpdate;
		passwordLineEdit = GetNode<LineEdit>("VBoxContainer/MarginContainer/VBoxContainer/PasswordLineEdit");
		passwordLineEdit.TextChanged += OnPasswordUpdate;
	}

	public void StartGame()
	{
		userNameLineEdit.Editable = false;
		passwordLineEdit.Editable = false;

		// Error error = gateway.ConnectToServer(userName, password);
		Error error = server.ConnectToServer();
		if (error == Error.Ok)
		{
			userNameLineEdit.Text = userName = "";
			passwordLineEdit.Text = password = "";

			GD.Print("Starting Game!");
			sceneManager.ShowScene(SceneManager.SCENES.TEST_2_PLAYER, this);
		} else
		{
			startGameButton.Disabled = false;
			userNameLineEdit.Editable = true;
			passwordLineEdit.Editable = true;
		}
	}

	public void onStartGameButtonDown()
	{
		StartGame();
	}

	public void OnUserNameUpdate(string userName)
	{
		this.userName = userName;
		UpdateButtonState();
	}

	public void OnPasswordUpdate(string password)
	{
		this.password = password;
		UpdateButtonState();
	}

	public void UpdateButtonState()
	{
		startGameButton.Disabled = userName == string.Empty || password == string.Empty;
	}
}
