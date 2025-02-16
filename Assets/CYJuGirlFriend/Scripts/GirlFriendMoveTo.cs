﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GirlFriendMoveTo : MonoBehaviour
{    
    // Transforms to act as start and end markers for the journey.
    public Transform startMarker;
    public Vector3 endMarker;

    // Movement speed in units/sec.
    private float speed = 0.1F;

    // Time when the movement started.
    private float startTime;

    // Total distance between the markers.
    private float journeyLength;

    private Animator GirlFriendAnim;

    void Start()
    {
        journeyLength = 0;
        GirlFriendAnim = GetComponent<Animator>();
    }

    // Follows the target position like with a spring
    void Update()
    {
        if(journeyLength>0)
        { 
            // Distance moved = time * speed.
            float distCovered = (Time.time - startTime) * speed;

            // Fraction of journey completed = current distance divided by total distance.
            float fracJourney = distCovered / journeyLength;

            // Set our position as a fraction of the distance between the markers.
            transform.position = Vector3.Lerp(startMarker.position, endMarker, fracJourney);

            if (fracJourney < 0.1)
            {
                var lookPos = endMarker - transform.position;
                lookPos.y = 0;
                var rotation = Quaternion.LookRotation(lookPos);
                transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 10f);
            }
        }
        if(Vector3.Distance(startMarker.position, endMarker)<0.1)
        {
            GirlFriendAnim.SetBool("IsJump", true);
            GirlFriendAnim.SetBool("IsWalking", false);
        }
    }

    public void StartMove(Vector3 endPos)
    {
        GirlFriendAnim.SetBool("IsJump", false);
        GirlFriendAnim.SetBool("IsWalking", true);
        startMarker = this.transform;
        endMarker = endPos;

        // Keep a note of the time the movement started.
        startTime = Time.time;

        // Calculate the journey length.
        journeyLength = Vector3.Distance(startMarker.position, endMarker);
    }
}
