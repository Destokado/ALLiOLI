using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;
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

    public Vector2 CameraMovement
    {
        get => !HasControl? Vector2.zero : _cameraMovement;
        private set => _cameraMovement = value;
    }
    // ReSharper disable once InconsistentNaming
    private Vector2 _cameraMovement;
    
    public int localPlayerNumber;
    
    private bool HasControl => !GameManager.Instance.PauseMenuShowing && MatchManager.instance.currentPhase.allowMovementAndCameraRotation;

    private void SetDynamicName()
    {
        string newName;
        if (Player == null)
            newName = "HumanLocalPlayer " + id + " of Unknown Player - ERROR";
        else
        {
            newName = "HumanLocalPlayer " + id + " of " + Player.name;
            transform.SetParent(Player.transform, true);
        }

        gameObject.name = newName;
    }

    private void Update()
    {
        if (Player == null)
            return;

        UpdateRadarTraps();
    }

    private void UpdateRadarTraps()
    {
        List<KeyValuePair<Trap, SortedList<float, Character>>> radarReport =
            MatchManager.instance.AllTraps.GetCharactersInEachTrapRadar(Player);
        playerGui.ReportInRadar(radarReport);
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
        if (Player == null || Player.Character == null) return; // Maybe not necessary
        CameraMovement = value.Get<Vector2>();
    }

    private void OnCharacterMove(InputValue value)
    {
        if (Player == null || Player.Character == null) return;

        Player.Character.movementController.horizontalMovementInput = HasControl ? value.Get<Vector2>() : Vector2.zero;
    }

    private void OnTrap()
    {
        if (Player == null) return;

        State currentState = MatchManager.instance.currentPhase;

        switch (currentState)
        {
            case Battle battle:
                Trap bestTrapToActivate = MatchManager.instance.AllTraps.GetBestTrapToActivate(Player);
                if (bestTrapToActivate != null)
                    Player.CmdActivateTrap(bestTrapToActivate.netId);
                break;
        }
    }

    private void OnReady()
    {
        if (Player == null || Player.Character == null) return;
        Player.CmdSetReady(!Player.isReady);
    }

    private void OnSuicide()
    {
        if (Player == null || Player.Character == null) return;
        Player.Character.CmdServerSuicide();
    }

    private void OnJump(InputValue value)
    {
        if (Player == null || Player.Character == null) return;
        Player.Character.movementController.jumping = value.isPressed;
    }
    
    private void OnPause()
    {
        GameManager.Instance.PauseButtonPressed();
    }

    #endregion

    public void SetupForCurrentPhase()
    {
        playerGui.SetupForCurrentPhase(Player);
    }

}