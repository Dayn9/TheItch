using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[RequireComponent(typeof(SpriteRenderer))]
public class Digit : MonoBehaviour {

    [SerializeField] private Sprite[] numbers;
    private SpriteRenderer render;

    private void Awake()
    {
        render = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        //SetNumber(10); //start blank
    }

    public void SetNumber(int num)
    {
        if (num < 0 || num > 10) { return; }
        render.sprite = numbers[num];
    }

    public void SetColor(Color newColor)
    {
        render.color = newColor;
    }


    /*                                                     IM JUST TRYNA DO SOMETIN COOL!
#if UNITY_EDITOR
    //custom editor for BasicFullTile
    [CustomEditor(typeof(Digit))]
    public class DigitEditor : Editor
    {
        private Digit digit { get { return (target as Digit); } }

        public void OnEnable()
        {
            if (digit.numbers == null || digit.numbers.Length != 10)
            {
                digit.numbers = new Sprite[10];
            }
        }
        public override void OnInspectorGUI()
        {
            EditorGUI.BeginChangeCheck();
            for(int i = 0; i< 10; i++)
            {
                digit.numbers[i] = (Sprite)EditorGUILayout.ObjectField(i.ToString(), digit.numbers[i], typeof(Sprite), false, null);
            }
            digit.numbers[10] = (Sprite)EditorGUILayout.ObjectField("none", digit.numbers[10], typeof(Sprite), false, null);

            if (EditorGUI.EndChangeCheck()) { EditorUtility.SetDirty(digit); }
        }
    }
#endif */
}
