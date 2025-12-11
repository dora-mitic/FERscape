using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.UI;

public struct zaposlenik
{
    public int id { get; set; }
    public string imeNastavnik { get; set; }
    public string prezimeNastavnik { get; set; }
    public string ured { get; set; }

    public zaposlenik(int id, string ime, string prezime, string ured)
    {
        this.id = id;
        this.imeNastavnik = ime;
        this.prezimeNastavnik= prezime;
        this.ured = ured;
    }
};


public class bazaZagonetka : MonoBehaviour
{
    [SerializeField] InputField inputField;
    [SerializeField] Transform tablicaHolder = null;
    [SerializeField] GameObject noviRedak = null;
    [SerializeField] GameObject textID = null;
    [SerializeField] GameObject textIme = null;
    [SerializeField] GameObject textPrezime = null;
    [SerializeField] GameObject textUred = null;

    public List<zaposlenik> listaZaposlenika = new List<zaposlenik>();


    private void Start() // na pocektu
    {
        dodajPodatke();
        prikaziTablicu("");
        noviRedak.SetActive(false);
    }
    public void pokreniQuery() // kada je pritisnu gumb
    {
        string query = inputField.text;
        Debug.Log("Pritisnut je gumb!");
        noviRedak.SetActive(true);
        prikaziTablicu(query);
        noviRedak.SetActive(false);
    }

    private void prikaziTablicu(string filtar) // ne smije biti vise od zaglavlja + 4 redaka
    {

        List<zaposlenik> zaposlenici = filtriraj(filtar);

        Debug.Log("Filtrirano!");
        foreach(zaposlenik z in zaposlenici)
        {
            Debug.Log(z.id.ToString() + ": " + z.prezimeNastavnik + " " + z.imeNastavnik + ", ured: " + z.ured);
        }

        foreach(Transform child in tablicaHolder)
        {
            Destroy(child.gameObject);
        }

        // napravi za gresku!!!

        for (int i = 0; i < zaposlenici.Count && i < 4; i++)
        {
            Text noviTekst = textID.GetComponentInChildren<Text>();
            noviTekst.text = zaposlenici[i].id.ToString();

            noviTekst = textIme.GetComponentInChildren<Text>();
            noviTekst.text = zaposlenici[i].imeNastavnik;

            noviTekst = textPrezime.GetComponentInChildren<Text>();
            noviTekst.text = zaposlenici[i].prezimeNastavnik;

            noviTekst = textUred.GetComponentInChildren<Text>();
            noviTekst.text = zaposlenici[i].ured;

            Instantiate(noviRedak, tablicaHolder);
        }
    }


