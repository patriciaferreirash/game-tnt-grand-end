using UnityEngine;
using System.Runtime.InteropServices;

public class ResizeAndCenterWindow : MonoBehaviour
{
    [DllImport("user32.dll")]
    private static extern bool MoveWindow(System.IntPtr hWnd, int x, int y, int width, int height, bool repaint);

    [DllImport("user32.dll")]
    private static extern System.IntPtr GetActiveWindow();

    void Start()
    {
        int targetWidth = 540;
        int targetHeight = 960;

        // Ajusta caso a tela seja pequena
        if (Screen.height < targetHeight)
        {
            float scale = (float)Screen.height / targetHeight;
            targetWidth = Mathf.RoundToInt(targetWidth * scale);
            targetHeight = Mathf.RoundToInt(targetHeight * scale);
        }

        // Define a resolução
        Screen.SetResolution(targetWidth, targetHeight, false);

        // Espera um pequeno tempo pra garantir que a janela atualize
        Invoke("CenterWindow", 0.1f);
    }

    void CenterWindow()
    {
        var handle = GetActiveWindow();
        int screenWidth = Display.main.systemWidth;
        int screenHeight = Display.main.systemHeight;

        int posX = (screenWidth - Screen.width) / 2;
        int posY = (screenHeight - Screen.height) / 2;

        MoveWindow(handle, posX, posY, Screen.width, Screen.height, true);
    }
}

