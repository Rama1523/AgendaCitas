using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Layout;
using DevExpress.ExpressApp.Model.NodeGenerators;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Templates;
using DevExpress.ExpressApp.Utils;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;

namespace AgendaCitas.Module.Controllers
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class vcNegocios_AgregarAmbito : ViewController
    {
        // Use CodeRush to create Controllers and Actions with a few keystrokes.
        // https://docs.devexpress.com/CodeRushForRoslyn/403133/
        public vcNegocios_AgregarAmbito()
        {
            InitializeComponent();
            // Target required Views (via the TargetXXX properties) and create their Actions.
        }
        protected override void OnActivated()
        {
            base.OnActivated();
            // Perform various tasks depending on the target View.
        }
        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            // Access and customize the target View control.
        }
        protected override void OnDeactivated()
        {
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
        }

        private void popupNegocios_AgregarAmbito_params(object sender, CustomizePopupWindowParamsEventArgs e)
        {
            NonPersistentObjectSpace objectSpace = (NonPersistentObjectSpace)Application.CreateObjectSpace(typeof(NotMaped.vcCargarAmbitoNegocio));
            NotMaped.vcCargarAmbitoNegocio parametros = objectSpace.CreateObject<NotMaped.vcCargarAmbitoNegocio>();
            DetailView detail = Application.CreateDetailView(objectSpace, parametros);
            detail.Caption = "Insertar un Ámbito de negocio";
            detail.ViewEditMode = DevExpress.ExpressApp.Editors.ViewEditMode.Edit;
            e.View = detail;

        }

        private void popupNegocios_AgregarAmbito_execute(object sender, PopupWindowShowActionExecuteEventArgs e)
        {
            //castear una sesion
            var sesion = ((XPObjectSpace)this.ObjectSpace).Session; 

            //castear un objeto de otro fichero
            var parametros =
            (NotMaped.vcCargarAmbitoNegocio)e.PopupWindowView.SelectedObjects[0];

            // si es diferente de null valida 
            if (!string.IsNullOrEmpty(parametros.AmbitoElegido))

            {
                var nuevo = new BusinessObjects.Catalogo.CAT_Ambitos(sesion)
                {
                    Ambito = parametros.AmbitoElegido.Trim(),
                    Visible = true
                };

                nuevo.Save();
                nuevo.Session.CommitTransaction();

                //Mensaje de retroalimentacion al usuario 
                Application.ShowViewStrategy.ShowMessage($"El Ámbito {parametros.AmbitoElegido} Fue insertado con éxito",InformationType.Success, 5000, InformationPosition.Top);
            }

            //Refrescar tablas
            Frame.View.RefreshDataSource();
        }
    }
}
    