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
	private Label signInError;
	private Label mfaError;
	private Label globalError;

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

		signInError = GetNode<Label>("VBoxContainer/MarginContainer/VBoxContainer/SignInError");
		mfaError = GetNode<Label>("Control/MarginContainer/VBoxContainer/MfaError");
		globalError = GetNode<Label>("VBoxContainer/MarginContainer/VBoxContainer/GlobalError");
	}

	public void Reset()
	{
		HideUserPass();
		HideMfa();

		ShowUserPass();

		globalError.Text = "";
	}

	public void FullyAuthenticated()
	{
		HideMfa();
		HideUserPass();

		Error error = server.ConnectToServer();
		if (error != Error.Ok)
		{
			ShowUserPass();
			ConnectionFailed( $"Error creating client, {error}");
		}
	}

	public void MfaChallenge()
	{
		HideUserPass();
		ShowMfa();
	}

	public void ShowMfa()
	{
		mfaControl.Visible = true;
		mfaTokenEdit.Editable = true;

		mfaTokenEdit.Text = mfaToken = "";
	}

	public void HideMfa()
	{
		mfaControl.Visible = false;
		mfaTokenEdit.Editable = false;

		mfaError.Visible = false;
	}

	public void ShowUserPass()
	{
		userNameLineEdit.Editable = true;
		passwordLineEdit.Editable = true;
		startGameButton.Disabled = true;

		passwordLineEdit.Text = password = "";
	}

	public void HideUserPass()
	{
		userNameLineEdit.Editable = false;
		passwordLineEdit.Editable = false;
		startGameButton.Disabled = true;
		signInError.Visible = false;
	}

	public void submitMfa()
	{
		authManager.AnswerChallenge(this.mfaToken);
		mfaTokenEdit.Editable = false;
	}

	public void MfaError(string message)
	{
		ShowMfa();
		mfaError.Text = $"Error: {message}";
		mfaError.Visible = true;
	}

	public void SignIn()
	{
		HideUserPass();

		authManager.SignIn(this.userName, this.password);
	}

	public void SignInError(string message)
	{
		ShowUserPass();
		signInError.Text = $"Error: {message}";
		signInError.Visible = true;
	}

	public void EnterGame(string mapId)
	{
		GD.Print("Starting Game!");
		sceneManager.ShowScene(SceneManager.SCENES.GAME, this, mapId);
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

	public void ConnectionFailed()
	{
		ConnectionFailed("Connection Failed");
	}

	public void ConnectionFailed(string message)
	{
		Reset();
		globalError.Text = $"Error: {message}";
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
