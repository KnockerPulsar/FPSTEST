using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum direction { Up, Down, Left, Right };

public class ScrollMatScript : MonoBehaviour
{
    public direction dir;

    public Texture2D emissionMaskUp;
    public Texture2D emissionMaskDown;
    public Texture2D emissionMaskLeft;
    public Texture2D emissionMaskRight;

    public float scrollSpeed = 1f;
    public Vector2 tiling = new Vector2(1, 1);
    public Vector2 offset = new Vector2(0, 0);
    public Color emissionColor = (Color.green + Color.white) / 2f;

    Renderer rend;
    Material mat;

    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<Renderer>();
        if (rend.materials.Length < 2)
            mat = rend.materials[0];
        else
            mat = rend.materials[1];
    }

    // Update is called once per frame
    void Update()
    {
        switch ((int)dir)
        {
            case 0:
                {
                    mat.SetTexture("Texture2D_B0D2CB7B", emissionMaskUp);
                    break;
                }
            case 1:
                {
                    mat.SetTexture("Texture2D_B0D2CB7B", emissionMaskDown);
                    break;
                }
            case 2:
                {
                    mat.SetTexture("Texture2D_B0D2CB7B", emissionMaskLeft);
                    break;
                }
            case 3:
                {
                    mat.SetTexture("Texture2D_B0D2CB7B", emissionMaskRight);
                    break;
                }
        }
        mat.SetFloat("ScrollSpeed", scrollSpeed);
        mat.SetColor("Color_24AB3104", emissionColor);
        mat.SetFloat("Vector1_1A4BD27B", (float)dir);
        mat.SetVector("Vector2_154AA344", tiling);
        mat.SetVector("Vector2_776E6361", offset);

    }
}
