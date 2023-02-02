using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserController : MonoBehaviour
{
    [Header("���Ƶļ���")]
    public GameObject controledLaser;
    [Header("�ر�ʱ����������")]
    public AudioClip switchAud;
    [Header("��Ļ�����Ĳ���")]
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
