using System.Collections;
using System.Collections.Generic;
using System.Xml;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlantInteractionScript : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform PlayerTransform;
    public float Radius = 4f;
    public LayerMask Mask;
    public bool isPlayerClose = false;
    GameObject canvas;
    public string PlantCommonName;
    void Start()
    {
    }
    bool isInstantiatedCanvas = false;

    void CreateCanvas(bool playerClose)
    {
        if (!isInstantiatedCanvas)
        {
            canvas = new GameObject($"Canvas {PlantCommonName}");

            Canvas canvasComp = canvas.AddComponent<Canvas>();
            canvasComp.renderMode = RenderMode.WorldSpace; 

            CanvasScaler canvasScaler = canvas.AddComponent<CanvasScaler>();
            canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;

            canvas.AddComponent<GraphicRaycaster>();
            RectTransform crt = canvas.GetComponent<RectTransform>();
            //crt.pivot = new Vector2(0.5f, 0.5f);
            crt.position = new Vector2(0, 5);
            crt.sizeDelta = new Vector2(3, 1);
            

            GameObject textObj = new GameObject("Interact Text");
            TextMeshProUGUI text = textObj.AddComponent<TextMeshProUGUI>();
            text.text = "Press E to Interact";
            text.fontSize = 0.5f;
            text.color = Color.white;
            text.alignment = TextAlignmentOptions.Center;
            textObj.transform.SetParent(canvas.transform, false);

            RectTransform rectTransform = text.GetComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(3, 1);  // Width and height of the text box
            rectTransform.position = new Vector2(0, 0);
            rectTransform.anchoredPosition = new Vector2(0, 0);  // co on the canvas

            InteractUiScript uiScript = canvas.AddComponent<InteractUiScript>();
            uiScript.BushTransform = transform;
            uiScript.PlayerTransform = PlayerTransform;
            uiScript.CommonName = PlantCommonName;
            uiScript.isPlayerClose = playerClose;

            //canvas = new Canvas();
            //canvas.AddComponent<InteractUiScript>();
            //canvas.GetComponent<InteractUiScript>().isPlayerClose = true;
            //canvas.GetComponent<InteractUiScript>().CommonName = PlantCommonName;
            //canvas.GetComponent<InteractUiScript>().PlayerTransform = PlayerTransform;
            //canvas.GetComponent<InteractUiScript>().BushTransform = transform;
            //canvas.transform.position = new Vector3(transform.position.x, transform.position.y + 1.5f, transform.position.z);
            //canvas.transform.SetParent(transform);
            //canvas.gameObject.SetActive(true);
            //canvas.transform.forward = PlayerTransform.forward;
            //text.transform.SetParent(canvas.transform);
            //Instantiate(canvas);
            isInstantiatedCanvas = true;
        }

    }

    // Update is called once per frame
    void Update()
    {
        
        float delta = (transform.position - PlayerTransform.position).magnitude;
        if (delta <= Radius)
        {
            isPlayerClose = true;
            //InteractUiScript.isPlayerClose = true;
            CreateCanvas(true);
            if (canvas != null)
            {
                canvas.gameObject.SetActive(true);
                canvas.transform.forward = PlayerTransform.forward;
            }
        }
        else
        {
            isPlayerClose = false;
            if (canvas != null)
            {
                canvas.gameObject.SetActive(false);
                canvas.GetComponent<InteractUiScript>().isPlayerClose = false;
            }
            //InteractUiScript.isPlayerClose = false;
        }
    }
}
