using UnityEngine;

public class StalactiteWater : MonoBehaviour
{

    public StalactiteDrip s;

    public GameObject water;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
     void OnCollisionEnter2D(Collision2D col)
    {
        water.SetActive(false);
        s.canDrip = true;


    }
    
}
