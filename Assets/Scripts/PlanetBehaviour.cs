using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetBehaviour : MonoBehaviour
{
    private Manager manager;
    private bool _gActive, _isDynamic;
    // gActive = if planet is confirmed to be added yet or not
    // isDynamic = if planet is supposed to move or not
    private Rigidbody rb;
    // rigidbody of planet
    public bool gActive // gActive getter and setter
    {
        get
        {
            return _gActive;
        }

        set
        {
            _gActive = value;
        }
    }
    public bool isDynamic // isDynamic getter and setter
    {
        get
        {
            return _isDynamic;
        }

        set
        {
            _isDynamic = value;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        manager = GameObject.Find("Manager").GetComponent<Manager>();
        rb = GetComponent<Rigidbody>();
        gActive = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.timeScale != 0 && gActive && isDynamic)
        {
            // add force to each planets if it is active and dynamic and not paused
            Vector2 totalForce = Vector2.zero;

            foreach(GameObject gObject in manager.planets)
            {
                Rigidbody gObjectRb = gObject.GetComponent<Rigidbody>();
                PlanetBehaviour planetBehaviour = gObject.GetComponent<PlanetBehaviour>();

                if(gObject != gameObject && planetBehaviour.gActive)
                {
                    float distanceSquare = (gObject.transform.position - transform.position).sqrMagnitude;

                    if(distanceSquare < 0.000001f)
                    {
                        distanceSquare = 0.000001f;
                    }

                    float forceMagnitude = manager.G * rb.mass * gObjectRb.mass / distanceSquare;
                    totalForce += (Vector2)(gObject.transform.position - transform.position).normalized * forceMagnitude;
                }
            }

            rb.AddForce(totalForce);
            // end add force to each planets if it is active and dynamic and not paused
        }
    }
}
