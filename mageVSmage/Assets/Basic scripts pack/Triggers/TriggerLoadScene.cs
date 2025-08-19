using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Basic.Triggers
{
    public class TriggerLoadScene : MonoBehaviour
    {
        [Header("Behavior (Trigger uses ''Player'' tag)")]
        [SerializeField] private string _compareTag = "Player";
        [SerializeField] private bool _doNotUseAnyTag;

        [Tooltip("Use this if you want to do it through exclude layers in collider. Also you potentially need them when raycasting")]
        [SerializeField] private string _sceneNameToLoad;

        [Header("Time.scales zero/one by default")]
        [Range(0, 10)][SerializeField] private float _stopTimer = 1f;
        [Tooltip("Do not use, if not ready to freeze all or feel potential issues. Doesn't use Time.scale at all")]
        [SerializeField] private bool _useOldSchoolThreadSleep;

        [Header("Can be empty")]
        [Tooltip("Uses in-scene disabled canvas group (Add Component it manually to any UGUI part)\n(Enables gO and alpha = 1.)")]
        [SerializeField] private CanvasGroup _loadingCanvas;


        private async void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(_compareTag) && !_doNotUseAnyTag)
            {
                await LoadSceneSequenceAsync();
            }
            else
            {
                await LoadSceneSequenceAsync();
            }
        }

        public async Task LoadSceneSequenceAsync()
        {
            try
            {
                // Show loading text
                if (_loadingCanvas != null)
                {
                    _loadingCanvas.alpha = 1;
                    _loadingCanvas.gameObject.SetActive(true);
                }

                if (_useOldSchoolThreadSleep)
                {
                    int ms = Mathf.RoundToInt(_stopTimer * 1000);
                    Thread.Sleep(ms);
                }
                else
                {
                    Time.timeScale = 0;
                    await Task.Delay(Mathf.RoundToInt(_stopTimer * 1000));
                    Time.timeScale = 1;
                }

                // Load scene asynchronously
                var loadOperation = SceneManager.LoadSceneAsync(_sceneNameToLoad);
                loadOperation.allowSceneActivation = true;

                while (!loadOperation.isDone)
                {
                    await Task.Delay(10); // Small delay to prevent tight loop
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"Error loading scene: {e.Message}");
                throw;
            }
        }

        // To call it from a button or other event
        public void StartLoadingScene()
        {
            _ = LoadSceneSequenceAsync();
        }

    }

}