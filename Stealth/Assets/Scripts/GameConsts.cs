using UnityEngine;
/// <summary>
/// 游戏常数类
/// </summary>
public class GameConsts
{
    #region Game Tags
    public const string TAG_MAINLIGHT = "MainLight";
    public const string TAG_ALARMLIGHT = "AlarmLight";
    public const string TAG_SIREN = "Siren";
    public const string TAG_PLAYER = "Player";
    public const string TAG_ENEMY = "Enemy";
    public const string TAG_INNERDOOR = "InnerDoor";
    public const string TAG_LIFT = "Lift";
    public const string TAG_GAMECONTROLLER = "GameController";
    #endregion

    #region Virtual Button & Axis
    public const string AXIS_HORIZONTAL = "Horizontal";
    public const string AXIS_VERTICAL = "Vertical";
    public const string BUTTON_SNEAK = "Sneak";
    public const string BUTTON_SHOUT = "Shout";
    public const string BUTTON_SWITCH = "Switch";
    #endregion

    #region Animation Parameters & States
    public static int PARAM_SPEED;
    public static int PARAM_SNEAK;
    public static int PARAM_SHOUT;
    public static int PARAM_DOOROPEN;
    public static int PARAM_ANGULARSPEED;
    public static int PARAM_ISPLAYERINSIGHT;
    public static int PARAM_DEAD;
    public static int PARAM_ENDGAME;
    public static int STATE_LOCOMOTION;
    public static int STATE_SHOOT;
    public static int STATE_RAISE;
    #endregion


    #region Static Constructor
    /// <summary>
    /// 静态构造
    /// </summary>
    static GameConsts()
    {
        //用整型
        PARAM_SPEED = Animator.StringToHash("Speed");
        PARAM_SNEAK = Animator.StringToHash("Sneak");
        PARAM_SHOUT = Animator.StringToHash("Shout");
        PARAM_DOOROPEN = Animator.StringToHash("DoorOpen");
        STATE_LOCOMOTION = Animator.StringToHash("Locomotion");
        PARAM_ANGULARSPEED = Animator.StringToHash("AngularSpeed");
        PARAM_ISPLAYERINSIGHT = Animator.StringToHash("IsPlayerInsight");
        PARAM_DEAD = Animator.StringToHash("Dead");
        PARAM_ENDGAME = Animator.StringToHash("EndGame");
        STATE_SHOOT = Animator.StringToHash("WeaponShoot");
        STATE_RAISE = Animator.StringToHash("WeaponRaise");

    }
    #endregion

    #region Game Data Paramters
    //摄像机俯视高度
    public const float OVERLOOKHEIGHT = 5f;
    //头顶观察距离偏移量
    public const float ENEMY_EYES_HEIGHT = 1.7f;
    //玩家身体调节偏移量
    public const float PLAYER_BODY_HEIGHT = 1.7f;

    #endregion
}
