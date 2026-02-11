namespace AgendaCitas.Module.Controllers
{
    partial class vcCargarAmbitosNegocios
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
            this.popupCargarAmbitoNegocios = new DevExpress.ExpressApp.Actions.PopupWindowShowAction(this.components);
            // 
            // popupCargarAmbitoNegocios
            // 
            this.popupCargarAmbitoNegocios.AcceptButtonCaption = null;
            this.popupCargarAmbitoNegocios.CancelButtonCaption = null;
            this.popupCargarAmbitoNegocios.Caption = "Cargar Ambitos";
            this.popupCargarAmbitoNegocios.ConfirmationMessage = null;
            this.popupCargarAmbitoNegocios.Id = "popupCargarAmbitoNegocios";
            this.popupCargarAmbitoNegocios.ToolTip = null;
            this.popupCargarAmbitoNegocios.CustomizePopupWindowParams += new DevExpress.ExpressApp.Actions.CustomizePopupWindowParamsEventHandler(this.popupCargarAmbitosNegocios_Params);
            this.popupCargarAmbitoNegocios.Execute += new DevExpress.ExpressApp.Actions.PopupWindowShowActionExecuteEventHandler(this.popupCargarAmbitosNegocios_Execute);
            // 
            // vcCargarAmbitosNegocios
            // 
            this.Actions.Add(this.popupCargarAmbitoNegocios);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.PopupWindowShowAction popupCargarAmbitoNegocios;
    }
}
