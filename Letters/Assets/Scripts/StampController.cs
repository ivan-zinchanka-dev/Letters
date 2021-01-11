using UnityEngine;

public class StampController : MonoBehaviour
{
    public bool isVisible = true;
    [SerializeField] public short stamp_number = 0;
    [SerializeField] private SceneController sceneController;
    private Collider2D collider;

    public void StopGame() {
  
        collider.enabled = false;
    }

    public void StartGame()
    {
        collider.enabled = true;
    }

    public void SetVisible(bool mode) {

        if (mode == true)
        {
            GetComponent<SpriteRenderer>().sortingLayerName = "Objects";
            isVisible = true;
        }
        else {

            GetComponent<SpriteRenderer>().sortingLayerName = "Background";
            isVisible = false;
        }

    }

    private void Start()
    {
        if (this.gameObject.GetComponent<BoxCollider2D>())
        {
            collider = GetComponent<BoxCollider2D>();
        }
        else if (this.gameObject.GetComponent<CircleCollider2D>())
        {
            collider = GetComponent<CircleCollider2D>();
        }

    }

    private void OnMouseUp()
    {
        if (isVisible)
        {
            SetVisible(false);
            sceneController.SetStamp((Stamps)stamp_number);
        }
        else {

            SetVisible(true);
            sceneController.CloseStamps();

        }       
    }

}


public enum Stamps {

    RED, BROWN, SWAMPY, GREEN, BLUE, PURPLE, EMPTY
} 
