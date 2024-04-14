using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CammeraController : MonoBehaviour
{
    Transform player;
    public float cameraHeight = 10f;
    Vector3 cameraSpeed;
    public float dampSpeed = 1f;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 targetPosition = player.position + Vector3.up * cameraHeight;

        //transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime);

        transform.position = Vector3.SmoothDamp(targetPosition, targetPosition, ref cameraSpeed, dampSpeed);

    }
}
