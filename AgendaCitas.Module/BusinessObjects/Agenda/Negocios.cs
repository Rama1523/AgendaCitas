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
using AgendaCitas.Module.BusinessObjects.Catalogo;

namespace AgendaCitas.Module.BusinessObjects.Agenda
{
    [NavigationItem("AgendaCitaciones")]
    [XafDisplayName("Negocios")]
    [DefaultProperty("Negocio")]
    //[ImageName("BO_Contact")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).

    public class Negocios : XPObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        // Use CodeRush to create XPO classes and properties with a few keystrokes.
        // https://docs.devexpress.com/CodeRushForRoslyn/118557


        public Negocios(Session session)
            : base(session)
        {
        }

        /*private string _PersistentProperty;
        [XafDisplayName("My display name"), ToolTip("My hint message")]
        //[ModelDefault("EditMask", "(000)-00"), Index(0), VisibleInListView(false)]
        //[Persistent("DatabaseColumnName"), RuleRequiredField(DefaultContexts.Save)]
        public string PersistentProperty
        {
            get { return _PersistentProperty; }
            set { SetPropertyValue(nameof(PersistentProperty), ref _PersistentProperty, value); }
        }*/

        //Propiedades de la clase
        #region Propiedades

        private string _Negocio;
        [XafDisplayName("Negocio"), ToolTip("Nombre del negocio")]
        [RuleRequiredField]

        //Validacion que restringe la duplicacion del campo
        [RuleUniqueValue("RuleUniqueValue_Negocios_Negocio", DefaultContexts.Save, "Ya existe un Negocio con este nombre.")]

        public string Negocio   
        {
            get { return _Negocio; }
            set { SetPropertyValue(nameof(Negocio), ref _Negocio, value); }
         }




        private DateTime _Fecha;
        [XafDisplayName("Fecha"), ToolTip("Fecha de union del negocio")]
        [VisibleInListView(false)]
        [RuleRequiredField]

        public DateTime Fecha
        {
            get { return _Fecha; }
            set { SetPropertyValue(nameof(Fecha), ref _Fecha, value); }
        }



        private bool _Activo;
        [XafDisplayName("Activo")]

        public bool Activo
        {
            get { return _Activo; }
            set { SetPropertyValue(nameof(Activo), ref _Activo, value); }
        }


        #endregion

        #region Asociaciones

        private CAT_Ambitos _Cat_Ambito;
        [XafDisplayName("Ámbito"), ToolTip("Ámbito del negocio")]
        [DataSourceCriteria("Visible = True")]
        public CAT_Ambitos Cat_Ambito
        {
            get { return _Cat_Ambito; }
            set { SetPropertyValue(nameof(Cat_Ambito), ref _Cat_Ambito, value); }
        }

        private Horarios_Negocios _Horario;
        [XafDisplayName("Horario"), ToolTip("Horario del negocio")]

        public Horarios_Negocios Horario
        {
            get { return _Horario; }
            set { SetPropertyValue(nameof(Horario), ref _Horario, value); }
        }

        [Association("Negocio-Sucursales")]
        [XafDisplayName("Sucursales de este Negocio")]
        public XPCollection<Sucursales> Sucursales
        {
            get { return GetCollection<Sucursales>(nameof(Sucursales)); }
        }

        #endregion

        //Métodos de la clase
        #region Metodos

        public override void AfterConstruction()
        {
            base.AfterConstruction();
            this.Activo = true;
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }

        /*//Boton de "Activo"
        [Action(Caption = "Activo/Inactivo", ConfirmationMessage = "Estas seguro?", AutoCommit = true)]
        public void Actividad()
        {
            // Trigger a custom business logic for the current record in the UI (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112619.aspx).
            this.Activo = !this.Activo;
        }*/
        #endregion
    }
}