using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TransitionScene : MonoBehaviour
{
    private Button _button;

    // Start is called before the first frame update
    void Start()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(() => { SceneManager.LoadScene("Title"); });
    }

    // Update is called once per frame
    void Update()
    {
    }
}
