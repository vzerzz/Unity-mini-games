using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public Transform player;//Transform->class transform->function
    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(player.position.x, 0, -10);
    }
}
