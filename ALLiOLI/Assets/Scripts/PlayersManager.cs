using System.Collections.Generic;
using System.Linq;
using Mirror;
using UnityEngine;
using UnityEngine.InputSystem;

// Serveix per a afegir i gestionar jugadors locals (dins del mateix client)
public class PlayersManager : NetworkBehaviour
{
    /// <summary>
    /// Component that generates the new input handlers ("players")
    /// </summary>
    public PlayerInputManager playerInputManager { get; private set; }
    /// <summary>
    /// All the players of the client associated with the PlayersManager
    /// </summary>
    public HashSet<Player> players { get; private set; }

    /// <summary>
    /// The prefab of the player (the one shared across clients, NOT the PlayerInputReader)
    /// </summary>
    [Tooltip("The prefab of the player (the one shared across clients, NOT the PlayerInputReader)")]
    [SerializeField] private GameObject playerPrefab;
    
    /// <summary>
    /// Object initialization.
    /// <para>Called on the local client (when this player object is network-ready)</para>
    /// </summary>
    public override void OnStartClient()
    {
        base.OnStartClient();
        
        players = new HashSet<Player>();
        playerInputManager = GetComponent<PlayerInputManager>();
        playerInputManager.enabled = false; // TODO: Enable the localclient is the one associated with this object
    }
    
    /// <summary>
    /// New input/player detected in the local client.
    /// </summary>
    /// <param name="playerInput">The PlayerInput component of the new player/input instantiated.</param>
    private void OnPlayerJoined(PlayerInput playerInput)
    {
        Debug.Log("Player " + playerInput.playerIndex + " joined with input device: " + playerInput.devices[0], playerInput.gameObject);
        
        /*Player player = playerInput.GetComponent<Player>();
        players.Add(player);
        player.Setup(GameManager.singleton.playerColors[playerInput.playerIndex]);

        MatchManager.Instance.SetAllPlayersAsNotReady();*/

        //playerInput.GetComponent<Player>().ownerNetId = Client.localClient.netId;
        //NetworkServer.Spawn(playerInput.gameObject, Client.localClient.connectionToClient);
        
        CmdSpawnNewPlayer();
    }

    /// <summary>
    /// Spawn the player in all clients
    /// <para>Command: Called from a client and run on the server.</para>
    /// </summary>
    /// <param name="playerPrefab">The prefab to spawn on all clients.</param>
    [Command]
    private void CmdSpawnNewPlayer()
    {
        GameObject player = Instantiate(playerPrefab);
        NetworkServer.Spawn(player);
    }

    /// <summary>
    /// An input/player was removed/left in the local client
    /// </summary>
    /// <param name="playerInput">The PlayerInput component of the player/input.</param>
    private void OnPlayerLeft(PlayerInput playerInput)
    {
        Debug.Log("Player left with input device: " + playerInput.devices[0], playerInput.gameObject);
        players.Remove(playerInput.gameObject.GetComponent<Player>());
    }
}