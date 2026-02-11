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
using AgendaCitas.Module.BusinessObjects.Mi_Negocio;

namespace AgendaCitas.Module.BusinessObjects.Agenda
{
    [DefaultClassOptions]
    [NavigationItem("AgendaCitaciones")]
    [XafDisplayName("Sucursales")]
    [DefaultProperty("Sucursal")]
    //[ImageName("BO_Contact")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class Sucursales : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        // Use CodeRush to create XPO classes and properties with a few keystrokes.
        // https://docs.devexpress.com/CodeRushForRoslyn/118557
        public Sucursales(Session session)
            : base(session)
        {
        }

        //hace bidireccional citas y sucursal
        [Association("Sucursal-Citas")]
        public XPCollection<CitaBase> Citas
        {
            get { return GetCollection<CitaBase>(nameof(Citas)); }
        }

        private Negocios _Negocio;
        [Association("Negocio-Sucursales")] // Relación con la empresa principal
        [XafDisplayName("Negocio / Empresa")]
        public Negocios Negocio
        {
            get { return _Negocio; }
            set { SetPropertyValue(nameof(Negocio), ref _Negocio, value); }
        }

        #region Propiedades

        private string _Sucursal;
        [XafDisplayName("Sucursal"), ToolTip("Nombre de la sucursal del negocio")]
        [RuleRequiredField]

        //Validacion que restringe la duplicacion del campo
        [RuleUniqueValue("RuleUniqueValue_Sucursales_Nombre", DefaultContexts.Save, "Ya existe una sucursal con este nombre.")]

        public string Sucursal
        {
            get { return _Sucursal; }
            set { SetPropertyValue(nameof(Sucursal), ref _Sucursal, value); }
        }

        private string _Ubicacion;
        [XafDisplayName("Ubicacion"), ToolTip("Ubicacion de la sucursal")]
        [RuleRequiredField]

        public string Ubicacion
        {
            get { return _Ubicacion; }
            set { SetPropertyValue(nameof(Ubicacion), ref _Ubicacion, value); }
        }

        private bool _Activo;
        [XafDisplayName("Activo")]

        public bool Activo
        {
            get { return _Activo; }
            set { SetPropertyValue(nameof(Activo), ref _Activo, value); }
        }

        #endregion

        public override void AfterConstruction()
        {
            base.AfterConstruction();
            this.Activo = true;
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }
        //private string _PersistentProperty;
        //[XafDisplayName("My display name"), ToolTip("My hint message")]
        //[ModelDefault("EditMask", "(000)-00"), Index(0), VisibleInListView(false)]
        //[Persistent("DatabaseColumnName"), RuleRequiredField(DefaultContexts.Save)]
        //public string PersistentProperty {
        //    get { return _PersistentProperty; }
        //    set { SetPropertyValue(nameof(PersistentProperty), ref _PersistentProperty, value); }
        //}

        //[Action(Caption = "My UI Action", ConfirmationMessage = "Are you sure?", ImageName = "Attention", AutoCommit = true)]
        //public void ActionMethod() {
        //    // Trigger a custom business logic for the current record in the UI (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112619.aspx).
        //    this.PersistentProperty = "Paid";
        //}
    }
}