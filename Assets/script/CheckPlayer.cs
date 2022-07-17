using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPlayer : MonoBehaviour
{
    [SerializeField] float noOfRays;
    float angle;

    [SerializeField] float range;
    public Vector3 playerDirection;

    AI ai;
    // Start is called before the first frame update
    void Start()
    {
        angle = 180 / noOfRays;
        ai = transform.root.GetComponent<AI>();
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        for(int i=0;i<noOfRays;i++)
        {
            var direction = Quaternion.Euler(0, angle * i, 0) * transform.forward;
            Ray ray = new Ray(transform.position, direction);
            if(Physics.Raycast(ray,out hit,range))
            {
                Debug.DrawLine(transform.position, hit.point, Color.green);

                if(hit.transform.CompareTag("Player"))     // player
                {
                    playerDirection = (hit.transform.position - transform.root.position);
                    var distance = playerDirection.magnitude;
                    playerDirection = playerDirection.normalized;
                  

                    ai.GetDirection(playerDirection,true);
                }


                if(hit.transform.CompareTag("AI"))
                {
                    var distance = (hit.transform.position - transform.root.position).magnitude;
                    //print(distance);
                    if(distance<4)
                    {
                        //ai.Avoidance(distance);
                    }
                }
               
            }
            
        }
    }
}
