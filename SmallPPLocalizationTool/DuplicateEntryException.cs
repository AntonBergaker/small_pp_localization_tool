using System;

namespace SmallPPLocalizationTool {
    class DuplicateEntryException : Exception {
        public Language.Entry Entry0 { get; }
        public Language.Entry Entry1 { get; }

        public DuplicateEntryException(Language.Entry entry0, Language.Entry entry1) {
            Entry0 = entry0;
            Entry1 = entry1;
        }
    }
}
