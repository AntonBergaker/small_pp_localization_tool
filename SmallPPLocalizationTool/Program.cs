using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace SmallPPLocalizationTool {
    class Program {
        static async Task Main(string[] args) {

            string url = null;
            string target = null;
            bool base64 = false;

            List<string> arguments = new List<string>(args);
            for (int i = 0; i < arguments.Count; i++) {
                string arg = arguments[i];
                if (arg == "-u" || arg == "-t") {
                    i++;
                    if (i < arguments.Count) {
                        if (arg == "-u") {
                            url = arguments[i];
                        }
                        else {
                            target = arguments[i];
                        }
                    }
                }

                if (arg == "-base64") {
                    base64 = true;
                }
            }

            if (url == null || target == null) {
                Console.WriteLine("Usage: LanguageExporter -u <url> -t <target>");
                return;
            }
            
            string data;
            using (var client = new HttpClient()) {
                try {
                    data = await client.GetStringAsync(url);
                } catch (WebException) {
                    Console.WriteLine("Failed to download specified file. Is the url valid?");
                    Environment.Exit(13);
                    return;
                } catch (Exception) {
                    Console.WriteLine("Something went wrong downloading the file.");
                    Console.WriteLine("");
                    throw;
                }
            }

            Document document;
            try {
                document = Document.Parse(data);
            }
            catch (DuplicateSectionException ex) {
                Console.WriteLine($"Section with the same name \"{ex.Section0.Name}\" already defined. Duplicates at lines {ex.Section0.Line}, {ex.Section1.Line}");
                Environment.Exit(13);
                return;
            }
            catch (DuplicateEntryException ex) {
                Console.WriteLine($"Entry with the same key \"{ex.Entry0.Key}\" already defined. Duplicates at lines {ex.Entry0.Line}, {ex.Entry1.Line}");
                Environment.Exit(13);
                return;
            }
            catch (Exception) {
                Console.WriteLine("Failed to parse the file. Is it a valid csv file?");
                Console.WriteLine("");
                throw;
            }

            try {
                Exporter exporter = new Exporter(document, base64);
                int result = exporter.Export(target);
                Console.WriteLine("Made " + result + " files.");
            }
            catch (Exception) {
                Console.WriteLine("Failed to export the resulting files.");
                Console.WriteLine("");
                throw;
            }
        }
    }
}
