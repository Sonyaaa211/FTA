using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    [SerializeField] ParticleSystem explosionParticle;
    private GameController gameController;
    private CameraFollow cam;
    public float timeToExpode = 5;
    public float parent = 0;
    public float damageExposion = 50;
    public float range = 10;
    private Collider[] playerInrange;
    [SerializeField] LayerMask playerLayer;
    // Start is called before the first frame update
    void Awake()
    {
        Invoke("DeleteSelf", timeToExpode);
        gameController = GameController.Instance.GetComponent<GameController>();
        cam = GameObject.Find("Main Camera").GetComponent<CameraFollow>();
        gameController.nadePos.Enqueue(gameObject.transform);
        playerInrange = new Collider[1];
    }

    void DeleteSelf()
    {
        int number = Physics.OverlapSphereNonAlloc(transform.position, range, playerInrange, playerLayer);
        for(int i = 0; i < number; i++)
        {
            
            int damage = Mathf.RoundToInt((range-Vector3.Distance(transform.position, playerInrange[i].transform.position))/range *damageExposion);
            Debug.Log(damage);
            playerInrange[i].gameObject.GetComponent<State>().UpdateHealth(damage);
        }
        Debug.Log(MethodBase.GetCurrentMethod().Name);
        if(parent != 0)
        gameController.PointerUp();
        CameraShaker.Invoke();
        gameController.explosionSFX.Play();
        gameController.nadePos.Dequeue();
        Destroy(this.gameObject);
        
        Instantiate(explosionParticle, transform.position, explosionParticle.transform.rotation);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        
    }
}
