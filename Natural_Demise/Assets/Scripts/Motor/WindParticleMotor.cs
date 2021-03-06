﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Motor for the wind particles
public class WindParticleMotor : BaseMotor {
    private float _timer = 0.0f;
    private int _initialDirection;
    
    protected override void Construct() {
        Mass = 0.05f;//Random.Range(0.03f, 0.09f);
        _initialDirection = (Random.Range(0, 2) * 2 - 1); //Basically, if its original direction is left or right. 
    }

    //Set the state for the wind particles
    protected override void SetState() {
        state = gameObject.AddComponent<FlyingState>();
        controller.detectCollisions = false;
    }

    protected override void UpdateMotor() {
        _timer += Time.deltaTime;

        //Get input
        MoveVector = WindDirection;

        //Send input to a filter 
        MoveVector = state.ProcessMotion(MoveVector);

        //Add sideways motion to the particles
        MoveVector += Quaternion.AngleAxis(90, Vector3.up) * MoveVector * Mathf.Sin(_timer) / 3 * _initialDirection;

        //RotationQuaternion = state.ProcessRotation(MoveVector);
        //Check if we should change current state
        state.Transition();
        
        //Move 
        Move();

        //Rotate
        Rotate();

        //Check if an object is grounded, used together with rays to debug
        //Grounded();
    }

    //Override the move function so that the wind particles aren't effected by other objects' colliders
    protected override void Move() {
        transform.position += MoveVector * Time.deltaTime;
    }

    public override bool Grounded() { return true; }

}
