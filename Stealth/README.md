# Stealth

教程网址 https://www.bilibili.com/video/BV1Yq4y1S7zJ

## 警报系统

* 灯光闪烁

主要用lerp函数计时，决定灯光的闪烁
创建一个direction light 附上AlarmLight脚本

```C#
//AlarmLight.cs
public class AlarmLight : MonoBehaviour
{
    [Header("是否开启警报灯")]
    public bool isAlarm;
    [Header("闪烁速度")]
    public float turnSpeed = 3f;
    private float highIntencity = 2f;
    private float lowIntencity = 0f;
    private float targetIntencity;

    private Light lt;

    private void Awake()
    {
        lt = GetComponent<Light>();
    }
    private void Start()
    {
        targetIntencity = highIntencity;
    }
    private void Update()
    {
        if (isAlarm)
        {
            if(Mathf.Abs(lt.intensity - targetIntencity) < 0.05f)
            {
                if(targetIntencity == highIntencity)
                {
                    targetIntencity = lowIntencity;
                }
                else
                {
                    targetIntencity = highIntencity;
                }
            }
            lt.intensity = Mathf.Lerp(lt.intensity, targetIntencity, Time.deltaTime*turnSpeed);//无限接近
        
        }
        else
        {
            lt.intensity = Mathf.Lerp(lt.intensity, lowIntencity, Time.deltaTime * turnSpeed);
            if (Mathf.Abs(lt.intensity - lowIntencity) < 0.05f)
            {
                lt.intensity = 0;//减少CPU的计算
            }
        }
    }

}

```

* 激光闪烁

激光对象上附有Trigger组件
关键在于时间间隔后激光如何消失的处理
不能直接Destroy，不然无法重现

1. 将对象移到超出游戏范围的地方，时间间隔后再移回来
2. 让mesh aud collid light等组件enable = false

```C#
//LaserTwinkle.cs
public class LaserTwinkle : MonoBehaviour
{
    [Header("闪烁间隔时间")]
    public float interval = 2f;
    private float timer = 0;
    Vector3 originPos;
    private bool isShow;
    private void Start()
    {
        originPos = transform.position;
    }
    private void Update()
    {
        timer += Time.deltaTime;
        if(timer > interval)
        {
            isShow = !isShow;
            timer = 0;
            
        }
        if (isShow)
        {
            transform.position = originPos;
        }
        else
        {
            transform.position = Vector3.up * 1000;
        }
    }
}
```

* 技巧:设置游戏常数类

对于容易写错，多次调用，无代码提示的常数可用一个类集合起来
如Tags Axis Button Animation(可化成哈希数)

```C#
//GameConsts.cs
public class GameConsts
{
    #region Game Tags
    public const string MAINLIGHT = "MainLight";
    public const string ALARMLIGHT = "AlarmLight";
    public const string SIREN = "Siren";
    public const string PLAYER = "Player";
    #endregion

    #region Virtual Button & Axis
    public const string HORIZONTAL = "Horizontal";
    public const string VERTICAL = "Vertical";
    public const string SNEAK = "Sneak";
    public const string SHOUT = "Shout";
    #endregion

    #region Animation Parameters & States
    //速度参数
    public static int SPEED_PARAM;
    //潜行参数
    public static int SNEAK_PARAM;
    //喊叫参数
    public static int SHOUT_PARAM;
    //运动状态
    public static int LOCOMOTION_STATE;
    #endregion

    #region Game Paramters
    //头顶观察距离偏移量
    public const float WATCH_OFFSET = 0f;
    //玩家身体调节偏移量
    public const float PLAYER_BODY_OFFSET = 1f;

    #endregion

    #region Static Constructor

    static GameConsts()
    {
        //用整型
        SPEED_PARAM = Animator.StringToHash("Speed");
        SNEAK_PARAM = Animator.StringToHash("Sneak");
        SHOUT_PARAM = Animator.StringToHash("Shout");
        LOCOMOTION_STATE = Animator.StringToHash("Locomotion");
    }
    #endregion
}
```

* 警报触发

对于警报是否触发的判定，除了定义bool变量isAlarm
还可定义一个向量存储安全区与非安全区，触发警报时将触发位置赋给向量，当向量不等于安全区向量时报警
将报警后的声音播放，灯光闪烁等操作集合在一起，运用单例，当触发器触发时调用单例即可
AlarmTrigger(附在激光等对象上) -> AlarmSystem

