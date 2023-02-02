
# Ruby's Adventure

教程网址 https://learn.unity.com/project/ruby-s-adventure-2d-chu-xue-zhe

## Unity

* “Pixels per Unit”通过定义 1 个单位内应该设置的像素数量来告知 Unity 如何设置精灵的大小。

* 游戏对象的排序问题

  > 调整瓦片的 Order in Layer 属性

  * “伪造”透视图

    > 根据直觉，玩家希望角色在立方体前面时首先绘制角色，而角色在立方体后面时最后绘制角色

    1. 选择 Edit > Project Settings>Graphics。
    2. 在 Camera Settings 中，找到 Transparency Sort Mode 字段。此字段决定了精灵的绘制顺序。使用下拉菜单将此设置从 Default 更改为 Custom Axis。
    3. 在 Transparency Sort Axis 中添加以下坐标：x = 0 y = 1 z = 0此设置告诉 Unity 在 y 轴上基于精灵的位置来绘制精灵。
    4. 游戏对象Sprite Sort Point 中选择pivot,再于Sprite Editor中更改pivot的位置

* 物体碰撞时的抖动问题

  > 在帧更新过程中移动角色,物理系统将自己的游戏对象副本移到相应的新位置,物理系统发现角色碰撞体现在位于另一个碰撞体内，然后将角色碰撞体移回,物理系统将 游戏对象与该新位置同步.不断在箱子内移动角色，而物理系统则将她移回。这种冲突就会导致发生抖动

  * 对于不加rigidbody 2d的对象可以用transform.position进行移动

    ```C#
    Vector2 position = transform.position;
    position.x = position.x + 3.0f* horizontal * Time.deltaTime;
    position.y = position.y + 3.0f * vertical * Time.deltaTime;
    transform.position = position;
    ```

  * 对于加上rigidbody 2d的对象则不行

    > When a Collider 2D is attached to the Rigidbody 2D, it moves with it. A Collider 2D should never be moved directly using the Transform or any collider offset; the Rigidbody 2D should be moved instead. This offers the best performance and ensures correct collision detection.

    * 调用rigidbody相关函数以及FixedUpdate

    > 需要定期进行更新,想直接影响物理组件或对象（例如刚体），就需要使用FixedUpdate,不应该读取 Fixedupdate 函数中的输入。FixedUpdate 不会持续运行，因此有可能会错过用户输入。

    ```C#
    void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
    }

    void FixedUpdate()
    {
        Vector2 position = rigidbody2d.position;
        position.x = position.x + 3.0f * horizontal * Time.deltaTime;
        position.y = position.y + 3.0f * vertical * Time.deltaTime;

        rigidbody2d.MovePosition(position);
    }
    ```

* 瓦片地图碰撞

  * 选择性碰撞

    > 在 Project 窗口中，找到存放tile的文件夹，选择所有不是水的瓦片,在 Inspector 中，找到 Collider Type 属性，然后将该属性从 Sprite（目前值）更改为 None。你选择的瓦片不再被视为碰撞体

  * 复合碰撞体

    > 在tilemap对象中添加Composite Collider 2D组件,启用 Used By Composite 复选框, 将 Rigidbody Body Type 属性设置为 Static 

* 伤害区域

  * 只有在角色进入区域时才会伤害角色。如果角色停留在区域内，则不会再伤害角色。

    > OnTriggerEnter2D 更改为 OnTriggerStay2D,刚体在触发器内的每一帧都会调用此函数，而不是在刚体刚进入时仅调用一次。

  * 站着不动时不会受到伤害

    > 在 Rigidbody 组件中将 Sleeping Mode 设置为 Never Sleep

  * 在五帧内便失去所有生命值

    > 添加无敌状态

    ```C#
    public float timeInvincible = 2.0f;//无敌状态时间
    bool isInvincible;//存储当前是否处于无敌状态
    float invincibleTimer;//剩下的无敌状态时间
    void Update()
    {
        if (isInvincible)
        {
            invincibleTimer -= Time.deltaTime;
            if (invincibleTimer < 0)
                isInvincible = false;
        }
    }
    public void ChangeHealth(int amount)
    {
        if (amount < 0)
        {
            if (isInvincible)
                return;
            
            isInvincible = true;
            invincibleTimer = timeInvincible;
        }
    }
    ```

  * 创建更大的伤害区域，则精灵会拉伸而显得很难看,可以让 Sprite Renderer 平铺精灵。将伤害区域的大小调整到足以将精灵容纳两次，则会多次并排绘制精灵：

    > 1. 确保游戏对象的缩放在 Transform 组件中设置为 1,1,1。在 Sprite Renderer 组件中将 Draw Mode 设置为 Tiled，并将 Tile Mode 更改为 Adaptive。
    > 2. 在 Project 窗口中选择精灵,将 Mesh Type 更改为 Full Rect。
    > 3. 选中 Box Collider 2D 组件的 Auto Tiling 属性，以便碰撞体随精灵一起平铺。

