using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Validation;

namespace AgendaCitas.Module.NotMaped
{
    [DomainComponent]
    [DefaultClassOptions]

    public class vcCargarAmbitoNegocio
    {

        public vcCargarAmbitoNegocio()
        {
            this.AmbitoElegido = string.Empty;
        }

        [XafDisplayName("Ámbito del negoco"), ToolTip("Escribe el ámbito que desea agregar al catalogo")]
        [RuleRequiredField]
        public string AmbitoElegido {
            get; set;
        }
    }
}
