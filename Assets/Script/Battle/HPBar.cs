using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HPBar : MonoBehaviour
{
    [SerializeField] private Slider hpSlider;

    public void SetupHpBar(int MaxHp, int currentHp)
    {
        hpSlider.maxValue = MaxHp;
        hpSlider.value = currentHp;
    }

    public IEnumerator UpdateHpAsync(int newHp)
    {
        float currentHP = hpSlider.value;
        
        if (newHp < currentHP)
        {
            float changeAmount = currentHP - newHp;

            while (currentHP - newHp > Mathf.Epsilon)
            {
                currentHP -= changeAmount * Time.deltaTime;
                hpSlider.value = currentHP;
                yield return null;
            }
        }
        else
        {
            float changeAmount = newHp - currentHP;

            while (newHp - currentHP > Mathf.Epsilon)
            {
                currentHP += changeAmount * Time.deltaTime;
                hpSlider.value = currentHP;
                yield return null;
            }           
        }
        hpSlider.value = newHp;
    }
}
