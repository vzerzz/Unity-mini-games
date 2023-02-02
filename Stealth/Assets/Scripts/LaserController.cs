using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserController : MonoBehaviour
{
    [Header("控制的激光")]
    public GameObject controledLaser;
    [Header("关闭时发出的声音")]
    public AudioClip switchAud;
    [Header("屏幕解锁的材质")]
    public Material unlockMat;

    private MeshRenderer screenMeshRenderer;
    private void Awake()
    {
        screenMeshRenderer = transform.GetChild(0).GetComponent<MeshRenderer>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag(GameConsts.TAG_PLAYER)&&Input.GetButtonDown(GameConsts.BUTTON_SWITCH)&&controledLaser.activeSelf)
        {
            controledLaser.SetActive(false);
            AudioSource.PlayClipAtPoint(switchAud, transform.position);
            screenMeshRenderer.material = unlockMat;
        }   
    }
}
