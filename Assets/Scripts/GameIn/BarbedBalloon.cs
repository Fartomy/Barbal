using UnityEngine;

public class BarbedBalloon : MonoBehaviour
{
//*********** Public Values Zone ************//
    public float randomSpeedMax;

//*********** Private Values Zone **********//
    private Rigidbody barbedRb;
    private float torkUpSpeed = 2;
    private float rotateSpeed = 0.7f;

    void Start()
    {
        barbedRb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        MoveUp();
    }

    void MoveUp()
    {
        float speed = Random.Range(1, randomSpeedMax);
        barbedRb.AddForceAtPosition(Vector3.up * speed, transform.position + (transform.up * torkUpSpeed));
        transform.Rotate(0, rotateSpeed, 0);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Visibleupwall"))
            Destroy(gameObject);
    }
}
