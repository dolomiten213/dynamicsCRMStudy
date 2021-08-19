
var Navicon = Navicon ?? {}

Navicon.communication = (function() {

  const EmailValue = 415210002
  const PhoneValue = 415210001
  
  let savedEmail = null
  let savedPhone = null

  let fields = {
    autod_name: "autod_name",
    autod_phone: "autod_phone",
    autod_email: "autod_email",
    autod_type: "autod_type"
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

      if (Navicon.myApi.getValue(context, fields.autod_name) == null) { //create
        
        Navicon.myApi.hideFields(context, [fields.autod_phone, fields.autod_email])
        
      } else { //modify

        if (Navicon.myApi.getValue(context, fields.autod_type) == EmailValue) {
          savedEmail = Navicon.myApi.getValue(context, fields.autod_email)
          Navicon.myApi.hideFields(context, [fields.autod_phone])
        }
        if (Navicon.myApi.getValue(context, fields.autod_type) == PhoneValue) {
          savedPhone = Navicon.myApi.getValue(context, fields.autod_phone)
          Navicon.myApi.hideFields(context, [fields.autod_email])
        }
      }

      context.getFormContext().getAttribute(fields.autod_type).addOnChange( context => {
        if (Navicon.myApi.getValue(context, fields.autod_type) == EmailValue) {
          Navicon.myApi.setValue(context, fields.autod_phone, null)
          Navicon.myApi.setValue(context, fields.autod_email, savedEmail)         
          Navicon.myApi.showFields(context, [fields.autod_email])
          Navicon.myApi.hideFields(context, [fields.autod_phone])
        } else {
          Navicon.myApi.setValue(context, fields.autod_email, null)
          Navicon.myApi.setValue(context, fields.autod_phone, savedPhone)
          Navicon.myApi.showFields(context, [fields.autod_phone])
          Navicon.myApi.hideFields(context, [fields.autod_email])
        }
      })  
    },
    onSave(context) {
      savedEmail = Navicon.myApi.getValue(context, fields.autod_email)
      savedPhone = Navicon.myApi.getValue(context, fields.autod_phone)
    }
  }
})();