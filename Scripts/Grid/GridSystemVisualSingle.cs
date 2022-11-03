using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSystemVisualSingle : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    private const float SCALE_MULTI = 5f;

    public void Show(Material material)
    {
        spriteRenderer.enabled = true;
        spriteRenderer.material = material;
    }

    public void Hide()
    {
        spriteRenderer.enabled = false;
    }

    public void Scale(float scale)
    {
        this.gameObject.transform.localScale = new Vector3(scale / SCALE_MULTI, scale / SCALE_MULTI, 15);
    }
}
