namespace MinigameTemplate
{
    using Boom;
    using UnityEngine;

    public class CanvasController : MonoBehaviour
    {
        [SerializeField] GameObject loginCanvas;
        [SerializeField] GameObject gameplayCanvas;

        [SerializeField, ShowOnly] bool isLoggedIn;


        private void Awake()
        {
            ///Register to listent to any login state change
            UserUtil.AddListenerMainDataChange<MainDataTypes.LoginData>(LoginStateChangeHandler);
        }

        private void Start()
        {
            loginCanvas.SetActive(true);
            gameplayCanvas.SetActive(false);
        }

        private void OnDestroy()
        {
            ///Unregister to stop listening to any login state change
            UserUtil.RemoveListenerMainDataChange<MainDataTypes.LoginData>(LoginStateChangeHandler);
        }


        /// <summary>
        /// Handler that listen to any login state change
        /// </summary>
        /// <param name="data"></param>
        private void LoginStateChangeHandler(MainDataTypes.LoginData data)
        {
            if (data.state == MainDataTypes.LoginData.State.LoggedIn)
            {
                loginCanvas.SetActive(false);
                gameplayCanvas.SetActive(true);
                isLoggedIn = true;
            }
            else isLoggedIn = false;
        }
    }
}