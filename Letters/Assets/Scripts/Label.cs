using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Label : MonoBehaviour
{
    [SerializeField] private Sprite[] _sprites = null;
    [SerializeField] private SpriteRenderer _render = null;
    private Stamps currentLabel = default;

    private void Start()
    {
        currentLabel = Stamps.EMPTY;
        if(_render == null) _render = GetComponent<SpriteRenderer>();
    }

    public Stamps GetCurrentLabel() {

        return currentLabel;
    }

    public void DrawLabel(Stamps label, Vector2 position) {
 
        if (label == Stamps.EMPTY) {

            return;
        }

        currentLabel = label;

        _render.sprite = _sprites[(int)label];
        transform.position = new Vector2(position.x, position.y);
        Debug.Log(position.x + "  " + position.y);
    }

    public void ClearLabel()
    {
        currentLabel = Stamps.EMPTY;
        _render.sprite = null;
    }

}
