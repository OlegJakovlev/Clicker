using UnityEngine;

namespace UI
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private EnemyUIManager enemyUIManager;
        [SerializeField] private PlayerUI playerUI;
        [SerializeField] private DebugUI debugUI;
        [SerializeField] private ControlsUI controlsUI;

        public void Initialise()
        {
            enemyUIManager.Initialise();
            playerUI.Initialise();
            debugUI.Initialise();
            controlsUI.Initialise();
        }
    }
}
