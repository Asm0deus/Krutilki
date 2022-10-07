using UnityEngine;
using UnityEngine.UI;

public class Krutilka : MonoBehaviour
{
    [SerializeField] private Vector2Int _index;
    [SerializeField] private int _state;
    [SerializeField] private Image _btnColor;

    [SerializeField] private GameObject _onText;
    [SerializeField] private GameObject _offText;

    [SerializeField] private GridList _gridStart;
    void Start()
    {
        ChangeStateZero();
        _gridStart = FindObjectOfType<GridList>();
    }

    public void ChangeColorByButton()
    {
        if (_state == 1)  ChangeStateZero();
        else ChangeOneState();
        
        _gridStart.Check(_index, _state);
    }
    public void ChangeColor()
    {
        if (_state == 1) ChangeStateZero();
        else ChangeOneState();
    }

    private void ChangeStateZero()
    {
        _state = 0;
        _btnColor.color = Color.white;
        _onText.SetActive(false);
        _offText.SetActive(true);
    }

    private void ChangeOneState()
    {
        _state = 1;
        _btnColor.color = new Color(1f, 0.5411765f, 0.2313726f);
        _onText.SetActive(true);
        _offText.SetActive(false);
    }

    public void SetIndex(int x, int y)
    {
        _index = new Vector2Int(x,y);
    }

    public int GetState() => _state;
}
