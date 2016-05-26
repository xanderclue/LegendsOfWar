using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HealthBarNumeric : MonoBehaviour {
    float curHP, maxHP;
    
    // Update is called once per frame
    void Update () {
        if (FindObjectOfType<HeroInfo>() != null)
        {
            curHP = Mathf.Round(FindObjectOfType<HeroInfo>().HP);
            maxHP = Mathf.Round(FindObjectOfType<HeroInfo>().MAXHP);

        }

        gameObject.GetComponent<Text>().text = curHP + " / " + maxHP;

    }
}
