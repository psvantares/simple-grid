using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Bootstrap
{
    public class BootstrapEntry : MonoBehaviour
    {
        private void Awake()
        {
            try
            {
                SceneManager.LoadScene(sceneBuildIndex: 1);
            }
            catch (Exception exception)
            {
                Debug.LogError(exception.Message);
            }
        }
    }
}