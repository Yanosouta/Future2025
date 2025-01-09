using UnityEngine;
using UnityEngine.Video;

public class moviemanagement : MonoBehaviour
{
    [SerializeField]
    VideoPlayer videoPlayer;

    void Start()
    {
        videoPlayer.loopPointReached += LoopPointReached;
        videoPlayer.Play();
    }

    public void LoopPointReached(VideoPlayer vp)
    {
        // “®‰æÄ¶Š®—¹‚Ìˆ—
        Debug.Log("‚¤‚ñ‚¿");
    }
}
