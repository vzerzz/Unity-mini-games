# SunnyLand

教程网址 https://space.bilibili.com/370283072/channel/collectiondetail?sid=2991
> 感谢麦扣老师带我迈入游戏制作的大门 >,<

## Scripts

### Manager 

* 所定义的函数将用于不同对象功能的实现

### GameManager

  > * 控制游戏进程，其中包括控制关卡的开始延迟、每一回合间的延迟、控制是否允许玩家操作等、判断游戏是否结束等；
  > * 初始化游戏信息，其中包括了生成地图等；
  > * 记录游戏当中的一些数值，包括玩家的生命值、当前进行到的关卡级别等；
  > * 持有敌人等部分对象的引用；
  > * 控制游戏的显示状态、比如是应该显示地图还是显示正在加载等辅助信息；
  > * 控制游戏的UI。

* 只是将相关函数定义出来，用于其它文件直接调用

* 单例对象

    ```
    static GameManager instance;
    ```

* 定义要管理的对象，以对象的类为定义的类型

  ```
    SceneFader fader;
    List<Orb> orbs;
    Door lockedDoor;
  ```

* Awake()

  > * 由于Awake()方法将在所有其它方法之前被执行，因此一般用这个方法来进行一些必要变量的初始化等等。
  > * 首先判断GameManager的单例是否有值，如果没有则将其指向当前对象；如果有值则将之前值的游戏对象（gameObject）销毁；
  > * 接下来用DontDestoryInLoad()方法将当前的GameManager标记为再下一个场景创建时不会被销毁，通过这个方法可以保留在上一关中记录的数值；
  > * 接下来再为enemies（敌方单位）数组以及boardScript（地图管理器）分配内存；
  > * 最后执行InitGame()方法初始化当前关的内容。
  > * 由于unity在每加载新的一关时，会把上一关创建的对象全部销毁，但是由于我们在GameManager中存放了很多数据需要在下一关使用，所以不能让自己被直接销毁，所以需要用DontDestoryOnLoad()方法来保持自己。

  ```
    private void Awake()
    {
        if(instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;

        orbs = new List<Orb>();

        DontDestroyOnLoad(this);
    }
  ```

* Update()

  > * 判断游戏结束
  > * 游戏的计时

  ```
    private void Update()
    {
        if (gameIsOver)
            return;
        //orbNum = instance.orbs.Count;
        gameTime += Time.deltaTime;
        UIManager.UpdateTimeUI(gameTime);
    }
  ```

* 对游戏的对象进行注册(即对之前定义的对象进行赋值)，对相关UI进行更新

  ```
   public static void RegisterDoor(Door door)//注册门
    {

        instance.lockedDoor = door;
    }
    public static void RegisterSceneFader(SceneFader obj)
    {
        instance.fader = obj;
    }

    public static void RegisterOrb(Orb orb)
    {
        if (instance == null)
            return;
        if (!instance.orbs.Contains(orb))
            instance.orbs.Add(orb);
        UIManager.UpdateOrbUI(instance.orbs.Count);
    }
  ```

* 控制游戏进程，玩家的部分行为

  ```
   public static void PlayerGrabbedOrb(Orb orb)
    {
        if (!instance.orbs.Contains(orb))
            return;
        instance.orbs.Remove(orb);

        if (instance.orbs.Count == 0)
            instance.lockedDoor.Open();
        UIManager.UpdateOrbUI(instance.orbs.Count);
    }

    public static void PlayerWon()
    {
        instance.gameIsOver = true;
        UIManager.DisplayGameOver();
        AudioManager.PlayerWonAudio();
    }

    public static void PlayerDied()
    {
        instance.fader.FadeOut();
        instance.deathNum++;
        UIManager.UpdateDeathUI(instance.deathNum);
        instance.Invoke("RestartScene", 1.5f);//用于场景切换的延迟
    }

    public static bool GameOver()
    {
        return instance.gameIsOver;
    }

    void RestartScene()
    {
        instance.orbs.Clear();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
  ```

