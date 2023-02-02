using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;

public class PlayerManager : MonoBehaviour
{
    PhotonView PV;

    GameObject controller;

    void Awake()
    {
        PV = GetComponent<PhotonView>();
    }
    // Start is called before the first frame update
    void Start()
    {
        if (PV.IsMine)
        {
            CreatController();
        }
    }

    void CreatController()
    {
        Transform spawnpoint = SpawnManager.instance.GetSpawnpoint();
        //Instantiate our player controller
        controller = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerController"), spawnpoint.position, spawnpoint.rotation,0,new object[] { PV.ViewID});
        /*
         Any prefab must have a PhotonView component. This contains a ViewID (the identifier for network messages),
        who owns the object, which scripts will write and read network updates (the "observed" list) 
        and how those updates are sent (the "Observe option"). 

        ��һ��������resource�е�·������photonʱ����Ԥ���嶼Ҫ����Resources�ļ���֮�У��Ҵ���photonview�����

        Ĭ������£�PUN ʹ�� DefaultPool Ĭ�϶���� ʵ���������� Resources �ļ��м���Ԥ���岢���Ժ����١�
        һ�������ӵĽӿ� IPunPrefabPool ����ʵ�������ٶ���ʱ���䷵�ص�����ز���ʵ����ʱ���á�
        ����������£��� Instantiate ʱ��û����Ĵ���������ζ�� Start() ��û�б� Unity ���á�
        ��ˣ���������Ϲ��صĶ������ʵ�� OnEnable() �� OnDisable() 

        group������Ⱥ�飬�����������ֻ��ĳһЩ�ͻ���ʵ����������һ�û�и����ף���һ�㶼��0

        ���һ������data��
        ʵ����ʱ�򴫵ݵ����ݣ���ֵ������PhotonView.instantiationData�����У�
        Ҫע�⴫�ݵ�ÿһ��Object�����ǿ����л��ģ�����ᱨ����ֵ���Բ��Ĭ��Ϊnull
         */
    }

    public void Die()
    {
        PhotonNetwork.Destroy(controller);
        CreatController();
    }
}
