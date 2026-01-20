using UnityEngine;



public class ButtonTest : MonoBehaviour
{
    public void TestButton()
    {
        Debug.Log("button radi");
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
            Debug.Log("klik registriran");
    }
}

