using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxManager : MonoBehaviour
{

    public Color[] Colors;
    public Color currentColor;

    private SpriteRenderer myRenderer;

    private Rigidbody2D myRB;
    // Start is called before the first frame update
    void Start()
    {
        myRenderer = this.GetComponent<SpriteRenderer>();
        myRB = this.GetComponent<Rigidbody2D>();
        setColor();
    }


    private void setColor()
    {
        int color = Random.Range(0,Colors.Length);
        myRenderer.color = Colors[color];
        currentColor = Colors[color];
    }

    public void PickedUp(Transform newParent){
        transform.position = newParent.position;
        transform.SetParent(newParent);
        myRB.simulated = false;
    }

    public void Drop()
    {
        transform.SetParent(null);
        myRB.simulated = true;
        this.gameObject.SetActive(false);
    }

}
