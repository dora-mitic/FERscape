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
        listaZaposlenika.Add(new zaposlenik(1, "Marta", "Vuletić", "C-549"));
        listaZaposlenika.Add(new zaposlenik(2, "Dora", "Tomić", "D-317"));
        listaZaposlenika.Add(new zaposlenik(3, "Vjekoslav", "Radović", "C-542"));
        listaZaposlenika.Add(new zaposlenik(4, "Božo", "Car", "D-135"));
        listaZaposlenika.Add(new zaposlenik(5, "Sanja", "Jukić", "D-256"));
        listaZaposlenika.Add(new zaposlenik(6, "Lucija", "Grgurić", "D-322"));
        listaZaposlenika.Add(new zaposlenik(7, "Josipa", "Janković", "C-440"));
        listaZaposlenika.Add(new zaposlenik(8, "Marica", "Jerković", "D-206"));
        listaZaposlenika.Add(new zaposlenik(9, "Dejan", "Barić", "D-364"));
        listaZaposlenika.Add(new zaposlenik(10, "Hrvoje", "Meštrović", "C-836"));
        listaZaposlenika.Add(new zaposlenik(11, "Marinko", "Nekić", "C-332"));
        listaZaposlenika.Add(new zaposlenik(12, "Bruno", "Kolić", "C-234"));
        listaZaposlenika.Add(new zaposlenik(13, "Ankica", "Vukušić", "D-111"));
        listaZaposlenika.Add(new zaposlenik(14, "Ana", "Kordić", "D-344"));
        listaZaposlenika.Add(new zaposlenik(15, "Danijel", "Tolić", "C-542"));
        listaZaposlenika.Add(new zaposlenik(16, "Mirela", "Milković", "D-324"));
        listaZaposlenika.Add(new zaposlenik(17, "Marinko", "Samardžić", "C-917"));
        listaZaposlenika.Add(new zaposlenik(18, "Daniel", "Šimić", "C-308"));
        listaZaposlenika.Add(new zaposlenik(19, "Antonio", "Babić", "C-322"));
        listaZaposlenika.Add(new zaposlenik(20, "Igor", "Horvat", "D-124"));
        listaZaposlenika.Add(new zaposlenik(21, "Irena", "Lovrić", "C-241"));
        listaZaposlenika.Add(new zaposlenik(22, "Štefanija", "Zelić", "C-405"));
        listaZaposlenika.Add(new zaposlenik(23, "Ljubica", "Antunović", "C-405"));
        listaZaposlenika.Add(new zaposlenik(24, "Mato", "Modrić", "D-315"));
        listaZaposlenika.Add(new zaposlenik(25, "Neven", "Špoljarić", "C-148"));
        listaZaposlenika.Add(new zaposlenik(26, "Natalija", "Živković", "C-842"));
        listaZaposlenika.Add(new zaposlenik(27, "Sara", "Pavlić", "D-349"));
        listaZaposlenika.Add(new zaposlenik(28, "Noa", "Meštrović", "C-827"));
        listaZaposlenika.Add(new zaposlenik(29, "Šime", "Andrić", "C-939"));
        listaZaposlenika.Add(new zaposlenik(30, "Jasmina", "Ivanović", "C-202"));
        listaZaposlenika.Add(new zaposlenik(31, "Ana", "Šarić", "D-326"));
        listaZaposlenika.Add(new zaposlenik(32, "Marinko", "Ružić", "D-264"));
        listaZaposlenika.Add(new zaposlenik(33, "Andrea", "Radović", "D-233"));
        listaZaposlenika.Add(new zaposlenik(34, "Mihael", "Jović", "C-735"));
        listaZaposlenika.Add(new zaposlenik(35, "Željka", "Pavić", "C-638"));
        listaZaposlenika.Add(new zaposlenik(36, "Slavica", "Kovačević", "C-815"));
        listaZaposlenika.Add(new zaposlenik(37, "Katica", "Tomašić", "D-207"));
        listaZaposlenika.Add(new zaposlenik(38, "Katica", "Benčić", "C-555"));
        listaZaposlenika.Add(new zaposlenik(39, "Zlata", "Barić", "C-643"));
        listaZaposlenika.Add(new zaposlenik(40, "Krešimir", "Bogdanović", "C-945"));
        listaZaposlenika.Add(new zaposlenik(41, "Nina", "Lovrić", "D-122"));
        listaZaposlenika.Add(new zaposlenik(42, "Mirela", "Lukić", "D-152"));
        listaZaposlenika.Add(new zaposlenik(43, "Katica", "Stojanović", "D-174"));
        listaZaposlenika.Add(new zaposlenik(44, "Lovro", "Glavaš", "C-710"));
        listaZaposlenika.Add(new zaposlenik(45, "Sonja", "Turkalj", "C-801"));
        listaZaposlenika.Add(new zaposlenik(46, "Nika", "Delić", "D-369"));
        listaZaposlenika.Add(new zaposlenik(47, "Štefanija", "Marijanović", "D-112"));
        listaZaposlenika.Add(new zaposlenik(48, "Jure", "Mišić", "D-113"));
        listaZaposlenika.Add(new zaposlenik(49, "Ema", "Katić", "D-371"));
        listaZaposlenika.Add(new zaposlenik(50, "Anton", "Perić", "C-845"));
        listaZaposlenika.Add(new zaposlenik(51, "Luka", "Radman", "D-110"));
        listaZaposlenika.Add(new zaposlenik(52, "Zoran", "Rončević", "C-833"));
        listaZaposlenika.Add(new zaposlenik(53, "Andreja", "Vukušić", "D-172"));
        listaZaposlenika.Add(new zaposlenik(54, "Kristijan", "Turković", "C-917"));
        listaZaposlenika.Add(new zaposlenik(55, "David", "Pavičić", "C-129"));
        listaZaposlenika.Add(new zaposlenik(56, "Jan", "Mitrović", "C-351"));
        listaZaposlenika.Add(new zaposlenik(57, "Štefanija", "Matković", "D-341"));
        listaZaposlenika.Add(new zaposlenik(58, "Helena", "Golub", "D-319"));
        listaZaposlenika.Add(new zaposlenik(59, "Magdalena", "Turkalj", "D-175"));
        listaZaposlenika.Add(new zaposlenik(60, "Božica", "Kovač", "D-373"));
        listaZaposlenika.Add(new zaposlenik(61, "Irena", "Dragičević", "D-277"));
        listaZaposlenika.Add(new zaposlenik(62, "Paula", "Vuk", "C-630"));
        listaZaposlenika.Add(new zaposlenik(63, "Danijel", "Jurić", "D-215"));
        listaZaposlenika.Add(new zaposlenik(64, "Dragica", "Brkić", "D-232"));
        listaZaposlenika.Add(new zaposlenik(65, "Marin", "Begović", "C-459"));
        listaZaposlenika.Add(new zaposlenik(66, "Mario", "Hodak", "D-268"));
        listaZaposlenika.Add(new zaposlenik(67, "Damir", "Kraljević", "C-542"));
        listaZaposlenika.Add(new zaposlenik(68, "Lorena", "Špoljarić", "C-602"));
        listaZaposlenika.Add(new zaposlenik(69, "Maja", "Savić", "C-549"));
        listaZaposlenika.Add(new zaposlenik(70, "Karlo", "Lončarić", "D-347"));
        listaZaposlenika.Add(new zaposlenik(71, "Miljenko", "Tomašević", "D-110"));
        listaZaposlenika.Add(new zaposlenik(72, "Štefica", "Filipović", "C-407"));
        listaZaposlenika.Add(new zaposlenik(73, "Mirjana", "Ivanišević", "C-703"));
        listaZaposlenika.Add(new zaposlenik(74, "Gordana", "Martinović", "C-537"));
        listaZaposlenika.Add(new zaposlenik(75, "Zdenko", "Bogdanović", "C-439"));
        listaZaposlenika.Add(new zaposlenik(76, "Marta", "Maričić", "D-108"));
        listaZaposlenika.Add(new zaposlenik(77, "Božo", "Kovač", "C-319"));
        listaZaposlenika.Add(new zaposlenik(78, "Ružica", "Dujmović", "C-820"));
        listaZaposlenika.Add(new zaposlenik(79, "Matea", "Gudelj", "D-250"));
        listaZaposlenika.Add(new zaposlenik(80, "Vanja", "Rukavina", "D-179"));
        listaZaposlenika.Add(new zaposlenik(81, "Leon", "Magdić", "D-109"));
        listaZaposlenika.Add(new zaposlenik(82, "Manda", "Brkić", "D-233"));
        listaZaposlenika.Add(new zaposlenik(83, "Ivano", "Milković", "D-106"));
        listaZaposlenika.Add(new zaposlenik(84, "Toni", "Kovač", "C-911"));
        listaZaposlenika.Add(new zaposlenik(85, "Sandra", "Juričić", "D-120"));
        listaZaposlenika.Add(new zaposlenik(86, "Melita", "Cvitković", "D-258"));
        listaZaposlenika.Add(new zaposlenik(87, "Anđela", "Erceg", "C-741"));
        listaZaposlenika.Add(new zaposlenik(88, "Miroslav", "Abramović", "C-436"));
        listaZaposlenika.Add(new zaposlenik(89, "Vito", "Šimunić", "D-300"));
        listaZaposlenika.Add(new zaposlenik(90, "Zorka", "Ivančić", "C-449"));
        listaZaposlenika.Add(new zaposlenik(91, "Saša", "Crnković", "D-336"));
        listaZaposlenika.Add(new zaposlenik(92, "Anka", "Banović", "D-151"));
        listaZaposlenika.Add(new zaposlenik(93, "Jela", "Vrdoljak", "C-833"));
        listaZaposlenika.Add(new zaposlenik(94, "Željko", "Ivanišević", "C-727"));
        listaZaposlenika.Add(new zaposlenik(95, "Mile", "Lončarić", "D-235"));
        listaZaposlenika.Add(new zaposlenik(96, "Anita", "Šimunić", "D-369"));
        listaZaposlenika.Add(new zaposlenik(97, "Monika", "Maras", "C-739"));
        listaZaposlenika.Add(new zaposlenik(98, "Vlado", "Vukić", "C-743"));
        listaZaposlenika.Add(new zaposlenik(99, "Slavko", "Ban", "D-168"));
        listaZaposlenika.Add(new zaposlenik(100, "Nada", "Modrić", "D-248"));
        listaZaposlenika.Add(new zaposlenik(101, "Kata", "Meštrović", "C-908"));
        listaZaposlenika.Add(new zaposlenik(102, "Borna", "Čović", "C-954"));
        listaZaposlenika.Add(new zaposlenik(103, "Klara", "Radman", "D-266"));
        listaZaposlenika.Add(new zaposlenik(104, "Željka", "Rukavina", "C-739"));
        listaZaposlenika.Add(new zaposlenik(105, "Petra", "Šimunović", "C-544"));
        listaZaposlenika.Add(new zaposlenik(106, "Jozo", "Radošević", "C-323"));
        listaZaposlenika.Add(new zaposlenik(107, "Lana", "Lončar", "D-253"));
        listaZaposlenika.Add(new zaposlenik(108, "Marina", "Kolarić", "D-110"));
        listaZaposlenika.Add(new zaposlenik(109, "Mateo", "Ljubić", "C-250"));
        listaZaposlenika.Add(new zaposlenik(110, "Marko", "Josipović", "D-339"));
        listaZaposlenika.Add(new zaposlenik(111, "Anka", "Jozić", "C-930"));
        listaZaposlenika.Add(new zaposlenik(112, "Ivo", "Maričić", "D-249"));
        listaZaposlenika.Add(new zaposlenik(113, "Leo", "Jurišić", "C-315"));
        listaZaposlenika.Add(new zaposlenik(114, "Krešimir", "Petričević", "C-758"));
        listaZaposlenika.Add(new zaposlenik(115, "Borna", "Špoljarić", "C-444"));
        listaZaposlenika.Add(new zaposlenik(116, "Anka", "Jovanović", "D-338"));
        listaZaposlenika.Add(new zaposlenik(117, "Maja", "Sever", "C-752"));
        listaZaposlenika.Add(new zaposlenik(118, "Viktor", "Matić", "C-513"));
        listaZaposlenika.Add(new zaposlenik(119, "Zoran", "Mandić", "D-242"));
        listaZaposlenika.Add(new zaposlenik(120, "Šime", "Bašić", "D-139"));
        listaZaposlenika.Add(new zaposlenik(121, "Petra", "Matijević", "D-248"));
        listaZaposlenika.Add(new zaposlenik(122, "Mirjana", "Ljubičić", "C-638"));
        listaZaposlenika.Add(new zaposlenik(123, "Mara", "Horvat", "D-367"));
        listaZaposlenika.Add(new zaposlenik(124, "Marijana", "Šoštarić", "C-413"));
        listaZaposlenika.Add(new zaposlenik(125, "Aleksandar", "Glavaš", "D-133"));
        listaZaposlenika.Add(new zaposlenik(126, "Mira", "Ivković", "D-243"));
        listaZaposlenika.Add(new zaposlenik(127, "Darko", "Josipović", "D-140"));
        listaZaposlenika.Add(new zaposlenik(128, "Laura", "Mišić", "D-128"));
        listaZaposlenika.Add(new zaposlenik(129, "Ivanka", "Orešković", "D-143"));
        listaZaposlenika.Add(new zaposlenik(130, "Anto", "Petrović", "D-332"));
        listaZaposlenika.Add(new zaposlenik(131, "Ana", "Savić", "C-504"));
        listaZaposlenika.Add(new zaposlenik(132, "Tomislav", "Ivić", "D-222"));
        listaZaposlenika.Add(new zaposlenik(133, "Mia", "Ban", "D-156"));
        listaZaposlenika.Add(new zaposlenik(134, "Stjepan", "Janković", "D-361"));
        listaZaposlenika.Add(new zaposlenik(135, "Leona", "Rajković", "D-260"));
        listaZaposlenika.Add(new zaposlenik(136, "Blaženka", "Petković", "D-246"));
        listaZaposlenika.Add(new zaposlenik(137, "Pero", "Baričević", "D-112"));
        listaZaposlenika.Add(new zaposlenik(138, "Leona", "Tomašević", "C-725"));
        listaZaposlenika.Add(new zaposlenik(139, "Anka", "Turkalj", "D-377"));
        listaZaposlenika.Add(new zaposlenik(140, "Nikolina", "Rašić", "C-703"));
        listaZaposlenika.Add(new zaposlenik(141, "Milena", "Miloš", "D-162"));
        listaZaposlenika.Add(new zaposlenik(142, "Mijo", "Brajković", "C-444"));
        listaZaposlenika.Add(new zaposlenik(143, "Martina", "Miloš", "D-346"));
        listaZaposlenika.Add(new zaposlenik(144, "Milka", "Hodak", "D-312"));
        listaZaposlenika.Add(new zaposlenik(145, "Gabriel", "Meštrović", "C-945"));
        listaZaposlenika.Add(new zaposlenik(146, "Iva", "Medić", "D-103"));
        listaZaposlenika.Add(new zaposlenik(147, "Božo", "Marinković", "C-825"));
        listaZaposlenika.Add(new zaposlenik(148, "Danijela", "Mišić", "D-302"));
        listaZaposlenika.Add(new zaposlenik(149, "Antonio", "Lukić", "D-119"));
        listaZaposlenika.Add(new zaposlenik(150, "Matej", "Dragičević", "D-232"));
        listaZaposlenika.Add(new zaposlenik(151, "Dražen", "Duvnjak", "C-116"));
        listaZaposlenika.Add(new zaposlenik(152, "Elena", "Soldo", "D-173"));
        listaZaposlenika.Add(new zaposlenik(153, "Đurđa", "Rajković", "D-261"));
        listaZaposlenika.Add(new zaposlenik(154, "Robert", "Grubišić", "D-335"));
        listaZaposlenika.Add(new zaposlenik(155, "Dario", "Kolarić", "D-165"));
        listaZaposlenika.Add(new zaposlenik(156, "Antonija", "Petković", "C-408"));
        listaZaposlenika.Add(new zaposlenik(157, "Leona", "Gudelj", "D-377"));
        listaZaposlenika.Add(new zaposlenik(158, "Dragutin", "Jurišić", "D-305"));
        listaZaposlenika.Add(new zaposlenik(159, "Branka", "Štimac", "C-721"));
        listaZaposlenika.Add(new zaposlenik(160, "Tatjana", "Golub", "C-551"));
        listaZaposlenika.Add(new zaposlenik(161, "Verica", "Bošnjak", "D-214"));
        listaZaposlenika.Add(new zaposlenik(162, "Ksenija", "Tomić", "D-349"));
        listaZaposlenika.Add(new zaposlenik(163, "Martin", "Marinković", "D-374"));
        listaZaposlenika.Add(new zaposlenik(164, "Branimir", "Ban", "C-737"));
        listaZaposlenika.Add(new zaposlenik(165, "Anamarija", "Radoš", "C-926"));
        listaZaposlenika.Add(new zaposlenik(166, "Vinko", "Bulić", "D-325"));
        listaZaposlenika.Add(new zaposlenik(167, "Vanja", "Grgić", "C-918"));
        listaZaposlenika.Add(new zaposlenik(168, "David", "Oršuš", "D-328"));
        listaZaposlenika.Add(new zaposlenik(169, "Kristina", "Horvatić", "C-925"));
        listaZaposlenika.Add(new zaposlenik(170, "Hana", "Petrović", "D-127"));
        listaZaposlenika.Add(new zaposlenik(171, "Nikola", "Ostojić", "D-340"));
        listaZaposlenika.Add(new zaposlenik(172, "Danijel", "Vlahović", "D-376"));
        listaZaposlenika.Add(new zaposlenik(173, "Marina", "Novak", "C-702"));
        listaZaposlenika.Add(new zaposlenik(174, "Darinka", "Modrić", "D-342"));
        listaZaposlenika.Add(new zaposlenik(175, "Mato", "Miloš", "C-139"));
        listaZaposlenika.Add(new zaposlenik(176, "Vito", "Barać", "C-921"));
        listaZaposlenika.Add(new zaposlenik(177, "Vlado", "Kolar", "D-233"));
        listaZaposlenika.Add(new zaposlenik(178, "Dragica", "Petrić", "D-261"));
        listaZaposlenika.Add(new zaposlenik(179, "Krešimir", "Mamić", "D-244"));
        listaZaposlenika.Add(new zaposlenik(180, "Ena", "Jakovljević", "C-603"));
        listaZaposlenika.Add(new zaposlenik(181, "Kristijan", "Raguž", "C-116"));
        listaZaposlenika.Add(new zaposlenik(182, "Barbara", "Vidaković", "C-919"));
        listaZaposlenika.Add(new zaposlenik(183, "Jasmina", "Perković", "D-129"));
        listaZaposlenika.Add(new zaposlenik(184, "Lorena", "Glavaš", "C-101"));
        listaZaposlenika.Add(new zaposlenik(185, "Matea", "Šarić", "C-710"));
        listaZaposlenika.Add(new zaposlenik(186, "Jasmina", "Vlašić", "D-233"));
        listaZaposlenika.Add(new zaposlenik(187, "Božo", "Oršuš", "D-249"));
        listaZaposlenika.Add(new zaposlenik(188, "Silvija", "Petković", "D-364"));
        listaZaposlenika.Add(new zaposlenik(189, "Vito", "Baković", "C-735"));
        listaZaposlenika.Add(new zaposlenik(190, "Manda", "Novosel", "C-907"));
        listaZaposlenika.Add(new zaposlenik(191, "Zvonko", "Kolić", "C-910"));
        listaZaposlenika.Add(new zaposlenik(192, "Dino", "Butković", "D-356"));
        listaZaposlenika.Add(new zaposlenik(193, "Bruno", "Petković", "D-327"));
        listaZaposlenika.Add(new zaposlenik(194, "Marica", "Šimunić", "C-305"));
        listaZaposlenika.Add(new zaposlenik(195, "Vladimir", "Ivković", "D-116"));
        listaZaposlenika.Add(new zaposlenik(196, "Nenad", "Begić", "C-951"));
        listaZaposlenika.Add(new zaposlenik(197, "Marin", "Jakšić", "C-326"));
        listaZaposlenika.Add(new zaposlenik(198, "Gordana", "Lončarić", "D-245"));
        listaZaposlenika.Add(new zaposlenik(199, "Mijo", "Nikolić", "C-816"));
        listaZaposlenika.Add(new zaposlenik(200, "Mirela", "Vuković", "D-213"));
        listaZaposlenika.Add(new zaposlenik(201, "Ines", "Pavičić", "C-351"));
        listaZaposlenika.Add(new zaposlenik(202, "Mirjana", "Mršić", "C-236"));
        listaZaposlenika.Add(new zaposlenik(203, "Renata", "Dragičević", "C-339"));
        listaZaposlenika.Add(new zaposlenik(204, "Alen", "Petričević", "C-343"));
        listaZaposlenika.Add(new zaposlenik(205, "Hrvoje", "Barišić", "D-245"));
        listaZaposlenika.Add(new zaposlenik(206, "Branko", "Ignac", "C-510"));
        listaZaposlenika.Add(new zaposlenik(207, "Božidar", "Magdić", "D-305"));
        listaZaposlenika.Add(new zaposlenik(208, "Mia", "Ilić", "C-248"));
        listaZaposlenika.Add(new zaposlenik(209, "Sandra", "Grgurić", "C-210"));
        listaZaposlenika.Add(new zaposlenik(210, "Jurica", "Marić", "D-268"));
        listaZaposlenika.Add(new zaposlenik(211, "Jakov", "Lukić", "C-945"));
        listaZaposlenika.Add(new zaposlenik(212, "Branimir", "Šimunić", "C-221"));
        listaZaposlenika.Add(new zaposlenik(213, "Renata", "Banović", "D-252"));
        listaZaposlenika.Add(new zaposlenik(214, "Tin", "Stipić", "D-133"));
        listaZaposlenika.Add(new zaposlenik(215, "Marta", "Milić", "C-805"));
        listaZaposlenika.Add(new zaposlenik(216, "Jela", "Palić", "D-351"));
        listaZaposlenika.Add(new zaposlenik(217, "Matea", "Zec", "C-103"));
        listaZaposlenika.Add(new zaposlenik(218, "Vinko", "Ivanović", "C-116"));
        listaZaposlenika.Add(new zaposlenik(219, "Mara", "Matošević", "C-614"));
        listaZaposlenika.Add(new zaposlenik(220, "Marija", "Dujmović", "D-178"));
        listaZaposlenika.Add(new zaposlenik(221, "Anđelka", "Kralj", "C-340"));
        listaZaposlenika.Add(new zaposlenik(222, "Gordana", "Andrić", "D-368"));
        listaZaposlenika.Add(new zaposlenik(223, "Lorena", "Ivančić", "C-135"));
        listaZaposlenika.Add(new zaposlenik(224, "Biserka", "Knežević", "D-101"));
        listaZaposlenika.Add(new zaposlenik(225, "Đurđa", "Miličević", "D-244"));
        listaZaposlenika.Add(new zaposlenik(226, "Darinka", "Ostojić", "C-358"));
        listaZaposlenika.Add(new zaposlenik(227, "Marijana", "Perković", "D-333"));
        listaZaposlenika.Add(new zaposlenik(228, "Bruno", "Matošević", "C-252"));
        listaZaposlenika.Add(new zaposlenik(229, "Renata", "Šimić", "D-336"));
        listaZaposlenika.Add(new zaposlenik(230, "Tea", "Tomašević", "C-147"));
        listaZaposlenika.Add(new zaposlenik(231, "Lara", "Rogić", "D-121"));
        listaZaposlenika.Add(new zaposlenik(232, "Pero", "Majstorović", "D-305"));
        listaZaposlenika.Add(new zaposlenik(233, "Dominik", "Lacković", "C-448"));
        listaZaposlenika.Add(new zaposlenik(234, "Ivo", "Šimunić", "D-223"));
        listaZaposlenika.Add(new zaposlenik(235, "Lucija", "Miloš", "C-628"));
        listaZaposlenika.Add(new zaposlenik(236, "Dijana", "Barišić", "D-305"));
        listaZaposlenika.Add(new zaposlenik(237, "Luka", "Bačić", "D-155"));
        listaZaposlenika.Add(new zaposlenik(238, "Jasmina", "Marić", "D-130"));
        listaZaposlenika.Add(new zaposlenik(239, "Zvonko", "Ivančić", "D-236"));
        listaZaposlenika.Add(new zaposlenik(240, "Filip", "Lončar", "C-639"));
        listaZaposlenika.Add(new zaposlenik(241, "Snježana", "Šoštarić", "D-122"));
        listaZaposlenika.Add(new zaposlenik(242, "Anamarija", "Turk", "C-804"));
        listaZaposlenika.Add(new zaposlenik(243, "Antonija", "Grubišić", "D-308"));
        listaZaposlenika.Add(new zaposlenik(244, "Neven", "Mandić", "D-335"));
        listaZaposlenika.Add(new zaposlenik(245, "Lara", "Horvat", "C-545"));
        listaZaposlenika.Add(new zaposlenik(246, "Marica", "Kovač", "C-926"));
        listaZaposlenika.Add(new zaposlenik(247, "Martin", "Balić", "D-344"));
        listaZaposlenika.Add(new zaposlenik(248, "Josipa", "Varga", "D-226"));
        listaZaposlenika.Add(new zaposlenik(249, "Veronika", "Ivić", "C-240"));
        listaZaposlenika.Add(new zaposlenik(250, "Antonio", "Ban", "D-117"));
        listaZaposlenika.Add(new zaposlenik(251, "Dragica", "Marjanović", "D-261"));
        listaZaposlenika.Add(new zaposlenik(252, "Sanja", "Golub", "D-132"));
        listaZaposlenika.Add(new zaposlenik(253, "Nikola", "Grgić", "D-123"));
        listaZaposlenika.Add(new zaposlenik(254, "Ivano", "Petričević", "C-700"));
        listaZaposlenika.Add(new zaposlenik(255, "Leo", "Vlašić", "D-204"));
        listaZaposlenika.Add(new zaposlenik(256, "Sanja", "Baričević", "D-342"));
        listaZaposlenika.Add(new zaposlenik(257, "Lucija", "Galić", "D-343"));
        listaZaposlenika.Add(new zaposlenik(258, "Tatjana", "Delić", "C-822"));
        listaZaposlenika.Add(new zaposlenik(259, "Jasna", "Oršuš", "D-261"));
        listaZaposlenika.Add(new zaposlenik(260, "Krešimir", "Perić", "D-372"));
        listaZaposlenika.Add(new zaposlenik(261, "Nikolina", "Gudelj", "D-168"));
        listaZaposlenika.Add(new zaposlenik(262, "Ilija", "Martić", "C-903"));
        listaZaposlenika.Add(new zaposlenik(263, "Dario", "Vrdoljak", "C-255"));
        listaZaposlenika.Add(new zaposlenik(264, "Zvonko", "Zec", "C-101"));
        listaZaposlenika.Add(new zaposlenik(265, "Jakov", "Delić", "D-357"));
        listaZaposlenika.Add(new zaposlenik(266, "Hrvoje", "Ljubičić", "C-355"));
        listaZaposlenika.Add(new zaposlenik(267, "Željko", "Salopek", "D-330"));
        listaZaposlenika.Add(new zaposlenik(268, "Ines", "Šoštarić", "D-201"));
        listaZaposlenika.Add(new zaposlenik(269, "Gordana", "Pavić", "C-702"));
        listaZaposlenika.Add(new zaposlenik(270, "Ivana", "Stanić", "C-150"));
        listaZaposlenika.Add(new zaposlenik(271, "Renato", "Vukić", "C-222"));
        listaZaposlenika.Add(new zaposlenik(272, "Zvonko", "Vuletić", "C-528"));
        listaZaposlenika.Add(new zaposlenik(273, "Ljubica", "Ivanović", "D-374"));
        listaZaposlenika.Add(new zaposlenik(274, "Petra", "Petrić", "C-558"));
        listaZaposlenika.Add(new zaposlenik(275, "Katarina", "Jovanović", "D-133"));
        listaZaposlenika.Add(new zaposlenik(276, "Đuro", "Brajković", "C-925"));
        listaZaposlenika.Add(new zaposlenik(277, "Tihomir", "Miličević", "C-351"));
        listaZaposlenika.Add(new zaposlenik(278, "David", "Pavelić", "D-337"));
        listaZaposlenika.Add(new zaposlenik(279, "Andreja", "Bilić", "D-315"));
        listaZaposlenika.Add(new zaposlenik(280, "Iva", "Marković", "D-262"));
        listaZaposlenika.Add(new zaposlenik(281, "Ana", "Grgić", "D-357"));
        listaZaposlenika.Add(new zaposlenik(282, "Ilija", "Jurčević", "C-355"));
        listaZaposlenika.Add(new zaposlenik(283, "Mario", "Prpić", "C-126"));
        listaZaposlenika.Add(new zaposlenik(284, "Jozo", "Ćosić", "C-501"));
        listaZaposlenika.Add(new zaposlenik(285, "Mia", "Petrić", "D-275"));
        listaZaposlenika.Add(new zaposlenik(286, "Danica", "Devčić", "D-101"));
        listaZaposlenika.Add(new zaposlenik(287, "Marko", "Ban", "D-342"));
        listaZaposlenika.Add(new zaposlenik(288, "Juraj", "Bogdan", "D-365"));
        listaZaposlenika.Add(new zaposlenik(289, "Dubravko", "Vidaković", "D-356"));
        listaZaposlenika.Add(new zaposlenik(290, "Danijela", "Zelić", "D-155"));
        listaZaposlenika.Add(new zaposlenik(291, "Matija", "Katić", "D-244"));
        listaZaposlenika.Add(new zaposlenik(292, "Vito", "Dukić", "C-507"));
        listaZaposlenika.Add(new zaposlenik(293, "Manda", "Marković", "C-605"));
        listaZaposlenika.Add(new zaposlenik(294, "Niko", "Matić", "C-319"));
        listaZaposlenika.Add(new zaposlenik(295, "Lea", "Perković", "C-355"));
        listaZaposlenika.Add(new zaposlenik(296, "Marin", "Štimac", "D-140"));
        listaZaposlenika.Add(new zaposlenik(297, "Vedran", "Tomić", "C-352"));
        listaZaposlenika.Add(new zaposlenik(298, "Sandra", "Lovrić", "C-857"));
        listaZaposlenika.Add(new zaposlenik(299, "Marinko", "Zec", "D-167"));
        listaZaposlenika.Add(new zaposlenik(300, "Denis", "Vukić", "D-142"));
        listaZaposlenika.Add(new zaposlenik(301, "Klara", "Ostojić", "C-115"));
        listaZaposlenika.Add(new zaposlenik(302, "Barbara", "Butković", "C-146"));
        listaZaposlenika.Add(new zaposlenik(303, "Hrvoje", "Zelić", "C-905"));
        listaZaposlenika.Add(new zaposlenik(304, "Dušan", "Šimunić", "C-115"));
        listaZaposlenika.Add(new zaposlenik(305, "Neven", "Šimunović", "D-202"));
        listaZaposlenika.Add(new zaposlenik(306, "Tamara", "Mlinarić", "C-645"));
        listaZaposlenika.Add(new zaposlenik(307, "Leon", "Maras", "D-260"));
        listaZaposlenika.Add(new zaposlenik(308, "Saša", "Novaković", "C-617"));
        listaZaposlenika.Add(new zaposlenik(309, "Valentina", "Dragičević", "C-937"));
        listaZaposlenika.Add(new zaposlenik(310, "Mihael", "Barišić", "C-551"));
        listaZaposlenika.Add(new zaposlenik(311, "Nevenka", "Pavlić", "D-115"));
        listaZaposlenika.Add(new zaposlenik(312, "Mara", "Marjanović", "C-535"));
        listaZaposlenika.Add(new zaposlenik(313, "Hrvoje", "Petričević", "D-225"));
        listaZaposlenika.Add(new zaposlenik(314, "Gabriel", "Posavec", "C-858"));
        listaZaposlenika.Add(new zaposlenik(315, "Štefanija", "Bulić", "C-913"));
        listaZaposlenika.Add(new zaposlenik(316, "Dora", "Đurić", "C-353"));
        listaZaposlenika.Add(new zaposlenik(317, "Ivo", "Marić", "C-440"));
        listaZaposlenika.Add(new zaposlenik(318, "Gordana", "Lukić", "D-324"));
        listaZaposlenika.Add(new zaposlenik(319, "Dražen", "Pavlić", "C-922"));
        listaZaposlenika.Add(new zaposlenik(320, "Kata", "Mijatović", "D-118"));
        listaZaposlenika.Add(new zaposlenik(321, "Zdenko", "Baković", "D-126"));
        listaZaposlenika.Add(new zaposlenik(322, "Lucija", "Oršoš", "C-130"));
        listaZaposlenika.Add(new zaposlenik(323, "Đuro", "Antunović", "D-300"));
        listaZaposlenika.Add(new zaposlenik(324, "Ivana", "Katić", "D-104"));
        listaZaposlenika.Add(new zaposlenik(325, "Dubravka", "Vučković", "C-302"));
        listaZaposlenika.Add(new zaposlenik(326, "Helena", "Grubišić", "D-216"));
        listaZaposlenika.Add(new zaposlenik(327, "Darko", "Vidaković", "D-147"));
        listaZaposlenika.Add(new zaposlenik(328, "Slavko", "Marijanović", "D-228"));
        listaZaposlenika.Add(new zaposlenik(329, "Branko", "Radoš", "D-157"));
        listaZaposlenika.Add(new zaposlenik(330, "Miroslav", "Jerković", "C-146"));
        listaZaposlenika.Add(new zaposlenik(331, "Jasna", "Zec", "D-345"));
        listaZaposlenika.Add(new zaposlenik(332, "Ivana", "Kovačić", "C-514"));
        listaZaposlenika.Add(new zaposlenik(333, "Katarina", "Martić", "D-361"));
        listaZaposlenika.Add(new zaposlenik(334, "Katica", "Begić", "D-236"));
        listaZaposlenika.Add(new zaposlenik(335, "Josip", "Begić", "C-432"));
        listaZaposlenika.Add(new zaposlenik(336, "Anton", "Petričević", "C-810"));
        listaZaposlenika.Add(new zaposlenik(337, "Igor", "Andrić", "C-825"));
        listaZaposlenika.Add(new zaposlenik(338, "Vladimir", "Dukić", "C-757"));
        listaZaposlenika.Add(new zaposlenik(339, "Goran", "Miloš", "C-854"));
        listaZaposlenika.Add(new zaposlenik(340, "Damir", "Samardžić", "D-335"));
        listaZaposlenika.Add(new zaposlenik(341, "Davor", "Đurić", "D-369"));
        listaZaposlenika.Add(new zaposlenik(342, "Đurđica", "Cvitković", "C-429"));
        listaZaposlenika.Add(new zaposlenik(343, "Jadranka", "Vidović", "D-243"));
        listaZaposlenika.Add(new zaposlenik(344, "Jan", "Miličević", "D-176"));
        listaZaposlenika.Add(new zaposlenik(345, "Paula", "Klarić", "D-179"));
        listaZaposlenika.Add(new zaposlenik(346, "Marina", "Delić", "D-112"));
        listaZaposlenika.Add(new zaposlenik(347, "Barica", "Vukelić", "D-178"));
        listaZaposlenika.Add(new zaposlenik(348, "Andrea", "Crnković", "D-135"));
        listaZaposlenika.Add(new zaposlenik(349, "Nada", "Jelić", "D-143"));
        listaZaposlenika.Add(new zaposlenik(350, "Marija", "Radović", "C-926"));
        listaZaposlenika.Add(new zaposlenik(351, "Mihaela", "Vidović", "C-837"));
        listaZaposlenika.Add(new zaposlenik(352, "Roko", "Galović", "D-303"));
        listaZaposlenika.Add(new zaposlenik(353, "Janja", "Jović", "C-823"));
        listaZaposlenika.Add(new zaposlenik(354, "Zvonko", "Babić", "C-555"));
        listaZaposlenika.Add(new zaposlenik(355, "Vera", "Mihaljević", "D-348"));
        listaZaposlenika.Add(new zaposlenik(356, "Anica", "Matijević", "C-946"));
        listaZaposlenika.Add(new zaposlenik(357, "Tena", "Benčić", "C-935"));
        listaZaposlenika.Add(new zaposlenik(358, "David", "Dragičević", "D-253"));
        listaZaposlenika.Add(new zaposlenik(359, "Dragutin", "Oršoš", "C-849"));
        listaZaposlenika.Add(new zaposlenik(360, "Nataša", "Radić", "D-171"));
        listaZaposlenika.Add(new zaposlenik(361, "Saša", "Marić", "D-276"));
        listaZaposlenika.Add(new zaposlenik(362, "Vinko", "Jurković", "C-237"));
        listaZaposlenika.Add(new zaposlenik(363, "Zvonko", "Dragičević", "C-317"));
        listaZaposlenika.Add(new zaposlenik(364, "Gabrijela", "Majić", "C-818"));
        listaZaposlenika.Add(new zaposlenik(365, "Dušan", "Soldo", "D-315"));
        listaZaposlenika.Add(new zaposlenik(366, "Renata", "Jurišić", "C-257"));
        listaZaposlenika.Add(new zaposlenik(367, "Ena", "Milošević", "C-507"));
        listaZaposlenika.Add(new zaposlenik(368, "Aleksandar", "Mijatović", "D-258"));
        listaZaposlenika.Add(new zaposlenik(369, "Barbara", "Vučković", "C-144"));
        listaZaposlenika.Add(new zaposlenik(370, "Saša", "Ljubičić", "C-622"));
        listaZaposlenika.Add(new zaposlenik(371, "Matea", "Banović", "D-273"));
        listaZaposlenika.Add(new zaposlenik(372, "Krešimir", "Orešković", "D-356"));
        listaZaposlenika.Add(new zaposlenik(373, "Ilija", "Gudelj", "C-332"));
        listaZaposlenika.Add(new zaposlenik(374, "Jelena", "Stojanović", "D-314"));
        listaZaposlenika.Add(new zaposlenik(375, "Gabrijela", "Tomljanović", "D-355"));
        listaZaposlenika.Add(new zaposlenik(376, "Andrea", "Grgurić", "C-412"));
        listaZaposlenika.Add(new zaposlenik(377, "Anica", "Mihaljević", "D-331"));
        listaZaposlenika.Add(new zaposlenik(378, "Tihomir", "Banović", "D-156"));
        listaZaposlenika.Add(new zaposlenik(379, "Martina", "Barić", "C-740"));
        listaZaposlenika.Add(new zaposlenik(380, "Antonio", "Devčić", "C-357"));
        listaZaposlenika.Add(new zaposlenik(381, "Željka", "Bulić", "C-613"));
        listaZaposlenika.Add(new zaposlenik(382, "Matija", "Turk", "C-821"));
        listaZaposlenika.Add(new zaposlenik(383, "Branko", "Pintarić", "C-651"));
        listaZaposlenika.Add(new zaposlenik(384, "Davor", "Rukavina", "C-727"));
        listaZaposlenika.Add(new zaposlenik(385, "Ruža", "Dragičević", "C-854"));
        listaZaposlenika.Add(new zaposlenik(386, "Helena", "Tadić", "C-346"));
        listaZaposlenika.Add(new zaposlenik(387, "Lea", "Ban", "D-370"));
        listaZaposlenika.Add(new zaposlenik(388, "Danijel", "Jović", "C-733"));
        listaZaposlenika.Add(new zaposlenik(389, "Miljenko", "Marković", "D-119"));
        listaZaposlenika.Add(new zaposlenik(390, "Leona", "Stanić", "C-801"));
        listaZaposlenika.Add(new zaposlenik(391, "Kristina", "Josipović", "C-515"));
        listaZaposlenika.Add(new zaposlenik(392, "Ivan", "Ljubić", "C-555"));
        listaZaposlenika.Add(new zaposlenik(393, "Vesna", "Matanović", "D-150"));
        listaZaposlenika.Add(new zaposlenik(394, "Drago", "Pejić", "C-243"));
        listaZaposlenika.Add(new zaposlenik(395, "Ivan", "Cindrić", "C-806"));
        listaZaposlenika.Add(new zaposlenik(396, "Željka", "Cindrić", "C-247"));
        listaZaposlenika.Add(new zaposlenik(397, "Zvonko", "Ćosić", "C-851"));
        listaZaposlenika.Add(new zaposlenik(398, "Zlata", "Pejić", "D-312"));
        listaZaposlenika.Add(new zaposlenik(399, "Saša", "Martinović", "C-543"));
        listaZaposlenika.Add(new zaposlenik(400, "Vjekoslav", "Ostojić", "C-626"));
        listaZaposlenika.Add(new zaposlenik(401, "Miroslav", "Pavlić", "C-652"));
        listaZaposlenika.Add(new zaposlenik(402, "Gordana", "Kos", "D-358"));
        listaZaposlenika.Add(new zaposlenik(403, "Terezija", "Kordić", "C-415"));
        listaZaposlenika.Add(new zaposlenik(404, "Zoran", "Brčić", "D-211"));
        listaZaposlenika.Add(new zaposlenik(405, "Željko", "Mršić", "D-271"));
        listaZaposlenika.Add(new zaposlenik(406, "Ivana", "Brajković", "D-149"));
        listaZaposlenika.Add(new zaposlenik(407, "David", "Prpić", "D-350"));
        listaZaposlenika.Add(new zaposlenik(408, "Štefica", "Jović", "D-207"));
        listaZaposlenika.Add(new zaposlenik(409, "Mladen", "Kovač", "D-118"));
        listaZaposlenika.Add(new zaposlenik(410, "Vito", "Miletić", "C-358"));
        listaZaposlenika.Add(new zaposlenik(411, "Ružica", "Jakšić", "C-603"));
        listaZaposlenika.Add(new zaposlenik(412, "Slavko", "Rašić", "C-831"));
        listaZaposlenika.Add(new zaposlenik(413, "Vinko", "Pavić", "C-257"));
        listaZaposlenika.Add(new zaposlenik(414, "Nina", "Zec", "D-142"));
        listaZaposlenika.Add(new zaposlenik(415, "Pero", "Vuković", "C-234"));
        listaZaposlenika.Add(new zaposlenik(416, "Ljiljana", "Petrović", "C-549"));
        listaZaposlenika.Add(new zaposlenik(417, "Anđela", "Lučić", "D-342"));
        listaZaposlenika.Add(new zaposlenik(418, "Zlata", "Banić", "D-332"));
        listaZaposlenika.Add(new zaposlenik(419, "Luka", "Matijević", "D-379"));
        listaZaposlenika.Add(new zaposlenik(420, "Zlatko", "Jurčević", "C-107"));
        listaZaposlenika.Add(new zaposlenik(421, "Anita", "Baričević", "D-167"));
        listaZaposlenika.Add(new zaposlenik(422, "Matija", "Oršuš", "C-952"));
        listaZaposlenika.Add(new zaposlenik(423, "Đurđica", "Tolić", "D-147"));
        listaZaposlenika.Add(new zaposlenik(424, "Vanja", "Sertić", "D-103"));
        listaZaposlenika.Add(new zaposlenik(425, "Domagoj", "Mihaljević", "C-826"));
        listaZaposlenika.Add(new zaposlenik(426, "Toni", "Tokić", "C-611"));
        listaZaposlenika.Add(new zaposlenik(427, "Krešimir", "Crnković", "C-247"));
        listaZaposlenika.Add(new zaposlenik(428, "Mile", "Janković", "C-303"));
        listaZaposlenika.Add(new zaposlenik(429, "Sandra", "Perković", "D-167"));
        listaZaposlenika.Add(new zaposlenik(430, "Mihael", "Rožić", "D-241"));
        listaZaposlenika.Add(new zaposlenik(431, "Danijela", "Perković", "D-354"));
        listaZaposlenika.Add(new zaposlenik(432, "Ankica", "Kovač", "D-120"));
        listaZaposlenika.Add(new zaposlenik(433, "Nika", "Gudelj", "C-129"));
        listaZaposlenika.Add(new zaposlenik(434, "Dragica", "Topić", "D-351"));
        listaZaposlenika.Add(new zaposlenik(435, "Ljubica", "Majić", "C-747"));
        listaZaposlenika.Add(new zaposlenik(436, "Manda", "Delić", "D-257"));
        listaZaposlenika.Add(new zaposlenik(437, "Marijana", "Martinović", "C-614"));
        listaZaposlenika.Add(new zaposlenik(438, "Danijel", "Janković", "D-217"));
        listaZaposlenika.Add(new zaposlenik(439, "Sandra", "Kolić", "D-257"));
        listaZaposlenika.Add(new zaposlenik(440, "Jasna", "Mišković", "C-441"));
        listaZaposlenika.Add(new zaposlenik(441, "David", "Dukić", "C-340"));
        listaZaposlenika.Add(new zaposlenik(442, "Antonio", "Barišić", "D-148"));
        listaZaposlenika.Add(new zaposlenik(443, "Branko", "Majstorović", "C-436"));
        listaZaposlenika.Add(new zaposlenik(444, "Lara", "Mišić", "D-334"));
        listaZaposlenika.Add(new zaposlenik(445, "Tamara", "Andrić", "C-304"));
        listaZaposlenika.Add(new zaposlenik(446, "Milena", "Mitrović", "C-220"));
        listaZaposlenika.Add(new zaposlenik(447, "Ema", "Kolić", "D-155"));
        listaZaposlenika.Add(new zaposlenik(448, "Jelena", "Jakšić", "C-926"));
        listaZaposlenika.Add(new zaposlenik(449, "Hrvoje", "Majić", "D-118"));
        listaZaposlenika.Add(new zaposlenik(450, "Nada", "Kordić", "C-937"));
        listaZaposlenika.Add(new zaposlenik(451, "Marina", "Knezović", "C-956"));
        listaZaposlenika.Add(new zaposlenik(452, "Josipa", "Salopek", "D-108"));
        listaZaposlenika.Add(new zaposlenik(453, "Maja", "Kraljević", "D-246"));
        listaZaposlenika.Add(new zaposlenik(454, "Bruno", "Špoljarić", "D-207"));
        listaZaposlenika.Add(new zaposlenik(455, "Dragutin", "Horvat", "C-946"));
        listaZaposlenika.Add(new zaposlenik(456, "Nevenka", "Jurčević", "D-128"));
        listaZaposlenika.Add(new zaposlenik(457, "Jela", "Matošević", "C-535"));
        listaZaposlenika.Add(new zaposlenik(458, "Zvonimir", "Matković", "D-247"));
        listaZaposlenika.Add(new zaposlenik(459, "Magdalena", "Benčić", "C-353"));
        listaZaposlenika.Add(new zaposlenik(460, "Martina", "Starčević", "C-307"));
        listaZaposlenika.Add(new zaposlenik(461, "Gabriel", "Matić", "C-903"));
        listaZaposlenika.Add(new zaposlenik(462, "Kristina", "Kovačević", "C-144"));
        listaZaposlenika.Add(new zaposlenik(463, "Božena", "Barać", "D-177"));
        listaZaposlenika.Add(new zaposlenik(464, "Roko", "Modrić", "C-835"));
        listaZaposlenika.Add(new zaposlenik(465, "Ruža", "Pranjić", "C-340"));
        listaZaposlenika.Add(new zaposlenik(466, "Darinka", "Grgić", "D-371"));
        listaZaposlenika.Add(new zaposlenik(467, "Marin", "Tokić", "C-710"));
        listaZaposlenika.Add(new zaposlenik(468, "Andreja", "Topić", "D-363"));
        listaZaposlenika.Add(new zaposlenik(469, "Marko", "Varga", "C-125"));
        listaZaposlenika.Add(new zaposlenik(470, "Toni", "Kolarić", "C-129"));
        listaZaposlenika.Add(new zaposlenik(471, "Božica", "Prpić", "D-175"));
        listaZaposlenika.Add(new zaposlenik(472, "Martin", "Marijanović", "C-353"));
        listaZaposlenika.Add(new zaposlenik(473, "Ivica", "Grubišić", "D-162"));
        listaZaposlenika.Add(new zaposlenik(474, "Mirko", "Pavlović", "D-361"));
        listaZaposlenika.Add(new zaposlenik(475, "Lorena", "Crnković", "D-360"));
        listaZaposlenika.Add(new zaposlenik(476, "Jadranka", "Tadić", "D-358"));
        listaZaposlenika.Add(new zaposlenik(477, "Anita", "Tokić", "D-149"));
        listaZaposlenika.Add(new zaposlenik(478, "Alen", "Tomašić", "D-219"));
        listaZaposlenika.Add(new zaposlenik(479, "Ivanka", "Abramović", "D-276"));
        listaZaposlenika.Add(new zaposlenik(480, "Mateo", "Horvat", "C-408"));
        listaZaposlenika.Add(new zaposlenik(481, "Viktor", "Janković", "C-648"));
        listaZaposlenika.Add(new zaposlenik(482, "Zdenko", "Dragičević", "C-936"));
        listaZaposlenika.Add(new zaposlenik(483, "Noa", "Vidaković", "D-130"));
        listaZaposlenika.Add(new zaposlenik(484, "Nenad", "Tokić", "D-363"));
        listaZaposlenika.Add(new zaposlenik(485, "Darko", "Tolić", "C-908"));
        listaZaposlenika.Add(new zaposlenik(486, "Noa", "Franić", "D-160"));
        listaZaposlenika.Add(new zaposlenik(487, "Zoran", "Majstorović", "D-258"));
        listaZaposlenika.Add(new zaposlenik(488, "Nada", "Pejić", "D-368"));
        listaZaposlenika.Add(new zaposlenik(489, "Leona", "Jukić", "C-504"));
        listaZaposlenika.Add(new zaposlenik(490, "Juraj", "Erceg", "D-137"));
        listaZaposlenika.Add(new zaposlenik(491, "Marijana", "Mišić", "C-558"));
        listaZaposlenika.Add(new zaposlenik(492, "Dragutin", "Vučković", "C-406"));
        listaZaposlenika.Add(new zaposlenik(493, "Višnja", "Majstorović", "D-300"));
        listaZaposlenika.Add(new zaposlenik(494, "Andrej", "Filipović", "C-837"));
        listaZaposlenika.Add(new zaposlenik(495, "Nikola", "Soldo", "C-606"));
        listaZaposlenika.Add(new zaposlenik(496, "Stjepan", "Popović", "D-275"));
        listaZaposlenika.Add(new zaposlenik(497, "Alen", "Ljubičić", "C-403"));
        listaZaposlenika.Add(new zaposlenik(498, "Melita", "Bašić", "D-276"));
        listaZaposlenika.Add(new zaposlenik(499, "Ksenija", "Majić", "C-305"));
        listaZaposlenika.Add(new zaposlenik(500, "Andrej", "Begović", "D-327"));
        listaZaposlenika.Add(new zaposlenik(501, "Ivan", "Rogić", "C-932"));
        listaZaposlenika.Add(new zaposlenik(502, "Dubravko", "Vukić", "D-221"));
        listaZaposlenika.Add(new zaposlenik(503, "Jakov", "Rašić", "D-246"));
        listaZaposlenika.Add(new zaposlenik(504, "Slavica", "Vuković", "C-315"));
        listaZaposlenika.Add(new zaposlenik(505, "Marta", "Kovačević", "C-238"));
        listaZaposlenika.Add(new zaposlenik(506, "Klara", "Miličević", "D-314"));
        listaZaposlenika.Add(new zaposlenik(507, "Dražen", "Dukić", "D-162"));
        listaZaposlenika.Add(new zaposlenik(508, "Petra", "Tomašević", "D-366"));
        listaZaposlenika.Add(new zaposlenik(509, "Dragica", "Oršuš", "C-511"));
        listaZaposlenika.Add(new zaposlenik(510, "Ivica", "Ružić", "C-446"));
        listaZaposlenika.Add(new zaposlenik(511, "Laura", "Tokić", "D-331"));
        listaZaposlenika.Add(new zaposlenik(512, "Leo", "Knežević", "D-367"));
        listaZaposlenika.Add(new zaposlenik(513, "Mirela", "Grgurić", "D-258"));
        listaZaposlenika.Add(new zaposlenik(514, "Tihomir", "Vlahović", "D-329"));
        listaZaposlenika.Add(new zaposlenik(515, "Lovro", "Vlašić", "C-541"));
        listaZaposlenika.Add(new zaposlenik(516, "Ljiljana", "Petković", "D-229"));
        listaZaposlenika.Add(new zaposlenik(517, "Danijela", "Josipović", "D-159"));
        listaZaposlenika.Add(new zaposlenik(518, "Magdalena", "Rukavina", "C-228"));
        listaZaposlenika.Add(new zaposlenik(519, "Stjepan", "Marušić", "C-441"));
        listaZaposlenika.Add(new zaposlenik(520, "Tina", "Horvatić", "C-514"));
        listaZaposlenika.Add(new zaposlenik(521, "Petra", "Katić", "D-338"));
        listaZaposlenika.Add(new zaposlenik(522, "Branka", "Starčević", "C-153"));
        listaZaposlenika.Add(new zaposlenik(523, "Terezija", "Miličević", "C-813"));
        listaZaposlenika.Add(new zaposlenik(524, "Viktor", "Bašić", "D-317"));
        listaZaposlenika.Add(new zaposlenik(525, "Jasmina", "Car", "D-378"));
        listaZaposlenika.Add(new zaposlenik(526, "Dragan", "Juričić", "D-150"));
        listaZaposlenika.Add(new zaposlenik(527, "Ksenija", "Milić", "D-228"));
        listaZaposlenika.Add(new zaposlenik(528, "Siniša", "Božić", "C-132"));
        listaZaposlenika.Add(new zaposlenik(529, "Nikola", "Marinović", "D-244"));
        listaZaposlenika.Add(new zaposlenik(530, "Boris", "Majstorović", "D-122"));
        listaZaposlenika.Add(new zaposlenik(531, "Daniel", "Đurić", "C-347"));
        listaZaposlenika.Add(new zaposlenik(532, "Laura", "Lacković", "C-909"));
        listaZaposlenika.Add(new zaposlenik(533, "Manda", "Zec", "D-249"));
        listaZaposlenika.Add(new zaposlenik(534, "Sanja", "Radić", "D-379"));
        listaZaposlenika.Add(new zaposlenik(535, "Juraj", "Šimunić", "D-152"));
        listaZaposlenika.Add(new zaposlenik(536, "Dragutin", "Ivanović", "D-134"));
        listaZaposlenika.Add(new zaposlenik(537, "Sara", "Begović", "D-278"));
        listaZaposlenika.Add(new zaposlenik(538, "Božena", "Jozić", "C-609"));
        listaZaposlenika.Add(new zaposlenik(539, "Zlata", "Lukić", "D-137"));
        listaZaposlenika.Add(new zaposlenik(540, "Natalija", "Milić", "C-640"));
        listaZaposlenika.Add(new zaposlenik(541, "Nataša", "Rašić", "C-845"));
        listaZaposlenika.Add(new zaposlenik(542, "Tatjana", "Bašić", "C-821"));
        listaZaposlenika.Add(new zaposlenik(543, "Matija", "Vuković", "C-744"));
        listaZaposlenika.Add(new zaposlenik(544, "Barica", "Marijanović", "D-302"));
        listaZaposlenika.Add(new zaposlenik(545, "Helena", "Delić", "C-529"));
        listaZaposlenika.Add(new zaposlenik(546, "Mateja", "Turkalj", "D-112"));
        listaZaposlenika.Add(new zaposlenik(547, "Nika", "Zelić", "C-459"));
        listaZaposlenika.Add(new zaposlenik(548, "Darko", "Kordić", "C-422"));
        listaZaposlenika.Add(new zaposlenik(549, "Leon", "Jurčević", "D-178"));
        listaZaposlenika.Add(new zaposlenik(550, "Sanja", "Ivanković", "C-931"));
        listaZaposlenika.Add(new zaposlenik(551, "Dijana", "Dragičević", "D-172"));
        listaZaposlenika.Add(new zaposlenik(552, "Kristijan", "Gudelj", "C-509"));
        listaZaposlenika.Add(new zaposlenik(553, "Vedran", "Sertić", "C-944"));
        listaZaposlenika.Add(new zaposlenik(554, "Ljubica", "Sertić", "D-310"));
        listaZaposlenika.Add(new zaposlenik(555, "Manda", "Vukić", "C-937"));
        listaZaposlenika.Add(new zaposlenik(556, "Đuro", "Petrić", "D-214"));
        listaZaposlenika.Add(new zaposlenik(557, "Matea", "Marić", "D-357"));
        listaZaposlenika.Add(new zaposlenik(558, "Nina", "Mišić", "D-337"));
        listaZaposlenika.Add(new zaposlenik(559, "Branimir", "Mandić", "C-359"));
        listaZaposlenika.Add(new zaposlenik(560, "Nada", "Kolar", "D-360"));
        listaZaposlenika.Add(new zaposlenik(561, "Milica", "Burić", "C-957"));
        listaZaposlenika.Add(new zaposlenik(562, "Ivo", "Prpić", "C-611"));
        listaZaposlenika.Add(new zaposlenik(563, "Tea", "Kljajić", "D-139"));
        listaZaposlenika.Add(new zaposlenik(564, "Lucija", "Sučić", "D-300"));
        listaZaposlenika.Add(new zaposlenik(565, "Karla", "Benčić", "C-538"));
        listaZaposlenika.Add(new zaposlenik(566, "Gabriel", "Lukić", "D-248"));
        listaZaposlenika.Add(new zaposlenik(567, "Leon", "Cindrić", "C-821"));
        listaZaposlenika.Add(new zaposlenik(568, "Jelena", "Tolić", "D-168"));
        listaZaposlenika.Add(new zaposlenik(569, "Mladen", "Mitrović", "C-833"));
        listaZaposlenika.Add(new zaposlenik(570, "Krunoslav", "Bašić", "C-701"));
        listaZaposlenika.Add(new zaposlenik(571, "Vera", "Živković", "C-812"));
        listaZaposlenika.Add(new zaposlenik(572, "Nevenka", "Sertić", "C-330"));
        listaZaposlenika.Add(new zaposlenik(573, "Zlata", "Rukavina", "D-170"));
        listaZaposlenika.Add(new zaposlenik(574, "Marija", "Ilić", "C-749"));
        listaZaposlenika.Add(new zaposlenik(575, "Goran", "Stanković", "C-132"));
        listaZaposlenika.Add(new zaposlenik(576, "Niko", "Puškarić", "D-274"));
        listaZaposlenika.Add(new zaposlenik(577, "Dragan", "Medved", "D-157"));
        listaZaposlenika.Add(new zaposlenik(578, "Zdravko", "Đurić", "D-231"));
        listaZaposlenika.Add(new zaposlenik(579, "Miljenko", "Radošević", "D-246"));
        listaZaposlenika.Add(new zaposlenik(580, "Klara", "Pejić", "D-358"));
        listaZaposlenika.Add(new zaposlenik(581, "Ksenija", "Kolić", "C-446"));
        listaZaposlenika.Add(new zaposlenik(582, "Mihael", "Ivanović", "C-942"));
        listaZaposlenika.Add(new zaposlenik(583, "Đurđa", "Marković", "C-454"));
        listaZaposlenika.Add(new zaposlenik(584, "Dominik", "Filipović", "D-218"));
        listaZaposlenika.Add(new zaposlenik(585, "Ivka", "Budimir", "D-345"));
        listaZaposlenika.Add(new zaposlenik(586, "Dragutin", "Vukić", "C-858"));
        listaZaposlenika.Add(new zaposlenik(587, "Franjo", "Begović", "C-214"));
        listaZaposlenika.Add(new zaposlenik(588, "Anđela", "Banović", "D-355"));
        listaZaposlenika.Add(new zaposlenik(589, "Alen", "Marjanović", "D-115"));
        listaZaposlenika.Add(new zaposlenik(590, "Ivica", "Mihalić", "D-342"));
        listaZaposlenika.Add(new zaposlenik(591, "Monika", "Brkić", "C-348"));
        listaZaposlenika.Add(new zaposlenik(592, "Andrija", "Majić", "D-318"));
        listaZaposlenika.Add(new zaposlenik(593, "Darko", "Banić", "D-371"));
        listaZaposlenika.Add(new zaposlenik(594, "Borna", "Živković", "D-309"));
        listaZaposlenika.Add(new zaposlenik(595, "Gordana", "Radošević", "C-923"));
        listaZaposlenika.Add(new zaposlenik(596, "Mateja", "Salopek", "D-217"));
        listaZaposlenika.Add(new zaposlenik(597, "Nikola", "Butković", "C-913"));
        listaZaposlenika.Add(new zaposlenik(598, "Biserka", "Ćurić", "C-526"));
        listaZaposlenika.Add(new zaposlenik(599, "Ema", "Jurjević", "D-116"));
        listaZaposlenika.Add(new zaposlenik(600, "Božena", "Đurđević", "D-327"));
        listaZaposlenika.Add(new zaposlenik(601, "Patrik", "Barišić", "C-949"));
        listaZaposlenika.Add(new zaposlenik(602, "Martina", "Mikić", "D-210"));
        listaZaposlenika.Add(new zaposlenik(603, "Martin", "Jovanović", "D-104"));
        listaZaposlenika.Add(new zaposlenik(604, "Anita", "Antunović", "D-148"));
        listaZaposlenika.Add(new zaposlenik(605, "Tena", "Ivanišević", "D-140"));
        listaZaposlenika.Add(new zaposlenik(606, "Mara", "Martić", "D-339"));
        listaZaposlenika.Add(new zaposlenik(607, "Lea", "Anić", "D-364"));
        listaZaposlenika.Add(new zaposlenik(608, "Anđa", "Rogić", "D-209"));
        listaZaposlenika.Add(new zaposlenik(609, "Andrea", "Golubić", "C-333"));
        listaZaposlenika.Add(new zaposlenik(610, "Boris", "Cvitković", "C-815"));
        listaZaposlenika.Add(new zaposlenik(611, "Karla", "Sertić", "C-808"));
        listaZaposlenika.Add(new zaposlenik(612, "Dragica", "Jovanović", "C-227"));
        listaZaposlenika.Add(new zaposlenik(613, "Ivica", "Vrdoljak", "C-231"));
        listaZaposlenika.Add(new zaposlenik(614, "Lea", "Matković", "C-116"));
        listaZaposlenika.Add(new zaposlenik(615, "Andrej", "Blažević", "C-856"));
        listaZaposlenika.Add(new zaposlenik(616, "Lidija", "Šimunić", "C-521"));
        listaZaposlenika.Add(new zaposlenik(617, "Tin", "Špoljarić", "C-333"));
        listaZaposlenika.Add(new zaposlenik(618, "Josip", "Milković", "C-513"));
        listaZaposlenika.Add(new zaposlenik(619, "Gordana", "Lacković", "D-256"));
        listaZaposlenika.Add(new zaposlenik(620, "Andrej", "Barišić", "D-230"));
        listaZaposlenika.Add(new zaposlenik(621, "Jan", "Cindrić", "C-952"));
        listaZaposlenika.Add(new zaposlenik(622, "Anita", "Jurić", "D-322"));
        listaZaposlenika.Add(new zaposlenik(623, "Karla", "Milanović", "C-105"));
        listaZaposlenika.Add(new zaposlenik(624, "Noa", "Maras", "C-541"));
        listaZaposlenika.Add(new zaposlenik(625, "Milica", "Tomašić", "C-244"));
        listaZaposlenika.Add(new zaposlenik(626, "Magdalena", "Matijević", "D-173"));
        listaZaposlenika.Add(new zaposlenik(627, "Ivana", "Glavaš", "D-277"));
        listaZaposlenika.Add(new zaposlenik(628, "Filip", "Sučić", "C-225"));
        listaZaposlenika.Add(new zaposlenik(629, "Renata", "Vukušić", "D-145"));
        listaZaposlenika.Add(new zaposlenik(630, "Borna", "Modrić", "D-172"));
        listaZaposlenika.Add(new zaposlenik(631, "Krunoslav", "Marinković", "C-550"));
        listaZaposlenika.Add(new zaposlenik(632, "Aleksandar", "Janković", "C-219"));
        listaZaposlenika.Add(new zaposlenik(633, "Matija", "Posavec", "D-110"));
        listaZaposlenika.Add(new zaposlenik(634, "Darinka", "Ivanković", "C-209"));
        listaZaposlenika.Add(new zaposlenik(635, "Antonija", "Lončar", "D-323"));
        listaZaposlenika.Add(new zaposlenik(636, "Dalibor", "Vrdoljak", "D-232"));
        listaZaposlenika.Add(new zaposlenik(637, "Matija", "Turković", "D-348"));
        listaZaposlenika.Add(new zaposlenik(638, "Zorka", "Vlašić", "C-259"));
        listaZaposlenika.Add(new zaposlenik(639, "Dražen", "Matković", "C-803"));
        listaZaposlenika.Add(new zaposlenik(640, "Dora", "Jurjević", "C-806"));
        listaZaposlenika.Add(new zaposlenik(641, "Jozo", "Cvitković", "C-156"));
        listaZaposlenika.Add(new zaposlenik(642, "Niko", "Bašić", "C-846"));
        listaZaposlenika.Add(new zaposlenik(643, "Jurica", "Mišić", "C-304"));

    }
}

