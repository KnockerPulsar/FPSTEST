using System;
using UnityEngine;

//Responsible for starting each object with the tag "FloatingEnvironment" with a random rotation and material.
public class TestLevelScript : MonoBehaviour
{
    public Material[] floatingMaterials;

    // Start is called before the first frame update
    void Start()
    {
        GameObject[] floatingGO = GameObject.FindGameObjectsWithTag("FloatingEnvironment");
        foreach (GameObject floating in floatingGO)
        {
            //Randomizes the starting rotation.
            float x = UnityEngine.Random.value * 360f;
            float y = UnityEngine.Random.value * 360f;
            float z = UnityEngine.Random.value * 360f;
            floating.transform.rotation = Quaternion.Euler(new Vector3(x, y, z));

            //Randomizes the starting material.
            Material mat = floating.GetComponent<MeshRenderer>().materials[1];
            int RandomIndex = UnityEngine.Random.Range(0, floatingMaterials.Length);
            mat.SetColor("_EmissionColor", floatingMaterials[RandomIndex].GetColor("_EmissionColor"));
        }
    }
}
