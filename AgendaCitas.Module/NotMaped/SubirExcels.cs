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
using DevExpress.Xpo;
using DevExpress.Persistent.BaseImpl;

namespace AgendaCitas.Module.NotMaped
{
    [DomainComponent]
    [DefaultClassOptions]
    public class SubirExcel
    {
        public SubirExcel()
        {
            this.Archivo = new FileData(new Session());
        }

        [XafDisplayName("Subir archivo de Excel"), ToolTip("Botón para subir archivos de Excel")]
        [RuleRequiredField]
        public FileData Archivo { get; set; }
    }
}
