using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class UserInput : Singleton<UserInput>
{
    private string[] _abilityActionNames = new string[]
    {
        "Primary Attack", // Index 0
        "Secondary Attack", // Index 1
        "Seal1", // Index 2
        "Seal2", // Index 3
        "Seal3", // Index 4
        "Seal4"  // Index 5
    };

//  GamepadIconCollection GamePadIcons;
    bool IsMenuOpened = false;
    public string CurrentControlScheme = "Keyboard";
    PlayerAbilityExecutor _abilityExecutor;

    public PlayerInput PlayerInput;
    public Vector2 MovementInput { get; private set; }
    public Vector2 LookInput { get; private set; }
    public bool Seal1Pressed { get; private set; }
    public bool Seal2Pressed { get; private set; }
    public bool Seal3Pressed { get; private set; }
    public bool Seal4Pressed { get; private set; }
    public bool InteractPressed { get; private set; }
    public bool SecondaryAttackPressed { get; private set; }
    public bool PrimaryAttackPressed { get; private set; }
    public bool MenuOpenClosePressed { get; private set; }
    public bool IsAttacking { get; private set; }
    public bool IsNextAttackSet { get; private set; }
    
    #region InputActions

    InputAction _interactAction;
    InputAction _moveAction;
    InputAction _lookAction;
    InputAction _primaryAttackAction;
    InputAction _secondaryAttackAction;
    InputAction _seal1Action;
    InputAction _seal2Action;
    InputAction _seal3Action;
    InputAction _seal4Action;
    InputAction _menuOpenCloseAction;

    #endregion

    bool _listenForInput = true;

    public bool IsMenuOpen()
    {
        return IsMenuOpened;
    }
    protected override void Awake()
    {
        KeepOnSceneLoad = false;
        base.Awake();
    }
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Initialize();
    }

    private void Initialize()
    {
        PlayerInput = GetComponent<PlayerInput>();
        var rebindings = ""; // SettingsManager.Instance.LoadRebindingSettings();

        if (!string.IsNullOrEmpty(rebindings))
            PlayerInput.actions.LoadBindingOverridesFromJson(rebindings);

        PlayerInput.onControlsChanged += OnControlsChanged;
        UIEvents.OnInGameMenuClose.AddListener(OnMenuClose);
        UIEvents.OnInGameMenuOpen.AddListener(OnMenuOpen);

        var player = EntityManager.Instance.Player;
        _abilityExecutor = player.GetComponent<PlayerAbilityExecutor>();
        SetupInputActions();
    }

    private void OnControlsChanged(PlayerInput input)
    {
        CurrentControlScheme = input.currentControlScheme;
    }

    private void OnDisable()
    {
        UIEvents.OnInGameMenuClose.RemoveListener(OnMenuClose);
        UIEvents.OnInGameMenuOpen.RemoveListener(OnMenuOpen);
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void SetupInputActions()
    {
        _interactAction = PlayerInput.actions["Interact"];
        _moveAction = PlayerInput.actions["Move"];
        _lookAction = PlayerInput.actions["Look"];
        _primaryAttackAction = PlayerInput.actions["Primary Attack"];
        _secondaryAttackAction = PlayerInput.actions["Secondary Attack"];
        _seal1Action = PlayerInput.actions["Seal1"];
        _seal2Action = PlayerInput.actions["Seal2"];
        _seal3Action = PlayerInput.actions["Seal3"];
        _seal4Action = PlayerInput.actions["Seal4"];
        _menuOpenCloseAction = PlayerInput.actions["MenuOpenClose"];
    }

    void Update()
    {
        UpdateInputs();
    }

    #region MenuEvents

    void OnMenuOpen()
    {
        IsMenuOpened = true;
        _listenForInput = false;
    }
    void OnMenuClose()
    {
        IsMenuOpened = false;
        _listenForInput = true;
    }

    #endregion

    private void UpdateInputs()
    {
        MenuOpenClosePressed = _menuOpenCloseAction.WasPressedThisFrame();
        if (!_listenForInput) return;

        MovementInput = _moveAction.ReadValue<Vector2>();
        LookInput = _lookAction.ReadValue<Vector2>();

        IsAttacking = _abilityExecutor.IsAttacking;
        IsNextAttackSet = CheckForAttack();
        PrimaryAttackPressed = _primaryAttackAction.WasPressedThisFrame();
        SecondaryAttackPressed = _secondaryAttackAction.WasPressedThisFrame();

        Seal1Pressed = _seal1Action.WasPressedThisFrame();
        Seal2Pressed = _seal2Action.WasPressedThisFrame();
        Seal3Pressed = _seal3Action.WasPressedThisFrame();
        Seal4Pressed = _seal4Action.WasPressedThisFrame();

        InteractPressed = _interactAction.WasPressedThisFrame();
    }

    private bool CheckForAttack()
    {
        if (_abilityExecutor.CanUseQueuedAbility())
            return true;
        else if (_abilityExecutor.CheckForAbilityPressed())
            return true;

        return false;
    }

    public InputAction GetInputActionForAbilityIndex(int abilityIndex)
    {
        if (abilityIndex < 0 || abilityIndex >= _abilityActionNames.Length)
        {
            Debug.LogError("Invalid ability index!");
            return null;
        }

        string actionName = _abilityActionNames[abilityIndex];
        InputAction action = PlayerInput.actions[actionName];
        return action;
    }

    //public Sprite GetSymbolForAbilityIndex(int index, string controlScheme)
    //{
    //    if (GamePadIcons == null)
    //        GamePadIcons = GetComponent<GamepadIconCollection>();

    //    if (GamePadIcons == null) Debug.Log("GamepadIconCollection not found");


    //    var inputAction = GetInputActionForAbilityIndex(index);
    //    string deviceLayoutName;
    //    string controlPath;

    //    int bindingIndex = controlScheme != "Keyboard" ? 1 : 0;
    //    var displayString = inputAction.GetBindingDisplayString(bindingIndex, out deviceLayoutName, out controlPath, DisplayStringOptions.DontOmitDevice);


    //    if (controlScheme != "Keyboard")
    //    {
    //        return GamePadIcons.GetControlSprite(deviceLayoutName, controlPath);
    //    }
    //    else
    //    {
    //        return GamePadIcons.KeyboardIcon;
    //    }
    //}
    public string GetKeybindForAbilityIndex(int index)
    {
        var inputAction = GetInputActionForAbilityIndex(index);
        return inputAction.bindings[0].effectivePath.Split("/").Last().ToUpper();
    }
}