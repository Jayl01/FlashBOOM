using AnotherLib.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FlashBOOM.UI
{
    public class PlayerUI : UIObject
    {
        public static Texture2D playerIndicatorTexture;
        public static Texture2D[] runeSymbolTextures;
        public static Texture2D heartSymbol;
        public static PlayerUI NewPlayerUI()
        {
            PlayerUI playerUI = new PlayerUI();
            playerUI.Initialize();
            return playerUI;
        }

        public override void Initialize()
        {

        }

        public override void ReInitializePositions()
        {

        }

        public override void Update()
        {

        }

        public override void Draw(SpriteBatch spriteBatch)
        {

        }
    }
}
