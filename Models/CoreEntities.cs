using System;
using System.Collections.Generic;

namespace webviewer.Models
{
    public class CoreEntities
    {
            public List<Language> LanguageList {get; set;}

            public List<Format> FormatList {get; set;}

            public List<Subformat> SubformatList {get; set;}

            public List<Authorization> AuthorizationList {get; set;}

    }
}