using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(A());
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    IEnumerator A()
    {
        yield return B();
        print("A"); 
    }

    IEnumerator B()
    {
        yield return new WaitForSeconds(1);
        print("B");
    }
}
