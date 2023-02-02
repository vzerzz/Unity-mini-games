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

        第一个参数是resource中的路径，用photon时所有预制体都要放在Resources文件夹之中，且搭载photonview组件，

        默认情况下，PUN 使用 DefaultPool 默认对象池 实例化，即从 Resources 文件夹加载预制体并在稍后销毁。
        一个更复杂的接口 IPunPrefabPool 可以实现在销毁对象时将其返回到对象池并在实例化时复用。
        在这种情况下，在 Instantiate 时并没有真的创建对象，意味着 Start() 并没有被 Unity 调用。
        因此，网络对象上挂载的对象必须实现 OnEnable() 和 OnDisable() 

        group参数：群组，好像可以限制只对某一些客户端实例化，这个我还没有搞明白，，一般都填0

        最后一个参数data：
        实例化时候传递的数据，该值保存在PhotonView.instantiationData变量中，
        要注意传递的每一个Object必须是可序列化的，否则会报错，该值可以不填，默认为null
         */
    }

    public void Die()
    {
        PhotonNetwork.Destroy(controller);
        CreatController();
    }
}
