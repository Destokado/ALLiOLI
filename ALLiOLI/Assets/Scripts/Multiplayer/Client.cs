using Mirror;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

[RequireComponent(typeof(PlayerInputManager))]
public class Client : NetworkBehaviour
{
    [field: SyncVar] public int clientId { get; private set; }
    [field: SyncVar] public string customName { get; private set; }

    public static Client LocalClient
    {
        get => _localClient;
        private set
        {
//            Debug.Log(value);
            if (_localClient != null)
                Debug.LogWarning($"Reseting the local client to '{value}'.");
            else
                Debug.Log($"Saving the reference to the local client '{value}'.");
            if (value != _localClient)
            {
                if (_localClient != null) _localClient.GetComponent<SoundManagerOnline>().enabled = false;
                if (value != null)
                {
                    Debug.Log("SoundManagerOnline component added to localclient.");
                    value.gameObject.GetComponent<SoundManagerOnline>().enabled = true;
                }
            }

            _localClient = value;
        }
    }

    // ReSharper disable once InconsistentNaming
    private static Client _localClient;

    public SoundManagerOnline SoundManagerOnline => soundManagerOnline;

    [FormerlySerializedAs("_soundManager")] [SerializeField] private SoundManagerOnline soundManagerOnline;

    public PlayersManager PlayersManager => _playersManager;
    // ReSharper disable once InconsistentNaming
    [SerializeField] private PlayersManager _playersManager;

    public bool isLocalClient => LocalClient == this;

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

        transform.SetParent(NetworkManager.singleton.transform, true);
        MatchManager.instance.clients.Add(this);
        
        
        MatchManager.instance.InitializePhaseSystem();

        gameObject.name = "Client " + clientId;
    }
    
    // Called on the local client (when this NetworkBehaviour is network-ready)
    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();
        LocalClient = this;
        PlayersManager.playerInputManager.enabled = true;

        CmdSynchCrustomName(GlobalConfiguration.Instance.clientName);
        // GameManager.Instance.GUI.UpdateOnlineLobby(false);
    }

    [Command]
    private void CmdSynchCrustomName(string name)
    {
        customName = name;
        Debug.Log($"Got the name '{name}' for the Client.");
    }

    //Called on remaining clients, when a client disconnects
    public override void OnStopClient()
    {
        base.OnStopClient();
        MatchManager.instance.clients.Remove(this);
    }

    public static bool IsThereALocalPlayer()
    {
        return LocalClient != null && LocalClient.PlayersManager.players.Count > 0;
    }
}