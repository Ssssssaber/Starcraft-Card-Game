using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace Menu
{
    public class PlayButton : MonoBehaviour
    {
        public static UnityEvent GameStart = new UnityEvent();
        public void PlayGame()
        {
            GameStart?.Invoke();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}