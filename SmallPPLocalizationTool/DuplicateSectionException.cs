using System;

namespace SmallPPLocalizationTool {
    class DuplicateSectionException : Exception {
        public Language.Section Section0 { get; }
        public Language.Section Section1 { get; }

        public DuplicateSectionException(Language.Section section0, Language.Section section1) {
            Section0 = section0;
            Section1 = section1;
        }
    }
}
