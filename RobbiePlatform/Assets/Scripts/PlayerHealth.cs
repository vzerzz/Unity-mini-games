using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public GameObject deathVFXPrefab;
    int trapsLayer;
    int touchTraps = 0;

    // Start is called before the first frame update
    void Start()
    {
        trapsLayer = LayerMask.NameToLayer("Traps");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == trapsLayer)//获得的是int型
        {
            touchTraps++;
            if (touchTraps == 1)
            {
                Instantiate(deathVFXPrefab, transform.position, transform.rotation);

                gameObject.SetActive(false);

                AudioManager.PlayDeathAudio();

                //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);//当前激活的场景的编号

                GameManager.PlayerDied();
            }
        }
    }
}
