using System.Collections.Generic;
using UnityEngine;

public class SpriteRendererActivator : MonoBehaviour
{
    [SerializeField] private List<SpriteRenderer> spriteRenderers = new List<SpriteRenderer>();
    void Update()
    {
        foreach (var renderer in spriteRenderers) { 
            renderer.enabled = true;
        }
    }

}
