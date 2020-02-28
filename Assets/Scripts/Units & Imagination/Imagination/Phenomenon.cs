using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Phenomenon : Imagination
{
    /// <summary>
    /// Whether the phenomenon should spin.
    /// </summary>
    public bool shouldSpin = false;

    /// <summary>
    /// Angle spinned over time.
    /// </summary>
    private float spinRate;

    /// <summary>
    /// Angle spinned over 1 second.
    /// </summary>
    public float spinAnglePerSec;

    /// <summary>
    /// Axis of spin.
    /// </summary>
    public Vector3 spinAxis;

    void Start()
    {
        type = IMAGINATION_TYPE.Phenomenon;
        spinRate = spinAnglePerSec * Time.deltaTime;
    }

    void Update()
    {
    }

    private void FixedUpdate()
    {
        if(shouldSpin)
            Spin();
    }

    void Spin()
    {
        transform.Rotate(spinAxis, spinRate);
    }

    void OnCollisionEnter(Collision collision)
    {
        
    }
}