### UIManager

> * 控制相关UI，以及游戏结束的字幕等

* 单例对象

* 定义相关TextMeshProUGUI(TMP)(定义好了要在unity中挂载)

* 经典Awake()
  ```
    private void Awake()
    {
        if(instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;

        DontDestroyOnLoad(this);
    }
  ```

* 把UI与UI要表示的对象联系起来 类似GameManager的Register

  ```
    public static void UpdateOrbUI(int orbCount)
    {
        instance.orbText.text = orbCount.ToString();
    }

    public static void UpdateDeathUI(int deathCount)
    {
        instance.deathText.text = deathCount.ToString();
    }

    public static void UpdateTimeUI(float time)
    {
        int minutes = (int)(time / 60);
        float seconds = time % 60;


        instance.timeText.text = minutes.ToString("00")+":"+seconds.ToString("00");
    }

    public static void DisplayGameOver()
    {
        instance.gameOverText.enabled = true;
    }
  ```

### AudioManager

> * 管理相关音效和BGM的播放

* AudioSource

  > * 音频源组件, 发出声音的源头

* AudioClip

  > * 要播放的音频(定义后需挂载)

* AudioMixerGroup

  > * 相当于AudioSource的输出

* AudioClip -> AudioSource -> AudioMixerGroup

* 单例

  ```
  static AudioManager current;
  ```

* 定义

  ```
    [Header("环境声音")]
    public AudioClip ambientClip;
    public AudioClip musicClip;

    [Header("FX音效")]
    public AudioClip deathFXClip;
    public AudioClip orbFXClip;
    public AudioClip doorFXClip;
    public AudioClip startLevelClip;
    public AudioClip winClip;

    [Header("Robbie音效")]
    public AudioClip[] walkStepClips;
    public AudioClip[] crouchStepClips;
    public AudioClip jumpClip;
    public AudioClip deathClip;

    public AudioClip jumpVoiceClip;//角色声音
    public AudioClip deathVoiceClip;
    public AudioClip orbVoiceClip;

    AudioSource ambientSource;
    AudioSource musicSource;
    AudioSource fxSource;
    AudioSource playerSource;
    AudioSource voiceSource;

    public AudioMixerGroup ambientGroup, musicGroup, FXGroup, playerGroup, voiceGroup;

  ```

* Awake() 进行初始化

  ```
    private void Awake()
    {
        if(current != null)//声音不会叠加
        {
            Destroy(gameObject);
            return;
        }
        current = this;

        DontDestroyOnLoad(gameObject);//场景切换时音乐保留

        ambientSource = gameObject.AddComponent<AudioSource>();
        musicSource = gameObject.AddComponent<AudioSource>();
        fxSource = gameObject.AddComponent<AudioSource>();
        playerSource = gameObject.AddComponent<AudioSource>();
        voiceSource = gameObject.AddComponent<AudioSource>();

        ambientSource.outputAudioMixerGroup = ambientGroup;
        musicSource.outputAudioMixerGroup = musicGroup;
        fxSource.outputAudioMixerGroup = FXGroup;
        playerSource.outputAudioMixerGroup = playerGroup;
        voiceSource.outputAudioMixerGroup = voiceGroup;

        StartLevelAudio();//起始BGM的播放
    }
  ```

