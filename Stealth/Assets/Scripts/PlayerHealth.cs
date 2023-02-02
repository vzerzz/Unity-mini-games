using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    [Header("³õÊ¼ÑªÁ¿")]
    public float hp = 100;

    public AudioClip endGameClip;

    private Animator ani;

    private bool isAlive = true;

    private void Awake()
    {
        ani = GetComponent<Animator>();
    }

    public void TakeDamage(float damage)
    {
        hp -= damage;

        if(hp <= 0 && isAlive)
        {
            isAlive = false;
            ani.SetTrigger(GameConsts.PARAM_DEAD);
            SceneManager.LoadScene(0);
        }
    }
}
