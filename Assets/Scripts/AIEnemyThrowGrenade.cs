using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class AIEnemyThrowGrenade : MonoBehaviour, StartGame
{
    private GameController gameController;
    [SerializeField] private Animator playerAnim;
    public float Rotation;
    [SerializeField] private Transform relesePos;
    [SerializeField] private Transform releseFrom;
    [SerializeField] private GameObject[] grenadePrefabs;
    public float strength = 13;
    private Rigidbody grenadeRB;
    private GameObject grenade;
    private Vector3 v;
    [SerializeField] private Transform playerTransform;
    private float s;

    public float coolDown = 6.5f;
    [NonSerialized] public float lastTimeCast = -10;

    public bool isThrowingGrenade = false;
    // Start is called before the first frame update
    void Awake()
    {
        gameController = GameController.Instance;
        int grenadeLayer = grenadePrefabs[0].layer;
        for (int i = 0; i < 32; i++)
        {
            if (Physics.GetIgnoreLayerCollision(grenadeLayer, i))
            {
                grenadeLayer |= 1 << i;
            }
        }
        if (playerAnim == null)
        {
            playerAnim = GetComponentInChildren<Animator>();
        }
        v = relesePos.position - releseFrom.position;
    }

    public IEnumerator StartGame()
    {
        InvokeRepeating("callAutoFire", 2 + gameController.timeToStar, 10);
        yield return null;
    }

    public void callAutoFire()
    {
        StartCoroutine(autoFire());
    }

    IEnumerator autoFire()
    {
        isThrowingGrenade = true;
        float delay = UnityEngine.Random.Range(0.65f, 3.0f);
        //float delay = grenadePrefabs[0].GetComponent<Grenade>().timeToExpode + s/(v.z*strength)-0.5f + UnityEngine.Random.Range(-0.5f,-s / (v.z * strength));
        Debug.Log(delay);
        yield return new WaitForSeconds(delay);
        StartCoroutine(Fire());
        isThrowingGrenade = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameController.isGamePlaying) return;
        if (isThrowingGrenade)
        {
            Vector3 lookDir = (playerTransform.position - transform.position).normalized;
            if (lookDir.z < 0) Rotation = -Mathf.Asin(lookDir.x) + 180 - 45;
            else Rotation = Mathf.Asin(lookDir.x);
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, Rotation / Mathf.PI * 180, transform.eulerAngles.z);

            if (playerAnim.GetInteger("WeaponType_int") != 10)
            {
                Debug.Log("wtf bro");
                playerAnim.SetFloat("Speed_f", 0);
                playerAnim.SetInteger("WeaponType_int", 10);
                StartCoroutine(StartAnimThrow());
            }
        }
        

        s = Vector3.Distance(playerTransform.position, transform.position);
        float t = Mathf.Sqrt(Mathf.Abs(2 * (s + 1) * v.y / (-Physics.gravity.y * v.z)));

        strength = Mathf.Abs((s + releseFrom.position.z - transform.position.z) * Mathf.Sqrt(v.z * v.z + v.y * v.y) / (v.z * t));

        if (grenade != null)
        {
            grenade.transform.position = releseFrom.position;
        }
    }
    private float throwStartTime;

    IEnumerator StartAnimThrow()
    {
        Debug.Log("StartAnimThrow");

        throwStartTime = Time.time;
        yield return new WaitForSeconds(0.65f);
        SpawnGrenade();
        if (playerAnim.GetInteger("WeaponType_int") == 10)
            playerAnim.speed = 0;

    }
    public void SpawnGrenade()
    {
        Debug.Log(MethodBase.GetCurrentMethod().Name);
        grenade = Instantiate(grenadePrefabs[UnityEngine.Random.Range(0, grenadePrefabs.Length)], releseFrom.position, grenadePrefabs[UnityEngine.Random.Range(0, grenadePrefabs.Length)].transform.rotation);
        grenadeRB = grenade.GetComponent<Rigidbody>();
        grenade.GetComponent<Collider>().enabled = false;
        grenadeRB.useGravity = false;
    }

    public IEnumerator Fire()
    {
        if (gameController.isGamePlaying)
        {
            if (throwStartTime + 0.65f > Time.time)
            {
                yield return new WaitForSeconds(throwStartTime + 0.65f - Time.time);
            }
            grenadeRB.useGravity = true;
            grenade.GetComponent<Collider>().enabled = true;
            grenade.transform.SetParent(null);

            grenadeRB.AddForce((relesePos.position - releseFrom.position).normalized * strength, ForceMode.Impulse);
            grenadeRB.AddTorque(UnityEngine.Random.Range(-10, 10), UnityEngine.Random.Range(-10, 10), UnityEngine.Random.Range(-10, 10), ForceMode.Impulse);
            playerAnim.speed = 1;
            grenade = null;
            grenadeRB = null;
            playerAnim.SetInteger("WeaponType_int", 0);
        }
    }
}
