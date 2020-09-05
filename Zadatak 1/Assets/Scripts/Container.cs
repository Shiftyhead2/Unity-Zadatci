using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Container : MonoBehaviour
{
    public Color currentColor;
    private SpriteRenderer mySprite;

    public int amountOfBoxes;

    // Start is called before the first frame update
    void Start()
    {
        mySprite = this.GetComponent<SpriteRenderer>();
        mySprite.color = currentColor;
    }

    public void IncreaseAmount()
    {
        amountOfBoxes++;
    }
}
