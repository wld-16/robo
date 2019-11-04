using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Looker : MonoBehaviour
{

    public GameObject lookAt;
    public GameObject panel;
    public float rangeShow = 40f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(transform.position, lookAt.transform.position) <= rangeShow)
        {
            panel.gameObject.SetActive(true);
            transform.rotation = Quaternion.LookRotation(transform.position - lookAt.transform.position);
        }
          else
        {
            panel.gameObject.SetActive(false);
        }
    }
}
