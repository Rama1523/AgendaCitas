using System;
using DevExpress.Xpo;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.ExpressApp.DC;
using AgendaCitas.Module.BusinessObjects.Agenda;
using System.ComponentModel;
using DevExpress.ExpressApp;

namespace AgendaCitas.Module.BusinessObjects.Mi_Negocio
{
    [NavigationItem("Agenda")]
    [XafDisplayName("Pedir Cita")]

    [RuleCriteria("CitaBase_NoSolapamiento", DefaultContexts.Save,
    "Not [<CitaBase>][Oid != ^.Oid And Sucursal == ^.Sucursal And StartOn < ^.EndOn And EndOn > ^.StartOn]",
    "Ya existe una cita en este horario para esta sucursal.",
    SkipNullOrEmptyValues = true)]

    public abstract class CitaBase : Event
    {
        public CitaBase(Session session) : base(session) { }

        private Negocios _Negocio;
        [ImmediatePostData]
        [RuleRequiredField]
        [Browsable(false)] // Esto evita que se vea doble en la interfaz
        public Negocios Negocio
        {
            get => _Negocio;
            set => SetPropertyValue(nameof(Negocio), ref _Negocio, value);
        }

        private Sucursales _Sucursal;
        [DataSourceCriteria("Negocio.Oid = '@This.Negocio.Oid' and Activo = true")]
        [RuleRequiredField]
        [Association("Sucursal-Citas")]
        public Sucursales Sucursal
        {
            get => _Sucursal;
            set => SetPropertyValue(nameof(Sucursal), ref _Sucursal, value);
        }
        
        private ApplicationUser _ClienteUsuario;
        [XafDisplayName("Propietario de la Cita")]
        [Persistent("ClienteUsuario")] // Forzamos el nombre de la columna
        [ExplicitLoading] // Ayuda a que la propiedad esté disponible rápido para el motor de seguridad
        public ApplicationUser ClienteUsuario
        {
            get => _ClienteUsuario;
            set => SetPropertyValue(nameof(ClienteUsuario), ref _ClienteUsuario, value);
        }

        private string _Dni;
        [XafDisplayName("DNI/Pasaporte")]
        [RuleRequiredField("CitaBase_Dni_Requerido", DefaultContexts.Save, "El DNI es obligatorio")]
        public string Dni
        {
            get => _Dni;
            set => SetPropertyValue(nameof(Dni), ref _Dni, value);
        }

        private string _Nombre;
        [RuleRequiredField]
        public string Nombre
        {
            get => _Nombre;
            set => SetPropertyValue(nameof(Nombre), ref _Nombre, value);
        }

        private string _Apellido1;
        [RuleRequiredField]
        public string Apellido1
        {
            get => _Apellido1;
            set => SetPropertyValue(nameof(Apellido1), ref _Apellido1, value);
        }

        private string _Apellido2;
        public string Apellido2
        {
            get => _Apellido2;
            set => SetPropertyValue(nameof(Apellido2), ref _Apellido2, value);
        }

        // Actualiza el asunto y tiempos por defecto
        protected override void OnChanged(string propertyName, object oldValue, object newValue)
        {
            base.OnChanged(propertyName, oldValue, newValue);
            if (!IsLoading && !IsSaving)
            {
                // Actualiza el título del evento en el calendario
                if (propertyName == nameof(Nombre) || propertyName == nameof(Apellido1) || propertyName == nameof(Sucursal))
                {
                    Subject = $"{Nombre} {Apellido1}";
                }
                // Si ponen fecha de inicio y no hay fin, poner 30 min por defecto
                if (propertyName == "StartOn" && newValue != null && (EndOn == StartOn || EndOn == DateTime.MinValue))
                {
                    EndOn = StartOn.AddMinutes(30);
                }
            }
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();

            // SecuritySystem.CurrentUser, más fiable para obtener el objeto completo en la sesión actual
            ApplicationUser actual = Session.GetObjectByKey<ApplicationUser>(SecuritySystem.CurrentUserId);

            if (actual != null)
            {
                // Si el usuario tiene Negocio, lo asignamos a la cita automáticamente
                if (actual.Negocio != null)
                {
                    this.Negocio = actual.Negocio;
                }

                // Si el usuario es un Cliente (no tiene negocio), se asigna como propietario
                if (actual.Negocio == null)
                {
                    this.ClienteUsuario = actual;
                    //Rellenar automáticamente Nombre y Apellidos del perfil del usuario
                    this.Nombre = actual.UserName;
                }
            }
        }
    }


