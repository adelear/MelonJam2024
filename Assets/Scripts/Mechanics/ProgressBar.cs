using UnityEngine.UI;
using UnityEngine;

public class ProgressBar : MonoBehaviour
{
    [SerializeField] private Transform startPoint;
    [SerializeField] private Transform endPoint;
    [SerializeField] private Transform player;

    private Slider progressSlider;

    void Start()
    {
        progressSlider = GetComponent<Slider>();
        if (progressSlider == null)
        {
            progressSlider = gameObject.AddComponent<Slider>();
        }
        progressSlider.minValue = 0f;
        progressSlider.maxValue = 1f;
    }

    void Update()
    {
        if (startPoint != null && endPoint != null && player != null)
        {
            float totalDistance = Mathf.Abs(startPoint.position.x - endPoint.position.x);
            float playerDistance = Mathf.Abs(startPoint.position.x - player.position.x);
            float progress = Mathf.Clamp01(playerDistance / totalDistance);
            progressSlider.value = progress;
        }
    }
}
