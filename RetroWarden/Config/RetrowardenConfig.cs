namespace Retrowarden.Config
{
    public sealed class RetrowardenConfig
    {
        public RetrowardenConfig(string? cliLocation = "")
        {
            CLILocation = cliLocation;
        }

        public string? CLILocation { get; set; }
    }
}