using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DescriptionText : MonoBehaviour
{
    private TextMeshProUGUI text;
    private Camera mainCamera;
    public void showItemText(Vector3 pos, ItemScript item)
    {
        if (!gameObject.activeInHierarchy)
        {
            gameObject.SetActive(true);
            pos += new Vector3(5, 5, 0);
            transform.position = mainCamera.WorldToScreenPoint(pos);
            text.text = item.description;
        }


    }

    public void stopShowText()
    {
        gameObject.SetActive(false);
        text.text = "";
    }
    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);
        text = GetComponentInChildren<TextMeshProUGUI>();
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
