using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.NanoServer.ApiScan;
using Mono.Options;

namespace NanoApiScan
{
    public class Program
    {
        // from https://github.com/mono/mono/blob/mono-6.4.0.198/mcs/class/Mono.Options/Mono.Options/Options.cs#L1249
        private const int MaxHelpWidth = 80 - 29;

        public static int Main (string[] args)
        {
            var showHelp = false;
            var verbose = false;

            // create the scanner
            var apiScanner = new ApiScanner();
            apiScanner.WindowsKitsPath = @"C:\Program Files (x86)\Windows Kits";
            apiScanner.UseCurrentDirectory = false;

            // create our working directory
            var temp = Path.Combine(Path.GetTempPath(), "nano-api-scan", Guid.NewGuid().ToString());
            Directory.CreateDirectory(temp);
            apiScanner.BinaryPath = temp;

            var options = new OptionSet
            {
                {
                    "w|winkits=",
                    "The path to the Windows Kits, typically:".PadRight(MaxHelpWidth, ' ') +
                        @"C:\Program Files (x86)\Windows Kits",
                    v => apiScanner.WindowsKitsPath = v
                },
                { "?|h|help",  "Show this help message and exit", _ => showHelp = true },
                { "v|verbose", "Use a more verbose output", _ => verbose = true },
            };

            // parse the args
            List<string> extras;
            try
            {
                extras = options.Parse (args);

                if (extras == null || extras.Count == 0)
                    throw new OptionException("At least 1 native binary needs to be specified.", "BINARY");

                if (string.IsNullOrWhiteSpace(apiScanner.WindowsKitsPath) || !Directory.Exists(apiScanner.WindowsKitsPath))
                    throw new OptionException("A valid path to the Windows Kits needs to be specified.", "winkits");

                foreach (var binary in extras)
                {
                    if (!File.Exists(binary))
                        throw new OptionException($"Native binary '{binary}' does not exist.", "BINARY");

                    var name = Path.GetFileName(binary);
                    var dest = Path.Combine(temp, name);
                    if (File.Exists(dest))
                    {
                        var newName = Guid.NewGuid().ToString() + Path.GetExtension(binary);
                        dest = Path.Combine(temp, newName);
                        Console.WriteLine($"WARNING: A native binary with the file name of '{name}' has already been added, renaming to '{newName}'.");
                    }

                    File.Copy(binary, dest, true);
                }
            }
            catch (OptionException ex)
            {
                Console.Error.WriteLine($"A problem occured: {ex.Message}");
                Console.Error.WriteLine($"Use nano-api-scan --help for more details.");
                if (verbose)
                    Console.Error.WriteLine(ex);
                return 1;
            }

            if (showHelp)
            {
                ShowHelp (options);
                return 0;
            }

            // copying forwarders
            // TODO

            // run the scanner
            var current = Directory.GetCurrentDirectory();
            try
            {
                Directory.SetCurrentDirectory(temp);
                apiScanner.Run();
                return 0;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"An problem occured: {ex.Message}");
                if (verbose)
                    Console.Error.WriteLine(ex);
                return 1;
            }
            finally
            {
                Directory.SetCurrentDirectory(current);
                if (Directory.Exists(temp))
                    Directory.Delete(temp, true);
            }
        }

        private static void ShowHelp (OptionSet options)
        {
            Console.WriteLine ("Usage: nano-api-scan [OPTIONS]+ BINARY ...");
            Console.WriteLine ();
            Console.WriteLine ("Analyze the native binary to determine if it can run on Windows Nano Server.");
            Console.WriteLine ();
            Console.WriteLine ("Options:");
            options.WriteOptionDescriptions (Console.Out);
        }
    }
}
