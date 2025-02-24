using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

//public class LoadScene : MonoBehaviour
//{
//    [SerializeField] private Slider loadbar;
//    [SerializeField] private GameObject loadPanel;

//    public void SceneLoad(int sceneIndex)
//    {
//        loadPanel.SetActive(true);
//        StartCoroutine(LoadAsync(sceneIndex));
//    }

//    IEnumerator LoadAsync(int sceneIndex)
//    {
//        Debug.Log("Cargando escena...");
//        AsyncOperation asyncOperation = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneIndex);
//        while (!asyncOperation.isDone)
//        {
//            Debug.Log(asyncOperation.progress);
//            loadbar.value = asyncOperation.progress / 0.9f;
//            yield return null;
//        }
//        yield return new WaitForSeconds(5f);
//    }
//}



public class LoadScene : MonoBehaviour
{
    [SerializeField] private Slider loadbar;  // Referencia al slider de la barra de carga.
    [SerializeField] private GameObject loadPanel;  // Referencia al panel de carga.

    public void SceneLoad(int sceneIndex)
    {
        loadPanel.SetActive(true);  // Muestra el panel de carga.
        StartCoroutine(LoadAsync(sceneIndex));
    }

    IEnumerator LoadAsync(int sceneIndex)
    {
        // Inicia la carga as�ncrona de la escena y no permite que se active autom�ticamente.
        AsyncOperation asyncOperation = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneIndex);
        asyncOperation.allowSceneActivation = false;  // Desactiva la activaci�n autom�tica de la escena.

        // Mientras la carga no alcance el 90%, actualiza la barra de progreso.
        while (asyncOperation.progress < 0.9f)
        {
            Debug.Log(asyncOperation.progress);
            loadbar.value = asyncOperation.progress / 0.9f;  // Actualiza la barra de carga en base al progreso.
            yield return null;
        }

        // La escena est� completamente cargada (al 90%).
        Debug.Log("La escena est� completamente cargada.");
        loadbar.value = 1f;  // Asegura que la barra de carga est� llena al 100%.

        // Espera 5 segundos adicionales con la pantalla de carga visible.
        yield return new WaitForSeconds(3f);
        Debug.Log("Esper� 5 segundos adicionales con la pantalla de carga visible.");

        // Ahora permite que la escena se active.
        asyncOperation.allowSceneActivation = true;
    }
}