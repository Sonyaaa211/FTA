using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State : MonoBehaviour, Skill
{
    [SerializeField] private GameObject shield;
    private bool isFirstTime = true;
    private int health;
    public int maxHealth;
    public HealthBar healthBar;
    private GameController gameController;
    private bool isProtected = false;
    private float lastTimeProtect = -30;
    public State enemyState;
    public GameObject VC2;
    public GameObject VC3;
    public GameObject Tut;
    // Start is called before the first frame update
    void Start()
    {
        gameController = GameController.Instance.GetComponent<GameController>();
        healthBar = GetComponentInChildren<HealthBar>();
        healthBar.SetMaxHealth(maxHealth);
        health = 0;
    }

    // Update is called once per frame
    void Update()
    {
            if (Tut.activeSelf && !isProtected && isFirstTime)
            {
                isProtected = true;
                shield.SetActive(true);
            }
    }

    public void Skill(GameObject coolDown)
    {
        if (Time.time - lastTimeProtect < 30) return;
        lastTimeProtect = Time.time;
        coolDown.SetActive(true);
        isProtected = true;
        shield.SetActive(true);
        Invoke("DeSkill", 5);
    }

    public void DeSkill ()
    {
        isProtected = false;
        shield.SetActive(false);
        isFirstTime = false;
    }

    public void UpdateHealth(int damage)
    {
        if (isProtected) return;
        health += damage;
        healthBar.SetHealth(health);
        if (health > maxHealth)
        {
            isProtected = true;
            enemyState.isProtected = true;
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            enemyState.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            VC2.SetActive(true);
            VC3.SetActive(true);
            VC2.SetActive(false);
            if (GetComponent<PlayerController>() != null) {
                gameController.Lose();
                GetComponentInChildren<Animator>().SetBool("Death_b", true);
                enemyState.gameObject.GetComponentInChildren<Animator>().SetInteger("Speed_f", 0);
                enemyState.gameObject.GetComponentInChildren<Animator>().SetInteger("Animation_int", UnityEngine.Random.Range(1, 7));
            }
            else
            {
                gameController.Win();
                GetComponentInChildren<Animator>().SetInteger("Speed_f", 0);
                GetComponentInChildren<Animator>().SetInteger("Animation_int", UnityEngine.Random.Range(1, 7));
                enemyState.gameObject.GetComponentInChildren<Animator>().SetInteger("DeathType_int", UnityEngine.Random.Range(0, 3));
                enemyState.gameObject.GetComponentInChildren<Animator>().SetBool("Death_b", true);
            }
        }
    }
}
