using UnityEngine;

public class BonusBalloon : MonoBehaviour
{
//******* Public Values Zone ******//
    public GameObject   burstEfct;
    public float        randomSpeedMax;
    public bool         isTouchPlayer = false;
    public bool         isTouchBwall = false;

//******* Private Values Zone ******//
    private Rigidbody       bonusRb;
    private GameManager     gameMng;
    private const float     rotateSpeed = 0.7f;
    private const float     torkUpSpeed = 2f;

    void Start()
    {
        bonusRb = GetComponent<Rigidbody>();
        gameMng = FindObjectOfType<GameManager>();
    }
    
    void Update()
    {
        MoveUp();
    }

    void MoveUp()
    {
        float speed = Random.Range(1, randomSpeedMax);
        bonusRb.AddForceAtPosition(Vector3.up * speed, transform.position + (transform.up * torkUpSpeed));
        transform.Rotate(0, rotateSpeed, 0);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            isTouchPlayer = true;
        if (collision.gameObject.CompareTag("Barbedwall")) 
        {
            isTouchBwall = true;
            if (isTouchBwall && isTouchPlayer)
                gameMng.DefineFuncs();
            GameObject _burstEfct = Instantiate(burstEfct, transform.position, transform.rotation);
            Destroy(_burstEfct, 1.5f);
            Destroy(gameObject);
        }
        if (collision.gameObject.CompareTag("Enemy"))
        {
            GameObject _burstEfct = Instantiate(burstEfct, transform.position, transform.rotation);
            Destroy(_burstEfct, 1.5f);
            Destroy(gameObject);
        }
        if (collision.gameObject.CompareTag("Visibleupwall"))
            Destroy(gameObject);
    }
}
