using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseCircle : MonoBehaviour
{
    [SerializeField]
    SpriteRenderer mouseSprite;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        transform.position = mousePos;
        if (GameManager.instance.EnablePlant(mousePos))
        {
            mouseSprite.color = new Color(1, 1, 1, 0.75f);
        }
        else
        {
            mouseSprite.color = new Color(1, 1, 1, 0.25f);
        }
    }
}
