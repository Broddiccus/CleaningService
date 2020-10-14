using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ShopItem : MonoBehaviour
{
    //VARIABLES
    public float[] increment;
    public Sprite image;
    public Image picture;
    public string nametxt;
    public Text nameplate;
    public float[] prices;
    public Text price;
    public string description;
    private int level = 0;
    private bool boughtout = false;

    private void Awake()
    {
        UIUpdate();
    }
    public void Loaded()
    {
        UIUpdate();
        for (int i = 0; i < level; i++)
        {
            GameManager.Instance.PowerupBuy(nametxt, increment[i]);
        }
    }
    public void UIUpdate()
    {
        if (!boughtout)
        {
            picture.sprite = image;
            nameplate.text = nametxt;
            price.text = "$" + prices[level];
        }
        else
        {
            picture.sprite = image;
            nameplate.text = nametxt;
            price.text = "SOLDOUT";
        }
    }
    public void Pressed()
    {
        if (!boughtout)
        {
            if (GameManager.Instance.score >= prices[level])
            {
                GameManager.Instance.PowerupBuy(nametxt, increment[level]);
                GameManager.Instance.score -= prices[level];
                UIManager.Instance.MenuMoneyUpdate(GameManager.Instance.score, GameManager.Instance.diamonds);
                //UPDATE DIAMONDS HERE LATER WHEN THEY'RE A THING
                if (level < prices.Length)
                {
                    level++;
                    UIUpdate();
                }
                else
                {
                    boughtout = true;
                    UIUpdate();
                }
            }
        }
    }
    public void DescPressed()
    {
        UIManager.Instance.ShowDescBox(nametxt, description);
    }
}
