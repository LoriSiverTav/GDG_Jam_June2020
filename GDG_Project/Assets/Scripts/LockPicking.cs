using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LockPicking : MonoBehaviour
{
    public bool isPuzzling = true;
    
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(isPuzzling)
        {

        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Tumbler")
        {
            var lockParentComp = collision.gameObject.transform.parent.GetComponent<Lock>();

            lockParentComp.UpdateTumbler(collision.gameObject);
        }
    }
}
