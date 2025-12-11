using UnityEngine;
using System.Collections;
using TMPro;

namespace TMPro.Examples
{
    public class TeleType : MonoBehaviour
    {        
        private string label02 = "Ladica se otvara lozinkom 1196";
        private TMP_Text m_textMeshPro;

        void Awake()
        {
            m_textMeshPro = GetComponent<TMP_Text>();

            m_textMeshPro.textWrappingMode = TextWrappingModes.Normal;
            m_textMeshPro.alignment = TextAlignmentOptions.Top;

            // na početku ne prikazujemo ništa
            m_textMeshPro.text = "";
            m_textMeshPro.maxVisibleCharacters = 0;
        }

        IEnumerator Start()
        {
            while (true) // 🔁 ponavljaj sve dok je komponenta enabled
            {
                // postavi tekst koji će se tipkati
                m_textMeshPro.text = label02;

                // update mesh info
                m_textMeshPro.ForceMeshUpdate();

                int totalChars = m_textMeshPro.textInfo.characterCount;
                int visibleCount = 0;

                m_textMeshPro.maxVisibleCharacters = 0;

                // ✍️ ispisuj slovo po slovo
                while (visibleCount <= totalChars)
                {
                    m_textMeshPro.maxVisibleCharacters = visibleCount;
                    visibleCount++;

                    yield return new WaitForSeconds(0.05f); // brzina tipkanja
                }

                // kratka pauza kad je cijeli tekst ispisan
                yield return new WaitForSeconds(1.0f);

                // "obriši" prikaz (kreni opet od nule)
                m_textMeshPro.maxVisibleCharacters = 0;

                // mala pauza prije nove runde (opcionalno)
                yield return new WaitForSeconds(0.3f);
            }
        }
    }
}
