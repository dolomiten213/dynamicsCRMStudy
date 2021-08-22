
var Navicon = Navicon ?? {}

Navicon.contract = (function() {

  let fields = {
    autod_number: "autod_number",
    autod_name: "autod_name",
    autod_creditid: "autod_creditid",
    autod_summa: "autod_summa",
    autod_fact: "autod_fact",
    autod_contact: "autod_contact",
    autod_autoid: "autod_autoid",
    autod_date: "autod_date",
    autod_creditperiod: "autod_creditperiod"
  };

  let tabs = {
    credit_tab: "creditTab"
  }

  function numberFormatter(context) {
    let numberAttr = context.getFormContext().getAttribute(fields.autod_number);
    numberAttr.setValue(numberAttr.getValue().replace(/[^\d-]/g, '')); 
  }

  async function checkCreditExpiration(context) {
    
    if (Navicon.myApi.getValue(context, fields.autod_creditid) === null) return;

    let creditDateExpiration;
    try {
      creditDateExpiration = await Navicon.myApi.getFieldFromReferencedEntity(context, fields.autod_creditid, "autod_dateend")
      if (creditDateExpiration === null || checkCreditExpiration === undefined) {
        console.log("Значение поля dateend равно null")
        return;
      }
    }
    catch (e) {
      console.log("Не удалось получить значение поля dateend\n" + e.message)
      alert("Не удалось получить значение поля dateend\n" + e.message)
      return;
    }
    
    let creditDate = new Date(creditDateExpiration)
    let contractDate = Navicon.myApi.getValue(context, fields.autod_date);

    if (contractDate === null) return;

    if (creditDate < contractDate) {
      alert("Кредитая программа неактуальна для этой даты договора");
      Navicon.myApi.setValue(context, fields.autod_creditid, null);
    }
  }
  async function pasteCreditPeriodIntoContract(context) {
    if (Navicon.myApi.getValue(context, fields.autod_creditid) === null) return;

    let creditPeriod;
    try {
      creditPeriod = await Navicon.myApi.getFieldFromReferencedEntity(context, fields.autod_creditid, "autod_creditperiod")
      if (creditPeriod === null || creditPeriod === undefined) {
        console.log("Значение поля dateend равно null")
        return;
      }
    }
    catch (e) {
      console.log("Не удалось получить значение поля creditPeriod\n" + e.message)
      alert("Не удалось получить значение поля creditPeriod\n" + e.message)
      return;
    }
    
    Navicon.myApi.setValue(context, fields.autod_creditperiod, creditPeriod);

  }
  async function calculateAmount(context) {

    if (Navicon.myApi.getValue(context, fields.autod_autoid) === null) return;

    let autoIsUsed;
    try {
      autoIsUsed = await Navicon.myApi.getFieldFromReferencedEntity(context, fields.autod_autoid, "autod_used")
      if (autoIsUsed === null || autoIsUsed === undefined) {
        console.log("Значение поля autod_used равно null")
        return;
      }
    }
    catch (e) {
      console.log("Не удалось получить значение поля autod_used\n" + e.message)
      alert("Не удалось получить значение поля autod_used\n" + e.message)
      return;
    }
    let amount;

    if (autoIsUsed) {      
      try {
        amount = await Navicon.myApi.getFieldFromReferencedEntity(context, fields.autod_autoid, "autod_amount")
        if (amount === null || amount === undefined) {
          console.log("Значение поля autod_amount равно null")
          return;
        }
        Navicon.myApi.setValue(context, fields.autod_summa, amount);
      }
      catch (e) {
        console.log("Не удалось получить значение поля autod_amount\n" + e.message)
        alert("Не удалось получить значение поля autod_amount\n" + e.message)
        return;
      }
    } else {
      let valuesArray = context.getFormContext().getAttribute(fields.autod_autoid);
      if (valuesArray == null) throw new Error("Не удалось найти поле " + entityKey);
      valuesArray = valuesArray.getValue();
      if (valuesArray === null || valuesArray.length < 1) {
        throw new Error("Не удалось получить значение из ключевого поля " + entityKey)
      }
      let refEntity = valuesArray[0];

      (async () => {
        let result = await Xrm.WebApi.retrieveRecord(refEntity.entityType, refEntity.id, "?$select=autod_name&$expand=autod_modelid($select=autod_recommendedamount)")
        amount = result?.autod_modelid?.autod_recommendedamount;
        alert(amount);
        Navicon.myApi.setValue(context, fields.autod_summa, amount);
      })();
    }    
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
      formContext.getAttribute(fields.autod_creditid).addOnChange(  checkCreditExpiration  );
      formContext.getAttribute(fields.autod_creditid).addOnChange(  pasteCreditPeriodIntoContract  );
      formContext.getAttribute(fields.autod_autoid).addOnChange(  calculateAmount  );
      formContext.getAttribute(fields.autod_autoid).addOnChange(  () => Navicon.myApi.setValue(context, fields.autod_creditid, null);  );

      
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