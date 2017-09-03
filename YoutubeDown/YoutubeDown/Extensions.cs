using System.Windows.Forms;

namespace YoutubeDown
{
    public static class Extensions
    {
        public static void Invoke(this Control control, MethodInvoker methodInvoker)
        {
            control.Invoke(methodInvoker);
        }
    }
}
