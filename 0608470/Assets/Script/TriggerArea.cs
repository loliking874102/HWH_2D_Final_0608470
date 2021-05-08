using UnityEngine;

public class TriggerArea : MonoBehaviour
{
    [Header("控制石頭數量")]
    public GameObject[] rock;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "石頭")
        {
            rock[0].SetActive(false);
            rock[1].SetActive(false);
        }
    }
}
