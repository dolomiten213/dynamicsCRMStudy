
var Navicon = Navicon ?? {}

Navicon.contract_ribbon = (function() {

  let fields = {
    autod_number: "autod_number",
    autod_name: "autod_name",
    autod_creditid: "autod_creditid",
    autod_summa: "autod_summa",
    autod_fact: "autod_fact",
    autod_contact: "autod_contact",
    autod_autoid: "autod_autoid",
    autod_date: "autod_date",
    autod_creditperiod: "autod_creditperiod",
    autod_initialfee: "autod_initialfee",
    autod_creditamount: "autod_creditamount",
    autod_fullcreditamount: "autod_fullcreditamount"
  };

  return  {
    async CalculateCredit(context) {
      let sum = Navicon.myApi.getValueRibbon(context, fields.autod_summa)
      let fee = Navicon.myApi.getValueRibbon(context, fields.autod_initialfee)

      sum -= fee
      if (sum != null && fee != null) {
        Navicon.myApi.setValueRibbon(context, fields.autod_creditamount, sum)
      } else {
        alert("Не хватает данных: сумма и первоначальный взнос")
        return
      }
      
      let percent = await Navicon.myApi.getFieldFromReferencedEntityRibbon(context, fields.autod_creditid, "autod_percent")
      let period = Navicon.myApi.getValueRibbon(context, fields.autod_creditperiod)

      if (sum != null && percent != null && period != null) {
        let res = percent / 100;
        res *= period;
        res *= sum;
        res += sum;
        Navicon.myApi.setValueRibbon(context, fields.autod_fullcreditamount, res)
      } else {
        alert("Не хватает данных: срок кредита и кредитная программа[процентная ставка]")
        return
      }
      
    }
  }
})();