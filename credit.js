
var Navicon = Navicon ?? {}

Navicon.credit = (function() {
  
  let fields = {
    autod_datestart: "autod_datestart",
    autod_dateend: "autod_dateend"
  };

  return  {
    onSave(context) {

      try {
        Navicon.myApi.checkFieldsExistance(context, fields)
      }
      catch (e) {
        console.error(e)
        alert(e.message)
        return
      }

      let startDate = context.getFormContext().getAttribute(fields.autod_datestart).getValue()
      let endDate = context.getFormContext().getAttribute(fields.autod_dateend).getValue() 
      if (endDate.getFullYear() - startDate.getFullYear() < 1) {
        alert("Дата окончания договора должна быть не раньше чем через 1 год!");
        context.getEventArgs().preventDefault();
      }
    }
  }
})();