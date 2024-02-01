using System;
using Microsoft.Xna.Framework;

namespace Celeste.Mod.AnalogViewer {
    public class AnalogViewerModule : EverestModule {
        public static AnalogViewerModule Instance { get; private set; }

        public override Type SettingsType => typeof(AnalogViewerModuleSettings);
        public static AnalogViewerModuleSettings Settings => (AnalogViewerModuleSettings) Instance._Settings;

        public AnalogViewerModule() {
            Instance = this;
#if DEBUG
            // debug builds use verbose logging
            Logger.SetLogLevel(nameof(AnalogViewerModule), LogLevel.Verbose);
#else
            // release builds use info logging to reduce spam in log files
            Logger.SetLogLevel(nameof(AnalogViewerModule), LogLevel.Info);
#endif
        }

        public override void Load() {
            Everest.Events.Level.OnLoadLevel += modLoadLevel;
        }

        public override void Unload() {
           Everest.Events.Level.OnLoadLevel -= modLoadLevel;
        }
        
        private void modLoadLevel(Level level, Player.IntroTypes playerIntro, bool isFromLoader)
        {
            level.Add(new AnalogDisplay());
        }
    }
}