    private List<zaposlenik> filtriraj(string filtar)
    {
        Debug.Log("Krenuo s filtracijom!");
        if (filtar  == "") { return listaZaposlenika; }

        List<zaposlenik> greska = new List<zaposlenik>();
        zaposlenik gr = new zaposlenik();
        gr.imeNastavnik = "greska";
        greska.Add(gr);

        if (!filtar.StartsWith("where ", StringComparison.OrdinalIgnoreCase)) { return greska; }

        List<zaposlenik> lista = listaZaposlenika;

        filtar = filtar.Substring(6); // makni where iz filtera
        do
        {

            int indeksKraja = filtar.IndexOf("and", StringComparison.OrdinalIgnoreCase);
            string izraz; int duljinaIzraza = 0;
            if (indeksKraja < 0) { izraz = filtar; }
            else { izraz = filtar.Substring(0, indeksKraja); duljinaIzraza = izraz.Length; }

            while ( izraz[0] == ' ') { izraz = izraz.Substring(1); } // makni razmake na pocetku

            Debug.Log(izraz);

            // dohvacanje argumenata u izrazu
            string prviArg;
            char[] anyOf = { ' ', '=', '<', '>' }; indeksKraja = izraz.IndexOfAny(anyOf);
            if (indeksKraja < 0) { Debug.Log("Greska!"); return greska; }
            else { prviArg = izraz.Substring(0, indeksKraja); } // prvi argument u izrazu
            izraz = izraz.Substring(prviArg.Length);

            string oper = ""; indeksKraja = 0;
            string validChars = "0123456789abcdefghijklmnoqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ\"\'";
            int brojac = 0;
            while (!validChars.Contains(izraz[brojac])){
                if (izraz[brojac] != ' ') oper += izraz[brojac];
                indeksKraja++; brojac++;
            }
            izraz = izraz.Substring(indeksKraja);

            string drugiArg;
            if (izraz[0] == '\'')
            {
                indeksKraja = izraz.Substring(1).IndexOf('\'');
                if (indeksKraja < 0) { Debug.Log("Losa sintaksa!"); return greska; }
                drugiArg = izraz.Substring(1, indeksKraja);
                izraz = izraz.Substring(indeksKraja + 2);
            } if (izraz[0] == '\"')
            {
                indeksKraja = izraz.Substring(1).IndexOf('\"');
                if (indeksKraja < 0) { Debug.Log("Losa sintaksa!"); return greska; }
                drugiArg = izraz.Substring(1, indeksKraja);
                izraz = izraz.Substring(indeksKraja + 2);
            } else
            {
                indeksKraja = izraz.Substring(1).IndexOf(' ');
                if (indeksKraja < 0) { drugiArg = izraz; izraz = ""; }
                else { drugiArg = izraz.Substring(0, indeksKraja); izraz = izraz.Substring(indeksKraja); }
                
            }


            Debug.Log(prviArg);
            Debug.Log(oper);
            Debug.Log(drugiArg);

            while (izraz.Length > 0 && izraz[0] == ' ') { izraz = izraz.Substring(1); }
            if (izraz != "") { Debug.Log(izraz); return greska; }

            // filtar
            List<zaposlenik> tmp = new List<zaposlenik>();
            for (int i = 0; i < lista.Count; i++)
            {
                Debug.Log(lista[i].id.ToString() + ": " + lista[i].imeNastavnik + " " + lista[i].prezimeNastavnik + ", " + lista[i].ured);
                int rez;
                // koja varijabla se provjerava
                switch (prviArg.ToLower())
                {
                    case "id":
                        int filtarID;
                        if (!int.TryParse(drugiArg, out filtarID)) { Debug.Log("Nije broj!"); return greska; }
                        rez = provjeri(lista[i].id, filtarID, oper);
                        break;
                    case "imenastavnik":
                        rez = provjeri(lista[i].imeNastavnik, drugiArg, oper);
                        break;
                    case "prezimenastavnik":
                        rez = provjeri(lista[i].prezimeNastavnik, drugiArg, oper);
                        break;
                    case "ured":
                        rez = provjeri(lista[i].ured, drugiArg, oper);
                        break;
                    default:
                        Debug.Log("Krivi prvi argument!");
                        return greska;
                }

                if (rez == 1) { tmp.Add(lista[i]); Debug.Log("Dodan element!"); }
                else if (rez == -1) { return greska; }
            }
            Debug.Log(tmp.Count);
            lista = tmp;


            // priprema za sljedeci izraz
            if (duljinaIzraza == 0) { filtar = ""; }
            else { filtar = filtar.Substring(duljinaIzraza - 1 + 4); }

            Debug.Log(filtar);
        }
        while (!(filtar == ""));

        return lista;
    }


    private int provjeri(string prvi, string drugi, string oper)
    {
        int rez = prvi.CompareTo(drugi);

        switch (oper)
        {
            case ">=":
                if (rez >= 0) { return 1; }
                break;
            case ">":
                if (rez > 0) { return 1; }
                break;
            case "<=":
                if (rez <= 0) { return 1; }
                break;
            case "<":
                if (rez < 0) { return 1; }
                break;
            case "=":
                if (rez == 0) { return 1; }
                break;
            case "<>":
                if (rez != 0) { return 1; }
                break;
            default:
                Debug.Log("Krivi operator!");
                return -1;

        }
        return 0;
    }
    private int provjeri(int prvi, int drugi, string oper)
    {
        int rez = prvi - drugi;

        switch (oper)
        {
            case ">=":
                if (rez >= 0) { return 1; }
                break;
            case ">":
                if (rez > 0) { return 1; }
                break;
            case "<=":
                if (rez <= 0) { return 1; }
                break;
            case "<":
                if (rez < 0) { return 1; }
                break;
            case "=":
                if (rez == 0) { return 1; }
                break;
            case "<>":
                if (rez != 0) { return 1; }
                break;
            default:
                Debug.Log("Krivi operator!");
                return -1;

        }
        return 0;
    }


    private void dodajPodatke()
    {
        listaZaposlenika.Add(new zaposlenik(1, "Ana", "Anic", "D-302"));
        listaZaposlenika.Add(new zaposlenik(2, "Marko", "Maric", "D-303"));
        listaZaposlenika.Add(new zaposlenik(3, "Tomo", "Tomic", "D-304"));
        listaZaposlenika.Add(new zaposlenik(4, "Ema", "Emic", "D-202"));
        listaZaposlenika.Add(new zaposlenik(5, "Ana", "Teic", "D-102"));
    }
}

