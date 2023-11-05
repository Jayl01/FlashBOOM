using AnotherLib;
using Microsoft.Xna.Framework.Audio;

namespace FlashBOOM.Utilities
{
    public class GameMusicPlayer
    {
        public static SoundEffectInstance mainTheme;

        public static void Update()
        {
            return;
            if (mainTheme.State != SoundState.Playing)
            {
                mainTheme.Volume = GameData.MusicVolume;
                mainTheme.Play();
            }
        }

        public static void StopMusic()
        {
            mainTheme.Stop();
        }
    }
}
