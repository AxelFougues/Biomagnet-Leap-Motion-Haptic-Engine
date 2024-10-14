using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour{

    public Canvas canvas;
    public Button exit;
    public Button lodetones;
    public Button engine;

    private void Start() {
        exit.onClick.AddListener(delegate {
            if(Application.isEditor) UnityEditor.EditorApplication.isPlaying = false;
            else Application.Quit();
        });
        lodetones.onClick.AddListener(delegate { Application.OpenURL("https://github.com/AxelFougues/Lodestone-biomagnet-tools"); });
        engine.onClick.AddListener(delegate { Application.OpenURL("https://github.com/AxelFougues/Biomagnet-Leap-Motion-Haptic-Engine"); });
        Cursor.lockState = CursorLockMode.Locked;
    }


    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            canvas.gameObject.SetActive(!canvas.gameObject.activeSelf);
            if (canvas.gameObject.activeSelf) {
                Time.timeScale = 0;
                Cursor.lockState = CursorLockMode.Confined;
            } else {
                Time.timeScale = 1;
                Cursor.lockState = CursorLockMode.Locked;
            }
        }
    }

}
