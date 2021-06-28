using UnityEngine;

public class TextFix : MonoBehaviour
{
    [SerializeField] private short _order = default;   // 2   //res paper 5  // 2
    [SerializeField] private TextMesh _text = null;
    private Renderer _rend = null;

    void Start()
    {
        if (TryGetComponent<Renderer>(out _rend)){

            _rend.sortingLayerName = "Objects";
            _rend.sortingOrder = _order;
        };         
    }

}
