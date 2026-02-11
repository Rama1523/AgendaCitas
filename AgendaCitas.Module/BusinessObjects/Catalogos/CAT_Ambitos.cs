using System;
using System.Linq;
using System.Text;
using DevExpress.Xpo;
using DevExpress.ExpressApp;
using System.ComponentModel;
using DevExpress.ExpressApp.DC;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using System.Collections.Generic;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;

namespace AgendaCitas.Module.BusinessObjects.Catalogo
{
    [DefaultClassOptions]
    [NavigationItem("Catalogos")]
    [XafDisplayName("Ámbito")]
    [DefaultProperty("Ambito")]
    public class CAT_Ambitos : BaseObject
    { 
        public CAT_Ambitos(Session session)
            : base(session)
        {
        }

        #region Propiedades
        private string _Ambito;
        [XafDisplayName("Ámbito"), ToolTip("Ámbito del negocio")]
        [RuleRequiredField]

        //Validacion que restringe la duplicacion del campo
        [RuleUniqueValue("RuleUniqueValue_CAT_Ambitos_Ambito", DefaultContexts.Save, "Ya existe un Ámbito con este nombre.")]

        public string Ambito
        {
            get { return _Ambito; }
            set { SetPropertyValue(nameof(Ambito), ref _Ambito, value); }
        }


        private bool _Visible;
        [XafDisplayName("Visible")]

        public bool Visible
        {
            get { return _Visible; }
            set { SetPropertyValue(nameof(Visible), ref _Visible, value); }
        }
        #endregion

        #region Metodos
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            this.Visible = true;
        }
        #endregion
        
    }
}