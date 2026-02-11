;#DropDB AgendaCitasEasyTest

#Application AgendaCitasWin
;#Application AgendaCitasWeb

*FillForm
 User Name = Admin
*Action Log In

#IfDef AgendaCitasWin

*Action About...
 
*Action OK

#EndIf

#IfDef AgendaCitasWeb
*Action Navigation(My Details)
#EndIf
