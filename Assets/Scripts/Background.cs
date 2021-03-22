using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Defines;

public class Background : Selectable
{
    public override sealed SelectedType type { get { return SelectedType.Background; } }

    public override sealed void ClickedOn(Player player)
    {
        player.Unselect();
        player.my_camera.followTransform = null;
    }
}
