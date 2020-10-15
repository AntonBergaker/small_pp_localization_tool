using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SmallPPLocalizationTool {
    interface IBuilder {
        void WriteToStream(Language language, Stream stream);
    }
}
