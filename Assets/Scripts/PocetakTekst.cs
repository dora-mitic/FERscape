using UnityEngine;
using System.Collections;

public class PocetakTekst : MonoBehaviour
{


    public GameObject tekst1;
    public GameObject tekst2;

    [SerializeField] public float prviTekst;
    [SerializeField] public float drugiTekst;

    IEnumerator MakniPrviTekst()
    {
        yield return new WaitForSeconds(prviTekst);
        tekst1.SetActive(false);
        yield return new WaitForSeconds(0.3f);
        tekst2.SetActive(true);
    }

    IEnumerator MakniDrugiTekst()
    {
        yield return new WaitForSeconds(drugiTekst);
        tekst2.SetActive(false);
    }

    public void Start()
    {
        tekst1.SetActive(true);
        StartCoroutine(MakniPrviTekst());
        StartCoroutine(MakniDrugiTekst());

    }

}
