using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicAsteroidController : MonoBehaviour
{
    GameObject player;
    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        //znajdz gracza na scenie
        player = GameObject.FindWithTag("Player");
        //obróæ siê przodem (z+) do gracza
        transform.LookAt(player.transform.position);
        //odniesienie do rigidbody asteroidy
        rb = GetComponent<Rigidbody>();
        //popchnij asteroide w kierunku gracza
        rb.AddForce(transform.forward, ForceMode.Impulse);
    }

    // Update is called once per frame
    void Update()
    {

    }
}