using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Logger : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void hoverEntered(){
        Debug.Log("Hove entered");
    }

    public void hoverExited(){
        Debug.Log("Hover exited"); 
    }

    public void selectedEntered(){
        Debug.Log("Select entered");
    }

    public void selectedExited(){
        Debug.Log("Select exited");
    }

    public void activated(){
        Debug.Log("SActivated");
    }

    public void deactivated(){
        Debug.Log("Deactivated");
    }
}
