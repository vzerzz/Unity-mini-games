using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LiftController : MonoBehaviour
{
    public float moveSpeed = 3f;
    [HideInInspector]
    public bool beginMove = false;
    private AudioSource aud;
    private float timer;
    private void Awake()
    {
        aud = GetComponent<AudioSource>();
    }
    public void BeginMove()
    {
        beginMove = true;
        aud.Play();
    }
    
    private void Update()
    {
        if (beginMove)
        {
            timer += Time.deltaTime;
            transform.position += Vector3.up * moveSpeed * Time.deltaTime;
            PlayerBag.instance.transform.position += Vector3.up * moveSpeed * Time.deltaTime;
            if(timer > aud.clip.length)
            {
                SceneManager.LoadScene(0);
            }
        }
    }
}
