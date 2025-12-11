using UnityEngine;

public class OnClickZoom : MonoBehaviour
{
    public GameObject Zoomed;
    public GameObject Zoomed1;

    private void OnMouseDown()
    {
        Zoomed.SetActive(true);
        Zoomed1.SetActive(false);
    }
}
