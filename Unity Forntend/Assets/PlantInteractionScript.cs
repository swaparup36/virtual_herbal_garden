using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlantInteractionScript : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform PlayerTransform;
    public float Radius = 4f;
    public LayerMask Mask;
    public bool isPlayerClose = false;
    public Canvas Canvas;
    void Start()
    {
        
    }


    // Update is called once per frame
    void Update()
    {
        
        float delta = (transform.position - PlayerTransform.position).magnitude;
        if (delta <= Radius)
        {
            isPlayerClose = true;
            InteractUiScript.isPlayerClose = true;
            Canvas.gameObject.SetActive(true);
            Canvas.transform.forward = PlayerTransform.forward;
        }else
        {
            isPlayerClose = false;
            InteractUiScript.isPlayerClose = false;
            Canvas.gameObject.SetActive(false);
        }
    }
}