    #region Clases Especializadas

    // Clínica
    public class CitaMedica : CitaBase
    {
        public CitaMedica(Session session) : base(session) { }

        [DataSourceCriteria("Cat_Ambito.Ambito = 'Salud' AND Activo = true")]
        [ImmediatePostData]
        public new Negocios Negocio
        {
            get => base.Negocio;
            set => base.Negocio = value;
        }

        private int _Visitas;
        public int Visitas
        {
            get => _Visitas;
            set => SetPropertyValue(nameof(Visitas), ref _Visitas, value);
        }

        private bool _Alta;
        public bool Alta
        {
            get => _Alta;
            set => SetPropertyValue(nameof(Alta), ref _Alta, value);
        }
    }

    // Restaurante
    public class CitaRestaurante : CitaBase
    {
        public CitaRestaurante(Session session) : base(session) { }

        [DataSourceCriteria("Cat_Ambito.Ambito = 'Gastronomía' AND Activo = true")]
        [ImmediatePostData]
        public new Negocios Negocio
        {
            get => base.Negocio;
            set => base.Negocio = value;
        }

        private comidas_Restaurante _Tipo;
        public comidas_Restaurante Tipo
        {
            get => _Tipo;
            set => SetPropertyValue(nameof(Tipo), ref _Tipo, value);
        }

        private int _Comensales;
        public int Comensales
        {
            get => _Comensales;
            set => SetPropertyValue(nameof(Comensales), ref _Comensales, value);
        }

        private bool _Recogido;
        public bool Recogido
        {
            get => _Recogido;
            set => SetPropertyValue(nameof(Recogido), ref _Recogido, value);
        }
    }

    // Peluquería
    public class CitaPeluqueria : CitaBase
    {
        public CitaPeluqueria(Session session) : base(session) { }

        [DataSourceCriteria("Cat_Ambito.Ambito = 'Estética' AND Activo = true")]
        [ImmediatePostData]
        public new Negocios Negocio
        {
            get => base.Negocio;
            set => base.Negocio = value;
        }

        private Servicios_peluqueria _Servicios;
        public Servicios_peluqueria Servicios
        {
            get => _Servicios;
            set => SetPropertyValue(nameof(Servicios), ref _Servicios, value);
        }

        private bool _PrimeraCita;
        public bool PrimeraCita
        {
            get => _PrimeraCita;
            set => SetPropertyValue(nameof(PrimeraCita), ref _PrimeraCita, value);
        }
    }

    // Entretenimiento 
    public class CitaJuego : CitaBase
    {
        public CitaJuego(Session session) : base(session) { }

        [DataSourceCriteria("Cat_Ambito.Ambito = 'Entretenimiento' AND Activo = true")]
        [ImmediatePostData]
        public new Negocios Negocio
        {
            get => base.Negocio;
            set => base.Negocio = value;
        }

        private Juegos_Entretener _Juegos;
        public Juegos_Entretener Juegos
        {
            get => _Juegos;
            set => SetPropertyValue(nameof(Juegos), ref _Juegos, value);
        }

        private int _HorasReservadas;
        [ImmediatePostData]
        public int HorasReservadas
        {
            get => _HorasReservadas;
            set
            {
                if (SetPropertyValue(nameof(HorasReservadas), ref _HorasReservadas, value) && !IsLoading && value > 0)
                {
                    EndOn = StartOn.AddHours(value);
                }
            }
        }
    }

