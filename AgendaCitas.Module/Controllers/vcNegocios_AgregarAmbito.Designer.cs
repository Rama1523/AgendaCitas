namespace AgendaCitas.Module.Controllers
{
    partial class vcNegocios_AgregarAmbito
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.popupNegocios_AgregarAmbito = new DevExpress.ExpressApp.Actions.PopupWindowShowAction(this.components);
            // 
            // popupNegocios_AgregarAmbito
            // 
            this.popupNegocios_AgregarAmbito.AcceptButtonCaption = null;
            this.popupNegocios_AgregarAmbito.CancelButtonCaption = null;
            this.popupNegocios_AgregarAmbito.Caption = "Agregar Ambito";
            this.popupNegocios_AgregarAmbito.ConfirmationMessage = null;
            this.popupNegocios_AgregarAmbito.Id = "AgregarAmbitosNegociosCatalogo";
            this.popupNegocios_AgregarAmbito.TargetObjectType = typeof(AgendaCitas.Module.BusinessObjects.Agenda.Negocios);
            this.popupNegocios_AgregarAmbito.TargetViewType = DevExpress.ExpressApp.ViewType.ListView;
            this.popupNegocios_AgregarAmbito.ToolTip = "Agrega un Ambito de negocios";
            this.popupNegocios_AgregarAmbito.TypeOfView = typeof(DevExpress.ExpressApp.ListView);
            this.popupNegocios_AgregarAmbito.CustomizePopupWindowParams += new DevExpress.ExpressApp.Actions.CustomizePopupWindowParamsEventHandler(this.popupNegocios_AgregarAmbito_params);
            this.popupNegocios_AgregarAmbito.Execute += new DevExpress.ExpressApp.Actions.PopupWindowShowActionExecuteEventHandler(this.popupNegocios_AgregarAmbito_execute);
            // 
            // vcNegocios_AgregarAmbito
            // 
            this.Actions.Add(this.popupNegocios_AgregarAmbito);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.PopupWindowShowAction popupNegocios_AgregarAmbito;
    }
}
