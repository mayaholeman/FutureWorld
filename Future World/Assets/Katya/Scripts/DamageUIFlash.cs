using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageUIFlash : MonoBehaviour
{
    public float flashSpeed = 5f;
    public Color flashColour = new Color(1f, 0f, 0f, 0.1f);

    private Image image;

    bool damaged;

    // Start is called before the first frame update
    void Start()
    {
        image = gameObject.GetComponent<Image>();
        damaged = false;
    }

    void Update()
    {
        if (damaged)
        {
            image.color = flashColour;
        }
        else
        {
            image.color = Color.Lerp(image.color, Color.clear, flashSpeed * Time.deltaTime);
        }

        damaged = false;
    }

    public void TakeDamage()
    {
        damaged = true;
    }
}
