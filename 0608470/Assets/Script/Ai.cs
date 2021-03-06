using UnityEngine;
public class Ai : MonoBehaviour
{
    [Header("追蹤範圍"), Range(0, 8)]
    public float rangeTrack = 2;
    [Header("攻擊範圍"), Range(0, 5)]
    public float rangeAtt = 1;
    private Transform player;
    [Header("移動速度"),Range(0, 10)]
    public float speed = 5;
    [Header("攻擊特效")]
    public ParticleSystem psparticle;
    [Header("攻擊冷卻時間"), Range(0, 10)]
    public float attTime;
    [Header("攻擊傷害"), Range(0, 100)]
    public float attackdam;
    [Header("血量")]
    public float hp = 200;
    [Header("血條系統")]
    public HpManager hpManager;
    private float hpMax;
    [Header("經驗值"), Range(0, 200)]
    public float exp = 50;
    private bool Isdead = false;
    private Player _player;
    [Header("動畫元件")]
    public Animator ani;
    /// <summary>
    /// 計時器
    /// </summary>
    private float timer;
    private void Start()
    {
        hpMax = hp;
        //搜尋玩家座標並取得物件(物件名稱).變形
        player = GameObject.Find("主角").transform;
        _player = GetComponent<Player>();
    }
    
    //繪製圖示事件:輔助開發
    private void OnDrawGizmos()
    {   
        Gizmos.color = new Color(0, 0, 3, 0.35f);
        Gizmos.DrawSphere(transform.position, rangeTrack);
        Gizmos.color = new Color(1, 0, 0, 0.35f);
        Gizmos.DrawSphere(transform.position, rangeAtt);
    }
    private void Update()
    {
        Track();
    }
    /// <summary>
    /// 追蹤玩家
    /// </summary>
    private void Track()
    {
        if (Isdead) return;
        float tra = Vector3.Distance(transform.position, player.position);

        if(tra <= rangeAtt)
        {
            Attack();
        }
        else if(tra <= rangeTrack)
        {
            transform.position=Vector3.MoveTowards(transform.position,player.position, speed*Time.deltaTime);
        }
    }
    /// <summary>
    /// 攻擊
    /// </summary>
    private void Attack()
    {
        timer += Time.deltaTime;  // 累加時間
        //如果時間大於等於攻擊冷卻時間 就攻擊
        if(timer >= attTime)
        {
            timer = 0;   
            psparticle.Play();   
            Collider2D hit = Physics2D.OverlapCircle(transform.position, rangeAtt, 1 << 9 );
            hit.GetComponent<Player>().Hit(attackdam);
            this.ani.SetTrigger("emiAttack");
        }
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
        Destroy(gameObject,1.5f);
    }
}