    // Deporte
    public class CitaDeportes : CitaBase
    {
        public CitaDeportes(Session session) : base(session) { }

        [DataSourceCriteria("Cat_Ambito.Ambito = 'Deporte' AND Activo = true")]
        [ImmediatePostData]
        public new Negocios Negocio
        {
            get => base.Negocio;
            set => base.Negocio = value;
        }

        private int _HorasReservadas;
        [ImmediatePostData]
        public int HorasReservadas
        {
            get => _HorasReservadas;
            set
            {
                if (SetPropertyValue(nameof(HorasReservadas), ref _HorasReservadas, value) && !IsLoading && value > 0)
                {
                    EndOn = StartOn.AddHours(value);
                }
            }
        }


        private bool _Principiante;
        public bool Principiante
        {
            get => _Principiante;
            set => SetPropertyValue(nameof(Principiante), ref _Principiante, value);
        }

    }

    // Mecánica 
    public class CitaTaller : CitaBase
    {
        public CitaTaller(Session session) : base(session) { }

        [DataSourceCriteria("Cat_Ambito.Ambito = 'Mecanica' AND Activo = true")]
        [ImmediatePostData]
        public new Negocios Negocio
        {
            get => base.Negocio;
            set => base.Negocio = value;
        }

        private bool _PrimeraRevision;
        public bool PrimeraRevision
        {
            get => _PrimeraRevision;
            set => SetPropertyValue(nameof(PrimeraRevision), ref _PrimeraRevision, value);
        }
    }

    // Tecnología
    public class CitaTecnologia : CitaBase
    {
        public CitaTecnologia(Session session) : base(session) { }

        [DataSourceCriteria("Cat_Ambito.Ambito = 'Tecnologia' AND Activo = true")]
        [ImmediatePostData]
        public new Negocios Negocio
        {
            get => base.Negocio;
            set => base.Negocio = value;
        }

        private bool _PrimeraConsulta;
        public bool PrimeraConsulta
        {
            get => _PrimeraConsulta;
            set => SetPropertyValue(nameof(PrimeraConsulta), ref _PrimeraConsulta, value);
        }
    }

    #endregion