* 把AudioClip -> AudioSource过程实现并播放

  ```
    void StartLevelAudio()
    {
        current.ambientSource.clip = current.ambientClip;
        current.ambientSource.loop = true;
        current.ambientSource.Play();

        current.musicSource.clip = current.musicClip;
        current.musicSource.loop = true;
        current.musicSource.Play();

        current.fxSource.clip = current.startLevelClip;
        current.fxSource.Play();
    }

    public static void PlayerWonAudio()
    {
        current.fxSource.clip = current.winClip;
        current.fxSource.Play();
        current.playerSource.Stop();
    }
    public static void PlayFootsteoAudio()
    {
        int index = Random.Range(0, current.walkStepClips.Length);

        current.playerSource.clip = current.walkStepClips[index];
        current.playerSource.Play();
    }

    public static void PlayDoorOpenAudio()
    {
        current.fxSource.clip = current.doorFXClip;
        current.fxSource.PlayDelayed(1f);
    }

    public static void PlayCrouchsteoAudio()
    {
        int index = Random.Range(0, current.crouchStepClips.Length);

        current.playerSource.clip = current.crouchStepClips[index];
        current.playerSource.Play();
    }

    public static void PlayJumpAudio()
    {
        current.playerSource.clip = current.jumpClip;
        current.playerSource.Play();

        current.voiceSource.clip = current.jumpVoiceClip;
        current.voiceSource.Play();
    }

    public static void PlayDeathAudio()
    {
        current.playerSource.clip = current.deathClip;
        current.playerSource.Play();

        current.voiceSource.clip = current.deathVoiceClip;
        current.voiceSource.Play();

        current.fxSource.clip = current.deathFXClip;
        current.fxSource.Play();
    }

    public static void PlayOrbAudio()
    {
        current.voiceSource.clip = current.orbVoiceClip;
        current.voiceSource.Play();

        current.fxSource.clip = current.orbFXClip;
        current.fxSource.Play();
    }
  ```


### Player

* 动作 动画

### PlayerMovement

> * 定义玩家的刚体和碰撞体，以及相关的尺寸
> * 定义移动，跳跃等相关的参数，关联相关的按键
> * 定义状态参数，判断玩家的状态
> * 定义环境监测的参数，用射线来判断环境

* Start()

  > * 代码挂载刚体，碰撞体，获取相关的尺寸

  ```
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();

        playerHeight = coll.size.y;
        colliderStandSize = coll.size;
        colliderStandOffset = coll.offset;
        colliderCrouchSize = new Vector2(coll.size.x, coll.size.y / 2f);
        colliderCrouchOffset = new Vector2(coll.offset.x, coll.offset.y / 2f);
    }
  ```

* Update()

  > * 判断游戏结束 跳 蹲 (按需要分短按和长按)

  ```
    void Update()
    {
        if (GameManager.GameOver())
        {
            xVelocity = 0f;
            rb.bodyType = RigidbodyType2D.Static;
            return;
        }

        if (Input.GetButtonDown("Jump"))
            jumpPressed = true;
        jumpHeld = Input.GetButton("Jump");//连续判断
        crouchHeld = Input.GetButton("Crouch");
        if (Input.GetButtonDown("Crouch"))
            crouchPressed = true;
    }
  ```

* FixedUpdate()

  > * 判断游戏结束
  > * 任何需要连续判断的行为

  ```
   private void FixedUpdate()
    {
        if (GameManager.GameOver())
        {
            xVelocity = 0f;
            rb.bodyType = RigidbodyType2D.Static;
            return;
        }

        if (isJump)
            jumpPressed = false;
        PhysicsCheck();
        GroundMovement();
        MidAirMovement();
    }
  ```

* PhysicsCheck()

  > 用射线判断环境 左右脚射线 头顶射线 悬挂射线
  ```
      void PhysicsCheck()
    {
        //左右脚射线
        RaycastHit2D leftCheck = Raycast(new Vector2(-footOffset, 0f), Vector2.down, groundDistane, groundLayer);
        RaycastHit2D rightCheck = Raycast(new Vector2(footOffset, 0f), Vector2.down, groundDistane, groundLayer);

        if (leftCheck || rightCheck)
            isOnGround = true;
        else isOnGround = false;

        //头顶射线
        RaycastHit2D headCheck = Raycast(new Vector2(0f, coll.size.y), Vector2.up, headClearance, groundLayer);

        isHeadBlocked = headCheck;

        float direction = transform.localScale.x;
        Vector2 grabDir = new Vector2(direction, 0f);

        RaycastHit2D blockedCheck = Raycast(new Vector2(footOffset * direction, playerHeight), grabDir, grabDistance, groundLayer);
        RaycastHit2D wallCheck = Raycast(new Vector2(footOffset * direction, eyeHeight), grabDir, grabDistance, groundLayer);
        RaycastHit2D ledgeCheck = Raycast(new Vector2(reachOffset * direction, playerHeight), Vector2.down, grabDistance, groundLayer);

        if(!isOnGround && rb.velocity.y<0f && ledgeCheck && wallCheck && !blockedCheck)
        {
            Vector3 pos = transform.position;

            pos.x += (wallCheck.distance - 0.05f)* direction;

            pos.y -= ledgeCheck.distance;

            transform.position = pos;

            rb.bodyType = RigidbodyType2D.Static;
            isHanging = true;
        }
    }
  ```

