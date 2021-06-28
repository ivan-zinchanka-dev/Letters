using UnityEngine;

public class StampController : MonoBehaviour
{
    [SerializeField] private bool _isVisible = true;
    [SerializeField] public short number = 0;
    [SerializeField] private SceneController _sceneController = null;
    
    [SerializeField] private Collider2D _collider = null;
    [SerializeField] private SpriteRenderer _spriteRend = null;

    public bool IsVisible { 
        
        get {
            
            return _isVisible; 
        } 
        set {

            _spriteRend.sortingLayerName = value ? "Objects" : "Background";
            _isVisible = value; 
        } 
    }

    public void StopGame() {
  
        _collider.enabled = false;
    }

    public void StartGame()
    {
        _collider.enabled = true;
    }

    private void OnMouseUp()
    {
        if (IsVisible)
        {
            IsVisible = false;
            _sceneController.SetStamp((Stamps)number);
        }
        else {

            IsVisible = true;
            _sceneController.CloseStamps();
        }       
    }
}

public enum Stamps {

    RED, BROWN, SWAMPY, GREEN, BLUE, PURPLE, EMPTY
} 
