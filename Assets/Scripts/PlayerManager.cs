using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviourPunCallbacks
{
    float SnakeMoveSpeed = 5;
    public float SnakeTurnSpeed = 150;
    float TailFollowSpeed = 5;
    int TailSpawnGap = 5;
    Touch touch;
    protected void SnakeHeadMovement()
    {
        transform.position += transform.forward * SnakeMoveSpeed * Time.deltaTime;

        if (Input.touchCount > 0)
        {
            touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Moved)
            {
                transform.Rotate(Vector3.up * touch.deltaPosition.x * SnakeTurnSpeed / 10 * Time.deltaTime);
            }
        }
        float snakeDirection = Input.GetAxis("Horizontal");
        transform.Rotate(Vector3.up * snakeDirection * SnakeTurnSpeed * Time.deltaTime);

    }

    protected void UpdateSnakePosition(List<Vector3> SnakeTailPosition, List<GameObject> SnakeTail)
    {
        SnakeTailPosition.Insert(0, transform.position);

        int index = 0;
        foreach (var body in SnakeTail)
        {
            Vector3 point = SnakeTailPosition[Mathf.Clamp(index * TailSpawnGap, 0, SnakeTailPosition.Count - 1)];
            Vector3 moveDirection = point - body.transform.position;
            body.transform.position += moveDirection * TailFollowSpeed * Time.deltaTime;
            body.transform.LookAt(point);

            index++;
        }
    }
}
