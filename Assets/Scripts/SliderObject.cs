using UnityEngine;
using UnityEngine.EventSystems;
using TMPro.Examples;

public class SliderObject : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Canvas canvas;

    private RectTransform rectTransform;
    private RectTransform parentRect;
    private Vector2 startPos;
    private float parentWidth;

    public TextFrequency textFrequency;
    private float freq;

    public TeleType teleType;
    public GameObject glas;

    [Header("Audio")]
    public AudioSource sumAudio;   // AudioSource s clipom šuma (Loop ON, Play On Awake OFF)

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();

        parentRect = transform.parent.GetComponent<RectTransform>();
        parentWidth = parentRect.rect.width;
        startPos = rectTransform.anchoredPosition;

        // Po defaultu ništa ne svira
        if (sumAudio != null && sumAudio.isPlaying)
            sumAudio.Stop();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // Ne palimo šum ovdje obavezno, nego u OnDrag kad freq postane > 0
        // (jer na početku može biti 0)
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

        // 7. Pretvori taj omjer u frekvenciju (0–10)
        float minFreq = 0f;
        float maxFreq = 10f;
        freq = Mathf.Lerp(minFreq, maxFreq, t);

        // 8. Pošalji frekvenciju u text input
        if (textFrequency != null)
            textFrequency.NewFrequency(freq.ToString("F1"));

        // ✅ Šum: svira dok slajdaš čim freq > 0
        if (sumAudio != null)
        {
            if (freq > 0.01f)
            {
                if (!sumAudio.isPlaying)
                    sumAudio.Play();
            }
            // Namjerno NE gasimo šum kad se vrati na 0 tijekom draga,
            // jer želiš da prestane samo ako je FINALNI drag na 0.
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        string inputText = (textFrequency != null) ? textFrequency.get() : "";

        // 1) Ako je FINALNA vrijednost 0 -> ugasi šum i ništa drugo
        if (freq <= 0.01f)
        {
            if (sumAudio != null && sumAudio.isPlaying)
                sumAudio.Stop();

            return;
        }

        // 2) Ako je točna frekvencija -> ugasi šum i pusti glas
        if (inputText == "1.3" || inputText == "1,3")
        {
            if (sumAudio != null && sumAudio.isPlaying)
                sumAudio.Stop();

            Debug.Log("Correct frequency set!");
            if (glas != null) glas.SetActive(true);
            if (teleType != null) teleType.enabled = true;

            return;
        }

        // 3) Inače (nije 0 i nije točno) -> šum OSTANE svirati
    }
}
