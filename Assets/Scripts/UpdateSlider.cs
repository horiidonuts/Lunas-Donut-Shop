using UnityEngine;
using UnityEngine.UI;

public class UpdateSlider : MonoBehaviour
{
    CookDonut _cookDonut;
    private Slider _slider;
    void Start()
    {
        _cookDonut = GameObject.Find("lunadonut").GetComponent<CookDonut>();
        _slider = GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        _slider.value = _cookDonut.GetCookingMeter();
    }
}