```C#
//AlarmSystem.cs(放在空对象gamecontoller上用于控制报警系统和播放背景音乐)
public class AlarmSystem : MonoBehaviour
{
    public static AlarmSystem instance;

    [Header("过渡速度")]
    public float turnSpeed = 3f;
    [HideInInspector]
    public Vector3 alarmPos = new Vector3(0,1000,0);
    [HideInInspector]
    public Vector3 safePos = new Vector3(0,1000,0);

    private AlarmLight alarmLight;

    private Light mainLight;

    private AudioSource normalAud;
    private AudioSource panicAud;
    private GameObject[] sirensObj;
    private AudioSource[] sirensAud;

    private void Awake()
    {
        instance = this;
        alarmLight = GameObject.FindWithTag(GameConsts.ALARMLIGHT).GetComponent<AlarmLight>();  
        mainLight = GameObject.FindWithTag(GameConsts.MAINLIGHT).GetComponent<Light>();
        normalAud = GetComponent<AudioSource>();
        panicAud = transform.GetChild(0).GetComponent<AudioSource>();
        sirensObj = GameObject.FindGameObjectsWithTag(GameConsts.SIREN);

    }

    private void Start()
    {
        sirensAud = new AudioSource[sirensObj.Length];
        for (int i = 0; i < sirensAud.Length; i++)
        {
            sirensAud[i] = sirensObj[i].GetComponent<AudioSource>();
        }
    }
    private void Update()
    {
        AlarmSystemOperation(alarmPos != safePos);//简化if else
    }
    /// <summary>
    /// 警报操作
    /// </summary>
    /// <param name="isAlarm"></param>
    private void AlarmSystemOperation(bool isAlarm)
    {
        float value = 0;
        if (isAlarm)
        {
            value = 1;
        }

        alarmLight.isAlarm = isAlarm;
        mainLight.intensity = Mathf.Lerp(mainLight.intensity, 1-value, Time.deltaTime * turnSpeed);
        normalAud.volume = Mathf.Lerp(normalAud.volume, 1-value, Time.deltaTime * turnSpeed);
        panicAud.volume = Mathf.Lerp(panicAud.volume, value, Time.deltaTime * turnSpeed);
        for (int i = 0; i < sirensAud.Length; i++)
        {
            sirensAud[i].volume = Mathf.Lerp(sirensAud[i].volume, value, Time.deltaTime * turnSpeed);
        }
    }
}

//AlarmTrigger.cs
public class AlarmTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(GameConsts.PLAYER))
        {
            AlarmSystem.instance.alarmPos = other.transform.position;
        }
    }
}

```

## 主人公

* 动画系统 EthanAnimatorController

Bleed Tree 用于控制走和跑 参数为speed

两个层
一个层LocomotionLayer控制站立走路跑步静步
另一个层ShoutingLayer控制喊叫动作 设置Mask只覆盖手和头

控制声音播放

1. 设置 AudioClip
2. AudioSource.PlayClipAtPoint静态方法

```C#
//PlayerMovement.cs
public class PlayerMovement : MonoBehaviour
{
    [Header("走到跑的时间")]
    [Range(0.1f,2f)]
    public float dempTime = 1.5f;
    [Header("转身速度")]
    public float turnSpeed = 10f;
    [Header("喊叫声音片段")]
    public AudioClip shoutClip;

    private float hor, ver;
    private bool sneak, shout;

    private Animator ani;
    private AudioSource aud;
    private Vector3 dir;
    private Quaternion targetQua;

    private void Awake()
    {
        ani = GetComponent<Animator>();
        aud = GetComponent<AudioSource>();
        dir = Vector3.zero;
    }
    private void Update()
    {
        hor = Input.GetAxis(GameConsts.HORIZONTAL);
        ver = Input.GetAxis(GameConsts.VERTICAL);

        sneak = Input.GetButton(GameConsts.SNEAK);
        shout = Input.GetButtonDown(GameConsts.SHOUT);

        dir.x = hor;
        dir.z = ver;

        if(hor != 0||ver != 0)
        {
            ani.SetFloat(GameConsts.SPEED_PARAM, 5.66f, dempTime, Time.deltaTime);
            //转身两步：
            //方向向量转换为四元数
            targetQua = Quaternion.LookRotation(dir);
            //Lerp过去
            transform.rotation = Quaternion.Lerp(transform.rotation, targetQua, Time.deltaTime * turnSpeed);
            
        }
        else
        {
            ani.SetFloat(GameConsts.SPEED_PARAM, 1.5f);
        }
        ani.SetBool(GameConsts.SNEAK_PARAM, sneak);

        if (shout)
        {
            ani.SetTrigger(GameConsts.SHOUT_PARAM);
        }
        AudioSetUp();
    }
    /// <summary>
    /// 声音配置
    /// </summary>
    private void AudioSetUp()
    {
        bool isLocomotion = ani.GetCurrentAnimatorStateInfo(0).shortNameHash == GameConsts.LOCOMOTION_STATE;
        if (isLocomotion)
        {
            if (!aud.isPlaying)
            {
                aud.Play();//loop选不选无所谓
            }
        }
        else
        {
            aud.Stop();
        }
    }
    public void PlayShoutAudio()//动画事件添加声音
    {
        AudioSource.PlayClipAtPoint(shoutClip, transform.position);
    }
}
```

