using System;
using System.Collections.Generic;
using Cinemachine;
using Mirror;
using UnityEngine;
using UnityEngine.InputSystem;

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
                HumanLocalPlayer.Camera.SetTarget(value.cameraTarget,value.cameraTarget);
        }
    }
    // ReSharper disable once InconsistentNaming
    private Character _character;

    /// <summary>
    /// The color that identifies the character.
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
        if (humanLocalPlayer)
            humanLocalPlayer.playerGui.SetColor(newColor);
    }

    /// <summary>
    /// If the player is "ready" or if it is not.
    /// </summary>
    [field: SyncVar(hook = nameof(SetReady))]
    public bool isReady;

    /// <summary>
    /// Updates the color in the playerGUI if it is a player with a humanLocalPlayer.
    /// </summary>
    /// <param name="oldValue">The old state of readiness for the player.</param>
    /// <param name="newValue">The new state of readiness for the player.</param>
    private void SetReady(bool oldValue, bool newValue)
    {
        if (humanLocalPlayer)
            humanLocalPlayer.playerGui.ShowReadiness(newValue);
    }

    /// <summary>
    /// A reference to the HumanLocalPlayer in charge of controlling this player.
    /// <para>If it is null, it means that this player is controlled and synchronized trough network, not locally.</para>
    /// <para>Giving it a value will set the the value as the referenced/controled player in the "HumanLocalPlayer".</para>
    /// </summary>
    public HumanLocalPlayer HumanLocalPlayer
    {
        get => humanLocalPlayer;
        private set
        {
            humanLocalPlayer = value;
            if (value != null)
                humanLocalPlayer.Player = this;
        }
    }

    private HumanLocalPlayer humanLocalPlayer;

    /// <summary>
    /// If the player is controlled by a human in this machine (locally).
    /// </summary>
    private bool IsControlledLocally => HumanLocalPlayer != null;

    // Called on all clients (when this NetworkBehaviour is network-ready)
    public override void OnStartClient()
    {
        Debug.Log("OnStartClient, Joined player.");

        Client.localClient.PlayersManager.players.Add(this);

        string customName = "Player " + GameManager.TotalPlayers;

        // Is any human waiting for a player to be available? If it is, set the player as their property
        HumanLocalPlayer = HumanLocalPlayer.inputsWaitingForPlayers.Count > 0
            ? HumanLocalPlayer.inputsWaitingForPlayers[0]
            : null;

        if (IsControlledLocally)
        {
            gameObject.name = customName + " - Input by " + HumanLocalPlayer.PlayerInput.user.controlScheme;
        }
        else
        {
            gameObject.name = customName + " - No Input stream";
        }

        if (hasAuthority)
        {
            CmdSetupPlayerOnServer();
            CmdSpawnNewCharacter();
        }
    }

    [Command]
    private void CmdSetupPlayerOnServer()
    {
        Color = GameManager.singleton.playerColors[GameManager.TotalPlayers - 1];
    }

    [Command]
    public void CmdSpawnNewCharacter()
    {
        Spawner.Instance.Spawn(characterPrefab, this.netId, connectionToClient);
    }

    public void SetupForCurrentPhase()
    {
        if (humanLocalPlayer != null)
            humanLocalPlayer.SetupForCurrentPhase();
    }

    [Command]
    public void CmdSetReady(bool newValue)
    {
        isReady = newValue;
    }
    
    [Command]
    public void CmdActivateTrap(uint trapNetId)
    {
        if (!NetworkIdentity.spawned.ContainsKey(trapNetId))
        {
            Debug.LogWarning("The trap with NetId " + trapNetId + " not found.");
            return;
        }
        
        NetworkIdentity.spawned[trapNetId].gameObject.GetComponent<Trap>().RpcActivate();
    }
}