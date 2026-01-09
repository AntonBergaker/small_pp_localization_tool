using System.IO;

namespace SmallPPLocalizationTool.Builder; 
public interface IBuilder {
    void WriteToStream(Language language, Stream stream);
}
