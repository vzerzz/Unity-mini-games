using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingEffect : MonoBehaviour
{
    private Transform player;
    private LineRenderer lineRenderer;
    private Light light;
    private AudioSource audioSource;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        light = GetComponent<Light>();
        audioSource = GetComponent<AudioSource>();
        player = GameObject.FindWithTag(GameConsts.TAG_PLAYER).transform;

    }
    public void PlayEffect()
    {
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, player.position + Vector3.up * GameConsts.PLAYER_BODY_HEIGHT);

        light.enabled = true;
        audioSource.Play();
        Invoke("CloseEffect", 0.1f);
    }
    private void CloseEffect()
    {
        lineRenderer.positionCount = 0;
        light.enabled = false;
    }
}
