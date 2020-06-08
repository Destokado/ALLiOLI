using System;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using Mirror;
using UnityEngine;
using UnityEngine.InputSystem;

[SelectionBase]
public class Player : NetworkBehaviour
{
    /// <summary>
    /// The prefab of the character that whe player will use to play.
    /// </summary>
    [Space] [SerializeField] private GameObject characterPrefab;

    /// <summary>
    /// A reference to the current active character for the player
    /// </summary>
    public Character Character
    {
        get => _character;
        set {
            _character = value;
            if (IsControlledLocally)
            {
                float blendingTime = HumanLocalPlayer.Camera.SetTargetWithCinematics(value.cameraTarget,value.transform);
                HumanLocalPlayer.DisablePlayerInputDuring(blendingTime);
            }
                
        }
    }
    // ReSharper disable once InconsistentNaming
    private Character _character;

    /// <summary>
    /// The color that identifies the player and their characters.
    /// </summary>
    [field: SyncVar(hook = nameof(SetColor))]
    // ReSharper disable once UnusedAutoPropertyAccessor.Local
    public Color Color { get; private set; }

    /// <summary>
    /// Updates the color in the playerGUI if it is a player with a humanLocalPlayer.
    /// </summary>
    /// <param name="oldColor">The old color.</param>
    /// <param name="newColor">The new color.</param>
    private void SetColor(Color oldColor, Color newColor)
    {
        if (HumanLocalPlayer)
            HumanLocalPlayer.playerGui.SetColor(newColor);
        if (Character != null)
            Character.UpdateColor();
    }

    /// <summary>
    /// If the player is "ready" or if it is not.
    /// </summary>
    [field: SyncVar(hook = nameof(SetReadyOnGUI))]
    public bool isReady;

    /// <summary>
    /// Updates the playerGUI (if it is a player with a humanLocalPlayer).
    /// </summary>
    /// <param name="oldValue">The old state of readiness for the player.</param>
    /// <param name="newValue">The new state of readiness for the player.</param>
    private void SetReadyOnGUI(bool oldValue, bool newValue)
    {
        if (HumanLocalPlayer)
            HumanLocalPlayer.playerGui.ShowReadiness(this);
    }

    /// <summary>
    /// A reference to the HumanLocalPlayer in charge of controlling this player.
    /// <para>If it is null, it means that this player is controlled and synchronized trough network, not locally.</para>
    /// <para>Giving it a value will set the the value as the referenced/controled player in the "HumanLocalPlayer".</para>
    /// </summary>
    public HumanLocalPlayer HumanLocalPlayer
    {
        get => _humanLocalPlayer;
        private set
        {
            if (HumanLocalPlayer != null)
            {
                Debug.LogWarning("Trying to change the humanLocalPlayer of a Player. Operation cancelled.");
                return;
            }
            
            _humanLocalPlayer = value;
            if (value != null)
            {
                _humanLocalPlayer.Player = this;
                HumanLocalPlayer.localPlayerNumber = Client.LocalClient.PlayersManager.players.Count;
            }
        }
    }
    private HumanLocalPlayer _humanLocalPlayer;


    [SyncVar(hook = nameof(NewIdOfHumanLocalPlayer))]
    public int idOfHumanLocalPlayer;
    private void NewIdOfHumanLocalPlayer(int oldVal, int newVal)
    {
        if (oldVal != 0)
            Debug.LogWarning("Trying to change the idOfHumanLocalPlayer of a Player. It shouldn't be done.");
    }
    
    [SyncVar(hook = nameof(NewIdOfHumanLocalPlayer))]
    public int idOfClient;
    private void NewIidOfClient(int oldVal, int newVal)
    {
        if (oldVal != 0)
            Debug.LogWarning("Trying to change the idOfClient of a Player. It shouldn't be done.");
    }

    public Client Client {
        get {
            if (!_client)
                foreach (Client client in MatchManager.instance.clients.Where(client => client.clientId == idOfClient))
                    _client = client;

            return _client;
        }
    }
    // ReSharper disable once InconsistentNaming
    private Client _client;
    
    [SyncVar(hook = nameof(NewPlayerIndex))]
    private int playerIndex = -1;
    
    [SyncVar(hook = nameof(UpdatedTrapActivationsNumber))] public int trapActivators = 0;
    private void UpdatedTrapActivationsNumber(int oldVal, int newVal)
    {
        if (HumanLocalPlayer)
            HumanLocalPlayer.playerGui.SetAmmunitionTo(newVal);
    }

    private void NewPlayerIndex(int oldVal, int newVal)
    {
        if (oldVal != -1)
            Debug.LogWarning("Trying to change the playerIndex of a Player. It shouldn't be done.");
        
        gameObject.name =
            $"Player {playerIndex} | {(IsControlledLocally ? $"Input by {HumanLocalPlayer.PlayerInput.user.controlScheme}" : "Controlled remotely")}";
        Color = MatchManager.instance.GetColor(playerIndex);
    }

    /// <summary>
    /// If the player is controlled by a human in this machine (locally).
    /// </summary>
    private bool IsControlledLocally => HumanLocalPlayer != null;

    // Called on all clients (when this NetworkBehaviour is network-ready)
    public override void OnStartClient()
    {
        Client.PlayersManager.players.Add(this);
        transform.SetParent(Client.transform, true);
        Debug.Log($"Added player to owner client's player list.");
        
        // Is any human waiting for a player to be available? If it is, set the player as their property
        HumanLocalPlayer[] allHumans = UnityEngine.Object.FindObjectsOfType<HumanLocalPlayer>();
        foreach (HumanLocalPlayer human in allHumans)
            if (human.id == idOfHumanLocalPlayer)
            {
                HumanLocalPlayer = human;
                break;
            }
        
        if (hasAuthority)
        {
            CmdSetupPlayerOnServer();
            CmdSpawnNewCharacter();
        }

        SetupForCurrentPhase();
    }

    [Command] //Only executed on server after being called by client with authority
    private void CmdSetupPlayerOnServer()
    {
        playerIndex = MatchManager.TotalCurrentPlayers;
    }

    [Command]
    public void CmdSpawnNewCharacter()
    {
        Spawner.Instance.Spawn(characterPrefab, this.netId, connectionToClient);
    }

    public void SetupForCurrentPhase()
    {
        if (HumanLocalPlayer != null)
            HumanLocalPlayer.SetupForCurrentPhase();
    }

    [Command]
    public void CmdSetReady(bool newValue)
    {
        isReady = newValue;
    }
    
    [Command] // On server, called by a client
    public void CmdActivateTrap(uint trapNetId)
    {
        if (trapActivators <= 0) return;
        
        GameObject trapGo = ((AllIOliNetworkManager) NetworkManager.singleton).GetGameObject(trapNetId);
        Trap trap = trapGo.GetComponent<Trap>();
        
        if (!trap) return;
        
        trap.Activate();
        trapActivators -= 1;
    }

}