* 翻转动画

  > Flip X,在帧 0 和帧 4 上的时候，选中属性名称旁的切换开关，即可将属性设置为 true，这样 Flip 在整个动画中将保持选中状态：

* 混合树

  > * 允许你根据参数来混合多段动画
  > * 希望 Blend Tree 使用两个参数来控制水平和垂直方向的更改，因此请将 Blend Type 设置为 2D Simple Directional。
  > * 添加参数与motion

* Instantiate

  > * Instantiate 的第一个参数是一个对象，在第二个参数的位置处创建一个副本，第三个参数是旋转(Quaternion.identity 表示“无旋转”)。
  > * 作用是复制一个预制件放在需要的位置,使其实例化,可用于游戏过程中产生游戏对象，如子弹,对于仅发生一次的效果（例如被击中或拾取生命值可收集对象），你可以将对预制件的引用存储在公共变量中，并在应该产生效果时调用 Instantiate。
  > * 使用实例化创建对象时用Awake不用Start，创建对象时 Unity 不会运行 Start，而是在下一帧才开始运行。在创建对象时（调用 Instantiate 时）就会立即调用 Awake，

* 输入

  * Input.GetKeyDown

    > 测试特定的键盘按键,只有用键盘时才有效果。

  * Input.GetButtonDown

    > 与轴名称一起使用,并在输入设置 (Edit > Project Settings > Input) 中定义该轴对应的按钮,确保在不同设备上有效

* 图层与碰撞

  > 用不同的图层来实现不同对象之间是否碰撞问题
  > 
  > 打开 Edit > Project Settings > Physics 2D ，查看底部的 Layer Collision Matrix，便可看到与更改哪些图层彼此碰撞

* Cinemachine

  > Cinemachine 使用虚拟摄像机，你可以在每个虚拟摄像机上选择不同的设置，然后告诉实际摄像机（Main Camera）当前哪个虚拟摄像机处于活动状态，实际摄像机就会复制这些设置。可与多个摄像机配合使用，并根据游戏的需要在多个摄像机之间进行切换.

  * 模式:

    > 透视：所有远离摄像机的线都会汇聚到一个点，因此距离摄像机越远，看起来就越小。
    >
    > 正交：所有平行线将保持平行。

  * 跟随

    > 将你的主角从 Hierarchy 拖放到 vcam Inspector 的 Follow 属性中

  * 边界

    > 1. Add Extension > 添加 Cinemachine Confiner
    > 2. Create Empty 来创建一个新的游戏对象CameraConfiner,添加Polygon Collider 2D 组件, Edit Collider 来规划摄像机边界(确保边框最好是直线)
    > 3. 将该游戏对象分配给 CinemachineConfiner 上的 Bounding Shape 2D
    > 4. 将该游戏对象设置为新建的图层 Confiner，在 Edit > Project Settings > Physics 2D中，取消勾选 Confiner 图层中的所有条目：不会与其他对象碰撞

