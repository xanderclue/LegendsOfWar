using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ManaBarNumeric : MonoBehaviour
{
    float curMP, maxMP;

    // Update is called once per frame
    void Update()
    {
        if (FindObjectOfType<HeroInfo>() != null)
        {
            curMP = Mathf.Round(FindObjectOfType<HeroInfo>().Mana);
            maxMP = Mathf.Round(FindObjectOfType<HeroInfo>().MaxMana);

        }

        gameObject.GetComponent<Text>().text = curMP + " / " + maxMP;

    }
}
