using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerMovement : MonoBehaviour, StartGame, Skill
{
    public Transform spawn;
    private GameController gameController;
    private Animator playerAnim;
    public NavMeshAgent agent;
    [SerializeField] private MeshTrail mt;
    [SerializeField] private Joystick joystick;
    public float speed;
    private Rigidbody playerRB;
    // Start is called before the first frame update
    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        gameController = GameController.Instance.GetComponent<GameController>();
        playerRB = GetComponent<Rigidbody>();
        if (playerAnim == null)
        {
            playerAnim = GetComponentInChildren<Animator>();
        }
        //playerAnim.SetInteger("Animation_int", 0);
        //playerAnim.SetFloat("Speed_f", speed);
        //Invoke("StartTransition", 0.1f);
    }

    public IEnumerator StartGame()
    {
        playerAnim.SetInteger("Animation_int", 0);
        playerAnim.SetFloat("Speed_f", speed);
        GameObject.Find("Virtual Camera 01").SetActive(false);
        yield return null;
    }

    void StartTransition()
    {
        GameObject.Find("Virtual Camera 01").SetActive(false);
    }
    public float lastTimeSkill = -100;
    public void Skill(GameObject coolDown)
    {
        if (Time.time - lastTimeSkill < 20) return;
        lastTimeSkill = Time.time;
        coolDown.SetActive(true);
        speed += 5;
        Invoke("DeSkill", 4);
    }
    
    public void DeSkill()
    {
        speed -= 5;
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameController.isGamePlaying) return;
        if(joystick.joystickVec != Vector2.zero && !gameController.isThrowingGrenade)
        {
            playerRB.velocity = new Vector3(joystick.joystickVec.x, 0, joystick.joystickVec.y) * speed;
            playerAnim.SetFloat("Speed_f", speed);
        }
        else
        {
            playerRB.velocity = Vector3.zero;
            playerAnim.SetFloat("Speed_f", 0);
        }
    }
}
