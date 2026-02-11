using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
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
using DevExpress.Xpo;
using ExcelDataReader;
using ValidarDatos;

namespace AgendaCitas.Module.Controllers
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class vcCargarAmbitosNegocios : ViewController
    {
        // Use CodeRush to create Controllers and Actions with a few keystrokes.
        // https://docs.devexpress.com/CodeRushForRoslyn/403133/
        public vcCargarAmbitosNegocios()
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

        private void popupCargarAmbitosNegocios_Params(object sender, CustomizePopupWindowParamsEventArgs e)
        {
            NonPersistentObjectSpace objectSpace = (NonPersistentObjectSpace)Application.CreateObjectSpace(typeof(NotMaped.SubirExcel));
            NotMaped.SubirExcel parametros = objectSpace.CreateObject<NotMaped.SubirExcel>();
            DetailView detail = Application.CreateDetailView(objectSpace, parametros);
            detail.Caption = "Subir informacion en formato xlsx";
            detail.ViewEditMode = DevExpress.ExpressApp.Editors.ViewEditMode.Edit;
            e.View = detail;
        }

        private void popupCargarAmbitosNegocios_Execute(object sender, PopupWindowShowActionExecuteEventArgs e)
        {
            var sesion = ((XPObjectSpace)this.ObjectSpace).Session;
            var parametros = (NotMaped.SubirExcel)e.PopupWindowView.SelectedObjects[0];
            bool esExcel = Path.GetExtension(parametros.Archivo.FileName) == ".xlsx" ? true : false;

            if (parametros.Archivo != null && !parametros.Archivo.IsEmpty && esExcel)
            {
                MemoryStream streamExcel = new MemoryStream();
                parametros.Archivo.SaveToStream(streamExcel);

                //Abrir Excel
                using (MemoryStream stream = streamExcel)
                {
                    IExcelDataReader excelDataReader = esExcel ? ExcelReaderFactory.CreateOpenXmlReader(stream) : ExcelReaderFactory.CreateBinaryReader(stream);
                    DataSet result = excelDataReader.AsDataSet();

                    int recorrido = 0;
                    foreach(DataRow row in result.Tables[0].Rows)
                    {
                        if (recorrido > 0)
                        {
                            try
                            {
                                int columna = 0;
                                string ambito = ValidarString.LimiteCatacteres(row.ItemArray[columna].ToString(), 100);
                                columna++;
                                BusinessObjects.Catalogo.CAT_Ambitos existeAmbito =
                                    ObjectSpace.FindObject<BusinessObjects.Catalogo.CAT_Ambitos>
                                    (CriteriaOperator.Parse($"Ambito = '{ambito}'"));
                                if(existeAmbito == null)
                                {
                                    existeAmbito = new BusinessObjects.Catalogo.CAT_Ambitos(sesion);
                                    existeAmbito.Ambito = ambito;
                                    
                                }
                                existeAmbito.Visible = true;
                                existeAmbito.Save();
                                existeAmbito.Session.CommitTransaction();
                            }

                            catch (Exception ex)
                            {
                                string error = ex.ToString();
                                Application.ShowViewStrategy.ShowMessage($"Ha habido un error durante la imprtación de datos", InformationType.Error, 5000, InformationPosition.Top);
                            }
                        }
                        recorrido++;
                    }
                    Application.ShowViewStrategy.ShowMessage($"Se han importado los ambitos correctamente", InformationType.Success, 5000, InformationPosition.Top);
                }
            }
            else
            {
                Application.ShowViewStrategy.ShowMessage($"El archivo no cumple los parametros requeridos, porfavor revise que esta importando un archivo de formato 'xslx' y no esté vacio", InformationType.Error, 5000, InformationPosition.Top);
            }
            Frame.View.RefreshDataSource();
        }
    }
}
