using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BikeController : MonoBehaviour
{
    public Rigidbody rbody;

    public float forwardAccel, reverseAccel, maxSpeed, turnStrength,gravityForce=10f,dragOnGround=3f;

    private float speedInput, turnInput,tempForwadValue;

    private bool grounded;

    public LayerMask whatIsGround;
    public float groundRayLength=.5f;
    public Transform groundRayPoint;

    [Header("SidewaysForce")]
    float sideVelocity;
    [SerializeField] float sideForce;
    [Header("pressed once")]
    [SerializeField] float sideModifier;//when side is pressed once
    float sign;

    [Header("wheels")]
    [SerializeField] GameObject wheel1, wheel2;


    //check sideways distance of bike traveled in one touch
    bool movingSide;
    //Vector3 movingPos;
    float xPos;

    //on still pos bike will explode
    [SerializeField] float timeTillExplosion;
    float timer;


    //popwer ups
    bool powerUpsActive;
    float powerTimer;
    [SerializeField] float powerUpSpeed;


    float groundCheckTimer;
    [SerializeField] float fallTimer;


    //score
    float score;
    float _Timer;
    [SerializeField] float scoreIncreement;
    [SerializeField] TextMeshProUGUI highScore;


    [SerializeField] GameObject lvlCompletionBG;
    // Start is called before the first frame update
    void Start()
    {
        lvlCompletionBG.gameObject.SetActive(false);
        score = 0;
        _Timer = 0;
        groundCheckTimer = 0;
        powerTimer = 0;
        powerUpsActive = false;
        timer = 0; 
        xPos = transform.position.x;
        movingSide = false;
        sideVelocity = 0;
        rbody.transform.parent = null;
    }

    // Update is called once per frame
    void Update()
    {
        tempForwadValue = Input.GetAxis("Vertical");      

        speedInput = 0;
        if (Input.GetAxis("Vertical") > 0)
        {           
            speedInput = tempForwadValue* forwardAccel * 1000f;

        }
        else if ((Input.GetAxis("Vertical") < 0))
        {
            speedInput = tempForwadValue * reverseAccel * 1000f;
        }

        turnInput = Input.GetAxis("Horizontal");
        

        if (grounded)
        {
            //transform.rotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y + (turnInput * turnStrength * Time.deltaTime * Input.GetAxis("Vertical")), transform.eulerAngles.z);
            sideVelocity = Input.GetAxis("Horizontal") * 50;
        }
        transform.position = rbody.transform.position;

       
        //check if it moving side;
        MovingSide();

        //check if the bike  is moving
        CheckIfTheBikeIsStill();

        //power ups timer
        if(powerUpsActive)
        {
            powerTimer += 1 * Time.deltaTime;
            if(powerTimer<3)
            {
                forwardAccel = powerUpSpeed;
            }
            else
            {
                powerTimer = 0;
                powerUpsActive = false;
            }
        }

        CheckDistance();


        ///  score timer
        _Timer += 1 * Time.deltaTime;

        //Sound
        if(rbody.velocity.z>.1f)
        {
           // AudioManager.instance.Play("bikeaccel");
        }
    }

    private void FixedUpdate()   
    {
        grounded = false;
        RaycastHit hit;
        if(Physics.Raycast(groundRayPoint.position,-transform.up,out hit, groundRayLength,whatIsGround))
        {
            //print(hit.transform.name);
            grounded = true; 
            transform.rotation *= Quaternion.FromToRotation(transform.up,hit.normal);
        }
        if (grounded && Mathf.Abs (tempForwadValue)>0)
        {
            rbody.drag = dragOnGround;
            if (Mathf.Abs(speedInput) > 0)
            {
                rbody.AddForce(transform.forward * speedInput);
            }
            rbody.AddForce(sideForce*transform.right*sideVelocity);    //constant side movement
        }

        else
        {
            rbody.drag = .1f;
            rbody.AddForce(Vector3.up * -gravityForce * 300f);//adding extra gravity
        }

        //move side when Key side key pressed once
        if (movingSide && grounded && Mathf.Abs(tempForwadValue) > 0)
        {
            rbody.AddForce(sideForce * transform.right * sideModifier * sign);

            //now check the limit of distanceit is supposed to move iin side direction
            if(Mathf.Abs(xPos-transform.position.x)>4)
            {
                movingSide = false;
            }
        }        
    }

    void MovingSide()
    {
        if (Input.GetKeyDown(KeyCode.A))  //pressed once
        {
            xPos = transform.position.x;
            sign = -1;
            movingSide = true;
        }

        else if (Input.GetKeyDown(KeyCode.D))
        {
            xPos = transform.position.x;
            sign = 1;
            movingSide = true;
        }
    }
   


    void CheckIfTheBikeIsStill()
    {
        if (Mathf.Abs( rbody.velocity.z)<.1f)
        {

            timer += 1 * Time.deltaTime;
            
            if (timer > timeTillExplosion)
            {
                BikeExplposion();
            }
        }
        else
        {
            timer = 0;
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("powerups"))
        {
            powerUpsActive = true;
            other.gameObject.SetActive(false);
        }

        if(other.gameObject.CompareTag("finishline"))
        {
            Score();
            lvlCompletionBG.SetActive(true);
        }
    }

    void CheckDistance()
    {
        if(!grounded )
        {
            groundCheckTimer += 1 * Time.deltaTime;
            if (groundCheckTimer > fallTimer)
            {               
                BikeExplposion();
            }
        }
        else
        {
            groundCheckTimer = 0;
        }
    }

    void BikeExplposion()
    {
        Instantiate(GeneralVariable.instance.bikeExplosion, transform.position, Quaternion.identity);
        gameObject.SetActive(false);
    }


    void Score()
    {
        PauseMenu.instance.TurnOnPanel();
        score += scoreIncreement *(25 / _Timer);
        var temp = (int)score;
        highScore.text =temp.ToString();

    }
}
