using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using AgendaCitas.Module.BusinessObjects.Agenda;
using AgendaCitas.Module.BusinessObjects.Mi_Negocio;    
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Security;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl.PermissionPolicy;
using DevExpress.Xpo;

namespace AgendaCitas.Module.BusinessObjects
{
    [MapInheritance(MapInheritanceType.ParentTable)]
    [DefaultProperty(nameof(UserName))]
    public class ApplicationUser : PermissionPolicyUser, IObjectSpaceLink, ISecurityUserWithLoginInfo
    {

        public ApplicationUser(Session session) : base(session) { }

        #region Propiedades de Estructura Organizativa

        private Negocios _Negocio;
        [XafDisplayName("Negocio/Empresa")]
        [ImmediatePostData] // Para que al cambiar el negocio se refresque el filtro de sucursales
        public Negocios Negocio
        {
            get => _Negocio;
            set => SetPropertyValue(nameof(Negocio), ref _Negocio, value);
        }

        private Sucursales _Sucursal;
        [XafDisplayName("Sucursal asignada")]
        // Filtro dinámico: solo muestra sucursales que pertenezcan al negocio seleccionado arriba
        [DataSourceCriteria("Negocio.Oid = '@This.Negocio.Oid' AND Activo = true")]
        public Sucursales Sucursal
        {
            get => _Sucursal;
            set => SetPropertyValue(nameof(Sucursal), ref _Sucursal, value);
        }

        #endregion

        [Browsable(false)]
        [DevExpress.ExpressApp.DC.Aggregated, Association("User-LoginInfo")]
        public XPCollection<ApplicationUserLoginInfo> LoginInfo
        {
            get { return GetCollection<ApplicationUserLoginInfo>(nameof(LoginInfo)); }
        }

        IEnumerable<ISecurityUserLoginInfo> IOAuthSecurityUser.UserLogins => LoginInfo.OfType<ISecurityUserLoginInfo>();

        IObjectSpace IObjectSpaceLink.ObjectSpace { get; set; }

        ISecurityUserLoginInfo ISecurityUserWithLoginInfo.CreateUserLoginInfo(string loginProviderName, string providerUserKey)
        {
            ApplicationUserLoginInfo result = ((IObjectSpaceLink)this).ObjectSpace.CreateObject<ApplicationUserLoginInfo>();
            result.LoginProviderName = loginProviderName;
            result.ProviderUserKey = providerUserKey;
            result.User = this;
            return result;
        }

        // Lógica de automatización al editar el usuario
        protected override void OnChanged(string propertyName, object oldValue, object newValue)
        {
            base.OnChanged(propertyName, oldValue, newValue);
            if (!IsLoading && !IsSaving)
            {
                // Si asignamos una sucursal y el negocio está vacío, lo deducimos de la sucursal
                if (propertyName == nameof(Sucursal) && Sucursal != null && Negocio == null)
                {
                    Negocio = Sucursal.Negocio;
                }

                // Si cambiamos el negocio y la sucursal anterior ya no pertenece a este, la limpiamos
                if (propertyName == nameof(Negocio) && Sucursal != null && Sucursal.Negocio != Negocio)
                {
                    Sucursal = null;
                }
            }
        }
    }
}