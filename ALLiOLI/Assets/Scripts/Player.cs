using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class Player : MonoBehaviour
{
    private Color _color;
    private bool _isReady;
    private Trap _trapInFront;

    [Space] [SerializeField] private GameObject characterPrefab;

    private GameObject lastObjectInFront;
    [SerializeField] private LayerMask layersThatCanInterfereWithInteractions;

    [Space] [SerializeField] private float maxDistanceToInteractWithTrap = 10;

    [Space] [SerializeField] private PlayerGuiManager playerGui;

    private PlayerInput playerInput;
    public Character character { get; private set; }
    public TrapManager ownedTraps { get; private set; }
    public int maxOwnableTraps => 50 / GameManager.singleton.totalPlayers;

    private Trap trapInFront
    {
        get => _trapInFront;
        set
        {
            _trapInFront = value;
            playerGui.ShowInteractionText(value != null &&
                                          (MatchManager.Instance.currentPhase is TrapUp ||
                                           MatchManager.Instance.currentPhase is FinishingTrapUp));
        }
    }

    public ThirdPersonCamera playerCamera { get; private set; }

    public Color color
    {
        get => _color;
        private set
        {
            _color = value;
            playerGui.SetColor(_color);
        }
    }

    public bool isReady
    {
        get => _isReady;
        set
        {
            _isReady = value;
            playerGui.ShowReadiness(isReady);
        }
    }

    public void Setup(Color color)
    {
        ownedTraps = new TrapManager();

        playerInput = GetComponent<PlayerInput>();
        playerCamera = playerInput.camera.gameObject.GetComponent<ThirdPersonCamera>();
        this.color = color;
        gameObject.name = "Player " + playerInput.playerIndex + " - " + playerInput.user.controlScheme;

        SpawnNewCharacter();
    }

    private void Update()
    {
        UpdateObjectsInFront();
        //TODO: highlight the 'trapInFront'
        UpdateRadarTraps();
    }

    private void UpdateRadarTraps()
    {
        List<KeyValuePair<Trap, SortedList<float, Character>>> radarReport =
            ownedTraps.GetCharactersInEachTrapRadar(this);
        playerGui.ReportInRadar(radarReport);
    }

    private void UpdateObjectsInFront()
    {
        Ray ray = new Ray(character.cameraTarget.position, playerCamera.transform.forward);
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

    public void SpawnNewCharacter()
    {
        character = Spawner.Instance.Spawn(characterPrefab).GetComponent<Character>();
        character.owner = this;
        playerCamera.Setup(character.cameraTarget);
    }

    private void SetUpTrapInFront()
    {
        if (trapInFront == null)
            return;

        if (!ownedTraps.Remove(trapInFront))
            ownedTraps.Add(trapInFront);

        playerGui.ShowNumberOfTraps(ownedTraps.Count, maxOwnableTraps);
        DebugPro.LogEnumerable(ownedTraps, ", ", "The current owned traps for the player " + gameObject.name + " are: ",
            gameObject);
    }

    public void SetupForCurrentPhase()
    {
        playerGui.SetupForCurrentPhase(this);
    }

    #region Input

    private void OnCameraMove(InputValue value)
    {
        playerCamera.movement = value.Get<Vector2>();
    }

    private void OnCharacterMove(InputValue value)
    {
        character.movementControllerController.horizontalMovementInput = value.Get<Vector2>();
    }

    private void OnTrap()
    {
        State currentState = MatchManager.Instance.currentPhase;

        switch (currentState)
        {
            case Battle battle:
                ownedTraps.GetBestTrapToActivate(this)?.Activate();
                break;
            case TrapUp trapUp:
                SetUpTrapInFront();
                break;
        }
    }

    private void OnReady()
    {
        isReady = !isReady;
    }

    private void OnSuicide()
    {
        character.Suicide();
    }

    private void OnJump(InputValue value)
    {
        character.movementControllerController.jumping = value.isPressed;
    }

    #endregion
}