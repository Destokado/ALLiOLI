using System.Collections.Generic;
using Cinemachine;
using Mirror;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : NetworkBehaviour
{
    [Space]
    [SerializeField] private PlayerGuiManager playerGui;
    
    [Space] 
    [SerializeField] private GameObject characterPrefab;
    public Character character { get; private set; }
    
    [Space] 
    [SerializeField] private float maxDistanceToInteractWithTrap = 10;
    [SerializeField] private LayerMask layersThatCanInterfereWithInteractions;
    public TrapManager ownedTraps { get; private set; }
    public int maxOwnableTraps => 50 / GameManager.singleton.totalPlayers;
    private Trap trapInFront {
        get => _trapInFront;
        set { _trapInFront = value; playerGui.ShowInteractionText(value != null && 
            (MatchManager.Instance.currentPhase is TrapUp || MatchManager.Instance.currentPhase is FinishingTrapUp));}}
    private Trap _trapInFront;
    private GameObject lastObjectInFront;

    [HideInInspector] public CmCamera playerCamera;
    public Color color { get { return _color; } private set { _color = value; playerGui.SetColor(_color); } }
    private Color _color;
    public bool isReady { get { return _isReady; } set { _isReady = value;  playerGui.ShowReadiness(isReady); } }
    private bool _isReady;

    // Called on all clients (when this NetworkBehaviour is network-ready)
    public override void OnStartClient()
    {
        Client.localClient.PlayersManager.players.Add(this);
        ownedTraps = new TrapManager();

        string customName = "Player " + GameManager.singleton.totalPlayers;
        
        // SetTarget depending on if the player is controlled by the local client or if it isn't
        CustomPlayerInput customPlayerInput = CustomPlayerInput.inputsWaitingForPlayers.Count > 0
            ? CustomPlayerInput.inputsWaitingForPlayers[0]
            : null;
        if (customPlayerInput != null)
        {
            CinemachineCore.GetInputAxis = customPlayerInput.GetAxisCustom;
            playerCamera = customPlayerInput.playerInput.camera.GetComponent<CmCamera>();
            gameObject.name = customName + " - Input by " + customPlayerInput.playerInput.user.controlScheme;
            
            customPlayerInput.player = this;
        }
        else // Shouldn't be controlled by the local client
        {
            gameObject.name = customName + " - No Input stream";
        }
        
        //this.color = color;

        SpawnNewCharacter();
    }

    /*private void Update()
    {
        UpdateObjectsInFront();
        //TODO: highlight the 'trapInFront'
        UpdateRadarTraps();
    }

    private void UpdateRadarTraps()
    {
        List<KeyValuePair<Trap, SortedList<float, Character>>> radarReport = ownedTraps.GetCharactersInEachTrapRadar(this);
        playerGui.ReportInRadar(radarReport);
    }

    private void UpdateObjectsInFront()
    {
        Ray ray = new Ray(character.cameraTarget.position, playerCamera.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, maxDistanceToInteractWithTrap, layersThatCanInterfereWithInteractions)) {
            if (lastObjectInFront != hit.collider.gameObject)
            {
                lastObjectInFront = hit.collider.gameObject;
                trapInFront = hit.transform.GetComponentInParent<Trap>();
            }
        }
        else
        {
            lastObjectInFront = null;
            trapInFront = null;
        }
    }*/

    public void SpawnNewCharacter()
    {
        this.character = Spawner.Instance.Spawn(characterPrefab).GetComponent<Character>();
        this.character.owner = this;
       // playerCamera.SetTarget(this.character.cameraTarget);
        playerCamera.SetTarget(this.character.cameraTarget,this.character.cameraTarget);
    }


    public void SetUpTrapInFront()
    {
        if (trapInFront == null)
            return;

        if (!ownedTraps.Remove(trapInFront))
            ownedTraps.Add(trapInFront);
        
        playerGui.ShowNumberOfTraps(ownedTraps.Count, maxOwnableTraps);
        DebugPro.LogEnumerable(ownedTraps, ", ", "The current owned traps for the player " + gameObject.name +" are: ", gameObject);
    }
    
    public void SetupForCurrentPhase()
    {
        playerGui.SetupForCurrentPhase(this);
    }
    
}