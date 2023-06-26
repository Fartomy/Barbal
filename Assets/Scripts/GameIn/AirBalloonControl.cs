using UnityEngine;

public class AirBalloonControl : MonoBehaviour
{
//******* Public Values Zone ******//
    public float        randomSpeedMax;
    public GameObject   airBlEffect;

//******* Private Values Zone ******//
    private PlayerController    playerCtrl;
    private float               rotateSpeed = 0.7f;
    private float               torkUpSpeed = 2;
    private Rigidbody           airRb;

    void Start()
    {
        playerCtrl = FindObjectOfType<PlayerController>();
        airRb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        MoveUp();
    }

    void MoveUp() 
    {
        float speed = Random.Range(1, randomSpeedMax);
        airRb.AddForceAtPosition(Vector3.up * speed, transform.position + (transform.up * torkUpSpeed));
        transform.Rotate(0, rotateSpeed, 0);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && playerCtrl.isLikeIron)
        {
            playerCtrl.CapacityVolumeIncrease();
            GameObject _airBlEffect = Instantiate(airBlEffect, transform.position, transform.rotation);
            Destroy(_airBlEffect, 1.5f);
            Destroy(gameObject);
        }
        if (collision.gameObject.CompareTag("Barbedwall")) 
        {
            GameObject _airBlEffect = Instantiate(airBlEffect, transform.position, transform.rotation);
            Destroy(_airBlEffect, 1.5f);
            if(playerCtrl && playerCtrl.isLikeIron == false)
                playerCtrl.CapacityVolumeIncrease();
            Destroy(gameObject);
        }
        if (collision.gameObject.CompareTag("Visibleupwall"))
            Destroy(gameObject);
        if (collision.gameObject.CompareTag("Enemy"))
        {
            GameObject _airBlEffect = Instantiate(airBlEffect, transform.position, transform.rotation);
            Destroy(_airBlEffect, 1.5f);
            Destroy(gameObject);
        }
    }
}
