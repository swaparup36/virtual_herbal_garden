using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class InteractUiScript : MonoBehaviour
{
    public Transform BushTransform;
    public Transform PlayerTransform;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(BushTransform.position.x, (BushTransform.position.y + 1.5f), BushTransform.position.z);
        transform.SetParent(BushTransform);
    }

    // Update is called once per frame
    void Update()
    {
    }
}
