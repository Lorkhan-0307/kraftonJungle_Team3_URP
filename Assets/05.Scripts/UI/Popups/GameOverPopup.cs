using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Michsky.UI.Dark;
using UnityEngine.SceneManagement;

public class GameOverPopup : MonoBehaviour
{
    [SerializeField] private GameObject[] winnerObjects;


    private ModalWindowManager _mwm;
    
    public void SetupWinner(bool isMonsterWon)
    {
        if (isMonsterWon)
        {
            _mwm.description = "MONSTERS HAS WON";
            winnerObjects[0].SetActive(true);
            winnerObjects[1].SetActive(false);
            
        }
        else
        {
            _mwm.description = "SCIENTISTS HAVE WON";
            winnerObjects[0].SetActive(false);
            winnerObjects[1].SetActive(true);
        }
        
    }

    public void OnClickConfirm()
    {
        // 여기에서 메인 화면으로 돌아가는 코드를 작성하면 됩니다.
        Destroy(NetworkManager.Instance.gameObject);
        SceneManager.LoadScene(0);
    }
}
