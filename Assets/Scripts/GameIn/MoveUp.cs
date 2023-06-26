using UnityEngine;

public class MoveUp : MonoBehaviour
{
//*********** Public Values Zone ************//
    public GameObject           burstEfct;
    public bool                 isTouchPlayer = false;
    public bool                 isTouchBwall = false;
    public bool                 isTouchPuff;
    public float                speed;

//*********** Private Values Zone **********//
    private float               rotateSpeed = 0.7f;
    private float               torkUpSpeed = 2f;
    private Rigidbody           scoreBlRb;
    private PlayerController    plCtrl;

    void Start()
    {
        scoreBlRb = GetComponent<Rigidbody>();
        plCtrl = FindObjectOfType<PlayerController>();
    }

    void Update()
    {
        scoreBlRb.AddForceAtPosition(Vector3.up * speed, transform.position + (transform.up * torkUpSpeed));
        transform.Rotate(0, rotateSpeed, 0);
    }

    void Score()
    {
        if (!plCtrl)
            return;
        if (isTouchBwall && (isTouchPlayer || plCtrl.isPuffTouchCtrl))
            GameManager.score++;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if(plCtrl.isLikeIron)
            {
                GameManager.score++;
                GameObject _burstEfct = Instantiate(burstEfct, transform.position, transform.rotation);
                Destroy(_burstEfct, 1.5f);
                Destroy(gameObject);
            }
            else
                isTouchPlayer = true;
        }
        if (collision.gameObject.CompareTag("Barbedwall"))
        {
            isTouchBwall = true;
            Score();
            GameObject _burstEfct = Instantiate(burstEfct, transform.position, transform.rotation);
            Destroy(_burstEfct, 1.5f);
            Destroy(gameObject);
        }
        if (collision.gameObject.CompareTag("Visibleupwall"))
            Destroy(gameObject);
        if (collision.gameObject.CompareTag("Enemy")) 
        {
            GameObject _burstEfct = Instantiate(burstEfct, transform.position, transform.rotation);
            Destroy(_burstEfct, 1.5f);
            Destroy(gameObject);
        }  
    }
}
