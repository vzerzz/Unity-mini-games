using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBag : MonoBehaviour
{
    public static PlayerBag instance;

    public bool hasKey = false;

    private void Awake()
    {
        instance = this;
    }
}