* 根据需要对Raycast函数进行重载

    ```
        RaycastHit2D Raycast(Vector2 offset,Vector2 rayDiraction,float length,LayerMask layer)
    {
        Vector2 pos = transform.position;

        RaycastHit2D hit = Physics2D.Raycast(pos + offset, rayDiraction, length, layer);

        Color color = hit ? Color.red : Color.green;

        Debug.DrawRay(pos + offset, rayDiraction * length,color);

        return hit;
    }
    ```

* 根据不同的状态定义相关的移动 转向 跳跃 下蹲函数

    > * 同时改变碰撞体大小

    ```
    
    void GroundMovement()
    {
        if (isHanging)
            return;

        if (crouchHeld && !isCrouch &&isOnGround)
            Crouch();
        else if (!crouchHeld && isCrouch && !isHeadBlocked)
            StandUp();
        else if (!isOnGround && isCrouch)
        {
            StandUp();
        }

        xVelocity = Input.GetAxis("Horizontal");//-1f 1f 0 不按键盘归零，所以不会滑动

        if (isCrouch)
            xVelocity /= crouchSpeedDivisor;

        rb.velocity = new Vector2(xVelocity * speed, rb.velocity.y);

        FilpDirection();
    }

    void MidAirMovement()
    {
        if (isHanging)
        {
            if(jumpPressed)
            {
                rb.bodyType = RigidbodyType2D.Dynamic;
                rb.velocity = new Vector2(rb.velocity.x, hangingJumpForce);
                isHanging = false;
            }

            if (crouchPressed)
            {
                rb.bodyType = RigidbodyType2D.Dynamic;

                isHanging = false;
            }
        }
        if (jumpPressed && isOnGround && !isJump && !isHeadBlocked)
        {
            if(isCrouch && isOnGround )
            {
                crouchPressed = false;
                StandUp();
                rb.AddForce(new Vector2(0f, crouchJumpBoost), ForceMode2D.Impulse);
            }

            isOnGround = false;
            isJump = true;

            jumpTime = Time.time + jumpHoldDuration;

            rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);//也可用velocity impulse表示突然给的一个力

            AudioManager.PlayJumpAudio();
        }

        else if (isJump)
        {
            if(jumpHeld)
                rb.AddForce(new Vector2(0f, jumpHoldForce), ForceMode2D.Impulse);
            if (jumpTime < Time.time)
                isJump = false;
        }
    }

    void FilpDirection()
    {
        if(xVelocity < 0)
            transform.localScale = new Vector3(-1, 1,1);
        if (xVelocity > 0)
            transform.localScale = new Vector3(1, 1,1);
    }

    void Crouch()
    {
        isCrouch = true;
        coll.size = colliderCrouchSize;
        coll.offset = colliderCrouchOffset;
    }

    void StandUp()
    {
        isCrouch = false;
        coll.size = colliderStandSize;
        coll.offset = colliderStandOffset;
    }
    ```

### PlayerAnimation

