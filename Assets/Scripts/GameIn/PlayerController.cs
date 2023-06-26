using UnityEngine;

public class PlayerController : MonoBehaviour
{
//******* Public Values Zone ******//
    public float                airCapacity = 100f;
    public float                airLosing;
    public float                airTake;
    public float                speedHorizontalCtrl;
    public float                speedVerticalCtrl;
    public float                airforceSpeed;
    public bool                 puffCtrl = false;
    public bool                 isPuffTouchCtrl = false;
    public bool                 isLikeIron = false;
    public bool                 isPlayerLive = true;
    public int                  puffRandomNbr;
    public GameObject           burstEffect;
    public AudioSource[]        audioSfxs;
    public ParticleSystem[]     partEffects;
    public Material             playerMat;
    public VariableJoystick     varJoystick;

//******* Private Values Zone ******//
    private bool                isTouchAirBl = false;
    private Rigidbody           playerRb;
    private AudioSource         playerMiniBurst;
    private Vector3             scaleChange;
    private Vector3             leftPos = new Vector3(2, -0.5f, 0);
    private Vector3             rightPos = new Vector3(-2, -0.5f, 0);
    private Vector3             boxSize = new Vector3(2, 0.5f, 2);

    void Start()
    {
        scaleChange = new Vector3(airLosing, airLosing, airLosing);
        playerRb = GetComponent<Rigidbody>();
        audioSfxs = GetComponentsInChildren<AudioSource>();
        playerMiniBurst = GetComponent<AudioSource>();
        playerMiniBurst = GameObject.Find("Chain").GetComponent<AudioSource>();
        playerMat = GetComponent<Renderer>().material;
    }

    void Update()
    {
        CapacityVolumeDecrease();
        BurstBalloon();
        MovementBorders();
    }

    void FixedUpdate()
    {
        if(Input.GetButton("Fire1"))
            MovePlayerTouch();
        if(puffCtrl && puffRandomNbr == 1)
            PuffLeft();
        if(puffCtrl && puffRandomNbr == 2)
            PuffRight();
    }

    /*void OnDrawGizmos() // Special Gizmoz function that I drew because I could not see the cube field in the OverlopBox function
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position - leftPos, boxSize);
        Gizmos.DrawWireCube(transform.position - rightPos, boxSize);
    }*/

    void PuffLeft()
    {
        Collider[] cols = Physics.OverlapBox(transform.position - leftPos, boxSize, Quaternion.identity, LayerMask.GetMask("Enemies")); // We get the Collider of the objects in a certain area (cube). With Layermask, it affects the objects on the layer we specify.
        foreach (Collider nearbyObj in cols)
        {
            Rigidbody otherObjRbs = nearbyObj.GetComponent<Rigidbody>();
            if (otherObjRbs != null)
            {
                otherObjRbs.AddForceAtPosition(Vector3.left * airforceSpeed, Vector3.left); // We add a force to the objects in the area we specified.
                isPuffTouchCtrl = true;
            }
        }
    }

    void PuffRight()
    {
        Collider[] cols = Physics.OverlapBox(transform.position - rightPos, boxSize, Quaternion.identity, LayerMask.GetMask("Enemies"));
        foreach (Collider nearbyObj in cols)
        {
            Rigidbody otherObjRbs = nearbyObj.GetComponent<Rigidbody>();
            if (otherObjRbs != null)
            {
                otherObjRbs.AddForceAtPosition(Vector3.right * airforceSpeed, Vector3.right);
                isPuffTouchCtrl = true;
            }
        }
    }

    void BurstBalloon() 
    {
        if (transform.localScale.x <= 0.25f || airCapacity <= 0.0f)
        {
            airCapacity = 0f;
            playerMiniBurst.PlayOneShot(playerMiniBurst.clip);
            isPlayerLive = false;
            Destroy(gameObject);
        }
    }

    public void CapacityVolumeIncrease() 
    {
        if (isTouchAirBl || isPuffTouchCtrl || isLikeIron)
        {
            partEffects[0].Play();
            audioSfxs[0].PlayOneShot(audioSfxs[0].clip);
            airCapacity += airTake;
            transform.localScale += scaleChange * airTake;
            isTouchAirBl = false;
        }
    }

    void CapacityVolumeDecrease() 
    {
        if (airCapacity > 0f && isPuffTouchCtrl == false)
        {
            airCapacity -= Time.deltaTime;
            transform.localScale -= scaleChange * Time.deltaTime;
        }
        else if(airCapacity > 0f && isPuffTouchCtrl)
        {
            airCapacity -= Time.deltaTime * 10;
            transform.localScale -= (scaleChange * 10) * Time.deltaTime;
        }
    }

    void MovementBorders()
    {
        if (transform.position.y < -10)
            transform.position = new Vector3(transform.position.x, -10, transform.position.z);
    }

    public void MovePlayerTouch()
    {
        float horizontal = varJoystick.Horizontal;
        float vertical = varJoystick.Vertical;

        playerRb.AddForce(Vector3.right * speedHorizontalCtrl * horizontal);
        playerRb.AddForce(Vector3.up * speedVerticalCtrl * vertical);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            if(isLikeIron == false)
            {
                GameObject _burstEffect = Instantiate(burstEffect, transform.position, transform.rotation);
                Destroy(_burstEffect, 1.5f);
                isPlayerLive = false;
                Destroy(gameObject);
            }
        }
        /*if (collision.gameObject.CompareTag("Barbedwall"))
        {
            GameObject _burstEffect = Instantiate(burstEffect, transform.position, transform.rotation);
            Destroy(_burstEffect, 1.5f);
            isPlayerLive = false;
            Destroy(gameObject);
        }*/
        if (collision.gameObject.CompareTag("Airballoon"))
            isTouchAirBl = true;
    }
}
