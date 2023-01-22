using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class FoodMovement : MonoBehaviour
{
    public float speed;
    public Transform Center;
    void Update()
    {
        transform.RotateAround(Center.position, Vector3.up, speed * Time.deltaTime);

    }
}
