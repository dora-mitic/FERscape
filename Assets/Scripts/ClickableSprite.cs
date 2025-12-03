using UnityEngine;

public class ClickableSprite : MonoBehaviour
{
    // Ova metoda se poziva kada korisnik klikne na collider ovog objekta
    private void OnMouseDown()
    {
        Debug.Log("Kliknuto na: " + gameObject.name);

        // Ovdje možeš dodati što god želiš da se dogodi kad klikneš:
        // npr. promjena boje:
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            sr.color = new Color(Random.value, Random.value, Random.value);
        }
    }
}
