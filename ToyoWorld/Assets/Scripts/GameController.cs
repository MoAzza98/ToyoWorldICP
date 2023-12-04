using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using Unity.VisualScripting;

public enum GameState { FreeRoam, Battle }

public class GameController : MonoBehaviour
{
    public static GameController instance;

    public SceneDataSaver sceneDataSaver;
    public SceneDataLoader sceneDataLoader;
    public Transform spawnPoint;
    public int toyosDefeated = 0;
    public bool lostBattle;
    [HideInInspector] public bool setName = false;

    private void Awake()
    {
        ConditionsDB.Init();
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

    public GameObject player;
    public PartyReference playerParty;
    public ToyoParty currentToyoParty;
    public ToyoParty storedToyoParty;
    [SerializeField] public Toyo wildToyo;
    [SerializeField] ToyoParty rewardToyo;

    public ToyoParty gcParty;
    bool partyInitialized;

    [SerializeField] public MapArea mapArea;
    [SerializeField] public AudioClip bossMusic;
    public AudioSource levelAudio;

    public TransitionAnimation transition;

    [HideInInspector]
    public int lastLevel = 0;
    public int level = 0;
    public int lastWorldScene = 2;


    public GameState state = GameState.FreeRoam;

    // Start is called before the first frame update
    void Start()
    {
        transition = FindObjectOfType<TransitionAnimation>();
    }

    // Update is called once per frame
    void Update()
    {
        if (lostBattle)
        {
            
        }

        if (state == GameState.FreeRoam)
        {       
            if (player == null)
            {
                try
                {
                    player = FindAnyObjectByType<ThirdPersonMovement>().gameObject;
                    currentToyoParty = player.GetComponent<ToyoParty>();
                    UpdateControllerParty(currentToyoParty);
                } catch(Exception e)
                {
                    Debug.Log(e);
                }

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
        lastLevel = level;
        level = scene.buildIndex;
        try
        {
            levelAudio = FindObjectOfType<AudioSource>();
        } catch(Exception e)
        {
            Debug.Log(e);
        }

        transition = FindObjectOfType<TransitionAnimation>();
        Debug.Log(mode);

        if(scene.name == "PlayerScene" || scene.name == "Holidays Scene")
        {
            lastWorldScene = scene.buildIndex;
        }

        if(scene.name == "PlayerScene")
        {
            Debug.Log("OnSceneLoaded: " + scene.name);
            
            //UpdatePlayerParty(currentToyoParty);
            instance.mapArea = FindAnyObjectByType<MapArea>();
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
        //index 1
        StartCoroutine(transition.LoadLevel(2));
        transition = FindObjectOfType<TransitionAnimation>();
    }

    public void SwitchToBossBattleScene()
    {
        //index 1
        StartCoroutine(transition.LoadLevel(3));
        transition = FindObjectOfType<TransitionAnimation>();
    }

    public void SwitchToPlayScene()
    {
        //index 2
        StartCoroutine(transition.LoadLevel(lastWorldScene));
        transition = FindObjectOfType<TransitionAnimation>();
        Debug.Log($"Swapping to scene {lastWorldScene}, Level var is {level}, Lastlevel is {lastWorldScene}");
        //SceneManager.LoadScene("PlayerScene");
        //InitController();
    }

    public void CallBattleStartMethod(Toyo toyo)
    {
        wildToyo = toyo;
        //wildToyo.Init();
        //UpdatePlayerParty(currentToyoParty);
        sceneDataSaver.SaveSceneData();
        sceneDataLoader.LoadSceneData();
        state = GameState.Battle;
        SwitchToBattleScene();
    }

    public void CallBossBattleStartMethod(Toyo toyo)
    {
        wildToyo = toyo;
        //wildToyo.Init();
        //UpdatePlayerParty(currentToyoParty);
        sceneDataSaver.SaveSceneData();
        sceneDataLoader.LoadSceneData();
        state = GameState.Battle;
        SwitchToBossBattleScene();
    }

    public void EndBattle() 
    {
        SwitchToPlayScene();
        //Debug.Log($"Swapping to scene {lastLevel}");
        sceneDataLoader.LoadSceneData();
        state = GameState.FreeRoam;
    }

    public void SetPlayer()
    {
        player = FindAnyObjectByType<ThirdPersonMovement>().gameObject;
    }

    public void AddToyoReward()
    {
        foreach(var toyo in rewardToyo.ToyoPartyList)
        {
            gcParty.ToyoPartyList.Add(toyo);
        }
    }
}
