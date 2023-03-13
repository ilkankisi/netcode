using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public Transform playerPos;
    public Vector3 offsetVector;
    [SerializeField] float cameraSpeed = 0.125f;
    // Start is called before the first frame update
    void Start()
    {
        //playerPos = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        //offsetVector = OffsetVectorCalculate(playerPos);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        MoveTheCamera();
    }

    private void MoveTheCamera()
    {
        if (playerPos != null)
        {
            Vector3 targetToMove = playerPos.position + offsetVector;
            transform.position = Vector3.Lerp(transform.position, targetToMove, cameraSpeed);

        }

    }
}
