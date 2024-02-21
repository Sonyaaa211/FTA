using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using System;
using UnityEngine.EventSystems;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.AI;
using BrokenVector.LowPolyFencePack;

public interface StartGame
{
    IEnumerator StartGame();
}
public enum Curency { Diamond, Coin };
public class GameController : Singleton<GameController>, StartGame
{
    
    public bool isIntroScreen = true;
    public bool isHomeScreen;
    public bool isThrowingGrenade = false;
    public bool isGamePlaying = false;
    [SerializeField] private GameObject mousePos;
    [SerializeField] private GameObject countdownAttack;
    [SerializeField] private GameObject door;
    [SerializeField] private GameObject showTutPos;
    public  AudioSource explosionSFX;
    private PlayerController playerController;
    private PlayerMovement playerMovement;
    private ThrowGrenade throwGrenade;


    [SerializeField] private Joystick joystick;
    [SerializeField] private GameObject gameplayCanvas;
    [SerializeField] private GameObject winUI;
    [SerializeField] private GameObject loseUI;
    [SerializeField] private GameObject homeCanvas;

    private float preClickTime;
    private float posClickTime;

    public NavMeshAgent enemyAgent;
    public Queue<Transform> nadePos = new Queue<Transform>();

    public float timeToStar = 0.5f;

    public PlayerInfor playerInfor;
    // Start is called before the first frame update
    void Awake()
    {
        Application.targetFrameRate = 300;
        if (mousePos == null) { 
            mousePos = GameObject.Find("Mouse Position");
        }
        playerMovement = GameObject.Find("Player").GetComponent<PlayerMovement>();
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        throwGrenade = GameObject.Find("Player").GetComponent<ThrowGrenade>();
        gameplayCanvas.SetActive(false);
        
    }

    public void callStart()
    {
        
        StartCoroutine(GameObject.Find("Enemy").GetComponent<AIEnemyThrowGrenade>().StartGame());
        
        StartCoroutine(StartGame());
        StartCoroutine(playerMovement.StartGame());
    }
    public IEnumerator StartGame()
    {
        playerMovement.agent.SetDestination(playerMovement.spawn.position);
        yield return new WaitForSeconds(timeToStar);
        isGamePlaying = true;
        gameplayCanvas.SetActive(true);
        
        GameObject.Find("Virtual Camera 02").SetActive(false);
        GameObject.Find("Player").GetComponent<PlayerMovement>().agent.Stop();
        door.GetComponent<DoorController>().CloseDoor();
        door.GetComponent<Collider>().isTrigger = false;
        yield return new WaitForSeconds(2);
        showTutPos.SetActive(false);
    }

    public void callStartIntro()
    {
        StartCoroutine(StartIntro());
    }
    
    public IEnumerator StartIntro()
    {
        GameObject.Find("Virtual Camera 00").SetActive(false);
        isIntroScreen = false;
        yield return new WaitForSeconds(2);
        homeCanvas.SetActive(true);
        
    }
    public void ChangeState()
    {
        isThrowingGrenade = !isThrowingGrenade;
        Debug.Log(MethodBase.GetCurrentMethod().Name);
    }
    float clickdelay = 0.3f;

    public void PointerDown()
    {
        if (isIntroScreen)
        {
            Debug.Log("Start Intro");
            StartCoroutine(StartIntro());
        }
        preClickTime = posClickTime;
        posClickTime = Time.time;

        if (posClickTime - preClickTime < clickdelay && posClickTime - throwGrenade.lastTimeCast > throwGrenade.coolDown)
        {
            Debug.Log(MethodBase.GetCurrentMethod().Name);
            throwGrenade.lastTimeCast = posClickTime;
            mousePos.SetActive(true);
            countdownAttack.SetActive(true);
            mousePos.transform.position = throwGrenade.transform.position + throwGrenade.transform.forward * 3 - new Vector3(0, 1, 0);
            ChangeState();
        }

    }

    public void PointerUp()
    {
        if(isThrowingGrenade)
        {
            Debug.Log(MethodBase.GetCurrentMethod().Name);
            StartCoroutine(throwGrenade.Fire());
            isThrowingGrenade = false;
            joystick.joystickRotation = throwGrenade.Rotation;
            throwGrenade.canDrawProj = false;
        }
        mousePos.SetActive(false);
    }

    public void Win()
    {
        isGamePlaying = false;
        gameplayCanvas.SetActive(false);
        winUI.SetActive(true);
        playerInfor.coin += 500;
        playerInfor.level++;
    }

    public void Lose()
    {
        isGamePlaying = false;
        gameplayCanvas.SetActive(false);
        loseUI.SetActive(true);
    }

    public void Restart(bool x2)
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void PauseGame()
    {
        isGamePlaying = false;
        enemyAgent.Stop();
    }

    public void ContinueGame()
    {
        isGamePlaying = true;
        enemyAgent.Resume();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
