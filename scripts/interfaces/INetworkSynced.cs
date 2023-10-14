using Godot;

public partial interface INetworkSynced
{
	MultiplayerSynchronizer multiplayerSynchronizer { get; set; }

	public bool HasAuthority();
	public void SetAuthority(int id, bool recursive);
}