using Mirror;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInputManager))]
public class Client : NetworkBehaviour
{
    [field: SyncVar] public int clientId { get; private set; }

    public static Client LocalClient
    {
        get => _localClient;
        private set
        {
            if (_localClient != null)
                Debug.LogWarning("Reseting the local client.");
            else
                Debug.Log("Saving the reference to the local client.");
            _localClient = value;
        }
    }

    // ReSharper disable once InconsistentNaming
    private static Client _localClient;

    public PlayersManager PlayersManager => _playersManager;
    // ReSharper disable once InconsistentNaming
    [SerializeField] private PlayersManager _playersManager;

    // Called on the server (when this NetworkBehaviour is network-ready)
    public override void OnStartServer()
    {
        base.OnStartServer();
        clientId = connectionToClient.connectionId;
    }

    // Called on all clients (when this NetworkBehaviour is network-ready)
    public override void OnStartClient()
    {
        Debug.Log("Client ready to be used in the network. Starting client.");
        
        base.OnStartClient();

        transform.SetParent(NetworkManager.singleton.transform, false);
        MatchManager.Instance.Clients.Add(this);

        
        // GameManager.Instance.GUI.UpdateOnlineLobby(false);

        MatchManager.Instance.FinishAndRestartCurrentPhase();

        gameObject.name = "Client " + clientId;
    }

    // Called on the local client (when this NetworkBehaviour is network-ready)
    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();
        LocalClient = this;
        PlayersManager.playerInputManager.enabled = true;
        // GameManager.Instance.GUI.UpdateOnlineLobby(false);
    }

    //Called on remaining clients, when a client disconnects
    public override void OnStopClient()
    {
        base.OnStopClient();
        MatchManager.Instance.Clients.Remove(this);
    }

    public static bool IsThereALocalPlayer()
    {
        return LocalClient != null && LocalClient.PlayersManager.players.Count > 0;
    }
}