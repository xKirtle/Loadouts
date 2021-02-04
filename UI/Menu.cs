using Terraria;
using Terraria.UI;
using Terraria.ModLoader;
using Terraria.GameContent.UI.Elements;
using Microsoft.Xna.Framework;
using Loadouts.Commands;
using Loadouts.Utils;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;

namespace Loadouts.UI
{
    public class Menu : DraggableUIPanel
    {
        //Might want to make this a ModConfig variable?
        public const int maxLoadouts = 10;
        const string texturePath = "Loadouts/Textures/";

        private UIImage[] elements;
        private UIText loadoutText;
        private readonly string[] names = new string[] {"Minus", "Left", "Right", "Plus"};
        private readonly int[] offset = new int[] {14, 48, 106, 140};
        private bool isLocked;

        //Show/Hide stuff
        private UIElement parent;
        public bool Visible { get; private set; }

        public Menu() : base(ModContent.GetTexture(texturePath + "Background"))
        {
            Width.Set(172, 0);
            Height.Set(46, 0);
            Left.Set(275f, 0);
            Top.Set(255f, 0);

            OnMouseDown += (element, listener) =>
            {
                Vector2 MenuPosition = new Vector2(Left.Pixels, Top.Pixels);
                Vector2 clickPos = Vector2.Subtract(element.MousePosition, MenuPosition);
                canDrag = !isLocked && clickPos.X >= 10 && clickPos.Y >= 10;
            };

            elements = new UIImage[5];
            for (int i = 0; i < elements.Length - 1; i++)
            {
                int index = i;
                UIImage temp = new UIImage(ModContent.GetTexture(texturePath + names[index]))
                {
                    Width = {Pixels = 28},
                    Height = {Pixels = 28},
                    Left = {Pixels = offset[index]},
                    Top = {Pixels = 14}
                };
                temp.OnClick += (__, _) => Click(index);
                temp.OnMouseOver += (__, _) => Hover(index, true);
                temp.OnMouseOut += (__, _) => Hover(index, false);

                elements[index] = temp;
                Append(temp);
            }

            loadoutText = new UIText("")
            {
                Width = {Pixels = 12},
                Height = {Pixels = 18},
                Left = {Pixels = 85},
                Top = {Pixels = 19}
            };
            Append(loadoutText);

            UIImage dragLock = new UIImage(ModContent.GetTexture(texturePath + "Lock0"))
            {
                Width = {Pixels = 22},
                Height = {Pixels = 22},
                Left = {Pixels = 2},
                Top = {Pixels = 2}
            };
            dragLock.OnMouseOver += (__, _) => LockHover(true);
            dragLock.OnMouseOut += (__, _) => LockHover(false);
            dragLock.OnClick += (__, _) =>
            {
                SoundEngine.PlaySound(SoundID.Unlock);
                isLocked = !isLocked;
                string lockName = "Lock" + (isLocked ? "0" : "1") + "Hover";
                dragLock.SetImage(ModContent.GetTexture(texturePath + lockName));
                
                if (isLocked)
                    Main.LocalPlayer.GetModPlayer<ELPlayer>().menuOffset =
                        new Vector2((int) Left.Pixels, (int) Top.Pixels);
            };
            elements[4] = dragLock;
            Append(dragLock);

            isLocked = Visible = true;
        }

        private void Hover(int index, bool mouseOver)
        {
            string path = texturePath + names[index];
            if (mouseOver)
            {
                SoundEngine.PlaySound(SoundID.MenuTick);
                path += "Hover";
            }

            elements[index].SetImage(ModContent.GetTexture(path));
        }

        private void LockHover(bool mouseOver)
        {
            if (mouseOver) SoundEngine.PlaySound(SoundID.MenuTick);

            string lockName = "Lock" + (isLocked ? "0" : "1") + (mouseOver ? "Hover" : "");
            int offset = mouseOver ? 0 : 2;
            elements[4].Left.Set(offset, 0);
            elements[4].Top.Set(offset, 0);
            elements[4].SetImage(ModContent.GetTexture(texturePath + lockName));
        }

        public static void Click(int index)
        {
            ELPlayer mp = Main.LocalPlayer.GetModPlayer<ELPlayer>();
            //Don't want the delete request to be on hold if player clicks somewhere else
            if (index != 0) ConfirmDelete.deleteRequest = false;

            switch (index)
            {
                case 0: //Minus
                    if (!ConfirmDelete.deleteRequest && mp.loadouts.Count > 1)
                    {
                        SoundEngine.PlaySound(SoundID.MenuTick);
                        Main.NewText(
                            "Are you sure you want to remove this loadout? Every item in the loadout will be deleted. Type \"/confirm\" to remove it.",
                            Color.Red);
                        ConfirmDelete.deleteRequest = true;
                    }

                    break;
                case 1: //Left
                    if (mp.loadoutIndex > 0)
                    {
                        SoundEngine.PlaySound(SoundID.MenuTick);
                        mp.loadouts[mp.loadoutIndex].SaveLoadout();
                        mp.loadouts[--mp.loadoutIndex].LoadLoadout();
                    }

                    break;
                case 2: //Right
                    if (mp.loadoutIndex < mp.loadouts.Count - 1)
                    {
                        SoundEngine.PlaySound(SoundID.MenuTick);
                        mp.loadouts[mp.loadoutIndex].SaveLoadout();
                        mp.loadouts[++mp.loadoutIndex].LoadLoadout();
                    }

                    break;
                case 3: //Plus
                    if (mp.loadouts.Count < maxLoadouts)
                    {
                        SoundEngine.PlaySound(SoundID.MenuTick);
                        mp.loadouts[mp.loadoutIndex].SaveLoadout();

                        Loadout loadout = new Loadout();
                        mp.loadouts.Add(loadout);
                        mp.loadoutIndex = mp.loadouts.Count - 1;

                        mp.loadouts[mp.loadoutIndex].LoadLoadout();
                    }

                    break;
            }
        }

        public void Update()
        {
            if (!Visible) return;

            UpdatePosition();
            loadoutText.SetText(Main.LocalPlayer.GetModPlayer<ELPlayer>().loadoutIndex.ToString());
        }

        public void Show()
        {
            if (Visible) return;

            Visible = true;
            parent?.Append(this);
        }

        public void Hide()
        {
            if (!Visible) return;

            Visible = false;
            parent = Parent;
            Remove();
        }
    }
}