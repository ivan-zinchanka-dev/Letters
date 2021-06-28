using UnityEngine;

public class ResultPaper : MonoBehaviour
{
    [SerializeField] private TextMesh _stats = null;
    [SerializeField] private TextMesh _generalMark = null;
    [SerializeField] private float _speed = 10.0f;                 // скорость движения отчётного листа
    [SerializeField] private Transform _paper = null;
    
    private bool _isMoving = false;    
    private const float TargetPointX = 0.0f;                       // целевые координаты отчётного листа
    private const float TargetPointY = -2.0f;
    private const float MaxAngle = 2.0f;                           // максимальный угол поворота листа

    public void SetMoving(bool mode)
    {
        _isMoving = mode;
    }

    public void SetData()
    {
        _stats.text = Stats.PrintStats();
        _generalMark.color = Stats.GetTextColor();
        _generalMark.text = Stats.PrintResult();       
    }

    private void Start()
    {       
        _paper.rotation = Quaternion.Euler(0.0f, 0.0f, Random.Range(-1 * MaxAngle, MaxAngle));
    }

    void Update()
    {
        if (_isMoving)
        {
            _paper.position = Vector2.MoveTowards(_paper.position, new Vector2(TargetPointX, TargetPointY), _speed * Time.deltaTime);
        }        
    }
}
