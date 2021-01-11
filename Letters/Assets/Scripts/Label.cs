using UnityEngine;

public class Label : MonoBehaviour
{
    [SerializeField] private Sprite[] sprites;
    private SpriteRenderer rend;
    private Stamps currentLabel;

    private void Start()
    {
        currentLabel = Stamps.EMPTY;
        rend = GetComponent<SpriteRenderer>();
    }

    public Stamps GetCurrentLabel() {

        return currentLabel;
    }

    public void DrawLabel(Stamps label, Vector2 position) {
 
        if (label == Stamps.EMPTY) {

            return;
        }

        currentLabel = label;

        rend.sprite = sprites[(int)label];
        transform.position = new Vector2(position.x, position.y);
        Debug.Log(position.x + "  " + position.y);
    }

    public void ClearLabel()
    {
        currentLabel = Stamps.EMPTY;
        rend.sprite = null;
    }

}
