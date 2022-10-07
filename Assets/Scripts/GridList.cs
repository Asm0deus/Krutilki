using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using TMPro;

public class GridList : MonoBehaviour
{
    [SerializeField] private GameObject _krutilka;
    [SerializeField] private GameObject[,] _krutilkas;
    [SerializeField] private List<GameObject> _listKrutilki;
    [SerializeField] private Dictionary<Vector2Int, GameObject> _dictKrutilki = new Dictionary<Vector2Int, GameObject>();

    [Space]
    [Header("Size of Array")]
    [Range(4, 7)]
    [SerializeField] private int _fieldSize = 4;
    [SerializeField] private Slider _sliderFieldSize;

    private IEnumerator coroutine;
    private int _stareAll;

    [SerializeField] private Transform parant;
    [SerializeField] private TMP_Text _topText;
    [SerializeField] private string _startText = "Start text";
    [SerializeField] private string _wonText = "Won text";

    private void Start()
    {
        _topText.text = _startText;
    }
    
    private void CreateKrutilki()
    {
        for (int x = 0; x < _fieldSize; x++)
        {
            for (int y = 0; y < _fieldSize; y++)
            {
                Vector2Int xy = new Vector2Int(x, y);
                
                _krutilkas[x, y] = _krutilka;
                _krutilkas[x, y].name = (x).ToString() + (y).ToString();

                GameObject newKrutilka = Instantiate(_krutilkas[x, y], new Vector2(transform.position.x + x, transform.position.y - y), transform.rotation, transform);

                _listKrutilki.Add(newKrutilka);

                Krutilka krutilkaNew = newKrutilka.GetComponent<Krutilka>();
                krutilkaNew.SetIndex(x, y);

                _dictKrutilki[xy] = krutilkaNew.gameObject;
            }
        }

        //Центровка поля по экрану
        float krutilkaSize = _krutilka.GetComponent<RectTransform>().transform.localScale.x;
        float centerXY = krutilkaSize / 2 * _fieldSize - krutilkaSize / 2;

        transform.position = new Vector3(-centerXY, centerXY, transform.position.z);
    }

    public void Check(Vector2Int index, int state)
    {

        for (int x = index.x - 1; x >= 0; x--)
        {
            _dictKrutilki[new Vector2Int(x, index.y)].gameObject.GetComponent<Krutilka>().ChangeColor();
        }

        if (index.x < _fieldSize-1)
        {
            for (int x = index.x + 1; x <= _fieldSize-1; x++)
            {
                _dictKrutilki[new Vector2Int(x, index.y)].gameObject.GetComponent<Krutilka>().ChangeColor();
            }
        }


        for (int y = index.y - 1; y >= 0; y--)
        {
            _dictKrutilki[new Vector2Int(index.x, y)].gameObject.GetComponent<Krutilka>().ChangeColor();
        }

        if (index.y < _fieldSize - 1)
        {
            for (int y = index.y + 1; y <= _fieldSize - 1; y++)
            {
                _dictKrutilki[new Vector2Int(index.x, y)].gameObject.GetComponent<Krutilka>().ChangeColor();
            }
        } 
        CheckStatement(state);
    }

    private void StartRandomKrutilkiPosition()
    {
        for (int i = 0; i < _krutilkas.Length; i++)
        {
            Vector2Int xyRandom = new Vector2Int(Random.Range(0, 3), Random.Range(0, 3));
            _dictKrutilki[xyRandom].gameObject.GetComponent<Krutilka>().ChangeColorByButton();
        }
    }
    private IEnumerator WaitAndPrint(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        StartRandomKrutilkiPosition();
    }

    public void CheckStatement(int state)
    {
        for (int i = 0; i < _fieldSize; i++)
        {
            for (int j = 0; j < _fieldSize; j++)
            {
                int statefromKrut = _dictKrutilki[new Vector2Int(i, j)].gameObject.GetComponent<Krutilka>().GetState();

                _stareAll += statefromKrut;
                
                if (_stareAll == _fieldSize * _fieldSize) _topText.text = _wonText;
            }
        }
        _stareAll = 0;
    }

    public void GenerateField()
    {
        _fieldSize = 0;
        _krutilkas = null;
        _listKrutilki = null;
        _topText.text = _startText;
        foreach (Transform child in parant)
        {
            Destroy(child.gameObject);
        }

        _fieldSize = Convert.ToInt32(_sliderFieldSize.value);
        _krutilkas = new GameObject[_fieldSize, _fieldSize];
        _listKrutilki = new List<GameObject>();
        CreateKrutilki();

        coroutine = WaitAndPrint(.1f);
        StartCoroutine(coroutine);
    }
}
