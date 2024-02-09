using Utilities;

public class Input : Singleton<Input> {
    public PlayerControls playerControls;

    private void Start() {
        playerControls = new PlayerControls();
        playerControls.Enable();
    }
}