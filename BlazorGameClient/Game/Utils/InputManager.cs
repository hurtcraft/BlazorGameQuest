using Microsoft.JSInterop;
using System.Collections.Generic;
using System.Threading.Tasks;

public class InputManager
{
    private readonly HashSet<string> pressedKeys = new();
    private readonly IJSRuntime js;

    public InputManager(IJSRuntime js)
    {
        this.js = js;
    }

    public async Task InitializeAsync()
    {
        await js.InvokeVoidAsync("inputManager.init", DotNetObjectReference.Create(this));
    }

    [JSInvokable]
    public void OnKeyDown(string key)
    {
        pressedKeys.Add(key);
    }

    [JSInvokable]
    public void OnKeyUp(string key)
    {
        pressedKeys.Remove(key);
    }

    public bool IsPressed(string key) => pressedKeys.Contains(key);
        public List<string> GetPressedKeys() => pressedKeys.ToList();

}
