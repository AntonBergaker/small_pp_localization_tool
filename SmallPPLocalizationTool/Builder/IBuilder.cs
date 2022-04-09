using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SmallPPLocalizationTool {
    public interface IBuilder {
        void WriteToStream(Language language, Stream stream);
    }
}
