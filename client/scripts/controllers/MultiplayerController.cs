using Godot;
using System.Collections.Generic;

public partial class MultiplayerController : Control, IInstanceMappable
{
	public static string LABEL = "Multiplayer";


	private Server server;
	private AuthManager authManager;
	private SceneManager sceneManager;
	private DataManager dataManager;

	private MultiplayerApi multiplayer;

	private Button startGameButton;
	private Button mfaButton;
	private Control mfaControl;
	private LineEdit userNameLineEdit;
	private LineEdit passwordLineEdit;
	private LineEdit mfaTokenEdit;	
	private string userName;
	private string password;
	private string mfaToken;

	public override void _Ready()
	{
		server = Server.GetInstance(this, MultiplayerController.LABEL);
		multiplayer = server.multiplayer;
		sceneManager = SceneManager.GetInstance(this);
		dataManager = DataManager.GetInstance(this);
		authManager = AuthManager.GetInstance(this);

		startGameButton = GetNode<Button>("VBoxContainer/MarginContainer2/StartGameButton");
		startGameButton.Pressed += this.SignIn;
		startGameButton.Disabled = true;
		userNameLineEdit = GetNode<LineEdit>("VBoxContainer/MarginContainer/VBoxContainer/UserNameLineEdit");
		userNameLineEdit.TextChanged += OnUserNameUpdate;
		passwordLineEdit = GetNode<LineEdit>("VBoxContainer/MarginContainer/VBoxContainer/PasswordLineEdit");
		passwordLineEdit.TextChanged += OnPasswordUpdate;

		mfaTokenEdit = GetNode<LineEdit>("Control/MarginContainer/VBoxContainer/MfaTokenEdit");
		mfaTokenEdit.TextChanged += OnMfaUpdatedUpdate;
		mfaButton = GetNode<Button>("Control/MarginContainer/VBoxContainer/MfaSubmit");
		mfaButton.Pressed += this.submitMfa;
		mfaButton.Disabled = true;
		mfaControl = GetNode<Control>("Control");
		mfaControl.Visible = false;
	}

	public void FullyAuthenticated()
	{
		userNameLineEdit.Editable = false;
		passwordLineEdit.Editable = false;
		mfaControl.Visible = false;

		Error error = server.ConnectToServer();
		if (error == Error.Ok)
		{
			userNameLineEdit.Text = userName = "";
			passwordLineEdit.Text = password = "";
			mfaTokenEdit.Text = mfaToken = "";

		} else
		{
			startGameButton.Disabled = false;
			userNameLineEdit.Editable = true;
			passwordLineEdit.Editable = true;
		}
	}

	public void ShowMfa()
	{
		userNameLineEdit.Editable = false;
		passwordLineEdit.Editable = false;
		startGameButton.Disabled = true;

		mfaControl.Visible = true;
		mfaTokenEdit.Editable = true;
	}

	public void submitMfa()
	{
		authManager.AnswerChallenge(this.mfaToken);
		mfaTokenEdit.Editable = false;
	}

	public void SignIn()
	{
		userNameLineEdit.Editable = false;
		passwordLineEdit.Editable = false;

		authManager.SignIn(this.userName, this.password);
	}

	public void EnterGame()
	{
		GD.Print("Starting Game!");
		sceneManager.ShowScene(SceneManager.SCENES.TEST_2_PLAYER, this);
	}

	public void onStartGameButtonDown()
	{
		SignIn();
	}

	public void OnMfaUpdatedUpdate(string mfaToken)
	{
		this.mfaToken = mfaToken;
		this.mfaButton.Disabled = this.mfaToken == null || this.mfaToken == string.Empty;
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
