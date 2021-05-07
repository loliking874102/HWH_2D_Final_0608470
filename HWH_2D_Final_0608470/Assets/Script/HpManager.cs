using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HpManager : MonoBehaviour
{
    [Header("血條")]
    public Image Hpimage ;
    [Header("傷害數值")]
    public RectTransform rectDamage;
    /// <summary>
    /// 血條與血量MAX更新血條
    /// </summary>
    /// <param name="hp">血量</param>
    /// <param name="hpMax">血量MAX</param>
    public void UpdateHPdata(float hp , float hpMax)
    {
        //血量.填滿數值 = 當前血量/血量最大值
        Hpimage.fillAmount = hp / hpMax;    
    }
    public IEnumerator ShowDamage(float damage)
    {
        RectTransform rect = Instantiate(rectDamage, transform);    //生成傷害在血條系統裡
        rect.anchoredPosition = new Vector2(5, 275);    //指定座標
        rect.GetComponent<Text>().text = damage.ToString();
        float y = rect.anchoredPosition.y;


        while (y < 400)
        {
            y += 20;
            rect.anchoredPosition = new Vector2(5, y);
            yield return new WaitForSeconds(0.02f);
        }
        Destroy(rect.gameObject, 0.5f);             //刪除傷害物件
    }
}
