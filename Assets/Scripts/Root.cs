using Audio;
using CombatSystem;
using Creatures;
using Database;
using Factories;
using ShopSystem;
using UI;
using UnityEngine;

[RequireComponent(typeof(ItemDatabase))]
[RequireComponent(typeof(EnemyCreatureDatabase))]
[RequireComponent(typeof(UpgradeDatabase))]
[RequireComponent(typeof(UIManager))]
public class Root : MonoBehaviour
{
    public static Root Instance { get; private set; }
    
    [Header("Player object")]
    [SerializeField] private CharacterData playerData;
    public Player Player;
    
    [Header("Shop settings")]
    [SerializeField] private int shopShownItems;
    [Tooltip("Time in seconds")] [SerializeField] private float shopRefreshTime;

    public ItemDatabase ItemDatabase { get; private set; }
    public EnemyCreatureDatabase EnemyCreatureDatabase { get; private set; }
    public UpgradeDatabase UpgradeDatabase { get; private set; }

    public CombatManager CombatManager { get; private set; }

    public UIManager UIManager { get; private set; }
    public AudioManager AudioManager { get; private set; }
    public Shop ShopManager { get; private set; }

    public bool TicksActive { get; set; } = true;

    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }

        ItemDatabase = GetComponent<ItemDatabase>();
        EnemyCreatureDatabase = GetComponent<EnemyCreatureDatabase>();
        UpgradeDatabase = GetComponent<UpgradeDatabase>();
        UIManager = GetComponent<UIManager>();
        
        Initialise();
    }

    private void Initialise()
    {
        ItemDatabase.Initialise();
        EnemyCreatureDatabase.Initialise();
        UpgradeDatabase.Initialise();

        CombatManager = new CombatManager();
        CombatManager.Initialise();
        
        Player = (Player)CharacterFactory.Create(playerData);
        Player.Initialise();

        ShopManager = new Shop(shopShownItems, shopRefreshTime);
        ShopManager.Initialise();
        
        UIManager.Initialise();
    }

    private void Start()
    {
        CombatManager.Start(); // Spawn enemies and populate ui with them
    }

    private void Update()
    {
        ShopManager.Update();

        if (TicksActive)
        {
            CombatManager.Tick();
        }
    }
}