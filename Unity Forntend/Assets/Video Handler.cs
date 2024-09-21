using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class VideoHandler : MonoBehaviour
{
    // Start is called before the first frame update
    public VideoPlayer Player;
    public TMP_Text PlayButtonText;
    public RectTransform PlayerVideoRect;
    public Button PlayButton;
    public Button ReplayButton;
    public Canvas Canvas;
    private GraphicRaycaster Raycaster;
    private EventSystem EventSystem;
    private PointerEventData pointerEventData;
    bool playing;
    bool isUrlSet = false;
    public static string url = null;
    void Start()
    {
        playing = true;
        Raycaster = Canvas.GetComponent<GraphicRaycaster>();
        EventSystem = EventSystem.current;
        //Debug.Log(PlayerPrefs.GetString("videoUrl"));
        Player.url = PlayerPrefs.GetString("videoUrl");
    }


    public void PlayVideo()
    {
        if (playing)
        {
            Player.Pause();
        }else
        {
            Player.Play();
        }
        playing=!playing;
    }

    public void Replay()
    {
        Player.Stop();
        Player.Play();
        playing=true;
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
        if (results.Count > 0 )
        {
            bool found = false;
            foreach ( var r in results)
            {
                if (r.gameObject.name.Equals("Video"))
                {
                    found = true;
                    break;
                }
            }
            if (found)
            {
                return true;
            }else
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
        if (!isUrlSet && !PlayerPrefs.GetString("videoUrl").Equals(""))
        {
            Debug.Log(PlayerPrefs.GetString("videoUrl"));
            Player.url = PlayerPrefs.GetString("videoUrl");
            isUrlSet = true;
        }

        PlayButtonText.text = playing ? "Pause" : "Play";
        if (CheckMousePos())
        {
            PlayButton.gameObject.SetActive(true);
            ReplayButton.gameObject.SetActive(true);
        }
        else
        {
            PlayButton.gameObject.SetActive(false);
            ReplayButton.gameObject.SetActive(false);
        }
        
    }
}
