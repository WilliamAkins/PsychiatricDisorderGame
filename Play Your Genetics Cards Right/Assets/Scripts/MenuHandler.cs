using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuHandler : MonoBehaviour
{
    [SerializeField] private Button _btnPlay;
    [SerializeField] private Button _btnHostGame;
    [SerializeField] private Button _btnSettings;
    [SerializeField] private Button _btnCredits;
    [SerializeField] private Button _btnExit;

    private void Awake()
    {
        _btnPlay.onClick.AddListener(BtnPlayPressed);
        _btnHostGame.onClick.AddListener(btnHostGamePressed);
        _btnCredits.onClick.AddListener(btnCreditsPressed);
        _btnSettings.onClick.AddListener(btnSettingsPressed);
        _btnCredits.onClick.AddListener(btnCreditsPressed);
        _btnExit.onClick.AddListener(btnExitPressed);
    }

    // Start is called before the first frame update
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
        
    }

    private void BtnPlayPressed()
    {
        SceneManager.LoadScene("MainScene");
    }

    private void btnHostGamePressed()
    {
        SceneManager.LoadScene("HostScene");
    }

    private void btnSettingsPressed()
    {

    }

    private void btnCreditsPressed()
    {

    }

    private void btnExitPressed()
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