* 烟雾效果(粒子效果)
  * Create > Effects > Particle System  创建一个默认的粒子系统
  * 启用Texture Sheet Animation 部分

    > 1. 将 Mode 设置为 Sprites。
    > 2. 单击显示的精灵条目旁边的 + 按钮，这样便可以得到 2 个精灵。
    > 3. 将精灵图集效果中的烟雾精灵分配给这些属性。
    > 4. 单击 Start Frame 属性右侧的小下拉箭头，选择 Random Between Two Constants，然后输入 0 和 2。系统将选择一个介于 0 到 2（不包括 2）之间的随机数，即 0 或 1，并对粒子使用相应的精灵。
    > 5. 单击 Frame over time 旁边的黑框，这个框显示曲线帧中相应的帧随时间的变化情况（从帧 0 到 1）。你根本不想要任何动画，所以请右键单击最右边的点，然后按 Delete 键：

  * Shape 显示发射粒子的锥体

    > Radius 设置为 0，希望所有粒子都从一个点发出
    >
    > 角度更改为大约 5 度，这样可以减少粒子的分散程度并以更加准直的线条生成

  * 随机性

    > Start Lifetime 右侧的小向下箭头，然后选择 Random Between Two Constants,输入 1.5 和 3,生命周期更短且各不相同
    >
    > Start Size 选择 Random Between Two Constants 并分别设置为 0.3 和 0.5,粒子现在变小了且大小不同
    >
    > Start Speed 选择 Random Between Two Constants 设置为 0.5 和 1

  * 粒子消失
    > 变透明
    >> 启用 Color over Lifetime 单击 Color 旁边的白色方框以打开 Gradient Editor,选择右上角的箭头，然后将 Alpha 从 255 更改为 0。
    >
    > 变小
    >> 启用Size Over Lifetime,单击 Size 框，然后查看显示在 Inspector 底部的曲线：通过将第一个点上调至 1 并将最后一个点下调至 0 来进行反转。

  * 设为预制件作为对象子对象

  * 已经产生的效果不随人物移动
    > Particle System >  Simulation Space 设置改为 World。

  * 脚本里用ParticleSystem创建对象 用Stop()停止，可用instantiate实例化

* UI

  * Create > UI > Canvas

  * 模式：
    > Screen Space - Overlay：默认模式,始终在游戏的上层绘制 UI
    >
    > Screen Space - Camera: 与摄像机对齐的平面上绘制 UI
    >
    > World Space：任何位置

  * Canvas Scaler
    > Constant Size 无论屏幕大小或形状如何，UI 均保持大小不变。
    >
    > Scale With Screen SizeUI 缩放取决于你设置为 Reference Resolution 的屏幕大小。

  * 添加图像 UI > Image为Canvas子对象Health，双击canvas，加入图像到Source Image ， Set Native Size 按钮将 Rect Transform 值更改为图像的大小，再调整大小、

  * Rect Transform 中更改瞄点

  * 添加图像到之前图像子对象，选择瞄点右下角，单击并拖动锚点，让锚点围住肖像。可使图像相对位置不变

  * 生命值条(遮罩技术)

    > Health下添加Image子对象为Mask(遮罩)，更改大小位置，同上拖动瞄点围住，同时移动轴心
    >
    > 创建遮罩的图像子对象，在瞄点菜单中按alt，选右下角，此操作将同时设置锚点和新图像的大小以填充其父级，再重新进入瞄点菜单不按alt，瞄点设为左上角
    >
    >  Mask中添加Mask组件，取消选中 Show Mask Graphic 以隐藏白色正方形。

  * 使用静态成员,生命条为单例对象，可以直接引用场景中的生命值条脚本，不必在 Inspector 中手动分配。脚本添加到生命值条游戏对象，将遮罩拖入 Mask 属性

  ```C#
    public class UIHealthBar : MonoBehaviour
    {
    public static UIHealthBar instance { get; private set; }
    
    public Image mask;
    float originalSize;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        originalSize = mask.rectTransform.rect.width;
    }

    public void SetValue(float value)
    {          
        mask.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.    Horizontal, originalSize * value);
    }
    }
  ```

* 在 Project 文件夹中选择了三个精灵，然后将它们全部一次拖动到 Hierarchy 中，则 Unity 会自动为你创建这三个帧的动画，并将动画分配给新创建的游戏对象。该动画会自动播放，不需要你采取其他操作：

