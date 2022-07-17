using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : MonoBehaviour
{
    public Rigidbody rbody;

    [SerializeField] float forwardAccel, reverseAccel, maxSpeed, turnStrength, gravityForce = 10f, dragOnGround = 3f;

    private float speedInput, turnInput, tempForwadValue;

    private bool grounded;

    public LayerMask whatIsGround;
    public float groundRayLength = .5f;
    public Transform groundRayPoint;

    [Header("SidewaysForce")]
   // float sideVelocity;
    [SerializeField] float sideForce;
    [Header("pressed once")]
    [SerializeField] float sideModifier;//when side is pressed once
   

    float originalSpeed;
                                                                                      
    [Header("wheels")]
    [SerializeField] GameObject wheel1, wheel2;


    Vector3 playerDirection;

    //player stuff
    Transform player;
    float playerForwardAccel;


    //check sideways distance of bike traveled in one touch
    bool chase;
   

    float interval;
    float nextInterval;
    bool continueChasing;
    bool increaseSpeed;
    bool startRandomNess;
    
    
      

    // Start is called before the first frame update
    void Start()
    {
        startRandomNess = false;
        continueChasing = false;
        interval = Random.Range(0,6);
        forwardAccel = Random.Range(6,10);
        originalSpeed = forwardAccel;

        chase = false;
       
        rbody.transform.parent = null;

        player = GameObject.FindGameObjectWithTag("Player").gameObject.transform;
        playerForwardAccel = player.GetComponent<BikeController>().forwardAccel;

        StartCoroutine(DelayChase());//delay chase
    }

    // Update is called once per frame
    void Update()
    {
        speedInput =forwardAccel * 1000f;          //forward forceValue
       
        if (grounded &&  startRandomNess)
        {
            //chasing player condition
            var distance = (player.position - transform.position).magnitude;
            if (distance < 20 && continueChasing)
            {
                chase = true;
            }
            else
                chase = false;
           
            IncreaseSpeed();
        }
        transform.position = rbody.transform.position; 
        
    }

    private void FixedUpdate()
    {
        
        grounded = false;
        RaycastHit hit;
        if (Physics.Raycast(groundRayPoint.position, -transform.up, out hit, groundRayLength, whatIsGround))
        {
            //print(hit.transform.name);
            grounded = true;
            transform.rotation *= Quaternion.FromToRotation(transform.up, hit.normal);
        }
        if (grounded)
        {
            rbody.drag = dragOnGround;
            if (Mathf.Abs(speedInput) > 0 && !chase)
            {
                rbody.AddForce(transform.forward * speedInput);
            }

            if (chase && startRandomNess)
                rbody.AddForce(sideForce * playerDirection * 100);  // move towardsplayer
        }
        else
        {
            rbody.drag = .1f;
            rbody.AddForce(Vector3.up * -gravityForce * 300f);//adding extra gravity
        }
    }   


    public void GetDirection(Vector3 direction,bool visible)
    {        
        playerDirection = direction;
    }


  
    void IncreaseSpeed()
    {
        var distance = (player.position.z- transform.position.z);                         //make the ai move faster if it falls behind
        var sign = Mathf.Sign(distance);
        if(Time.time>nextInterval && sign>0)
        {
            var temp = Random.Range(3, 6);
            nextInterval = Time.time + interval;    //random intervals
            interval = temp;


            var tempValue = Random.Range(0, 2);         //randChasing state
            if (tempValue == 0)
            {
                continueChasing = false;
            }
            else
                continueChasing = true;

            //increase or decrease speed on random interval
            var rand = Random.Range(0, 2);
            if (rand == 1)
            {
                increaseSpeed = true;
            }
            else
                increaseSpeed = false;

            if (distance > 60 && increaseSpeed)
            {
                forwardAccel = playerForwardAccel + Random.Range(4,8);
            }

            else  if(distance>30 && distance<60 && increaseSpeed)
            {
                forwardAccel = playerForwardAccel + Random.Range(-2, 2);
            }

            else if (distance < 30)
            {
                forwardAccel = 9;
            }
        }

        if(Mathf.Abs(distance)>40 && sign<0)
        {
            forwardAccel =playerForwardAccel -Random.Range(1,4);
        }
       
    }

    private void OnTriggerEnter(Collider other)   //player gameobject diable and bike explosion effect
    {
        if(other.gameObject.CompareTag("Player"))
        {
            Instantiate(GeneralVariable.instance.bikeExplosion, other.transform.position, Quaternion.identity);
            other.gameObject.SetActive(false);
            gameObject.SetActive(false);
        }
    }

    IEnumerator DelayChase()
    {
        yield return new WaitForSeconds(4f);
        startRandomNess = true;
    }
}
