using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Pretty similar to the WallclimbMaterialScroll, but changes the X offset instead.
public class WallrunMaterialScroll : MonoBehaviour
{

    Renderer rend;
    Material mat;
    Vector2 Offset = new Vector2(0, 0);
    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<Renderer>();
        mat = rend.materials[1];
        Offset.y = mat.GetTextureOffset("_MainTex").y;
    }

    // Update is called once per frame
    void Update()
    {
        Offset.x -= 0.2f  * Time.deltaTime;
        mat.SetTextureOffset("_MainTex",Offset);
    }
}
