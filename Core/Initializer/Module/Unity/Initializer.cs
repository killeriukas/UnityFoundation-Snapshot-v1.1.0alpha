using System;
using TMI.AssetManagement;
using TMI.ConfigManagement.Unity;
using TMI.ConfigManagement.Unity.UI;
using TMI.Notification;
using TMI.SceneManagement;
using TMI.UI;
using UnityEngine;

namespace TMI.Core.Unity {
    
    public abstract class Initializer : BaseInitializer {

        [SerializeField]
        private DefaultConfig config;

        private IAssetManager assetManager;
        private IExecutionManager executionManager;
        private IPauseManager pauseManager;
        
        protected INotificationManager notificationManager { get; private set; }
        protected ISceneManager sceneManager { get; private set; }
        protected IUIManager uiManagerDefault { get; private set; }
        
        protected override void PreAwake() {
            base.PreAwake();
            ConfigManager.Load(config);
        }

        protected override void Awake() {
            base.Awake();
            IGroup assetGroup = CreateUIAssetCacheGroup();
            Assert.IsNull(assetGroup, "Asset cache cannot be null! If you don't need it, inherit from BaseInitializer instead!");

            IRequestHandler requestHandler = RequestHandler.Create(OnUIGroupLoaded);
            assetManager.LoadAsync(assetGroup, requestHandler);
        }
        
        protected override void RegisterManagers(IAcquirer acquirer) {
            executionManager = acquirer.AcquireManager<ExecutionManager, IExecutionManager>();
            pauseManager = acquirer.AcquireManager<PauseManager, IPauseManager>();
            assetManager = acquirer.AcquireManager<AssetManager, IAssetManager>();
            
            notificationManager = acquirer.AcquireManager<NotificationManager, INotificationManager>();
            sceneManager = acquirer.AcquireManager<SceneManager, ISceneManager>();

            UISettingsConfig uiSettingsConfig = ConfigManager.GetConfig<UISettingsConfig>();
            Type defaultUIManagerType = uiSettingsConfig.GetDefaultUIManagerType();
            uiManagerDefault = acquirer.AcquireManager<IUIManager>(defaultUIManagerType);
        }

        private void OnUIGroupLoaded(ILoaderComplete complete) {
            uiManagerDefault.Precache(complete);
            OnUIAssetsCached();
        }

        protected abstract void OnUIAssetsCached();
        protected abstract IGroup CreateUIAssetCacheGroup();
        

        

        


        
    }

    
}