> * 把动画与人物的动作联系起来，把Animator中不同的状态编号，其值由人物现在的状态决定
> * 若Animation中在动画中插入的动作的声音如走路，则在此还要编写播放声音的函数，同时在Animation中挂载改函数

    ```
    Animator anim;
    PlayerMovement movement;
    Rigidbody2D rb;

    int groundID;
    int hangingID;
    int crouchID;
    int speedID;
    int fallID;
    //定义编号
    void Start()
    {
        anim = GetComponent<Animator>();
        movement = GetComponentInParent<PlayerMovement>();
        rb = GetComponentInParent<Rigidbody2D>();

        groundID = Animator.StringToHash("isOnGround");
        hangingID = Animator.StringToHash("isHanging");
        crouchID = Animator.StringToHash("isCrouching");
        speedID = Animator.StringToHash("speed");
        fallID = Animator.StringToHash("verticalVelocity");
    }
    //改变编号
    void Update()
    {
        anim.SetFloat(speedID, Mathf.Abs(movement.xVelocity));
        //anim.SetBool("isOnGround", movement.isOnGround);
        anim.SetBool(groundID, movement.isOnGround);//用编号
        anim.SetBool(hangingID, movement.isHanging);
        anim.SetBool(crouchID, movement.isCrouch);
        anim.SetFloat(fallID, rb.velocity.y);
    }
    //动作声音
    public void StepAudio()
    {
        AudioManager.PlayFootsteoAudio();
    }

    public void CrouchStepAudio()
    {
        AudioManager.PlayCrouchsteoAudio();
    }
    ```

### 其它

> * 其它的则为和其它游戏对象的相关功能实现的脚本
> * 与玩家相关的可挂载在玩家上，也可挂载在其它对象上，区别主要在于碰撞的layer的不同，一般一类对象有多种但对玩家的功能相同则挂载在玩家上，如陷阱
> * VFXPrefab类的要先用Instantiate实例化
> * 触发类(trigger)的要先在Start()中规定触碰物体的layer(数字表示),再调用OnTriggerEnter2D编写出发后的动作,一定要定义好不同的Layer层以及设置对象的Layer

    ```
    int xxxLayer;
    void Start(){
        xxxLayer = LayerMask.NameToLayer("Layer");//确定触碰物体的层级
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == xxxLayer)//触发
            ....

        ....
    }
    ```

    ```
    /*PlayerrHealth*/
    //挂载在玩家上
    public GameObject deathVFXPrefab;
    int trapsLayer;
    int touchTraps = 0;
    void Start()
    {
        //碰撞类型为陷阱
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
    ```
    ```
    /*Door*/
    Animator anim;
    int openID;

    void Start()
    {
        anim = GetComponent<Animator>();
        openID = Animator.StringToHash("Open");
        GameManager.RegisterDoor(this);
    }

    public void Open()
    {
        anim.SetTrigger(openID);

        AudioManager.PlayDoorOpenAudio();

    }
    ```
    ```
    /*Orb*/
    int player;
    public GameObject explosionVFXPrefab;

    // Start is called before the first frame update
    void Start()
    {
        player = LayerMask.NameToLayer("Player");//记录图层编号

        GameManager.RegisterOrb(this);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == player)
        {
            Instantiate(explosionVFXPrefab, transform.position, transform.rotation);

            gameObject.SetActive(false);

            AudioManager.PlayOrbAudio();

            GameManager.PlayerGrabbedOrb(this);
        }
    }
    ```
    ```
    /*WinZone*/
        int playerLayer;
    // Start is called before the first frame update
    void Start()
    {
        playerLayer = LayerMask.NameToLayer("Player");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == playerLayer)
            Debug.Log("Player Won!");

        GameManager.PlayerWon();
    }
    ```
    ```
    /*SceneFader*/
    Animator anim;
    int faderID;

    private void Start()
    {
        anim = GetComponent<Animator>();

        faderID = Animator.StringToHash("Fade");

        GameManager.RegisterSceneFader(this);
    }

    public void FadeOut()
    {
        anim.SetTrigger(faderID);
    }
    ```
    ```
    /*DeathPose*/
    private void Awake()
    {
        DontDestroyOnLoad(this);
    }
    ```
