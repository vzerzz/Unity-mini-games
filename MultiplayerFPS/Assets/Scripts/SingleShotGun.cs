using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SingleShotGun : Gun
{
    [SerializeField] Camera cam;

    PhotonView PV;

    private Animator ani;
    public AudioClip readyAud;
    public AudioClip fireAud;
    public AudioClip reloadAud;

    private void Awake()
    {
        PV = GetComponent<PhotonView>();
    }
    public override void Use()
    {
        Shoot();
    }
    public void Reload()
    {
        ani.SetTrigger("Reload");
        AudioSource.PlayClipAtPoint(reloadAud, transform.position);
    }

    public void Ready()
    {
        ani = GetComponentInChildren<Animator>();
        AudioSource.PlayClipAtPoint(readyAud, transform.position);
    }

    public void PlayShootAudio()
    {
        AudioSource.PlayClipAtPoint(fireAud, transform.position);
    }

    void Shoot()
    {
        ani.SetTrigger("Fire");
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f));
        ray.origin = cam.transform.position;
        if(Physics.Raycast(ray,out RaycastHit hit))
        {
            hit.collider.gameObject.GetComponent<IDamageable>()?.TakeDamage(((GunInfo)itemInfo).damage);
            PV.RPC("RPC_Shoot", RpcTarget.All, hit.point, hit.normal);
        }
    }

    [PunRPC]
    void RPC_Shoot(Vector3 hitPosition, Vector3 hitNormal)
    {
        Collider[] colliders = Physics.OverlapSphere(hitPosition, 0.3f);
        if(colliders.Length != 0)
        {
            GameObject bulletImpactObj = Instantiate(bulletImpactPrefab, hitPosition + hitNormal*0.001f, Quaternion.LookRotation(hitNormal, Vector3.up)* bulletImpactPrefab.transform.rotation);
            Destroy(bulletImpactObj, 10f);
            bulletImpactObj.transform.SetParent(colliders[0].transform);
        }
    }
}
