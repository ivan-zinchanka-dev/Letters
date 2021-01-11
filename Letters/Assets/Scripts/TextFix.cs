using UnityEngine;

public class TextFix : MonoBehaviour
{
    [SerializeField] private short order = 2;

    void Start()
    {
        Renderer rend = GetComponent<TextMesh>().GetComponent<Renderer>(); 
        rend.sortingLayerName = "Objects";
        rend.sortingOrder = order; 
    }

}
