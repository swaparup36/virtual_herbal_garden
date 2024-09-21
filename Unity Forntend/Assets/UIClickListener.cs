using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIClickListener : MonoBehaviour
{
    // Start is called before the first frame update
    public PointerEventData pointerEventData;
    public EventSystem EventSystem;
    public GraphicRaycaster Raycaster;
    void Start()
    {
        
    }

    bool CheckMousePos()
    {
        //Vector2 mpos = Input.mousePosition - new Vector3(Screen.width/2, Screen.height/2, 0);
        //Vector3 pos = this.transform.position;
        //Vector2 dim = new Vector2(PlayerVideoRect.rect.width, PlayerVideoRect.rect.height);

        ////Debug.Log($"{mpos} {pos}");
        //if ((mpos.x >= pos.x && mpos.x <= pos.x + dim.x) && (mpos.y >= pos.y && mpos.y <= pos.y + dim.y))
        //{
        //    return true;
        //}
        //return false;

        pointerEventData = new PointerEventData(EventSystem);
        pointerEventData.position = Input.mousePosition;

        // Create a list to store Raycast results
        List<RaycastResult> results = new List<RaycastResult>();

        // Raycast using the GraphicRaycaster
        Raycaster.Raycast(pointerEventData, results);

        // Check if any UI elements were hit
        if (results.Count > 0)
        {
            bool found = false;
            foreach (var r in results)
            {
                if (r.gameObject.name.Equals("Guided"))
                {
                    found = true;
                    break;
                }
            }
            if (found)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            SceneManager.LoadScene("GuidedTour");
        }
    }
}
