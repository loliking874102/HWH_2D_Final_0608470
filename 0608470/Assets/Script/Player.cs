using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class Player : MonoBehaviour
{
    [Header("速度"),Range(0,10),Tooltip("調整角色速度")]
    public float speed = 10.5f;
    [Header("攻擊"), Tooltip("調整角色攻擊傷害")]
    public float attack = 10;
    [Header("名子"),Tooltip("角色名稱")]
    public string cName = "主角";
    [Header("虛擬搖桿")]
    public FixedJoystick joystick;
    [Header("變形元件")]
    public Transform tra;
    [Header("動畫元件")]
    public Animator ani;
    [Header("偵測範圍")]
    public float rangeAttack = 2.5f;
    [Header("音效來源")]
    public AudioSource aud;
    [Header("攻擊音效")]
    public AudioClip soundAttack;
    [Header("血量")]
    public float hp = 200;
    [Header("血條系統")]
    public HpManager hpManager; 
    [Header("攻擊傷害"), Range(0, 100)]
    public float attackdam;
    [Header("等級文字")]
    public Text textLv;
    private float hpMax;
    private bool Isdead = false;
    //事件:繪製圖示
    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 0, 0, 0.2f);
        Gizmos.DrawSphere(transform.position, rangeAttack);
    }
    /// <summary>
    /// 移動
    /// </summary>
    private void Move()
    {
        if (Isdead) return;
        //獲取虛擬搖桿水平Horizontal
        float h = joystick.Horizontal;
        //獲取虛擬搖桿垂直Vertical
        float v = joystick.Vertical;
        transform.Translate(h * speed * Time.deltaTime,0 ,0);
        ani.SetFloat("水平", h);
        ani.SetFloat("垂直", v);
    }
    public void Att()
    {
        if (Isdead) return;
        aud.PlayOneShot(soundAttack, 0.5f);
        this.ani.SetTrigger("PlayAttack");
        RaycastHit2D hit = Physics2D.CircleCast(transform.position, rangeAttack, -transform.up,  0,1 << 8);
        if (hit && hit.collider.tag == "敵人") hit.collider.GetComponent<Ai>().Hit(attack);
    }
    /// <summary>
    /// 被傷害系統
    /// </summary>
    /// <param name="damage">傷害</param>
    public void Hit(float damage)
    {
        hp -= damage;                             //扣除傷害
        hpManager.UpdateHPdata(hp, hpMax);        //更新血條
        StartCoroutine(hpManager.ShowDamage(damage));
        if (hp <= 0) Dead();
    }
    private void Dead()
    {
        hp = 0;
        Isdead = true;
        Invoke("Replay", 2);
    }
    private void Replay()
    {
        SceneManager.LoadScene("2Dapp");
    }
    private void Start()
    {
        hpMax = hp;    //更新血量最大值
    }
    private void Update()
    {
        Move();  //呼叫Move
    }
}
