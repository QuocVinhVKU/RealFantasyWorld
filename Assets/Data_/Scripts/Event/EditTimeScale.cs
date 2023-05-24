using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditTimeScale : MonoBehaviour
{
    [SerializeField]float scaleTime = 2f;
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = scaleTime;
    }

   
}
