using UnityEngine;
public static class Texture2DExtensions {
    public static Sprite ToSprite(this Texture2D texture) {
        return Sprite.Create(texture, new Rect(0f, 0f, texture.width, texture.height), Vector2.one * 0.5f);
    }
}