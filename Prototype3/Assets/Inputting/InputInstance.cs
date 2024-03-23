public static class InputInstance
{
    private static Controls controls;
    public static Controls Controls
    {
        get
        {
            if (controls == null)
            {
                controls = new Controls();
            }

            controls.Enable();

            return controls;
        }
    }
}