* 对话UI

  * 射线投射(触发对话)

    > 用触发器可能会转过脸背对青蛙，而仍能与青蛙对话。交互式中射线投射很有用

    ```C#
    if (Input.GetKeyDown(KeyCode.X))
    {
    RaycastHit2D hit = Physics2D.Raycast(rigidbody2d.position + Vector2.up * 0.2f, lookDirection, 1.5f, LayerMask.GetMask("NPC"));
    if (hit.collider != null)
    {
        character.DisplayDialog();
    }
    }
    ```

  * UI

    > UI > Canvas。此时将为发起对话的对象创建一个子级 Canvas 游戏对象。 Rect Transform 中更改大小
    >
    > Canvas 下 UI > Image,瞄点菜单里alt填充画布
    >
    > 在 Hierarchy 中选择 Canvas，然后在 Inspector 中将 Order in Layer 设置为较高的值（例如 10）：防止在游戏对象后面渲染
    >
    >  Image 下 UI >  Text - TextMeshPro，按住 Alt 的同时单击扩展锚点控制柄以将文本扩展到图像的整个尺寸，白色的小控制柄移动黄色框（文本边界），给文本框留出边距，输入文字
    >
    > 禁用画布将画布隐藏起来,脚本控制

    ```C#
    public float displayTime = 4.0f;
    public GameObject dialogBox;
    float timerDisplay;
    void Start()
    {
    dialogBox.SetActive(false);
    timerDisplay = -1.0f;
    }
    void Update()
    {
    if (timerDisplay >= 0)
    {
        timerDisplay -= Time.deltaTime;
        if (timerDisplay < 0)
        {
            dialogBox.SetActive(false);
        }
    }
    }
    public void DisplayDialog()
    {
    timerDisplay = displayTime;
    dialogBox.SetActive(true);
    }
    ```

* 音频

  > AudioClip 音频剪辑也是资源。可以从音频文件导入音频剪辑
  > Audio Listener 音频监听器 默认情况下，监听器位于摄像机
  > Audio Source 音频源 在该组件所在的游戏对象的位置播放音频剪辑

  * BGM
    > 创建空的游戏对象，命名为 BackgroundMusic，添加 Audio Source 组件,放入clip，选择loop， Spatial Blend 滑动条可以从 2D滑动到 3D

  * 一次性声音

    > 1. 创建一个新的音频源，分配一个音频剪辑，取消选中 Play On Awake，通过脚本设置当事件发生时播放音频剪辑。但这种做法需要为游戏中每个细小的声音都创建一个游戏对象和音频源。
    > 2. 将音频源添加到 Ruby 游戏对象，并使用该音频源播放与 Ruby 所做的游戏操作,用PlayOneShot函数 将音频剪辑作为第一个参数，并在音频源的位置使用音频源的所有设置播放一次该音频剪辑。

    * PlayOneShot方法
      > 在Ruby下添加一个名为 audioSource 的私有 AudioSource 变量来存储音频源，用 GetComponent 在 Start 函数中获取该变量。

      ```C#
      //ruby脚本中
      AudioSource audioSource;

        void Start()
        {
        rigidbody2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;

        audioSource= GetComponent<AudioSource>();
        }

        public void PlaySound(AudioClip clip)
        {
            audioSource.PlayOneShot(clip);
        }
      ```

      > 在需要播放音乐的地方

      ```C#
      public AudioClip collectedClip;//Clip记得拖入

      controller.PlaySound(collectedClip);
      ```

  * 空间化

    > AudioSource中Spatial Blend 滑动条一直向右移动到 3D
    >
    > 3D Sound Settings 部分找到 Min Distance 和 Max Distance ：Max Distance 改为 10 个单位.因为声音系统是为 3D 设计的。所以该圆圈实际上是一个球体。由于摄像机在z轴远处，衰减不起作用，并且听不到声音
    >
    > 选择主摄像机并创建一个新的空游戏对象作为子对象。将这个对象命名为 Listener，向这个对象添加一个 Audio Listener，在 Transform 组件中将坐标设置为 x = 0、y = 0 且 z = 10。.在主摄像机上，右键单击 Audio Listener，然后选择 Remove Component。使listener正好在z=0处

