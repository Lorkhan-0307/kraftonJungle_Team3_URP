using Michsky.UI.Dark;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LoadingManager : MonoBehaviour
{
    [SerializeField] ModalWindowManager loadingPanel;

    public static LoadingManager instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LoadingStart()
    {
        loadingPanel.ModalWindowIn();
    }
    public void LoadingEnd()
    {
        StartCoroutine(EndCoroutine());
    }
    IEnumerator EndCoroutine()
    {
        yield return new WaitForSeconds(1.5f);
        loadingPanel.ModalWindowOut();
    }
}
