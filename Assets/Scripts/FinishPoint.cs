using UnityEngine;
using GameAnalyticsSDK;

public class FinishPoint : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete, "Level1");
    }    
}
