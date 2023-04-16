using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxisSpin : MonoBehaviour
{
    [SerializeField] float speedx, speedy, speedz; // our spin speed

    private void FixedUpdate()
    {
        transform.eulerAngles = new Vector3(
            transform.eulerAngles.x + speedx * Time.fixedDeltaTime,
            transform.eulerAngles.y + speedy * Time.fixedDeltaTime,
            transform.eulerAngles.z + speedz * Time.fixedDeltaTime);
    }
}
