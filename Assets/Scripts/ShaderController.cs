using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShaderController : MonoBehaviour
{
    [SerializeField, Range(-1, 1)] private float curveX;
    [SerializeField, Range(-1, 1)] private float curveY;
    [SerializeField] private Material[] materials;

    void Update()
    {
        foreach (var material in materials)
        {
            material.SetFloat(Shader.PropertyToID("_CurveX"), curveX);
            material.SetFloat(Shader.PropertyToID("_CurveY"), curveY);
        }
    }
}