## 摄像头

## 其它杂项

* 小门

* 电梯大门

* 电梯

* 钥匙

## 机器人

四个主要脚本

### 机器人警报

添加Capsule Collider
rigidbody 选is Kinematic 玩家撞不飞
加tag
视觉听觉 位置消息的接收脚本EnemySightingAndHearing.cs
警报位置
全局 触发激光 摄像头 机器人看见 所有机器人都追逐
私人 机器人听见 只有单个机器人追逐
两个警报谁更新去哪个

* 全局(监听警报位置更新):
if(当全局警报位置发生变化的时候){
    将全局警报位置发送给当前机器人并存储
}

判断变化: 当前帧警报位置 **对比** 上一帧警报位置

* 视觉检测:

1. 角度判断
机器人指向玩家向量与正前方夹角

2. 距离判断
每帧计算距离 or
球形触发器(消耗少)OnTrigger 听觉与视觉判断都放在里面而不放在Update里

3. 是否遮挡
向玩家发射物理射线 设置偏移量
看到玩家触发全局警报

* 听觉检测:

走路/喊叫
墙壁完全隔音 只通过空气传播
路径--导航系统
添加组件 设置静态 烘焙网格
假设通过导航方式找玩家看是否找的到
节点坐标数组得到总距离
足够短则可以听到声音触发私人警报

小门bug: 会检测到触发器 改成只检测碰撞体
if (other.isTrigger)
    return;

### 机器人动画

融合树 主要在于两个参数Speed AngularSpeed的确定
EnemyAnimation.cs
导航 期望速度向量在机器人前方的投影向量为Speed
AngularSpeed
期望速度向量和自身前方向量夹角
自身前方向量和期望速度向量法向量 上--右 下--左(夹角变负)
角度转弧度 *=Mathf.Deg2Rad

两种位移 一个动画一个导航
OnAnimatorMove()
期望速度 -> 动画参数【speed, angularSpeed】 -> 执行动画 -> 产生位移【deltaPosition】 -> velocity
导航瞬时速度设为动画位移/每帧时间
nav.velocity = ani.deltaPosition/Time.deltaTime
旋转使用动画的根旋转
transform.rotation = ani.rootRotation

擦墙: 调整烘焙的宽度和高度

到终点后旋转: 期望速度为0时角速度强行变为0

僵硬: 加延时dampTime

玩家容易摆脱机器人的追捕，容易绕:
期望速度向量和自身前方向量夹角小时直接转不用角速度直接lookat转向玩家
在视觉检测添加一个bool判断看不看的见玩家

### 机器人AI

EnemyAI.cs

PlayerHealth.cs单例

射击 追捕 巡逻

* 射击 看到玩家且玩家活着
停止导航
新动画层ShootingLayer 参数 是否看见玩家
放置枪
脚本中设置参数改变
IK动画
EnemyShoot.cs
OnAnimatorIK()
射击动画时加IK
射击特效:
ShootingEffect.cs
设置激光LineRenderer
设置激光端点个数和坐标
开启灯光Light
播放声音AudioClip
延时关闭

添加动画事件

计算伤害

给主人公添加死亡动画 any status 都能转到  can transition to self 取消

死亡后取消玩家控制权 解除警报 取消玩家标签

* 追捕 私人警报坐标不等于安全坐标
恢复导航
导航私人警报位置
判断是否到达目标
滞留时间后 解除警报

* 巡逻 else
设置巡逻点wayPoint
Transform数组记录坐标
设置导航目标

门加导航动态路障组件
path.status==PathP
有路障 解除警报返回巡逻

## 优化

Draw Call : CPU向GPU发布渲染命令
Draw Call越大: CPU占有率就会越高

优化Draw Call方法:

1. 批处理:
    1. 静态批处理:要求宽松，批处理量比较大
    2. 动态批处理:要求苛刻，批处理量比较少

2. 调节摄像机的渲染路径
    1. 正向渲染
    2. 延时光照
    3. 顶点光照


雾效