
var Navicon = Navicon ?? {}

Navicon.brand = (function() {

  let fields = {
    used: "autod_used",
    isdamaged: "autod_isdamaged",
    ownersCount: "autod_ownerscount",
    km: "autod_km"
  };

  return  {
    onLoad(context) {

      let formContext = context.getFormContext();
      (async () => {
        let frameControl = await formContext.getControl("WebResource_brand_frame").getContentWindow();
        frameControl.pasteDataIntoFrame(formContext);
      })();
      
    }
  }
})();