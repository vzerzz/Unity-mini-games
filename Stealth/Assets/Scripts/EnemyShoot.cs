using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyShoot : MonoBehaviour
{
    public float fadeSpeed = 3f;

    private Animator ani;
    private Transform player;
    private PlayerHealth playerHealth;
    private float ikWeight = 0;
    private ShootingEffect shootingEffect;

    private void Awake()
    {
        ani = GetComponent<Animator>();
        player = GameObject.FindWithTag(GameConsts.TAG_PLAYER).transform;
        playerHealth = player.GetComponent<PlayerHealth>();
        shootingEffect = GetComponentInChildren<ShootingEffect>();
    }
    private void OnAnimatorIK(int layerIndex)
    {
        bool isShooting = ani.GetCurrentAnimatorStateInfo(1).shortNameHash == GameConsts.STATE_SHOOT;
        bool isRaising = ani.GetCurrentAnimatorStateInfo(1).shortNameHash == GameConsts.STATE_RAISE;
        if(isShooting || isRaising)
        {
            ikWeight = Mathf.Lerp(ikWeight, 1, Time.deltaTime * fadeSpeed);

            ani.SetIKPositionWeight(AvatarIKGoal.RightHand, ikWeight);
            ani.SetLookAtWeight(ikWeight);

            ani.SetIKPosition(AvatarIKGoal.RightHand, player.position + Vector3.up * GameConsts.PLAYER_BODY_HEIGHT);
            ani.SetLookAtPosition(player.position + Vector3.up * GameConsts.PLAYER_BODY_HEIGHT);

        }
        else
        {
            ikWeight = Mathf.Lerp(ikWeight, 0, Time.deltaTime * fadeSpeed);
            ani.SetIKPositionWeight(AvatarIKGoal.RightHand, ikWeight);
            ani.SetLookAtWeight(ikWeight);
        }
    }
    public void Shoot()
    {
        shootingEffect.PlayEffect();
        float distance = Vector3.Distance(player.position, transform.position);
        playerHealth.TakeDamage((10 - distance) * 20 + 10);
    }
}
