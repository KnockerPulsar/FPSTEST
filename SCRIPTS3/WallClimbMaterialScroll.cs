using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Responsible for scrolling the wallclimable walls' materials in the Y direction.
public class WallClimbMaterialScroll : MonoBehaviour
{
    Renderer rend;                          //The renderer of the object.
    Material mat;                               //The material on the object.
    Vector2 Offset = new Vector2(0, 0);         //The x and y offsets of the material
    
    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<Renderer>();
        mat = rend.materials[1];
        Offset.x = mat.GetTextureOffset("_MainTex").x;
    }

    // Update is called once per frame
    //Changest the Y offset of the material to imitate scrolling.
    void Update()
    {
        Offset.y -= 0.2f * Time.deltaTime;
        mat.SetTextureOffset("_MainTex", Offset);
    }
}
