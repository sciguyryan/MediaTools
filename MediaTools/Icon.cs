namespace MediaTools
{
    internal class IconModifier
    {
        public static void SetFormIcon(Form f)
        {
            var module = Environment.ProcessPath;
            if (module is null)
            {
                return;
            }

            var icon = Icon.ExtractAssociatedIcon(module);
            f.Icon = icon;
        }
    }
}
