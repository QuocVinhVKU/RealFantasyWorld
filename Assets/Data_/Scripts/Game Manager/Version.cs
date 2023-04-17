using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Version : MonoBehaviour
{
    public TextMeshProUGUI verText;
    void Start()
    {
        
        verText.text = "Version: " + Application.version;
    }

}
