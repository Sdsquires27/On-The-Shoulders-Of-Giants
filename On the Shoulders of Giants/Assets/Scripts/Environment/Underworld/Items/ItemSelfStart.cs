using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSelfStart : ItemScript
{
    // used for testing

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(wait());
    }

    private IEnumerator wait()
    {
        yield return new WaitForSeconds(.1f);
        setItem(1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
