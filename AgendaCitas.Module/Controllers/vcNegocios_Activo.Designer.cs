namespace AgendaCitas.Module.Controllers
{
    partial class vcNegocios_Activo
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
            this.popup_Negocios_Activo = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // popup_Negocios_Activo
            // 
            this.popup_Negocios_Activo.Caption = "Activo/Inactivo";
            this.popup_Negocios_Activo.ConfirmationMessage = null;
            this.popup_Negocios_Activo.Id = "popup_Negocios_Activo";
            this.popup_Negocios_Activo.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireMultipleObjects;
            this.popup_Negocios_Activo.TargetObjectType = typeof(AgendaCitas.Module.BusinessObjects.Agenda.Negocios);
            this.popup_Negocios_Activo.TargetViewType = DevExpress.ExpressApp.ViewType.ListView;
            this.popup_Negocios_Activo.ToolTip = "Cambia el estado de un negocio";
            this.popup_Negocios_Activo.TypeOfView = typeof(DevExpress.ExpressApp.ListView);
            this.popup_Negocios_Activo.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.popup_Negocios_Activo_Execute);
            // 
            // vcNegocios_Activo
            // 
            this.Actions.Add(this.popup_Negocios_Activo);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction popup_Negocios_Activo;
    }
}
