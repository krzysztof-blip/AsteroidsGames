using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidGenerator : MonoBehaviour
{
    GameObject model;
    Vector3 rotation = Vector3.one;
    // Start is called before the first frame update
    void Start()
    {
        model = transform.Find("Model").gameObject;

        foreach(Transform cube in model.transform)
        {
            cube.rotation = Random.rotation;
            float scale = Random.Range(0.9f, 1.1f);
            cube.localScale = new Vector3 (scale, scale, scale);
        }

        rotation.x = Random.value;
        rotation.y = Random.value;
        rotation.z = Random.value;
        rotation *= Random.Range(10, 20);
    }

    // Update is called once per frame
    void Update()
    {
        model.transform.Rotate(rotation);
    }
}
