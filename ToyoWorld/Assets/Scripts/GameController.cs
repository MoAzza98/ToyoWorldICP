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
        gcParty = GetComponent<ToyoParty>();
    }

    [SerializeField] BattleSystem battleSystem;
    public GameObject player;
    public PartyReference playerParty;
    public ToyoParty currentToyoParty;
    public ToyoParty storedToyoParty;
    [SerializeField] Toyo wildToyo;

    public ToyoParty gcParty;
    bool partyInitialized;
    bool mouseLocked = true;

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
        if (state == GameState.FreeRoam)
        {       
            if (player == null)
            {
                player = FindAnyObjectByType<ThirdPersonMovement>().gameObject;
                currentToyoParty = player.GetComponent<ToyoParty>();
                UpdateControllerParty(currentToyoParty);

                if(!partyInitialized)
                {
                    foreach(var member in currentToyoParty.ToyoPartyList)
                    {
                        gcParty.ToyoPartyList.Add(member);
                    }
                    partyInitialized = true;
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
    public void UpdateControllerParty(ToyoParty newParty)
    {
        playerParty.partyMembers = newParty.ToyoPartyList;
        //gcParty = newParty;
    }

    public List<Toyo> GetStoredToyoPartyList()
    {
        return playerParty.partyMembers;
    }

    public ToyoParty GetStoredToyoParty()
    {
        return gcParty;
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
        player = FindAnyObjectByType<ThirdPersonMovement>().gameObject;
    }
}
