using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using GEAR.Gadgets.RemoteTexture;

[RequireComponent(typeof(Image))]
public class RemoteTextureLoader : MonoBehaviour
{
    [SerializeField] private string url;

    private void Start()
    {
        var image = GetComponent<Image>();
        _ = LoadImage(image);
    }

    private async Task LoadImage(Image image)
    {
        image.sprite = await RemoteTexture.GetSprite(url);
    }
}
