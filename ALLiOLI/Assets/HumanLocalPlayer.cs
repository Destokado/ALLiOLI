using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class HumanLocalPlayer : MonoBehaviour
{
    /// <summary>
    /// Gui that displays the information that is only useful to the player itself.
    /// </summary>
    [SerializeField] public PlayerGuiManager playerGui;
    
    public static readonly List<HumanLocalPlayer> inputsWaitingForPlayers = new List<HumanLocalPlayer>();
    public Player Player {
        get => _player;
        set {
            if (_player != null && value != null)
                Debug.LogWarning("Trying to assign a player to a HumanLocalPlayer with a Player already assigned. Operation Cancelled.", this);
            else {
                _player = value;
                if (value == null) inputsWaitingForPlayers.Add(this); 
                else inputsWaitingForPlayers.Remove(this);
            }
            SetDynamicName();
        }
    }
    // ReSharper disable once InconsistentNaming
    private Player _player;
    
    public PlayerInput PlayerInput { 
        get {
            if (_playerInput == null) { _playerInput = gameObject.GetComponentRequired<PlayerInput>(); }
            return _playerInput; }
        private set => _playerInput = value;
    }
    // ReSharper disable once InconsistentNaming
    private PlayerInput _playerInput;


    public CmCamera Camera { 
        get {
            if (_camera == null) { _camera = PlayerInput.camera.gameObject.GetComponentRequired<CmCamera>(); }
            return _camera; }
        private set => _camera = value;
    }
    // ReSharper disable once InconsistentNaming
    private CmCamera _camera;

    /// <summary>
    /// The maximum distance at which a trap can be to the character so the player can interact with it. // TODO: ensure if the distance id from the character or the camera
    /// </summary>
    [Space] [SerializeField] private float maxDistanceToInteractWithTrap = 7;
    //TODO: Documentation...
    [SerializeField] private LayerMask layersThatCanInterfereWithInteractions;
    private Trap trapInFront { get => _trapInFront;
        set { _trapInFront = value; playerGui.ShowInteractionText(value != null && 
                                                                                    (MatchManager.Instance.currentPhase is TrapUp || MatchManager.Instance.currentPhase is FinishingTrapUp));}}
    private Trap _trapInFront;
    private GameObject lastObjectInFront;
    
    public TrapManager ownedTraps { get; private set; }
    public int maxOwnableTraps => 50 / GameManager.TotalPlayers;
    
    private void Awake()
    {
        inputsWaitingForPlayers.Add(this);
        int layer = Client.localClient.PlayersManager.players.Count + 10;
        Camera.SetLayer(layer,PlayerInput.camera);
        ownedTraps = new TrapManager();
        SetDynamicName();
    }

    private void SetDynamicName()
    {
        string newName;
        if (Player == null)
            newName = "HumanLocalPlayer of Unknown Player";
        else 
            newName = "HumanLocalPlayer of " + Player.name;

        gameObject.name = newName;
    }

    private void Update()
    {
        if (Player == null)
            return;
        
        if (Player.Character != null)
            UpdateObjectsInFront();
        
        //TODO: highlight the 'trapInFront'
        UpdateRadarTraps();
    }

    private void UpdateRadarTraps()
    {
        List<KeyValuePair<Trap, SortedList<float, Character>>> radarReport =
            ownedTraps.GetCharactersInEachTrapRadar(Player);
        playerGui.ReportInRadar(radarReport);
    }

    private void UpdateObjectsInFront()
    {
        Ray ray = new Ray(Player.Character.cameraTarget.position, Camera.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, maxDistanceToInteractWithTrap,
            layersThatCanInterfereWithInteractions))
        {
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

    #region input

    private void OnCameraMove(InputValue value)
    {
        if (Player == null) return;
        Camera.cameraMovement = value.Get<Vector2>();
    }

    private void OnCharacterMove(InputValue value)
    {
        if (Player == null) return;
        
        Player.Character.movementController.horizontalMovementInput = value.Get<Vector2>();
    }

    private void OnTrap()
    {
        if (Player == null) return;
        
        State currentState = MatchManager.Instance.currentPhase;
        
        switch (currentState)
        {
            case Battle battle:
                Player.CmdActivateTrap(ownedTraps.GetBestTrapToActivate(Player).netId);
                break;
            case TrapUp trapUp:
                SetUpTrapInFront();
                break;
        }
    }

    private void OnReady()
    {
        if (Player == null) return;
        Player.CmdSetReady(!Player.isReady);
    }

    private void OnSuicide()
    {
        if (Player == null) return;
        Player.Character.Suicide();
    }
    
    private void OnJump(InputValue value)
    {
        if (Player == null) return;
        Player.Character.movementController.jumping = value.isPressed;
    }
    
    #endregion
    
    public void SetupForCurrentPhase()
    {
        playerGui.SetupForCurrentPhase(Player);
    }
    

}
