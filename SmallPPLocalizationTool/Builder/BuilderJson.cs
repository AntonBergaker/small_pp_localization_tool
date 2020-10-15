using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;

namespace SmallPPLocalizationTool
{
    class BuilderJson : IBuilder
    {
        public void WriteToStream(Language language, Stream stream) {
            using Utf8JsonWriter writer = new Utf8JsonWriter(stream, new JsonWriterOptions()
                { Indented = true}
            );
            writer.WriteStartObject();
            foreach (Language.Section section in language.GetSections()) {
                writer.WriteStartObject(section.Name);
                foreach (Language.Entry entry in section.GetEntries()) {
                    writer.WriteString(entry.Key, entry.Value);
                }
                writer.WriteEndObject();
            }

            writer.WriteEndObject();
        }
    }
}
