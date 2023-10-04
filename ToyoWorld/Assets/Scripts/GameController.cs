using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState { FreeRoam, Battle }

public class GameController : MonoBehaviour
{
    public static GameController instance;

    [SerializeField] PlayerSpawner spawner;
    public SceneDataSaver sceneDataSaver;
    public SceneDataLoader sceneDataLoader;
    public Transform spawnPoint;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        sceneDataSaver = GetComponent<SceneDataSaver>();
        sceneDataLoader = GetComponent<SceneDataLoader>();
    }

    [SerializeField] BattleSystem battleSystem;
    public GameObject player;
    public PartyReference playerParty;
    public ToyoParty currentToyoParty;
    public ToyoParty storedToyoParty;
    [SerializeField] Toyo wildToyo;

    [SerializeField] public MapArea mapArea;

    private PlayerMovement playerMovement;

    public GameState state = GameState.FreeRoam;

    // Start is called before the first frame update
    void Start()
    {
        //UpdatePlayerParty(currentToyoParty);
    }

    // Update is called once per frame
    void Update()
    {
        if(state == GameState.FreeRoam)
        {
            if(player == null)
            {
                player = FindAnyObjectByType<PlayerMovement>().gameObject;
                if(playerParty.partyMembers.Count <= 0)
                {
                    currentToyoParty = player.GetComponent<ToyoParty>();
                    UpdatePlayerParty(currentToyoParty);
                }

            }
        }
        if(state == GameState.Battle)
        {
            //battleSystem.HandleUpdate();
            //Debug.Log("In battle");
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        Debug.Log("OnDisable");
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("OnSceneLoaded: " + scene.name);
        Debug.Log(mode);
        if(scene.name == "PlayerScene")
        {
            Debug.Log("OnSceneLoaded: " + scene.name);
            
            //UpdatePlayerParty(currentToyoParty);
            instance.mapArea = FindAnyObjectByType<MapArea>();
            spawner = FindAnyObjectByType<PlayerSpawner>();
            state = GameState.FreeRoam;
        }

    }

    // Example method to update the player's party information
    public void UpdatePlayerParty(ToyoParty newParty)
    {
        playerParty.partyMembers = newParty.ToyoPartyList;
    }

    public List<Toyo> GetStoredToyoPartyList()
    {
        return playerParty.partyMembers;
    }

    public void SwitchToBattleScene()
    {
        SceneManager.LoadScene("MainBattle");
    }

    public void SwitchToPlayScene()
    {
        SceneManager.LoadScene("PlayerScene");
        //InitController();
    }

    public void CallBattleStartMethod()
    {
        //UpdatePlayerParty(currentToyoParty);
        sceneDataSaver.SaveSceneData();
        sceneDataLoader.LoadSceneData();
        state = GameState.Battle;
        SwitchToBattleScene();
    }

    public void EndBattle() 
    {
        SwitchToPlayScene();
        sceneDataLoader.LoadSceneData();
        state = GameState.FreeRoam;
    }

    public void SetPlayer()
    {
        player = FindAnyObjectByType<PlayerMovement>().gameObject;
    }
}
