using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;


public interface Skill
{
    public void Skill(GameObject coolDown);

    public void DeSkill();
}
public class ThrowGrenade : MonoBehaviour, Skill
{
    private GameController gameController;
    private Animator playerAnim;

    //public bool isThrowingGrenade = true;
    public float Rotation;

    [SerializeField] private Camera _camera;
    [SerializeField] GameObject[] grenadePrefabs;
    [SerializeField] private LayerMask grenadeLayer;
    [SerializeField] private float strength = 13;
    [SerializeField] public LineRenderer lineRenderer;
    [SerializeField] private Transform relesePos;
    [SerializeField] private Transform releseFrom;
    private Rigidbody grenadeRB;
    private GameObject grenade;
    private Collider[] incomingGrenade;
    private Vector3 v;
    [Header("Display Controls")]
    [SerializeField]
    [Range(10, 100)] private int linePoints = 25;
    [SerializeField]
    [Range(0.01f, 0.25f)] private float timeBetweenPoints = 0.1f;
    public bool canDrawProj = false;

    [SerializeField] Transform mousePos;
    private float s;

    public float coolDown = 6.5f;
    [NonSerialized] public float lastTimeCast = -10;
    private float lastTimeRA;

    private LayerMask grenadeCollisionMask;
    // Start is called before the first frame update

    private bool isUltimate = false;
    private float lastTimeUlti = -30;
    void Awake()
    {
        gameController = GameController.Instance.GetComponent<GameController>();
        int grenadeLayer = grenadePrefabs[0].layer;
        for (int i = 0; i < 32; i++)
        {
            if(Physics.GetIgnoreLayerCollision(grenadeLayer, i))
            {
                grenadeLayer |= 1 << i;
            }
        }
        if(playerAnim == null)
        {
            playerAnim = GetComponentInChildren<Animator>();
        }
        v = relesePos.position - releseFrom.position;
        v = new Vector3(Mathf.Abs(v.x), Mathf.Abs(v.y), Mathf.Abs(v.z));
        incomingGrenade = new Collider[5];
    }

    // Update is called once per frame
    void Update()
    {
        if (gameController.isThrowingGrenade)
        {
            Vector3 lookDir = (mousePos.position - transform.position).normalized;
            if (lookDir.z < 0) Rotation = -Mathf.Asin(lookDir.x) + 180 - 45;
            else Rotation = Mathf.Asin(lookDir.x);

            transform.eulerAngles = new Vector3(transform.eulerAngles.x, Rotation / Mathf.PI * 180, transform.eulerAngles.z);
            DrawProJection();

            if (playerAnim.GetInteger("Animation_int") != 10)
            {
                Debug.Log(MethodBase.GetCurrentMethod().Name);
                playerAnim.SetInteger("Animation_int", 10);
                StartCoroutine(StartAnimThrow());
            }
            
        }
        s = Vector3.Distance(mousePos.position, transform.position - new Vector3(0, 1, 0));
        float t = Mathf.Sqrt(2 * (s + 1) * v.y / (-Physics.gravity.y * v.z));
        strength = (s + releseFrom.position.z - transform.position.z) * Mathf.Sqrt(v.z * v.z + v.y * v.y) / (v.z * t);
        if (canDrawProj)
        {
            

        }

        if(grenade != null)
        {
            grenade.transform.position = releseFrom.position;
        }

    }

    public void revertAttack(GameObject coolDown)
    {
        if (Time.time - lastTimeRA < 2.0f) return; 
        lastTimeRA = Time.time;
        coolDown.SetActive(true);
        int numberOfResult = Physics.OverlapSphereNonAlloc(transform.position, 3, incomingGrenade, grenadeLayer);
        if (numberOfResult > 0)
        {
            Debug.Log("has incoming nade");
            incomingGrenade[0].gameObject.GetComponent<Rigidbody>().AddForce((relesePos.position - releseFrom.position).normalized * 20, ForceMode.Impulse);
        }
    }

    public void Skill(GameObject coolDown)
    {
        if (Time.time - lastTimeUlti < 30) return;
        lastTimeUlti = Time.time;
        coolDown.SetActive(true);
        isUltimate = true;
    }

    public void DeSkill()
    {

    }

    private float throwStartTime;
    IEnumerator StartAnimThrow()
    {
        Debug.Log("StartAnimThrow");

        throwStartTime = Time.time;
        yield return new WaitForSeconds(0.65f);
        SpawnGrenade();
        if (playerAnim.GetInteger("Animation_int") == 10)
        playerAnim.speed = 0;
        
    }

    private void DrawProJection()
    {
        lineRenderer.enabled = true;
        lineRenderer.positionCount = Mathf.CeilToInt(linePoints/timeBetweenPoints) + 1;
        Vector3 startPos = releseFrom.position;
        Vector3 startVelocity = strength * (relesePos.position - releseFrom.position).normalized / grenadePrefabs[0].GetComponent<Rigidbody>().mass;
        int i = 0;
        lineRenderer.SetPosition(i, startPos);
        for(float time = 0;  time < linePoints; time += timeBetweenPoints)
        {
            i++;
            Vector3 point = startPos + time * startVelocity;
            point.y = startPos.y + startVelocity.y*time + (Physics.gravity.y / 2f * time * time);
            lineRenderer.SetPosition(i, point);

            Vector3 lastPos = lineRenderer.GetPosition(i - 1);

            if(Physics.Raycast(lastPos, (point - lastPos).normalized, out RaycastHit hit, (point- lastPos).magnitude, grenadeCollisionMask))
            {
                lineRenderer.SetPosition(i, hit.point);
                lineRenderer.positionCount = i + 1;
                return;
            }
        }
    }

    public void SpawnGrenade()
    {
        Debug.Log(MethodBase.GetCurrentMethod().Name);
        canDrawProj = true;
        grenade = Instantiate(grenadePrefabs[UnityEngine.Random.Range(0, grenadePrefabs.Length)], releseFrom.position, grenadePrefabs[UnityEngine.Random.Range(0, grenadePrefabs.Length)].transform.rotation);
        grenadeRB = grenade.GetComponent<Rigidbody>();
        grenade.GetComponent<Collider>().enabled = false;
        grenade.GetComponent<Grenade>().parent = 1;
        if (isUltimate)
        {
            grenade.GetComponent<Light>().enabled = true;
            grenade.GetComponent<Grenade>().range = 15;
            grenade.GetComponent<Grenade>().damageExposion = 100;
            isUltimate = false;
        }
        grenadeRB.useGravity = false;
        gameController.GetComponent<Timer>().StartTimer(grenade.GetComponent<Grenade>().timeToExpode);
    }

    public IEnumerator Fire()
    {
        Debug.Log("Fire");
        canDrawProj = false;
        yield return new WaitForSeconds(throwStartTime + 0.70f - Time.time);

        
        lineRenderer.enabled = false;
        grenadeRB.useGravity = true;
        grenade.GetComponent<Collider>().enabled = true;
        grenade.transform.SetParent(null);

        grenadeRB.AddForce((relesePos.position-releseFrom.position).normalized * strength, ForceMode.Impulse);
        grenadeRB.AddTorque(UnityEngine.Random.Range(-10, 10), UnityEngine.Random.Range(-10, 10), UnityEngine.Random.Range(-10, 10), ForceMode.Impulse);
        playerAnim.speed = 1;
        grenade = null;
        grenadeRB = null;
        playerAnim.SetInteger("Animation_int", 0);
    }
}
