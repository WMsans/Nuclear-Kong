using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MoveToWinScreen : MonoBehaviour
{
    void OnEnable()
    {
        StartCoroutine(GoNext());
    }

    private IEnumerator GoNext()
    {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("WinScene", LoadSceneMode.Single);
    }
}
