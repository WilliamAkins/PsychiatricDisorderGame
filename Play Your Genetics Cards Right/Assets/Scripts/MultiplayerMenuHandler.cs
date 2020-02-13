using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MultiplayerMenuHandler : MonoBehaviour
{
    [SerializeField] private Button _btnHostGame;
    [SerializeField] private Button _btnConnectToGame;
    [SerializeField] private Button _btnReturn;

    private void Awake()
    {
        _btnHostGame.onClick.AddListener(BtnHostGamePressed);
        _btnConnectToGame.onClick.AddListener(BtnConnectToGamePressed);
        _btnReturn.onClick.AddListener(BtnReturnPressed);
    }

    // Start is called before the first frame update
    private void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        
    }

    private void BtnHostGamePressed()
    {
    }

    private void BtnConnectToGamePressed()
    {
        SceneManager.LoadScene("MainScene");
    }

    private void BtnReturnPressed()
    {
    }

    private IEnumerator LerpGameObjectY(GameObject c, float finalY, float speed)
    {
        float lerpTimer = 0.0f;
        Vector3 pos = c.transform.localPosition;

        while (lerpTimer < speed)
        {
            lerpTimer += Time.deltaTime;

            pos = Vector3.Lerp(pos, new Vector3(pos.x, finalY, pos.z), lerpTimer / speed);
            c.transform.localPosition = pos;

            yield return null;
        }
    }
}