using UnityEngine;

public class MainMenuBalSpawner : MonoBehaviour
{
    public Texture[] ballTextures;
    public GameObject balloon;
    public float spawnDelay;
    public float destroyTime;
    
    private float timer = 0f;
    private float balMoveUpSpeed;
    private Renderer balRend;
    private Material balColMat;
    private Rigidbody balRb;

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= spawnDelay)
        {
            GameObject _Balloon = Instantiate(balloon, new Vector3(Random.Range(-8, 8), -32, -3), Quaternion.identity);
            balRb = _Balloon.GetComponent<Rigidbody>();
            balRend = _Balloon.GetComponent<Renderer>();
            balColMat = balRend.materials[0];
            int Randnb = Random.Range(0, 5);
            balColMat.mainTexture = ballTextures[Randnb];
            Destroy(_Balloon, destroyTime);
            timer = 0f;
        }
        if (balRb)
        {
            balMoveUpSpeed = Random.Range(3f, 5f);
            balRb.AddForceAtPosition(Vector3.up * balMoveUpSpeed, transform.position + (transform.up * 2f));
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.rigidbody.useGravity == true)
            collision.rigidbody.useGravity = false;
    }
}
