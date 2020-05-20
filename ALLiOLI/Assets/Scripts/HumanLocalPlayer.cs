using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class HumanLocalPlayer : MonoBehaviour
{
    public int id
    {
        get
        {
            if (_id == 0)
            {
                _id = UnityEngine.Random.Range(0, int.MaxValue);
            }
            return _id;
        }
        private set { _id = value; }
    }

    private int _id = 0;

    /// <summary>
    /// Gui that displays the information that is only useful to the player itself.
    /// </summary>
    [SerializeField] public PlayerGuiManager playerGui;

    //public static readonly List<HumanLocalPlayer> inputsWaitingForPlayers = new List<HumanLocalPlayer>();

    public Player Player
    {
        get => _player;
        set
        {
            if (_player != null && value != null)
                Debug.LogWarning(
                    "Trying to assign a player to a HumanLocalPlayer with a Player already assigned. Operation Cancelled.",
                    this);
            else
            {
                _player = value;
            }

            SetDynamicName();
        }
    }

    // ReSharper disable once InconsistentNaming
    private Player _player;

    public PlayerInput PlayerInput
    {
        get
        {
            if (_playerInput == null)
            {
                _playerInput = gameObject.GetComponentRequired<PlayerInput>();
            }

            return _playerInput;
        }
        private set => _playerInput = value;
    }

    // ReSharper disable once InconsistentNaming
    private PlayerInput _playerInput;


    public CmCamera Camera
    {
        get
        {
            if (_camera == null)
            {
                _camera = PlayerInput.camera.gameObject.GetComponentRequired<CmCamera>();
                _camera.HumanLocalPlayer = this;
                _camera.SetLayer(localPlayerNumber + 10, PlayerInput.camera);
            }

            return _camera;
        }
        private set => _camera = value;
    }

    // ReSharper disable once InconsistentNaming
    private CmCamera _camera;
    
    public Vector2 cameraMovement;

    /// <summary>
    /// The maximum distance at which a trap can be to the character so the player can interact with it. // TODO: ensure if the distance id from the character or the camera
    /// </summary>
    [Space] [SerializeField] private float maxDistanceToInteractWithTrap = 7;
    
    [SerializeField] private LayerMask layersThatCanInterfereWithInteractions;

    private Trap trapInFront
    {
        get => _trapInFront;
        set
        {
            
            //if (value != _trapInFront)
                playerGui.ShowInteractionText(value != null &&
                                          (MatchManager.Instance.CurrentPhase is TrapUp ||
                                           MatchManager.Instance.CurrentPhase is FinishingTrapUp));
            _trapInFront = value;
        }
    }

    private Trap _trapInFront;
    private GameObject lastObjectInFront;
    public int localPlayerNumber;

    public TrapManager ownedTraps { get; private set; }
    public int maxOwnableTraps => 50 / GameManager.TotalPlayers;

    private void Awake()
    {
        //inputsWaitingForPlayers.Add(this);
        ownedTraps = new TrapManager();
        // SetDynamicName();
    }

    private void SetDynamicName()
    {
        string newName;
        if (Player == null)
            newName = "HumanLocalPlayer " + id + " of Unknown Player - ERROR";
        else
        {
            newName = "HumanLocalPlayer " + id + " of " + Player.name;
            transform.SetParent(Player.transform);
        }

        gameObject.name = newName;
    }

    private void Update()
    {
        if (Player == null)
            return;

        if (Player.Character != null)
            //TODO: highlight the 'trapInFront'
            UpdateObjectsInFront();

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
        Ray ray = new Ray(Player.Character.interactionRayOrigin.position, Camera.transform.forward);
        Debug.DrawRay(ray.origin, ray.direction, Color.green);
        if (Physics.Raycast(ray, out RaycastHit hit, maxDistanceToInteractWithTrap, layersThatCanInterfereWithInteractions))
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
        DebugPro.LogEnumerable(ownedTraps, ", ", "The current owned traps for the player " + gameObject.name + " are: ",
            gameObject);
    }

    #region input

    public void DisablePlayerInputDuring(float time)
    {
        CancelInvoke(nameof(EnablePlayerInput));
        if (time <= 0)
        {
            EnablePlayerInput();
            return;
        }
        PlayerInput.DeactivateInput();
        Invoke(nameof(EnablePlayerInput), time);
    }

    public void EnablePlayerInput()
    {
        PlayerInput.ActivateInput();
        CancelInvoke(nameof(EnablePlayerInput));
    }

    private void OnCameraMove(InputValue value)
    {
        if (Player == null) return;
        cameraMovement = value.Get<Vector2>();
    }

    private void OnCharacterMove(InputValue value)
    {
        if (Player == null) return;

        Player.Character.movementController.horizontalMovementInput = value.Get<Vector2>();
    }

    private void OnTrap()
    {
        if (Player == null) return;

        State currentState = MatchManager.Instance.CurrentPhase;

        switch (currentState)
        {
            case Battle battle:
                Trap bestTrapToActivate = ownedTraps.GetBestTrapToActivate(Player);
                if (bestTrapToActivate != null)
                    Player.CmdActivateTrap(bestTrapToActivate.netId);
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
    
    #if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            foreach (Trap trap in ownedTraps)
            {
                Vector3 characterPosition = Player.Character.transform.position;
                Vector3 trapPos = trap.transform.position;
                //float halfHeight = (characterPosition.y-trapPos.y)*0.5f;
                //Vector3 offset = Vector3.up * halfHeight;
             
                Handles.DrawBezier(
                    characterPosition, trapPos, 
                    characterPosition + Vector3.up, trap.transform.position + trap.transform.forward + Vector3.up, 
                    Player.Color, EditorGUIUtility.whiteTexture, 1f);
            }
        }
    #endif
}