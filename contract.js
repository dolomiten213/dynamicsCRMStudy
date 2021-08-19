
var Navicon = Navicon ?? {}

Navicon.contract = (function() {

  let fields = {
    autod_number: "autod_number",
    autod_name: "autod_name",
    autod_creditid: "autod_creditid",
    autod_summa: "autod_summa",
    autod_fact: "autod_fact",
    autod_contact: "autod_contact",
    autod_autoid: "autod_autoid"
  };

  let tabs = {
    credit_tab: "creditTab"
  }

  function numberFormatter(context) {
    let numberAttr = context.getFormContext().getAttribute(fields.autod_number);
    numberAttr.setValue(numberAttr.getValue().replace(/[^\d-]/g, '')); 
  }

  return  {
    onLoad(context) {

      try {
        Navicon.myApi.checkFieldsExistance(context, fields)
        Navicon.myApi.checkTabsExistance(context, tabs)
      }
      catch (e) {
        console.error(e)
        alert(e.message)
        return
      }

      let formContext = context.getFormContext();
      let nameAttr = formContext.getAttribute(fields.autod_name);

      formContext.getAttribute(fields.autod_number).addOnChange(  numberFormatter  );
      
      if (nameAttr === null) { //wrong primary field
        
        alert("Incorrect primary field name");
      
      } else if (nameAttr.getValue() === null) { //creating 
        
        let invisibleTabs = [tabs.credit_tab];
        let invisibleFields = [fields.autod_creditid, fields.autod_summa, fields.autod_fact];
        let requiredFieldsToUnlockCreditProgram = [fields.autod_contact, fields.autod_autoid];
        let requiredFieldsToUnlockCreditTab = [fields.autod_creditid];

        Navicon.myApi.hideTabs(context, invisibleTabs);
        Navicon.myApi.hideFields(context, invisibleFields);
        
        Navicon.myApi.fillFieldsToUnlockFields(context, requiredFieldsToUnlockCreditProgram, [fields.autod_creditid]);
        Navicon.myApi.fillFieldsToUnlockTabs(context, requiredFieldsToUnlockCreditTab, [tabs.credit_tab]);
        Navicon.myApi.fillFieldsToUnlockFields(context, requiredFieldsToUnlockCreditTab, [fields.autod_summa, fields.autod_fact]);
      
      } else { // modify

      }
    }
  }
})();