## Scripts

* RubyControler

  ```C#
  using System.Collections;
  using System.Collections.Generic;
  using UnityEngine;

  public class RubyControler : MonoBehaviour
  {
    public int maxHealth = 5;
    public float speed = 3.0f;
    public float timeInvincible = 2.0f;//无敌时间

    int currentHealth;
    public int health { get { return currentHealth; } }//属性
    //属性的用法很像变量，而不是像函数。

    bool isInvincible;
    float invincibleTimer;

    Rigidbody2D rigidbody2d;
    Animator animator;

    public GameObject projectilePrefab;

    Vector2 lookDirection = new Vector2(1, 0);//存储观察方向
    float vertical;
    float horizontal;

    AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        //每秒帧率设为10帧
        //QualitySettings.vSyncCount = 0;
        //Application.targetFrameRate = 10;
        rigidbody2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        currentHealth = maxHealth;
        currentHealth = 5;

        audioSource = GetComponent<AudioSource>();
    }

    public void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }

    // Update is called once per frame
    void Update()
    {
        vertical = Input.GetAxis("Vertical");
        horizontal = Input.GetAxis("Horizontal");

        Vector2 move = new Vector2(horizontal, vertical);

        if (!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
        {
            lookDirection.Set(move.x, move.y);
            lookDirection.Normalize();
        }
        animator.SetFloat("Look X", lookDirection.x);
        animator.SetFloat("Look Y", lookDirection.y);
        animator.SetFloat("Speed", move.magnitude);

        if (isInvincible)
        {
            invincibleTimer -= Time.deltaTime;
            if (invincibleTimer < 0)
                isInvincible = false;
        }

        if (Input.GetKeyDown(KeyCode.C))
            Launch();

        //射线投射
        if (Input.GetKeyDown(KeyCode.X))
        {
            RaycastHit2D hit = Physics2D.Raycast(rigidbody2d.position + Vector2.up * 0.2f, lookDirection, 1.5f, LayerMask.GetMask("NPC"));
            if (hit.collider != null)
            {
                NonPlayerCharacter character = hit.collider.GetComponent<NonPlayerCharacter>();
                if (character != null)
                    character.DisplayDialog();
            }
        }
    }

    private void FixedUpdate()
    {
        Vector2 position = transform.position;
        position.x = position.x + speed * horizontal * Time.deltaTime;
        position.y = position.y + speed * vertical * Time.deltaTime;

        //position = position + speed * move * Time.deltaTime;

        //transform.position = position;

        rigidbody2d.MovePosition(position);
    }

    public void ChangeHealth(int amount)
    {
        if(amount < 0)
        {
            if (isInvincible)
                return;

            isInvincible = true;
            invincibleTimer = timeInvincible;
        }
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        //Debug.Log(currentHealth + "/" + maxHealth);
        UIHealthBar.instance.SetValue(currentHealth / (float)maxHealth);
    }

    void Launch()
    {
        GameObject projectileObject = Instantiate(projectilePrefab, rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity);//创建副本

        Projectile projectile = projectileObject.GetComponent<Projectile>();
        projectile.Launch(lookDirection, 300);

        animator.SetTrigger("Launch");
    }
  }
  ```

* EnemyController

  ```C#
  using System.Collections;
  using System.Collections.Generic;
  using UnityEngine;

  public class EnemyController : MonoBehaviour
  {
    public float speed;
    public bool vertical;
    public float changeTime = 3.0f;

    bool broken = true;

    private Rigidbody2D rigidbody2d;
    Animator animator;
    float timer;
    int direction = 1;

    public ParticleSystem smokeEffect;

    public AudioClip collectedClip;
    // Start is called before the first frame update
    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        timer = changeTime;
    }
    private void Update()
    {
        if (!broken)
            return;

        timer -= Time.deltaTime;

        if(timer < 0)
        {
            direction = -direction;
            timer = changeTime;
        }
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (!broken)
            return;
        Vector2 position = rigidbody2d.position;
        if (vertical)
        {
            position.y += Time.deltaTime * speed*direction;
            animator.SetFloat("Move X", 0);
            animator.SetFloat("Move Y", direction);
        }
        else
        {
            position.x = position.x + Time.deltaTime * speed*direction;
            animator.SetFloat("Move X", direction);
            animator.SetFloat("Move Y", 0);
        }

        rigidbody2d.MovePosition(position);
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        RubyControler player = collision.gameObject.GetComponent<RubyControler>();
        //此处的类型是 Collision2D，而不是 Collider2D。Collision2D 没有 GetComponent 函数，但是它包含大量有关碰撞的数据，例如与敌人碰撞的游戏对象。因此，在这个游戏对象上调用 GetComponent。
        if (player != null)
            player.ChangeHealth(-1);

        player.PlaySound(collectedClip);
     
    }

    public void Fix()
    {
        broken = false;
        rigidbody2d.simulated = false;
        animator.SetTrigger("Fixed");
        smokeEffect.Stop();
    }
  }
  ```

