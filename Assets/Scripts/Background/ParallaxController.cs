using UnityEngine;

public class ParallaxController : MonoBehaviour
{
    Transform cam;
    Vector3 camStartPos;
    float distance;

    GameObject[] backgrounds;
    Material[] mat;
    float[] backSpeed;

    float farthesBack;

    [Range(0.01f, 0.05f)]
    public float parallaxSpeed = 0.01f;

    void Start()
    {
        cam = Camera.main.transform;

        // Simpan posisi awal kamera
        camStartPos = cam.position;

        int backCount = transform.childCount;

        mat = new Material[backCount];
        backSpeed = new float[backCount];
        backgrounds = new GameObject[backCount];

        for (int i = 0; i < backCount; i++)
        {
            backgrounds[i] = transform.GetChild(i).gameObject;

            Renderer renderer = backgrounds[i].GetComponent<Renderer>();

            if (renderer != null)
            {
                mat[i] = renderer.material;
            }
        }

        BackSpeedCalculate(backCount);
    }

    void BackSpeedCalculate(int backCount)
    {
        farthesBack = 0f;

        // Cari background yang paling jauh
        for (int i = 0; i < backCount; i++)
        {
            float distanceFromCamera =
                backgrounds[i].transform.position.z - cam.position.z;

            if (distanceFromCamera > farthesBack)
            {
                farthesBack = distanceFromCamera;
            }
        }

        // Hitung kecepatan masing-masing background
        for (int i = 0; i < backCount; i++)
        {
            float distanceFromCamera =
                backgrounds[i].transform.position.z - cam.position.z;

            if (farthesBack != 0)
            {
                backSpeed[i] =
                    1 - (distanceFromCamera / farthesBack);
            }
            else
            {
                backSpeed[i] = 1f;
            }
        }
    }

    void LateUpdate()
    {
        distance = cam.position.x - camStartPos.x;
        transform.position = new Vector3(cam.position.x, transform.position.y, 0);
        for (int i = 0; i < backgrounds.Length; i++)
        {
            if (mat[i] != null)
            {
                float speed = backSpeed[i] * parallaxSpeed;

                mat[i].SetTextureOffset(
                    "_MainTex",
                    new Vector2(distance, 0) * speed
                );
            }
        }
    }
}