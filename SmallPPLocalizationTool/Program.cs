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
            ExporterType type = ExporterType.Buffer;

            List<string> arguments = new List<string>(args);
            for (int i = 0; i < arguments.Count; i++) {
                string arg = arguments[i];
                if (arg == "-url" || arg == "-target" || arg == "-type") {
                    i++;
                    if (i < arguments.Count) {
                        if (arg == "-url") {
                            url = arguments[i];
                            continue;
                        }
                        if (arg == "-target")  {
                            target = arguments[i];
                            continue;
                        }

                        if (arg == "-type") {
                            switch (arguments[i]) {
                                case "json":
                                    type = ExporterType.Json;
                                    break;
                                case "base64":
                                    type = ExporterType.BufferBase64;
                                    break;
                                case "buffer":
                                    type = ExporterType.Buffer;
                                    break;
                                default:
                                    Console.WriteLine("Unsupported file type. Valid types are \"json\", \"base64\" or \"buffer\".");
                                    Environment.Exit(13);
                                    return;
                            }
                        }
                    }
                }
            }

            if (url == null || target == null) {
                Console.WriteLine("Usage: ./small_pp_localization_tool -url <url> -target <target_directory> [-type <file_type>]");
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
                Exporter exporter = new Exporter(document, type);
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
