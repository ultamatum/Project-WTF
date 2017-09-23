using System;

namespace Level_Editor
{
#if WINDOWS || LINUX
    public static class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            MapEditor form = new MapEditor();
            form.Show();
            form.game = new Game1(form.pctSurface.Handle, form, form.pctSurface);
            form.game.Run();
        }
    }
#endif
}
