using SmallPPLocalizationTool.Builder;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
namespace SmallPPLocalizationTool; 
public class Exporter {
    private readonly Document document;
    private readonly IBuilder builder;

    public Exporter(Document document, IBuilder builder) {
        this.document = document;
        this.builder = builder;
    }

    public int Export(string targetDirectory, bool includeMetaFile = false) {
        Directory.CreateDirectory(targetDirectory);

        var languagesThatAreReady = document.Where(x => x.HasEntry("meta", "completed") && x["meta"]["completed"].Value == "Yes").ToArray();

        foreach (var language in languagesThatAreReady) {
            ExportLanguage(targetDirectory, language, builder);
        }

        if (includeMetaFile) {
            var meta = CreateMetaLanguage(languagesThatAreReady);
            ExportLanguage(targetDirectory, meta, builder);
        }

        return languagesThatAreReady.Length + (includeMetaFile ? 1 : 0);
    }

    private static void ExportLanguage(string targetDirectory, Language language, IBuilder builder) {
        string path = Path.Combine(targetDirectory, language.ID + ".lang");

        using FileStream stream = new FileStream(path, FileMode.Create);

        builder.WriteToStream(language, stream);
    }

    private static Language CreateMetaLanguage(Language[] languagesThatAreReady) {
        List<Language.Section> metaSections = new();
        foreach (var lang in languagesThatAreReady) {
            var metaSection = lang["meta"];
            
            metaSections.Add(new Language.Section(lang.ID, 0, metaSection.GetEntries()));
        }

        return new Language("meta", metaSections);
    }
}
