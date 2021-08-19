
var Navicon = Navicon ?? {}

Navicon.auto = (function() {

  let fields = {
    used: "autod_used",
    isdamaged: "autod_isdamaged",
    ownersCount: "autod_ownerscount",
    km: "autod_km"
  };

  return  {
    onLoad(context) {

      try {
        Navicon.myApi.checkFieldsExistance(context, fields)
      }
      catch (e) {
        console.error(e)
        alert(e.message)
        return
      }

      if (!Navicon.myApi.getValue(context, fields.used)) {
        Navicon.myApi.hideFields(context, [fields.isdamaged, fields.ownersCount, fields.km]);
      }
      context.getFormContext().getAttribute(fields.used).addOnChange( context => {
        if (!Navicon.myApi.getValue(context, fields.used)) {
          Navicon.myApi.hideFields(context, [fields.isdamaged, fields.ownersCount, fields.km]);
        } else {
          Navicon.myApi.showFields(context, [fields.isdamaged, fields.ownersCount, fields.km]);
        }
      })
    }
  }
})();