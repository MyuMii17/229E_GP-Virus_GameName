using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeartManager : MonoBehaviour
{
    public List<Image> fHearts = new List<Image>();
    public GameObject fullHeart;
    private int damage;
    private static HeartManager StaticInstance = null;
    public static HeartManager GetStatic()
    {
        return StaticInstance;
    }
    void Awake()
    {
        if(StaticInstance != null)
        {
            Destroy(this.gameObject);
            return;
        }

        StaticInstance = this;
        damage = 0;
    }

    void Start()
    {
        for(int i = 0; i < 12; i++)
        {
            fHearts.Add(fullHeart.transform.GetChild(i).GetComponent<Image>());
        }
    }

    public void OnDamageSetHeart(float currentDamage)
    {
        if(currentDamage == 0.5f)
        {
            fHearts[damage].enabled = false;
            damage++;
        }
        if(currentDamage >= 1f)
        {
            fHearts[damage].enabled = false;
            damage++;
            fHearts[damage].enabled = false;
            damage++;
        }

    }
    public void OnHealSetHeart()
    {
        for(int i = 0; i < 12; i++)
        {
            fHearts[i].enabled = true;
        }
        damage = 0;
    }
}
