using Spectre.Console;
using System;

namespace PixelSim.Services
{
    public class LoggingService
    {
        public static void WriteLine(string message) 
            => AnsiConsole.MarkupLine($"[gray][[{DateTime.Now.ToString()}]][/] {message}");
    }
}
