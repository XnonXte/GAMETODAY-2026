using UnityEngine;

public class Background : MonoBehaviour
{
    private Material mat;
    private float distance;

    [Range(0f, 0.5f)]
    public float speed = 0.5f;

    void Start()
    {
        mat = GetComponent<Renderer>().material;
    }

    void Update()
    {
        distance += Time.deltaTime * speed;

        mat.SetTextureOffset(
            "_MainTex",
            Vector2.right * distance
        );
    }
}