* HealthCollectible

  ```C#
  using System.Collections;
  using System.Collections.Generic;
  using UnityEngine;

  public class HealthCollectible : MonoBehaviour
  {
    public AudioClip collectedClip;

    private void OnTriggerEnter2D(Collider2D other)
    {
        RubyControler controller = other.GetComponent<RubyControler>();

        if(controller != null)
        {
            if (controller.health < controller.maxHealth)
            {
                controller.ChangeHealth(1);
                Destroy(gameObject);

                controller.PlaySound(collectedClip);
            }
        }
    }
  }
  ```

* NonPlayerCharacter

  ```C#
  using System.Collections;
  using System.Collections.Generic;
  using UnityEngine;

  public class NonPlayerCharacter : MonoBehaviour
  {
    public float displayTime = 4.0f;
    public GameObject dialogBox;
    float timerDisplay;

    // Start is called before the first frame update
    void Start()
    {
        dialogBox.SetActive(false);
        timerDisplay = -0.1f;
    }

    // Update is called once per frame
    void Update()
    {
        if(timerDisplay >= 0)
        {
            timerDisplay -= Time.deltaTime;
            if(timerDisplay < 0)
            {
                dialogBox.SetActive(false);
            }
        }
    }

    public void DisplayDialog()
    {
        timerDisplay = displayTime;
        dialogBox.SetActive(true);
    }
  }
  ```

* UIHealthBar

  ```C#
  using System.Collections;
  using System.Collections.Generic;
  using UnityEngine;
  using UnityEngine.UI;

  public class UIHealthBar : MonoBehaviour
  {
    public static UIHealthBar instance { get; private set; }//set为私有属性 get均会调用

    public Image mask;
    float originalSize;

    void Awake()
    {
        instance = this;//单例对象 只有一个生命值条
    }
    // Start is called before the first frame update
    void Start()
    {
        originalSize = mask.rectTransform.rect.width;//获取屏幕上的大小
    }

    public void SetValue(float value)
    {
        mask.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, originalSize * value);//从代码中设置大小和瞄点
    }
  }
  ```

* Projectile

  ```C#
  using System.Collections;
  using System.Collections.Generic;
  using UnityEngine;

  public class Projectile : MonoBehaviour
  {
    Rigidbody2D rigidbody2d;
    // Start is called before the first frame update
    void Awake()//实例化时不调用start
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        if (transform.position.magnitude > 1000.0f)
            Destroy(gameObject);
    }
    public void Launch(Vector2 direction, float force)
    {
        rigidbody2d.AddForce(direction * force);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log("Projectile Collision with " + collision.gameObject);
        EnemyController e = collision.collider.GetComponent<EnemyController>();
        if (e != null)
            e.Fix();
        Destroy(gameObject);
    }
  }
  ```

* Damageable

  ```C#
  using System.Collections;
  using System.Collections.Generic;
  using UnityEngine;

  public class Damageable : MonoBehaviour
  {
    //public AudioClip collectedClip;
    private void OnTriggerStay2D(Collider2D other)
    {
        RubyControler controller = other.GetComponent<RubyControler>();

        if (controller != null)
        {
                controller.ChangeHealth(-1);
                //controller.PlaySound(collectedClip);
        }
    }
  }
  ```
