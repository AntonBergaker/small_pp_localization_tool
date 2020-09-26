using System.IO;
using System.Text;

namespace SmallPPLocalizationTool {
    class BufferWriter : BinaryWriter {
        public BufferWriter(Stream output) : base(output) { }

        public override void Write(string value) {
            byte[] buffer = Encoding.UTF8.GetBytes(value);
            base.Write(buffer);
            base.Write((byte)0);
        }
    }
}
