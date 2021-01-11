using UnityEngine;

public class Results : MonoBehaviour
{
    [SerializeField] private TextMesh stats;
    [SerializeField] private TextMesh generalMark;
    [SerializeField] private float speed = 10.0f;           // скорость отчётного листа

    private bool isMoving = false;
    private Transform papper;
    private const float targetPointX = 0.0f;                // целевые координаты отчётного листа
    private const float targetPointY = -2.0f;
    const float max_angle = 2.0f;                           // максимальный угол поворота листа

    public void SetMoving(bool mode)
    {
        isMoving = mode;
    }

    public void SetData()
    {
        stats.text = Stats.PrintStats();
        generalMark.color = Stats.GetTextColor();
        generalMark.text = Stats.PrintResult();
       
    }

    private void Start()
    {
        papper = GetComponent<Transform>();
        papper.rotation = Quaternion.Euler(0.0f, 0.0f, Random.Range(-1 * max_angle, max_angle));
    }

    void Update()
    {
        if (isMoving)
        {
            papper.position = Vector2.MoveTowards(papper.position, new Vector2(targetPointX, targetPointY), speed * Time.deltaTime);
        }

        
    }
}
