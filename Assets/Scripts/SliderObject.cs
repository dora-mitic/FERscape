using UnityEngine;
using UnityEngine.EventSystems;
using TMPro.Examples;

public class SliderObject : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Canvas canvas;

    RectTransform rectTransform;
    RectTransform parentRect;
    Vector2 startPos;
    float parentWidth;

    //string correctAnswer = "6.3";
    public TextFrequency textFrequency;

    float freq;


    public TeleType teleType;

    public GameObject glas;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();

        parentRect = transform.parent.GetComponent<RectTransform>();
        parentWidth = parentRect.rect.width;
        startPos = rectTransform.anchoredPosition;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
    }

    public void OnDrag(PointerEventData eventData)
   {
    // 1. Izračun normalnog pomaka
    Vector2 newPos = rectTransform.anchoredPosition + eventData.delta / canvas.scaleFactor;

    // 2. Granice kretanja po X osi
    float minX = startPos.x;
    float maxX = startPos.x + parentWidth;

    // 3. Ograniči kretanje slidera između minX i maxX
    newPos.x = Mathf.Clamp(newPos.x, minX, maxX);

    // 4. Zaključaj Y na početnu vrijednost (slider ide samo vodoravno)
    newPos.y = startPos.y;

    // 5. Postavi poziciju slidera
    rectTransform.anchoredPosition = newPos;

    // 6. Izračun koliko je slider daleko od početka (0 do 1)
    float t = (newPos.x - minX) / (maxX - minX);

    // 7. Pretvori taj omjer u frekvenciju (ovdje 100–1000)
    float minFreq = 0f;
    float maxFreq = 10f;
    freq = Mathf.Lerp(minFreq, maxFreq, t);

    // 8. Pošalji frekvenciju u text input
    textFrequency.NewFrequency(freq.ToString("F1"));
}


    public void OnEndDrag(PointerEventData eventData)
    {
        string inputText = textFrequency.get();
        
        // Prihvaća oba formata: "6.3" i "6,3"
        if (inputText == "6.3" || inputText == "6,3")
        {
            Debug.Log("Correct frequency set!");
            glas.SetActive(true);
            teleType.enabled = true;
        }
    }
}