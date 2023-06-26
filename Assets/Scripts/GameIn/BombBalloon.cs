using UnityEngine;
using EZCameraShake;

public class BombBalloon : MonoBehaviour
{
//******* Public Values Zone ******//
    public float randomSpeedMax;
    public float explosionForce;

    public GameObject bombEffect;

//******* Private Values Zone ******//
    private Rigidbody bombRb;

    private float torkUpSpeed = 2;
    private float rotateSpeed = 0.7f;
    private float radius = 250;

    void Start()
    {
        bombRb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        MoveUp();
    }

    void MoveUp()
    {
       float speed = Random.Range(1, randomSpeedMax);
       bombRb.AddForceAtPosition(Vector3.up * speed, transform.position + (transform.up * torkUpSpeed));
       transform.Rotate(0, rotateSpeed, 0);
    }

    void Boom() 
    {
        GameObject _bombEffect = Instantiate(bombEffect, transform.position, transform.rotation);
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);

        foreach (Collider nearbyObjects in colliders)
        {
            Rigidbody rb = nearbyObjects.GetComponent<Rigidbody>();
            if (rb != null) 
            {
                if (transform.position.x < -2)
                {
                    rb.AddForce(explosionForce, 0, 0, ForceMode.VelocityChange);
                }
                else
                    rb.AddForce(-explosionForce, 0, 0, ForceMode.VelocityChange);
            }
        }
        Destroy(_bombEffect, 2f);
        Destroy(gameObject);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Visibleupwall"))
            Destroy(gameObject);
        if (collision.gameObject.CompareTag("Barbedwall"))
        {
            CameraShaker.Instance.ShakeOnce(4f, 4f, .1f, 1f);
            Boom();
        }
    }
}