    /*#region Clinica Alonso
    [DefaultClassOptions]
    [NavigationItem("Gestión")]
    [XafDisplayName("ClinicaAlonso_01")]
    public class ClinicaA_01 : BaseObject
    {
        public ClinicaA_01(Session session) : base(session) { }

        private string _Dni;
        [XafDisplayName("DNI"), ToolTip("DNI del paciente")]
        [RuleRequiredField]
        [RuleUniqueValue("RuleUniqueValue_citas_ClinicaA_01", DefaultContexts.Save, "Ya existe DNI en la base de datos.")]
        public string Dni
        {
            get => _Dni;
            set => SetPropertyValue(nameof(Dni), ref _Dni, value);
        }

        private string _Nombre;
        [XafDisplayName("Nombre"), ToolTip("nombre del paciente")]
        [RuleRequiredField]
        public string Nombre
        {
            get => _Nombre;
            set => SetPropertyValue(nameof(Nombre), ref _Nombre, value);
        }

        private string _Apellido1;
        [XafDisplayName("Apellido 1"), ToolTip("Apellido 1 del paciente")]
        [RuleRequiredField]
        public string Apellido1
        {
            get => _Apellido1;
            set => SetPropertyValue(nameof(Apellido1), ref _Apellido1, value);
        }

        private string _Apellido2;
        [XafDisplayName("Apellido 2"), ToolTip("Apellido 2 del paciente")]
        [RuleRequiredField]
        public string Apellido2
        {
            get => _Apellido2;
            set => SetPropertyValue(nameof(Apellido2), ref _Apellido2, value);
        }

        private int _Visitas;
        [XafDisplayName("Visitas del paciente"), ToolTip("Veces acudidas del paciente")]
        [RuleRequiredField]
        public int Visitas
        {
            get => _Visitas;
            set => SetPropertyValue(nameof(Visitas), ref _Visitas, value);
        }

        private DateTime _FechaNacimiento;
        [XafDisplayName("Fecha de Nacimiento"), ToolTip("Fecha de Nacimiento del paciente")]
        [VisibleInListView(false)]
        [RuleRequiredField]
        public DateTime FechaNacimiento
        {
            get { return _FechaNacimiento; }
            set { SetPropertyValue(nameof(FechaNacimiento), ref _FechaNacimiento, value); }
        }

        private DateTime _FechaCita;
        [XafDisplayName("Fecha de proxima cita"), ToolTip("Fecha de la porxima cita")]
        [VisibleInListView(false)]
        [RuleRequiredField]
        public DateTime FechaCita
        {
            get { return _FechaCita; }
            set { SetPropertyValue(nameof(FechaCita), ref _FechaCita, value); }
        }

        private TimeSpan _HoraCita;
        [XafDisplayName("Hora de cita"), ToolTip("Hora de la cita")]
        [VisibleInListView(false)]
        [RuleRequiredField]
        [RuleUniqueValue("RuleUniqueValue_citas_ClinicaA_01_Hora", DefaultContexts.Save, "Ya se ha ocupado la hora de la cita.")]
        public TimeSpan HoraCita
        {
            get { return _HoraCita; }
            set { SetPropertyValue(nameof(HoraCita), ref _HoraCita, value); }
        }

        private bool _Alta;
        [XafDisplayName("Alta"), ToolTip("Rellenar solo si el paciente ya obtuvo el alta")]

        public bool Alta
        {
            get { return _Alta; }
            set { SetPropertyValue(nameof(Alta), ref _Alta, value); }
        }

    }


    [DefaultClassOptions]
    [NavigationItem("Gestión")]
    [XafDisplayName("ClinicaAlonso_02")]
    public class ClinicaA_02 : BaseObject
    {
        public ClinicaA_02(Session session) : base(session) { }

        private string _Dni;
        [XafDisplayName("DNI"), ToolTip("DNI del paciente")]
        [RuleRequiredField]
        [RuleUniqueValue("RuleUniqueValue_citas_ClinicaA_02", DefaultContexts.Save, "Ya existe DNI en la base de datos.")]
        public string Dni
        {
            get => _Dni;
            set => SetPropertyValue(nameof(Dni), ref _Dni, value);
        }

        private string _Nombre;
        [XafDisplayName("Nombre"), ToolTip("nombre del paciente")]
        [RuleRequiredField]
        public string Nombre
        {
            get => _Nombre;
            set => SetPropertyValue(nameof(Nombre), ref _Nombre, value);
        }

        private string _Apellido1;
        [XafDisplayName("Apellido 1"), ToolTip("Apellido 1 del paciente")]
        [RuleRequiredField]
        public string Apellido1
        {
            get => _Apellido1;
            set => SetPropertyValue(nameof(Apellido1), ref _Apellido1, value);
        }

        private string _Apellido2;
        [XafDisplayName("Apellido 2"), ToolTip("Apellido 2 del paciente")]
        [RuleRequiredField]
        public string Apellido2
        {
            get => _Apellido2;
            set => SetPropertyValue(nameof(Apellido2), ref _Apellido2, value);
        }

        private int _Visitas;
        [XafDisplayName("Visitas del paciente"), ToolTip("Veces acudidas del paciente")]
        [RuleRequiredField]
        public int Visitas
        {
            get => _Visitas;
            set => SetPropertyValue(nameof(Visitas), ref _Visitas, value);
        }

        private DateTime _FechaNacimiento;
        [XafDisplayName("Fecha de Nacimiento"), ToolTip("Fecha de Nacimiento del paciente")]
        [VisibleInListView(false)]
        [RuleRequiredField]
        public DateTime FechaNacimiento
        {
            get { return _FechaNacimiento; }
            set { SetPropertyValue(nameof(FechaNacimiento), ref _FechaNacimiento, value); }
        }

        private DateTime _FechaCita;
        [XafDisplayName("Fecha de proxima cita"), ToolTip("Fecha de la porxima cita")]
        [VisibleInListView(false)]
        [RuleRequiredField]
        public DateTime FechaCita
        {
            get { return _FechaCita; }
            set { SetPropertyValue(nameof(FechaCita), ref _FechaCita, value); }
        }

        private TimeSpan _HoraCita;
        [XafDisplayName("Hora de cita"), ToolTip("Hora de la cita")]
        [VisibleInListView(false)]
        [RuleRequiredField]
        [RuleUniqueValue("RuleUniqueValue_citas_ClinicaA_02_Hora", DefaultContexts.Save, "Ya se ha ocupado la hora de la cita.")]
        public TimeSpan HoraCita
        {
            get { return _HoraCita; }
            set { SetPropertyValue(nameof(HoraCita), ref _HoraCita, value); }
        }

        private bool _Alta;
        [XafDisplayName("Alta"), ToolTip("Rellenar solo si el paciente ya obtuvo el alta")]

        public bool Alta
        {
            get { return _Alta; }
            set { SetPropertyValue(nameof(Alta), ref _Alta, value); }
        }

    }
       #endregion

    #region Restaurante Enrrique
    [DefaultClassOptions]
    [NavigationItem("Gestión")]
    [XafDisplayName("Restaurante Enrrique_01")]
    public class RestauranteEnr_01 : BaseObject
    {
        public RestauranteEnr_01(Session session) : base(session) { }

        private string _Dni;
        [XafDisplayName("DNI"), ToolTip("DNI del cliente que reservó")]
        [RuleRequiredField]
        [RuleUniqueValue("RuleUniqueValue_citas_RestauranteEnr_01", DefaultContexts.Save, "Ya existe DNI en la base de datos.")]
        public string Dni
        {
            get => _Dni;
            set => SetPropertyValue(nameof(Dni), ref _Dni, value);
        }

        private string _Nombre;
        [XafDisplayName("Nombre"), ToolTip("nombre del cliente que reservó")]
        [RuleRequiredField]
        public string Nombre
        {
            get => _Nombre;
            set => SetPropertyValue(nameof(Nombre), ref _Nombre, value);
        }

        private string _Apellido1;
        [XafDisplayName("Apellido 1"), ToolTip("Apellido 1 del cliente que reservó")]
        [RuleRequiredField]
        public string Apellido1
        {
            get => _Apellido1;
            set => SetPropertyValue(nameof(Apellido1), ref _Apellido1, value);
        }

        private string _Apellido2;
        [XafDisplayName("Apellido 2"), ToolTip("Apellido 2 del cliente que reservó")]
        [RuleRequiredField]
        public string Apellido2
        {
            get => _Apellido2;
            set => SetPropertyValue(nameof(Apellido2), ref _Apellido2, value);
        }

        private int _Comensales;
        [XafDisplayName("Comensales"), ToolTip("Cantidad de comensales")]
        [RuleRequiredField]
        public int Comensales
        {
            get => _Comensales;
            set => SetPropertyValue(nameof(Comensales), ref _Comensales, value);
        }

        private DateTime _FechaReserva;
        [XafDisplayName("Fecha de reserva"), ToolTip("Fecha de la reserva")]
        [VisibleInListView(false)]
        [RuleRequiredField]
        public DateTime FechaReserva
        {
            get { return _FechaReserva; }
            set { SetPropertyValue(nameof(FechaReserva), ref _FechaReserva, value); }
        }

        private TimeSpan _HoraReserva;
        [XafDisplayName("Hora de reserva"), ToolTip("Hora de la reserva")]
        [VisibleInListView(false)]
        [RuleRequiredField]

        //Validacion que restringe la duplicacion del campo
        [RuleUniqueValue("RuleUniqueValue_Citaciones_HoraReserva",DefaultContexts.Save,
                         "Tenga en cuenta que ya existe una reserva a esta hora.", ResultType = ValidationResultType.Warning)]
        public TimeSpan HoraReserva
        {
            get { return _HoraReserva; }
            set { SetPropertyValue(nameof(HoraReserva), ref _HoraReserva, value); }
        }

        private bool _Recogido;
        [XafDisplayName("Mesa Recogida"), ToolTip("Rellenar solo si el cliente ya se retiró de la mesa")]

        public bool Recogido
        {
            get { return _Recogido; }
            set { SetPropertyValue(nameof(Recogido), ref _Recogido, value); }
        }

    }

    [DefaultClassOptions]
    [NavigationItem("Gestión")]
    [XafDisplayName("Restaurante Enrrique_02")]
    public class RestauranteEnr_02 : BaseObject
    {
        public RestauranteEnr_02(Session session) : base(session) { }

        private string _Dni;
        [XafDisplayName("DNI"), ToolTip("DNI del cliente que reservó")]
        [RuleRequiredField]
        [RuleUniqueValue("RuleUniqueValue_citas_RestauranteEnr_02", DefaultContexts.Save, "Ya existe DNI en la base de datos.")]
        public string Dni
        {
            get => _Dni;
            set => SetPropertyValue(nameof(Dni), ref _Dni, value);
        }

        private string _Nombre;
        [XafDisplayName("Nombre"), ToolTip("nombre del cliente que reservó")]
        [RuleRequiredField]
        public string Nombre
        {
            get => _Nombre;
            set => SetPropertyValue(nameof(Nombre), ref _Nombre, value);
        }

        private string _Apellido1;
        [XafDisplayName("Apellido 1"), ToolTip("Apellido 1 del cliente que reservó")]
        [RuleRequiredField]
        public string Apellido1
        {
            get => _Apellido1;
            set => SetPropertyValue(nameof(Apellido1), ref _Apellido1, value);
        }

        private string _Apellido2;
        [XafDisplayName("Apellido 2"), ToolTip("Apellido 2 del cliente que reservó")]
        [RuleRequiredField]
        public string Apellido2
        {
            get => _Apellido2;
            set => SetPropertyValue(nameof(Apellido2), ref _Apellido2, value);
        }

        private int _Comensales;
        [XafDisplayName("Comensales"), ToolTip("Cantidad de comensales")]
        [RuleRequiredField]
        public int Comensales
        {
            get => _Comensales;
            set => SetPropertyValue(nameof(Comensales), ref _Comensales, value);
        }

        private DateTime _FechaReserva;
        [XafDisplayName("Fecha de reserva"), ToolTip("Fecha de la reserva")]
        [VisibleInListView(false)]
        [RuleRequiredField]
        public DateTime FechaReserva
        {
            get { return _FechaReserva; }
            set { SetPropertyValue(nameof(FechaReserva), ref _FechaReserva, value); }
        }

        private TimeSpan _HoraReserva;
        [XafDisplayName("Hora de reserva"), ToolTip("Hora de la reserva")]
        [VisibleInListView(false)]
        [RuleRequiredField]

        //Validacion que restringe la duplicacion del campo
        [RuleUniqueValue("RuleUniqueValue_Citaciones_HoraReserva_02", DefaultContexts.Save,
                         "Tenga en cuenta que ya existe una reserva a esta hora", ResultType = ValidationResultType.Warning)]
        public TimeSpan HoraReserva
        {
            get { return _HoraReserva; }
            set { SetPropertyValue(nameof(HoraReserva), ref _HoraReserva, value); }
        }

        private bool _Recogido;
        [XafDisplayName("Mesa Recogida"), ToolTip("Rellenar solo si el cliente ya se retiró de la mesa")]

        public bool Recogido
        {
            get { return _Recogido; }
            set { SetPropertyValue(nameof(Recogido), ref _Recogido, value); }
        }

    }
    #endregion

    #region Peluqueria Gonzalez
    [DefaultClassOptions]
    [NavigationItem("Gestión")]
    [XafDisplayName("Peluqueria Gonzalez_01")]
    public class PeluqueriaGon_01 : BaseObject
    {
        public PeluqueriaGon_01(Session session) : base(session) { }

        private string _Dni;
        [XafDisplayName("DNI"), ToolTip("DNI del cliente que reservó")]
        [RuleRequiredField]
        [RuleUniqueValue("RuleUniqueValue_citas_PeluqueriaGon_01", DefaultContexts.Save, "Ya existe DNI en la base de datos.")]
        public string Dni
        {
            get => _Dni;
            set => SetPropertyValue(nameof(Dni), ref _Dni, value);
        }

        private string _Nombre;
        [XafDisplayName("Nombre"), ToolTip("nombre del cliente que reservó")]
        [RuleRequiredField]
        public string Nombre
        {
            get => _Nombre;
            set => SetPropertyValue(nameof(Nombre), ref _Nombre, value);
        }

        private string _Apellido1;
        [XafDisplayName("Apellido 1"), ToolTip("Apellido 1 del cliente que reservó")]
        [RuleRequiredField]
        public string Apellido1
        {
            get => _Apellido1;
            set => SetPropertyValue(nameof(Apellido1), ref _Apellido1, value);
        }

        private string _Apellido2;
        [XafDisplayName("Apellido 2"), ToolTip("Apellido 2 del cliente que reservó")]
        [RuleRequiredField]
        public string Apellido2
        {
            get => _Apellido2;
            set => SetPropertyValue(nameof(Apellido2), ref _Apellido2, value);
        }

        private DateTime _FechaCita;
        [XafDisplayName("Fecha de Cita"), ToolTip("Fecha de la Cita")]
        [VisibleInListView(false)]
        [RuleRequiredField]
        public DateTime FechaCita
        {
            get { return _FechaCita; }
            set { SetPropertyValue(nameof(FechaCita), ref _FechaCita, value); }
        }

        private TimeSpan _HoraCita;
        [XafDisplayName("Hora de cita"), ToolTip("Hora de la cita")]
        [VisibleInListView(false)]
        [RuleRequiredField]

        //Validacion que restringe la duplicacion del campo
        [RuleUniqueValue("RuleUniqueValue_Citaciones_HoraCita_01", DefaultContexts.Save,
                         "Tenga en cuenta que ya existe una reserva a esta hora", ResultType = ValidationResultType.Warning)]
        public TimeSpan HoraCita
        {
            get { return _HoraCita; }
            set { SetPropertyValue(nameof(HoraCita), ref _HoraCita, value); }
        }

        private Servicios_peluqueria _Servicio;
        [XafDisplayName("Servicio"), ToolTip("Servicio efecturado con el cliente")]

        public Servicios_peluqueria Servicio
        {
            get { return _Servicio; }
            set { SetPropertyValue(nameof(Servicio), ref _Servicio, value); }
        }


        private bool _Finalizado;
        [XafDisplayName("Servicio Finalizado"), ToolTip("Rellenar solo si el cliente ya completó el servicio")]

        public bool Finalizado
        {
            get { return _Finalizado; }
            set { SetPropertyValue(nameof(Finalizado), ref _Finalizado, value); }
        }

    }
    #endregion

    #region El Juego
    [DefaultClassOptions]
    [NavigationItem("Gestión")]
    [XafDisplayName("El Juego_01")]
    public class ElJuego_01 : BaseObject
    {
        public ElJuego_01(Session session) : base(session) { }

        private string _Dni;
        [XafDisplayName("DNI"), ToolTip("DNI del cliente que reservó")]
        [RuleRequiredField]
        [RuleUniqueValue("RuleUniqueValue_citas_ElJuego_01", DefaultContexts.Save, "Ya existe DNI en la base de datos.")]
        public string Dni
        {
            get => _Dni;
            set => SetPropertyValue(nameof(Dni), ref _Dni, value);
        }

        private string _Nombre;
        [XafDisplayName("Nombre"), ToolTip("nombre del cliente que reservó")]
        [RuleRequiredField]
        public string Nombre
        {
            get => _Nombre;
            set => SetPropertyValue(nameof(Nombre), ref _Nombre, value);
        }

        private string _Apellido1;
        [XafDisplayName("Apellido 1"), ToolTip("Apellido 1 del cliente que reservó")]
        [RuleRequiredField]
        public string Apellido1
        {
            get => _Apellido1;
            set => SetPropertyValue(nameof(Apellido1), ref _Apellido1, value);
        }

        private string _Apellido2;
        [XafDisplayName("Apellido 2"), ToolTip("Apellido 2 del cliente que reservó")]
        [RuleRequiredField]
        public string Apellido2
        {
            get => _Apellido2;
            set => SetPropertyValue(nameof(Apellido2), ref _Apellido2, value);
        }

        private DateTime _FechaReserva;
        [XafDisplayName("Fecha de reserva"), ToolTip("Fecha de la reserva")]
        [VisibleInListView(false)]
        [RuleRequiredField]
        public DateTime FechaReserva
        {
            get { return _FechaReserva; }
            set { SetPropertyValue(nameof(FechaReserva), ref _FechaReserva, value); }
        }

        private TimeSpan _HoraReserva;
        [XafDisplayName("Hora de reserva"), ToolTip("Hora de la reserva")]
        [VisibleInListView(false)]
        [RuleRequiredField]

        //Validacion que restringe la duplicacion del campo
        [RuleUniqueValue("RuleUniqueValue_Citaciones_HoraReserva_01", DefaultContexts.Save,
                         "Tenga en cuenta que ya existe una reserva a esta hora", ResultType = ValidationResultType.Warning)]
        public TimeSpan HoraReserva
        {
            get { return _HoraReserva; }
            set { SetPropertyValue(nameof(HoraReserva), ref _HoraReserva, value); }
        }

        private Juegos_Entretener _Juego;
        [XafDisplayName("Juego"), ToolTip("Juego reservado por el cliente")]

        public Juegos_Entretener Juego
        {
            get { return _Juego; }
            set { SetPropertyValue(nameof(Juego), ref _Juego, value); }
        }

        private int _cantidadJugadores;
        [XafDisplayName("Cantidad de jugadores"), ToolTip("Cantidad de personas que acudiran con el cliente para jugar")]
        [VisibleInListView(false)]
        [RuleRequiredField]
        public int cantidadJugadores
        {
            get { return _cantidadJugadores; }
            set { SetPropertyValue(nameof(cantidadJugadores), ref _cantidadJugadores, value); }
        }

        private double _limite;
        [XafDisplayName("Horas reservadas"), ToolTip("Cantidad de horas que reserva el cliente")]
        [VisibleInListView(false)]
        [RuleRequiredField]
        public double limite
        {
            get { return _limite; }
            set { SetPropertyValue(nameof(limite), ref _limite, value); }
        }


        private bool _PartidaTerminada;
        [XafDisplayName("Partida Finalizada"), ToolTip("Rellenar solo si el cliente ya completó la partida")]

        public bool PartidaTerminada
        {
            get { return _PartidaTerminada; }
            set { SetPropertyValue(nameof(PartidaTerminada), ref _PartidaTerminada, value); }
        }

    }
    #endregion*/
}