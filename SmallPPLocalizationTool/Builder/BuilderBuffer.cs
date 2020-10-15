using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SmallPPLocalizationTool {
    class BuilderBuffer : IBuilder
    {
        private readonly bool useBase64;

        public BuilderBuffer(bool useBase64) {
            this.useBase64 = useBase64;
        }

        public void WriteToStream(Language language, Stream stream) {
            Language.Section[] sections = language.GetSections();
            using MemoryStream memStream = new MemoryStream();
            BufferWriter writer = new BufferWriter(memStream);

            writer.Write(sections.Length);

            foreach (Language.Section section in sections)
            {
                writer.Write(section.Name);

                Language.Entry[] entries = section.GetEntries();
                writer.Write(entries.Length);

                foreach (Language.Entry entry in entries)
                {
                    writer.Write(entry.Key);
                    writer.Write(entry.Value);
                }
            }

            if (useBase64) {
                byte[] bytes = Encoding.UTF8.GetBytes(Convert.ToBase64String(memStream.ToArray()));
                stream.Write(bytes);
            }
            else {
                memStream.WriteTo(stream);
            }
        }
    }
}
